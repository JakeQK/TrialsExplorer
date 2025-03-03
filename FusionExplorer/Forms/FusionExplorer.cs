using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zlib;
using Ookii.Dialogs.WinForms;
using System.Globalization;
using FusionExplorer.src;
using FusionExplorer.Services;
using FusionExplorer.Models;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FusionExplorer 
{
    public partial class FusionExplorer : Form
    {
        private static string filename;
        private static string safe_filename;

        private static int data_offset;
        private static int file_count;

        private static byte[] pak_data;
        private static List<ArchiveFile> archive_files = new List<ArchiveFile>();

        bool changesMade = false;



        private ArchiveService service = new ArchiveService();

        private ContextMenuStrip fileContextMenu;
        private ContextMenuStrip directoryContextMenu;



        public FusionExplorer() 
        {
            InitializeComponent();
            SetupContextMenus();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pak files (*.pak)|*.pak|All files (*.*)|*.*";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                service.OpenArchive(ofd.FileName);
                experimentalPopulateTreeView();

                toolStripStatusLabel1.Text = filename;
                closeFileToolStripMenuItem.Enabled = true;
            }
        }

        private void experimentalPopulateTreeView()
        {
            tvDirectoryDisplay.Nodes.Clear();

            ArchiveDirectory rootDirectory = service.GetRootDirectory();

            if (rootDirectory != null)
            {
                TreeNode rootNode = new TreeNode(rootDirectory.Name);
                rootNode.Tag = rootDirectory;
                tvDirectoryDisplay.Nodes.Add(rootNode);

                AddChildNodes(rootNode, rootDirectory);

                rootNode.Expand();
            }

            tvDirectoryDisplay.TreeViewNodeSorter = new NodeSorter();

        }

        private void AddChildNodes(TreeNode parentNode, ArchiveDirectory directory)
        {
            foreach (IArchiveNode node in directory.Children)
            {
                TreeNode childNode = new TreeNode(node.Name);
                childNode.Tag = node;

                if (node.IsDirectory)
                {
                    childNode.ImageIndex = 0;
                    childNode.SelectedImageIndex = 0;

                    AddChildNodes(childNode, (ArchiveDirectory)node);


                }
                else
                {
                    var fileNode = (Models.ArchiveFile)node;
                    childNode.ImageIndex = fileNode.GetImageIndex();
                    childNode.SelectedImageIndex = childNode.ImageIndex;

                    childNode.Text = fileNode.SafeFilename;
                }

                parentNode.Nodes.Add(childNode);
            }
        }

        private void SetupContextMenus()
        {
            fileContextMenu = new ContextMenuStrip();
            fileContextMenu.Items.Add("Quick extract", null, QuickExtractFile_Click);
            fileContextMenu.Items.Add("Extract to selected path", null, ExtractFileToSelectedPath_Click);
            fileContextMenu.Items.Add("Replace", null, ReplaceFile_Click);
            fileContextMenu.Items.Add(new ToolStripSeparator());
            fileContextMenu.Items.Add("Properties", null, null);

            directoryContextMenu = new ContextMenuStrip();
            directoryContextMenu.Items.Add("Quick extract directory", null, QuickExtractDirectory_Click);
            directoryContextMenu.Items.Add("Extract directory to selected path", null, ExtractDirectoryToSelectedPath_Click);
        }

        private void QuickExtractFile_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveFile file)
            {
                service.QuickExtractFile(file);
            }
        }

        private void ExtractFileToSelectedPath_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveFile file)
                {
                    // Extract the file data
                    byte[] data = service.ExtractFile(file);

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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to extract file: {ex.Message}");
            }
        }

        private void QuickExtractDirectory_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveDirectory directory)
            {
                service.QuickExtractDirectory(directory);
            }
        }

        private void ExtractDirectoryToSelectedPath_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveDirectory directory)
            {
                service.ExtractDirectoryToSelectedPath(directory);
            }
        }

        private void ReplaceFile_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveFile file)
            {

                using (CommonOpenFileDialog fileDialog = new CommonOpenFileDialog())
                {
                    fileDialog.Title = $"Select file to replace '{file.Name}'";
                    fileDialog.IsFolderPicker = false;
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    fileDialog.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    fileDialog.AddToMostRecentlyUsedList = true;
                    fileDialog.AllowNonFileSystemItems = false;
                    fileDialog.EnsureFileExists = true;
                    fileDialog.EnsurePathExists = true;
                    fileDialog.EnsureReadOnly = false;
                    fileDialog.EnsureValidNames = true;
                    fileDialog.Multiselect = false;
                    fileDialog.ShowPlacesList = true;

                    if(fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        byte[] data = File.ReadAllBytes(fileDialog.FileName);

                        bool status = service.ReplaceFile(file, data);


                        if (status == true)
                        {
                            MessageBox.Show($"Successfully replaced {file.Name} with {fileDialog.FileName}");
                        }
                        else
                        {
                            MessageBox.Show($"Failed to replace {file.Name} with {fileDialog.FileName}");
                        }
                    }

                }
            }
        }

        /* Import file
         * Replaces the selected file in the Directory Display with a new file selected through a OpenFileDialog
         * Recalculates the compressed and uncompressed size for the file entry
         * Updates the data offsets for all file entries 
         */
        private void import_file(object sender, EventArgs e)
        {
            ArchiveFile selected_archive_file = get_selected_archive_file();
            int index = selected_archive_file.filename.IndexOf(".");
            string extension = selected_archive_file.filename.Substring(index, selected_archive_file.filename.Length - index);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = string.Format("file (*{0})|*{1}| all files (*.*)|*.*", extension, extension);
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] new_file_data = File.ReadAllBytes(ofd.FileName);
                import_file(new_file_data);
            }
        }

        private void import_file(byte[] data)
        {
            ArchiveFile selected_archive_file = get_selected_archive_file();
            byte[] compressed_data = ZlibStream.CompressBuffer(data);
            bool compressed = false;
            if (selected_archive_file.fileEntry.compressionFlag == 1 || selected_archive_file.fileEntry.compressionFlag == 8)
                compressed = true;

            using (MemoryStream ms = new MemoryStream(pak_data))
            {
                // update file entry compressed and uncompressed sizes
                ms.Position = selected_archive_file.fileEntry.fileEntryOffset + 4;
                if (compressed)
                    ms.Write(BitConverter.GetBytes(compressed_data.Length), 0, 4);
                else
                    ms.Write(BitConverter.GetBytes(data.Length), 0, 4);
                ms.Write(BitConverter.GetBytes(data.Length), 0, 4);

                // set the archive sizes as we use them in the update_data_offsets function to recalculate the data offsets
                if (compressed)
                    selected_archive_file.fileEntry.compressedSize = compressed_data.Length;
                else
                    selected_archive_file.fileEntry.compressedSize = data.Length;
                selected_archive_file.fileEntry.decompressedSize = data.Length;

                ms.Position = selected_archive_file.fileEntry.dataOffset + selected_archive_file.fileEntry.compressedSize;

                // must calculate the ending stride length before update_data_offsets is called, as we're taking the data from the unaltered pak data
                // read ending stride [c]
                byte[] ending = new byte[pak_data.Length - (selected_archive_file.fileEntry.dataOffset + selected_archive_file.fileEntry.compressedSize)];
                ms.Read(ending, 0, ending.Length);

                // update data_offsets
                // must be done before [a] stride is read, as we need the data offsets updated
                update_data_offsets();

                // position stream at the beginning of data ready to read the [a] stride
                // data offset of selected file is the ending of the [a] stride
                ms.Position = 0;
                byte[] beginning = new byte[selected_archive_file.fileEntry.dataOffset];
                ms.Read(beginning, 0, beginning.Length);


                // construct new pak data
                // [a] - recalculated data offsets and sizes
                // [b] - new file data
                // [c] - ending stride

                int len = data.Length;
                if (compressed)
                    len = compressed_data.Length;

                byte[] new_pak_data = new byte[beginning.Length + len + ending.Length];
                Buffer.BlockCopy(beginning, 0, new_pak_data, 0, beginning.Length);
                if(compressed)
                    Buffer.BlockCopy(compressed_data, 0, new_pak_data, beginning.Length, len);
                else
                    Buffer.BlockCopy(data, 0, new_pak_data, beginning.Length, len);
                Buffer.BlockCopy(ending, 0, new_pak_data, beginning.Length + len, ending.Length);

                // replace old pak with new pak data
                pak_data = new_pak_data;
            }
            changesMade = true;
            saveToolStripMenuItem.Enabled = true;
        }
        /* Get File Info
         * Returns file entry information from selected file
         */
        private void get_file_info(object sender, EventArgs e)
        {
            ArchiveFile selected_archive_file = get_selected_archive_file();
            string formattedStr = string.Format("Compressed Size: {0:n0} bytes\nSize: {1:n0} bytes\nCompressed: {2}\nData Offset: 0x{3} ({4})\nEntry Offset: 0x{5} ({6})\nID: {7}",
                            selected_archive_file.fileEntry.compressedSize, 
                            selected_archive_file.fileEntry.decompressedSize, 
                            BitConverter.ToBoolean(new byte[] { selected_archive_file.fileEntry.compressionFlag }, 0).ToString(), 
                            selected_archive_file.fileEntry.dataOffset.ToString("X"), 
                            selected_archive_file.fileEntry.dataOffset, 
                            selected_archive_file.fileEntry.fileEntryOffset.ToString("X"),
                            selected_archive_file.fileEntry.fileEntryOffset,
                            selected_archive_file.fileEntry.ID);
            MessageBox.Show(formattedStr, selected_archive_file.filename);
        }

        /* View Texture
         * Opens selected png.tex files in Texture Viewer form
         * And displays the image
         */
        private void view_texture(object sender, EventArgs e)
        {
            ArchiveFile selected_archive_file = get_selected_archive_file();

            byte[] tex = null;
            using (MemoryStream ms = new MemoryStream(pak_data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.BaseStream.Seek(selected_archive_file.fileEntry.dataOffset, SeekOrigin.Begin);

                    switch (selected_archive_file.fileEntry.compressionFlag)
                    {
                        // Uncompressed
                        case 0:
                                tex = br.ReadBytes(selected_archive_file.fileEntry.decompressedSize);
                            break;
                        // zlib Compressed
                        case 1:
                                tex = ZlibStream.UncompressBuffer(br.ReadBytes(selected_archive_file.fileEntry.compressedSize));
                            break;
                    }
                }
            }

            byte[] dds = Texture.TEXToDDS(tex);
            if (dds != null)
            {
                TextureViewer fm = new TextureViewer(dds);
                fm.Show();
                fm.Text = selected_archive_file.filename;
            }
        }

        /* Export DDS
         * Gets tex data from selected file
         * Converts tex data to DDS
         * Opens a VistaSaveFileDialog to choose save location and name
         */
        private void export_dds(object sender, EventArgs e)
        {
            byte[] tex = get_file_data();
            byte[] dds = Texture.TEXToDDS(tex);

            string filename = get_selected_archive_file().filename;
            string safeFilename = filename.Remove(0, filename.LastIndexOf("/") + 1);
            safeFilename = safeFilename.Remove(safeFilename.IndexOf("."), safeFilename.Length - safeFilename.IndexOf("."));

            VistaSaveFileDialog sfd = new VistaSaveFileDialog();
            sfd.Filter = "dds image (*.dds)|*.dds|all files (*.*)|*.*";
            sfd.FileName = safeFilename + ".dds";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                using(BinaryWriter bw = new BinaryWriter(File.Create(sfd.FileName)))
                {
                    bw.Write(dds);
                }
            }
        }

        /* Import DDS
         * Gets the tex version from selected file
         * Gets DDS data from new file, converts it to appropriate tex data
         * Imports new tex data
         */
        private void import_dds(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ArchiveFile selected_file = get_selected_archive_file();
                byte[] data = File.ReadAllBytes(ofd.FileName);

                byte[] tex = get_file_data();
                Texture.TextureVersion textureVersion = Texture.TextureVersion.T5X;
                switch(tex[1])
                {
                    case 0x35:
                        textureVersion = Texture.TextureVersion.T5X;
                        break;
                    case 0x38:
                        textureVersion = Texture.TextureVersion.T8X;
                        break;

                }

                tex = Texture.DDStoTEX(data, textureVersion);
                import_file(tex);
            }
        }

        private void add_new_file(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ArchiveFile selected_file = get_selected_archive_file();
                byte[] data = File.ReadAllBytes(ofd.FileName);
            }
        }

        private void add_new_dds(object sender, EventArgs e)
        {

        }

        private void open_with_hxd(object sender, EventArgs e)
        {
            string path = Path.GetTempFileName();

            File.WriteAllBytes(path, get_file_data());

            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = path;

            string HxDPath = Utility.GetConfigValue("HxD");
            if (File.Exists(HxDPath))
            {
                start.FileName = HxDPath;
                Process proc = Process.Start(start);
            }
            else
            {
                MessageBox.Show("HxD not found at " + HxDPath + Properties.FusionExplorer.HXD_NOT_FOUND_TEXT, Properties.FusionExplorer.HXD_NOT_FOUND_TITLE);
            }
        }

        byte[] get_file_data()
        {
            ArchiveFile selected_archive_file = get_selected_archive_file();
            using (MemoryStream ms = new MemoryStream(pak_data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // Goto data offset of selected file
                    br.BaseStream.Seek(selected_archive_file.fileEntry.dataOffset, SeekOrigin.Begin);

                    // Obtain Path and Filename as seperate strings (ex: "/localization/", "evo1_chinese.xml")
                    string[] split_filename = filename_split(selected_archive_file.filename);

                    // Create a directory for exported file if it doesn't exist
                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Exports/" + safe_filename + "/" + split_filename[0];
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);

                    // Create the file being exported and setup BinaryWriter to write the data
                    using (BinaryWriter bw = new BinaryWriter(File.Create(dir + split_filename[1])))
                    {
                        switch (selected_archive_file.fileEntry.compressionFlag)
                        {
                            // Uncompressed
                            case 0:
                                return br.ReadBytes(selected_archive_file.fileEntry.decompressedSize);
                            // zlib Compressed
                            case 1:
                                return ZlibStream.UncompressBuffer(br.ReadBytes(selected_archive_file.fileEntry.compressedSize));
                            // Trials Evolution (Xbox 360) lzx compressed
                            case 4:
                                // Unsupported
                                break;
                            // Trials Evolution: Gold Edition zlib compressed
                            case 8:
                                return ZlibStream.UncompressBuffer(br.ReadBytes(selected_archive_file.fileEntry.compressedSize));
                            // Unknown Case (Found in data_patch.pak)
                            case 16:
                                return br.ReadBytes(selected_archive_file.fileEntry.compressedSize);
                            // Unknown Case (Found in data_patch.pak)
                            case 17:
                                return br.ReadBytes(selected_archive_file.fileEntry.compressedSize);
                        }
                    }
                }
            }
            return null;
        }

        private void update_data_offsets()
        {
            using (MemoryStream ms = new MemoryStream(pak_data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // Skip Pak Header (Magic, Data Offset, File Count)
                    ms.Seek(12, SeekOrigin.Begin);
                    // Skip First File Entry (Data Offset for this shouldn't change, File Entries * 17) 
                    // *Note: This is for the current state of fusion explorer, when we want to add NEW files, 
                    // this will have to change, as there will be more file entries
                    ms.Seek(17, SeekOrigin.Current);

                    
                    // Calculating Offsets by length of each file
                    int offset = data_offset;
                    for(int i = 1; i < file_count; i++)
                    {
                        // Seek to the Data Offset section of file entries (ms.write makes up the other 4 byte forward movement)
                        ms.Seek(13, SeekOrigin.Current);
                        // Offset calculation
                        offset += archive_files[i - 1].fileEntry.compressedSize;

                        // Update our ArchiveFile list
                        archive_files[i].fileEntry.dataOffset = offset;
                        // Update the pak data
                        ms.Write(BitConverter.GetBytes(offset), 0, 4);
                    }
                }
            }
        }

        private ArchiveFile get_selected_archive_file()
        {
            foreach (ArchiveFile file in archive_files)
            {
                if (file.tree_node == tvDirectoryDisplay.SelectedNode)
                {
                    return file;
                }
            }

            return null;
        }

        // Returns file directory and filename as 2 seperate strings. ex: "C:\folder\photos\image.png" -> { "C:\folder\photos\", "image.png" }
        private static string[] filename_split(string filename)
        {
            string[] values = filename.Split('/', '\\').ToArray();
            string this_safe_filename = values[values.Length - 1];
            return new string[] { filename.Remove(filename.Length - this_safe_filename.Length, this_safe_filename.Length), this_safe_filename };
        }

        // Sorts nodes based on their image index/whether or not it's a file or a folder
        private class NodeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (((TreeNode)x).ImageIndex == 1)
                    return 1;
                return 0;
            }
        }

        public class ArchiveFile
        {
            public FileEntry fileEntry;
            public string filename;
            public TreeNode tree_node;
        }

        public class FileEntry
        {
            public FileEntry(int CompressedSize, int DecompressedSize, byte CompressionFlag, int DataOffset, long FileEntryOffset)
            {
                this.compressedSize = CompressedSize;
                this.decompressedSize = DecompressedSize;
                this.compressionFlag = CompressionFlag;
                this.dataOffset = DataOffset;
                this.fileEntryOffset = FileEntryOffset;
            }

            public FileEntry(int ID, int CompressedSize, int DecompressedSize, byte CompressionFlag, int DataOffset, long FileEntryOffset)
            {
                this.ID = ID;
                this.compressedSize = CompressedSize;
                this.decompressedSize = DecompressedSize;
                this.compressionFlag = CompressionFlag;
                this.dataOffset = DataOffset;
                this.fileEntryOffset = FileEntryOffset;
            }

            public int ID { get; set; }
            public int compressedSize { get; set; }
            public int decompressedSize { get; set; }
            public byte compressionFlag { get; set; }
            public int dataOffset { get; set; }
            public long fileEntryOffset { get; set; }
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.Nodes.Count > 0)
            {
                tvDirectoryDisplay.Nodes.Clear();
                pak_data = null;
                archive_files.Clear();
                toolStripStatusLabel1.Text = "";

                saveToolStripMenuItem.Enabled = false;
                closeFileToolStripMenuItem.Enabled = false;
                changesMade = false;
            }
        }

        private void tvDirectoryDisplay_DoubleClick(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode != null)
            {
                if(tvDirectoryDisplay.SelectedNode.Text.Contains(".xml"))
                {
                    byte[] data = get_file_data();
                    string str = UTF8Encoding.ASCII.GetString(data);
                    Notepad.ShowMessage(str);
                }

                if (tvDirectoryDisplay.SelectedNode.Text.Contains("png.tex"))
                {
                    view_texture(sender, e);
                }

                if (tvDirectoryDisplay.SelectedNode.ImageIndex == 0)
                {
                    if (tvDirectoryDisplay.SelectedNode.IsExpanded)
                        tvDirectoryDisplay.SelectedNode.Collapse();
                    tvDirectoryDisplay.SelectedNode.Expand();
                }
            }
        }

        private bool SaveChanges()
        {
            if (changesMade)
            {
                File.WriteAllBytes(filename, pak_data);
                changesMade = false;
                saveToolStripMenuItem.Enabled = false;
                toolStripStatusLabel1.Text = "Saved " + filename;
                return true;
            }
            return false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(changesMade)
            {
                DialogResult result = MessageBox.Show(Properties.FusionExplorer.FORM_CLOSING_SAVE_TEXT + filename, Properties.FusionExplorer.FORM_CLOSING_SAVE_TITLE, MessageBoxButtons.YesNoCancel);
                switch(result)
                {
                    case DialogResult.Yes:
                        File.WriteAllBytes(filename, pak_data);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            else if (tvDirectoryDisplay.Nodes.Count == 1)
            {
                e.Cancel = (MessageBox.Show(Properties.FusionExplorer.FORM_CLOSING_CHECK_TEXT, Properties.FusionExplorer.FORM_CLOSING_CHECK_TITLE, MessageBoxButtons.YesNo) == DialogResult.No);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                SaveChanges();
            }
        }

        private void btnArchiveBuilder_Click(object sender, EventArgs e)
        {
            ArchiveBuilder form = new ArchiveBuilder();
            form.Show();
        }

        private void btnTrackTool_Click(object sender, EventArgs e)
        {
            TrackTool tt = new TrackTool(); // lol tt
            tt.Show();
        }

        private void btnOpenObjectCollection_Click(object sender, EventArgs e)
        {
            ObjectCollection oc = new ObjectCollection();
            oc.Show();
        }

        private void btnEditorFavouritesTool_Click(object sender, EventArgs e)
        {
            EditorFavouritesTool eft = new EditorFavouritesTool();
            eft.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Properties.FusionExplorer.ABOUT_TEXT);
            sb.AppendLine(Properties.FusionExplorer.ABOUT_VERSION);
            sb.AppendLine(Properties.FusionExplorer.ABOUT_CREATOR);
            MessageBox.Show(sb.ToString(), Properties.FusionExplorer.ABOUT_TITLE, MessageBoxButtons.OK);
        }

        private void btnCustomization_Click(object sender, EventArgs e)
        {
            Customization customization = new Customization();
            customization.Show();
        }

        private void imageRipperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageRipper form = new ImageRipper();
            form.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PixelArtGenerator form = new PixelArtGenerator();
            form.Show();
        }

        private void tvDirectoryDisplay_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tvDirectoryDisplay.SelectedNode = e.Node;

                if (e.Node.Tag is IArchiveNode node)
                {
                    if (node.IsDirectory)
                    {
                        directoryContextMenu.Show(tvDirectoryDisplay, e.Location);
                    }
                    else
                    {
                        fileContextMenu.Show(tvDirectoryDisplay, e.Location);
                    }
                }
            }
        }
    }
}
