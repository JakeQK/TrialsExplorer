using FusionExplorer.Models;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public event EventHandler ArchiveOpened;
        public event EventHandler ArchiveSaved;

        public bool IsArchiveLoaded => _archiveData != null;
        public string CurrentFilePath => _currentFilePath;
        public bool IsDirty => _isDirty;
        public IReadOnlyList<ArchiveFile> Files => _files?.AsReadOnly();
        public ArchiveHeader Header => _header;

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
                            ArchiveFileEntry = entry
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
                _rootDirectory = new ArchiveDirectory(Path.GetFileName(_currentFilePath));

                Dictionary<string, ArchiveDirectory> directories = new Dictionary<string, ArchiveDirectory>();
                directories[""] = _rootDirectory;

                foreach (var file in _files)
                {
                    if (file.IsFilenameTable)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(file.DirectoryPath) && !directories.ContainsKey(file.DirectoryPath))
                    {
                        string[] parts = file.DirectoryPath.Split('/');
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

                    directories[file.DirectoryPath].AddChild(file);
                }
                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Failed to build file hierarchy: {ex.Message}");
                return false;
            }
        }

        public ArchiveDirectory GetRootDirectory() => _rootDirectory;

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

                    string directory = $"{AppDomain.CurrentDomain.BaseDirectory}Extracted Files/{Path.GetFileName(_currentFilePath)}/{file.DirectoryPath}";

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    using(BinaryWriter writer = new BinaryWriter(File.Create($"{directory}{file.SafeFilename}")))
                    {
                        if (file.ArchiveFileEntry.DecompressedSize > int.MaxValue || file.ArchiveFileEntry.CompressedSize > int.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException("File Size", "Byte count exceeds maximum value of an int");
                        }

                        switch(file.ArchiveFileEntry.CompressionFlag)
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
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to extract file: {ex.Message}");

                return null;
            }
        }




    }
}
