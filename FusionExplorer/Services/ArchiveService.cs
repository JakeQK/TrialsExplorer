using FusionExplorer.Models;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Drawing;
using Microsoft.AspNetCore.Localization;

namespace FusionExplorer.Services
{
    public class ArchiveService
    {
        private byte[] _archiveData;
        private string _currentFilePath;
        private ArchiveHeader _header;
        private List<ArchiveFile> _files;
        private bool _isDirty = false;
        private ArchiveDirectory _rootDirectory;

        public bool IsArchiveLoaded => _archiveData != null;
        public string CurrentFilePath => _currentFilePath;
        public bool IsDirty => _isDirty;
        public IReadOnlyList<ArchiveFile> Files => _files?.AsReadOnly();
        public ArchiveHeader Header => _header;
        public ArchiveDirectory GetRootDirectory() => _rootDirectory;

        #region Core Operations
        public bool OpenArchive(string filePath)
        {
            try
            {
                _archiveData = File.ReadAllBytes(filePath);
                _currentFilePath = filePath;

                if (!ParseArchiveStructure())
                {
                    return false;
                }

                return BuildFileHierarchy();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening archive: {ex.Message}");
                
                _archiveData = null;
                _currentFilePath = null;

                return false;
            }
        }

        public bool ParseArchiveStructure()
        {
            try
            {
                using(MemoryStream ms = new MemoryStream(_archiveData))
                using(BinaryReader reader = new BinaryReader(ms))
                {
                    _header = new ArchiveHeader
                    {
                        FileSignature = reader.ReadInt32(),
                        OffsetToData = reader.ReadUInt32(),
                        FileEntryCount = reader.ReadUInt32()
                    };

                    if(!_header.IsValidSignature())
                    {
                        MessageBox.Show("Invalid file signature");
                        return false;
                    }

                    
                    _files = new List<ArchiveFile>();
                    for(int i = 0; i < _header.FileEntryCount; i++)
                    {
                        ArchiveFileEntry entry = new ArchiveFileEntry
                        {
                            Id = reader.ReadInt32(),
                            CompressedSize = reader.ReadUInt32(),
                            DecompressedSize = reader.ReadUInt32(),
                            CompressionFlag = reader.ReadByte(),
                            OffsetToData = reader.ReadUInt32()
                        };

                        ArchiveFile file = new ArchiveFile
                        {
                            ArchiveFileEntry = entry,
                            ArchiveFileEntryOffset = reader.BaseStream.Position - 17
                        };

                        _files.Add(file);
                    }

                    if(_files.Count > 0)
                    {
                        ArchiveFile filenameTable = _files.Where(file => file.ArchiveFileEntry.IsFilenameTableEntry()).FirstOrDefault();

                        if(filenameTable != null)
                        {
                            ParseFilenameTable(filenameTable);
                        }
                    }

                    return true;
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show($"Failed to parse archive structure: {ex.Message}");

                return false;
            }
        }

        private bool ParseFilenameTable(ArchiveFile filenameTable)
        {
            try
            {
                using(MemoryStream ms = new MemoryStream(_archiveData))
                using(BinaryReader br = new BinaryReader(ms))
                {
                    br.BaseStream.Seek(filenameTable.ArchiveFileEntry.OffsetToData, SeekOrigin.Begin);

                    uint _count = br.ReadUInt32();

                    foreach (ArchiveFile file in _files)
                    {
                        if (!file.ArchiveFileEntry.IsFilenameTableEntry())
                        {
                            UInt16 _stringLength = br.ReadUInt16();
                            string filename = new string(br.ReadChars(_stringLength));
                            file.FullPath = filename;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to parse filename table: {ex.Message}");

                return false;
            }
        }

        private bool BuildFileHierarchy()
        {
            try
            {
                _rootDirectory = new ArchiveDirectory("");

                Dictionary<string, ArchiveDirectory> directories = new Dictionary<string, ArchiveDirectory>();
                directories[""] = _rootDirectory;

                foreach (var file in _files)
                {
                    if (file.IsFilenameTable)
                    {
                        continue;
                    }

                    string dirPath = file.DirectoryPath?.Replace("\\", "/") ?? "";

                    if (!string.IsNullOrEmpty(dirPath) && !directories.ContainsKey(dirPath))
                    {
                        string[] parts = dirPath.Split('/');
                        string currentPath = "";

                        foreach (string part in parts)
                        {
                            string parentPath = currentPath;
                            currentPath = string.IsNullOrEmpty(currentPath) ? part : $"{currentPath}/{part}";

                            if(!directories.ContainsKey(currentPath))
                            {
                                var newDir = new ArchiveDirectory(currentPath);
                                directories[parentPath].AddChild(newDir);
                                directories[currentPath] = newDir;
                            }
                        }
                    }

                    directories[dirPath].AddChild(file);
                }

                foreach(var child in _rootDirectory.Children)
                {
                    Console.WriteLine(child.Name);
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to build file hierarchy: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region File Operations
        public byte[] ExtractFile(ArchiveFile file)
        {
            if (!IsArchiveLoaded || file == null)
            {
                MessageBox.Show("Archive is not loaded or file is null");
                return null;
            }

            try
            {
                using(MemoryStream ms = new MemoryStream(_archiveData))
                using(BinaryReader reader = new BinaryReader(ms))
                {
                    reader.BaseStream.Seek(file.ArchiveFileEntry.OffsetToData, SeekOrigin.Begin);

                    switch (file.ArchiveFileEntry.CompressionFlag)
                    {
                        // Uncompressed
                        case 0:
                            return reader.ReadBytes((int)file.ArchiveFileEntry.DecompressedSize);
                        // zlib Compressed
                        case 1:
                            return ZlibStream.UncompressBuffer(reader.ReadBytes((int)file.ArchiveFileEntry.CompressedSize));
                        // Trials Evolution (Xbox 360) compressed
                        case 4:
                            MessageBox.Show("Trials Evolution: Xbox 360 compression unsupported", "Fusion Explorer");
                            return null;
                        // Trials Evolution: Gold Edition zlib compressed
                        case 8:
                            return ZlibStream.UncompressBuffer(reader.ReadBytes((int)file.ArchiveFileEntry.CompressedSize));
                        // Unknown Case (Found in data_patch.pak)
                        case 16:
                            MessageBox.Show("Unsupported file type", "Currently unable to extract files from data_patch.pak\n\nExtracting raw data...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reader.ReadBytes((int)file.ArchiveFileEntry.CompressedSize);
                        // Unknown Case (Found in data_patch.pak)
                        case 17:
                            MessageBox.Show("Unsupported file type", "Currently unable to extract files from data_patch.pak\n\nExtracting raw data...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reader.ReadBytes((int)file.ArchiveFileEntry.CompressedSize);
                        // Unknown Case (Found in Trials Rising)
                        case 32:
                            MessageBox.Show("Unsupported file type", "Currently unable to extract files from data_patch.pak\n\nExtracting raw data...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reader.ReadBytes((int)file.ArchiveFileEntry.CompressedSize);
                        // Unknown Case (Found in Trials Rising)
                        case 33:
                            MessageBox.Show("Unsupported file type", "Currently unable to extract files from data_patch.pak\n\nExtracting raw data...", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return reader.ReadBytes((int)file.ArchiveFileEntry.CompressedSize);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to extract file: {ex.Message}");

                return null;
            }
        }

        public void QuickExtractFile(ArchiveFile file)
        {
            try
            {
                byte[] data = ExtractFile(file);

                if (data != null)
                {
                    string fullPath = Path.Combine(Application.StartupPath,"Extracted Files", Path.GetFileName(_currentFilePath), file.DirectoryPath);

                    Directory.CreateDirectory(fullPath);

                    string filePath = Path.Combine(fullPath, file.Name);

                    using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to extract file to extracts folder: {ex.Message}");
            }
        }

        public void ExtractFileToSelectedPath(ArchiveFile file)
        {
            try
            {
                // Extract the file data
                byte[] data = ExtractFile(file);

                if (data != null)
                {
                    // Create and configure Save File Dialog
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.FileName = file.Name;
                        saveDialog.Title = "Save Extracted File";
                        saveDialog.Filter = "All Files (*.*)|*.*";
                        saveDialog.OverwritePrompt = true;

                        // Show the dialog and get result
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {
                            string selectedPath = saveDialog.FileName;

                            // Create directory if it doesn't exist
                            string directory = Path.GetDirectoryName(selectedPath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            // Write the file to the selected location
                            using (BinaryWriter writer = new BinaryWriter(File.Create(selectedPath)))
                            {
                                writer.Write(data);
                            }

                            // MessageBox.Show($"File successfully extracted to: {selectedPath}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to extract file: {ex.Message}");
            }
        }
        
        public bool ReplaceFile(ArchiveFile file, byte[] data)
        {
            if (file == null || data == null)
            {
                return false;
            }

            if (!IsArchiveLoaded)
            {
                return false;
            }

            try
            {
                ArchiveFileEntry entry = file.ArchiveFileEntry;

                byte[] dataToWrite = data;

                if (entry.IsCompressed)
                {
                    dataToWrite = ZlibStream.CompressBuffer(data);

                    // potentially implement other compression/encryption methods once discovered
                }

                int sizeDifference = dataToWrite.Length - (int)entry.CompressedSize;

                byte[] newArchiveData = new byte[_archiveData.Length + sizeDifference];

                // 1. Copy everythin up to the file entry's size fields
                Buffer.BlockCopy(_archiveData, 0, newArchiveData, 0, (int)file.ArchiveFileEntryOffset + 4);

                // 2. Update the file entry's size fields
                BitConverter.GetBytes(dataToWrite.Length).CopyTo(newArchiveData, (int)file.ArchiveFileEntryOffset + 4);
                BitConverter.GetBytes(data.Length).CopyTo(newArchiveData, (int)file.ArchiveFileEntryOffset + 8);

                // 3. Copy from after the size fields to the data offset
                int currentPosition = (int)file.ArchiveFileEntryOffset + 12; // 4 + 4 + 4 (ID + two sizes)
                Buffer.BlockCopy(_archiveData, currentPosition, newArchiveData, currentPosition, (int)entry.OffsetToData - currentPosition);

                // 4. Insert the new file data
                Buffer.BlockCopy(dataToWrite, 0, newArchiveData, (int)entry.OffsetToData, dataToWrite.Length);

                // 5. Copy the remaining data
                int sourcePos = (int)(entry.OffsetToData + entry.CompressedSize);
                int destPos = (int)(entry.OffsetToData + dataToWrite.Length);
                int remainingLength = _archiveData.Length - sourcePos;
                Buffer.BlockCopy(_archiveData, sourcePos, newArchiveData, destPos, remainingLength);

                if (sizeDifference != 0)
                {
                    foreach (var otherFile in _files)
                    {
                        if (otherFile.ArchiveFileEntry.OffsetToData <= entry.OffsetToData)
                            continue;

                        // Update OffsetToData in memory
                        otherFile.ArchiveFileEntry.OffsetToData = (uint)((int)otherFile.ArchiveFileEntry.OffsetToData + sizeDifference);

                        int entryOffsetPos = (int)otherFile.ArchiveFileEntryOffset + 9; // 4 + 4 + 4 + 1 (ID, Size, Size, Compression Flag)
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to replace file: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Directory Operations
        public void QuickExtractDirectory(ArchiveDirectory directory)
        {
            try
            {
                // Create the extraction path
                string rootPath = Path.Combine(Application.StartupPath, "Extracted Files", Path.GetFileName(_currentFilePath), directory.FullPath);
                Directory.CreateDirectory(rootPath);

                // Process all children
                foreach (var child in directory.Children)
                {
                    try
                    {
                        if (child.IsDirectory)
                        {
                            // Recursively extract subdirectories
                            QuickExtractDirectory((ArchiveDirectory)child);
                        }
                        else
                        {
                            // Extract individual file
                            ArchiveFile file = (ArchiveFile)child;
                            byte[] data = ExtractFile(file);

                            if (data != null)
                            {
                                string filePath = Path.Combine(rootPath, file.Name);

                                using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
                                {
                                    writer.Write(data);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other files
                        Console.WriteLine($"Failed to extract {child.Name}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle directory-level errors
                MessageBox.Show($"Failed to extract directory {directory.FullPath}: {ex.Message}");
            }
        }

        public void ExtractDirectoryToSelectedPath(ArchiveDirectory directory)
        {
            try
            {
                // Show folder browser dialog to select destination
                using (CommonOpenFileDialog folderDialog = new CommonOpenFileDialog())
                {
                    folderDialog.Title = $"Select destination folder for extracting '{directory.Name}'";
                    folderDialog.IsFolderPicker = true;
                    folderDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    folderDialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    folderDialog.AddToMostRecentlyUsedList = false;
                    folderDialog.AllowNonFileSystemItems = false;
                    folderDialog.EnsureFileExists = true;
                    folderDialog.EnsurePathExists = true;
                    folderDialog.EnsureReadOnly = false;
                    folderDialog.EnsureValidNames = true;
                    folderDialog.Multiselect = false;
                    folderDialog.ShowPlacesList = true;

                    // Show dialog and check if user selected a folder
                    if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        string selectedPath = folderDialog.FileName;

                        // Create target directory named after the source directory
                        string targetPath = Path.Combine(selectedPath, directory.Name);
                        Directory.CreateDirectory(targetPath);

                        // Use helper function to handle all extraction
                        ExtractDirectoryContents(directory, targetPath);

                        // Optional: Show success message
                        MessageBox.Show($"Directory '{directory.Name}' extracted successfully to {selectedPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to extract directory: {ex.Message}");
            }
        }

        // Helper method to recursively extract directory contents
        private void ExtractDirectoryContents(ArchiveDirectory directory, string targetPath)
        {
            foreach (var child in directory.Children)
            {
                try
                {
                    if (child.IsDirectory)
                    {
                        // For subdirectories, create them and extract their contents
                        ArchiveDirectory subDir = (ArchiveDirectory)child;
                        string subDirPath = Path.Combine(targetPath, subDir.Name);
                        Directory.CreateDirectory(subDirPath);

                        // Recursive call to extract contents
                        ExtractDirectoryContents(subDir, subDirPath);
                    }
                    else
                    {
                        // Extract individual file
                        ArchiveFile file = (ArchiveFile)child;
                        byte[] data = ExtractFile(file);

                        if (data != null)
                        {
                            string filePath = Path.Combine(targetPath, file.Name);

                            using (BinaryWriter writer = new BinaryWriter(File.Create(filePath)))
                            {
                                writer.Write(data);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to extract {child.Name}: {ex.Message}");
                }
            }
        }
        #endregion



    }
}
