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

namespace FusionExplorer 
{
    public partial class FusionExplorer : System.Windows.Forms.Form
    {
        private static string filename;
        private static string safe_filename;

        private static int data_offset;
        private static int file_count;

        private static byte[] pak_data;
        private static List<ArchiveFile> archive_files = new List<ArchiveFile>();

        ContextMenu texture_contextmenu = new ContextMenu();
        ContextMenu item_contextmenu = new ContextMenu();
        ContextMenu folder_contextmenu = new ContextMenu();

        bool changesMade = false;

        public FusionExplorer() {
            InitializeComponent();

            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_VIEW, new EventHandler(view_texture));
            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_EXPORT, new EventHandler(export_file));
            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_EXPORT_DDS, new EventHandler(export_dds));
            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_IMPORT, new EventHandler(import_file));
            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_IMPORT_DDS, new EventHandler(import_dds));
            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_INFO, new EventHandler(get_file_info));
            texture_contextmenu.MenuItems.Add(Properties.FusionExplorer.TEXTURE_CONTEXTMENU_HXD, new EventHandler(open_with_hxd));
            item_contextmenu.MenuItems.Add(Properties.FusionExplorer.ITEM_CONTEXTMENU_EXPORT, new EventHandler(export_file));
            item_contextmenu.MenuItems.Add(Properties.FusionExplorer.ITEM_CONTEXTMENU_IMPORT, new EventHandler(import_file));
            item_contextmenu.MenuItems.Add(Properties.FusionExplorer.ITEM_CONTEXTMENU_INFO, new EventHandler(get_file_info));
            item_contextmenu.MenuItems.Add(Properties.FusionExplorer.ITEM_CONTEXTMENU_HXD, new EventHandler(open_with_hxd));

            folder_contextmenu.MenuItems.Add(Properties.FusionExplorer.FOLDER_CONTEXTMENU_EXPORT, new EventHandler(export_folder));
            folder_contextmenu.MenuItems.Add("Add new file", new EventHandler(add_new_file));
            folder_contextmenu.MenuItems.Add("Add new DDS", new EventHandler(add_new_dds));
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pak files (*.pak)|*.pak|All files (*.*)|*.*";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                filename = ofd.FileName;
                safe_filename = ofd.SafeFileName;
                pak_data = File.ReadAllBytes(ofd.FileName);
                parse_header();
                parse_file_entries();
                get_file_names();
                populate_directory_display();
                toolStripStatusLabel1.Text = filename;

                closeFileToolStripMenuItem.Enabled = true;
            }
        }

        private void parse_header()
        {
            using (MemoryStream ms = new MemoryStream(pak_data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    if (br.ReadInt32() == 305419896)
                    {
                        data_offset = br.ReadInt32();
                        file_count = br.ReadInt32();
                    }
                }
            }
        }

        private void parse_file_entries()
        {
            archive_files.Clear();
            using(MemoryStream ms = new MemoryStream(pak_data))
            {
                using(BinaryReader br = new BinaryReader(ms))
                {
                    br.BaseStream.Seek(12, SeekOrigin.Begin);
                    for (int i = 0; i < file_count; i++)
                    {
                        ArchiveFile af = new ArchiveFile();
                        long file_entry_offset = br.BaseStream.Position;
                        af.fileEntry = new FileEntry(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadByte(), br.ReadInt32(), file_entry_offset);
                        archive_files.Add(af);
                    }
                }
            }
        }

        private void get_file_names()
        {
            try
            {

                using (MemoryStream ms = new MemoryStream(pak_data))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        for(int i = file_count - 1; i != 0; i--)
                        {
                            if (archive_files[i].fileEntry.ID == -576875544)
                            {
                                archive_files[i].filename = "filenames";
                                br.BaseStream.Seek(archive_files[i].fileEntry.dataOffset, SeekOrigin.Begin);
                                br.ReadInt32();
                                for (int j = 0; j < file_count; j++)
                                {
                                    if (archive_files[j].fileEntry.ID != -576875544)
                                    { 
                                        Int16 strlen = br.ReadInt16();
                                        string str = new string(br.ReadChars(strlen));
                                        archive_files[j].filename = str;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void populate_directory_display()
        {
            tvDirectoryDisplay.Nodes.Clear();

            TreeNode root = new TreeNode(safe_filename);
            root.ContextMenu = folder_contextmenu;

            TreeNode current_node = root;
            for(int i = 0; i < archive_files.Count; i++)
            {
                if (archive_files[i].fileEntry.ID != -576875544)
                {
                    string[] path_split = archive_files[i].filename.Split('/').ToArray();
                    current_node = root;

                    foreach (string split in path_split)
                    {
                        bool found = false;
                        // looks if node exists 
                        foreach (TreeNode node in current_node.Nodes)
                        {
                            if (node.Text == split)
                            {
                                found = true;
                                current_node = node;
                                break;
                            }
                        }

                        // if node wasn't found, creates it
                        if (!found)
                        {
                            TreeNode node = new TreeNode(split);
                            if (split.Contains('.'))
                            {
                                //node.Text += archive_files[i].file_entry.Compression_Flag.ToString();
                                node.ImageIndex = 1;
                                node.SelectedImageIndex = 1;
                                if (split.Contains("png.tex"))
                                    node.ContextMenu = texture_contextmenu;
                                else
                                    node.ContextMenu = item_contextmenu;
                                archive_files[i].tree_node = node;
                            }
                            else
                            {
                                node.ContextMenu = folder_contextmenu;
                            }
                            current_node.Nodes.Add(node);
                            current_node = node;
                        }
                    }
                }
            }

            tvDirectoryDisplay.Nodes.Add(root);
            tvDirectoryDisplay.TreeViewNodeSorter = new NodeSorter();
            tvDirectoryDisplay.Sort();
        }

        


        /* Export File
         * Exports individual files from archive to exports folder
         */
        private void export_file(object sender, EventArgs e)
        {
            // Get the currently selected file from the Directory Display
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
                                bw.Write(br.ReadBytes(selected_archive_file.fileEntry.decompressedSize));
                                break;
                            // zlib Compressed
                            case 1:
                                bw.Write(ZlibStream.UncompressBuffer(br.ReadBytes(selected_archive_file.fileEntry.compressedSize)));
                                //bw.Write(br.ReadBytes(selected_archive_file.file_entry.Compressed_Size));
                                break;
                            // Trials Evolution (Xbox 360) compressed
                            case 4:
                                MessageBox.Show("Trials Evolution: Xbox 360 compression unsupported", "Fusion Explorer");
                                break;
                            // Trials Evolution: Gold Edition zlib compressed
                            case 8:
                                bw.Write(ZlibStream.UncompressBuffer(br.ReadBytes(selected_archive_file.fileEntry.compressedSize)));
                                break;
                            // Unknown Case (Found in data_patch.pak)
                            case 16:
                                //bw.Write(DeflateStream.UncompressBuffer(br.ReadBytes(selected_archive_file.file_entry.Decompressed_Size)));
                                bw.Write(br.ReadBytes(selected_archive_file.fileEntry.compressedSize));
                                break;
                            // Unknown Case (Found in data_patch.pak)
                            case 17:
                                //bw.Write(DeflateStream.UncompressBuffer(br.ReadBytes(selected_archive_file.file_entry.Decompressed_Size)));
                                bw.Write(br.ReadBytes(selected_archive_file.fileEntry.compressedSize));
                                break;
                        }
                    }
                }
            }
        }

        /* Export folder setup 
         * Creates a thread to export all files from within the selected folder and subdirectories
         * Tracks the progress of the thread and reports it to the toolStripStatusLabel1 */
        private async void export_folder(object sender, EventArgs e)
        {
            TreeNode n = tvDirectoryDisplay.SelectedNode;
            var progress = new Progress<string>(s => toolStripStatusLabel1.Text = s);
            await Task.Factory.StartNew(() => export_folder(progress, n), TaskCreationOptions.LongRunning);
            toolStripStatusLabel1.Text = filename;
        }

        /* Export folder function
         * Recursively searches through all subdirectories of the selected folder
         * And exports all the files to their appropriate directory
         */
        private static void export_folder(IProgress<string> progress, TreeNode node)
        {
            foreach(TreeNode n in node.Nodes)
            {
                if(n.Text.Contains('.'))
                {
                    foreach(ArchiveFile af in archive_files)
                    {
                        if(af.tree_node == n)
                        {
                            using (MemoryStream ms = new MemoryStream(pak_data))
                            {
                                using (BinaryReader br = new BinaryReader(ms))
                                {
                                    // Goto data offset of selected file
                                    br.BaseStream.Seek(af.fileEntry.dataOffset, SeekOrigin.Begin);

                                    // Obtain Path and Filename as seperate strings (ex: "/localization/", "evo1_chinese.xml")
                                    string[] split_filename = filename_split(af.filename);

                                    // Create a directory for exported file if it doesn't exist
                                    string dir = AppDomain.CurrentDomain.BaseDirectory + "Exports/" + safe_filename + "/" + split_filename[0];
                                    if (!Directory.Exists(dir))
                                        Directory.CreateDirectory(dir);

                                    // Create the file being exported and setup BinaryWriter to write the data
                                    using (BinaryWriter bw = new BinaryWriter(File.Create(dir + split_filename[1])))
                                    {
                                        switch (af.fileEntry.compressionFlag)
                                        {
                                            // Uncompressed
                                            case 0:
                                                bw.Write(br.ReadBytes(af.fileEntry.decompressedSize));
                                                break;
                                            // zlib Compressed
                                            case 1:
                                                bw.Write(ZlibStream.UncompressBuffer(br.ReadBytes(af.fileEntry.compressedSize)));
                                                break;
                                            // Trials Evoultion lzx compressed
                                            case 4:
                                                break;
                                            // Trials Evolution zlib compressed
                                            case 8:
                                                bw.Write(ZlibStream.UncompressBuffer(br.ReadBytes(af.fileEntry.compressedSize)));
                                                break;
                                            // Unknown Case (Found in data_patch.pak)
                                            case 16:
                                                bw.Write(br.ReadBytes(af.fileEntry.compressedSize));
                                                break;
                                            // Unknown Case (Found in data_patch.pak)
                                            case 17:
                                                bw.Write(br.ReadBytes(af.fileEntry.compressedSize));
                                                break;
                                        }
                                    }

                                    progress.Report(split_filename[1]);
                                }
                            }
                            break;
                        }
                    }
                }
                else
                {
                    export_folder(progress, n);
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

        private void tvDirectoryDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = tvDirectoryDisplay.GetNodeAt(e.X, e.Y);

                if(node != null)
                {
                    tvDirectoryDisplay.SelectedNode = node;
                }
            }
        }

        ArchiveService service = new ArchiveService();

        private void experimentalOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pak files (*.pak)|*.pak|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                /*filename = ofd.FileName;
                safe_filename = ofd.SafeFileName;
                pak_data = File.ReadAllBytes(ofd.FileName);
                parse_header();
                parse_file_entries();
                get_file_names();
                populate_directory_display();
                toolStripStatusLabel1.Text = filename;

                closeFileToolStripMenuItem.Enabled = true;*/

                service.OpenArchive(ofd.FileName);
                experimentalPopulateTreeView();
            }
        }

        private void experimentalPopulateTreeView()
        {
            tvDirectoryDisplay.Nodes.Clear();

            ArchiveDirectory rootDirectory = service.GetRootDirectory();

            if(rootDirectory != null )
            {
                TreeNode rootNode = new TreeNode(rootDirectory.Name);
                rootNode.Tag = rootDirectory;
                tvDirectoryDisplay.Nodes.Add(rootNode);

                AddChildNodes(rootNode, rootDirectory);

                rootNode.Expand();
            }

        }

        private void AddChildNodes(TreeNode parentNode, ArchiveDirectory directory)
        {
            foreach(IArchiveNode node in directory.Children)
            {
                TreeNode childNode = new TreeNode(node.Name);
                childNode.Tag = node;

                childNode.ImageIndex = node.IsDirectory ? 0 : 1;
                childNode.SelectedImageIndex = childNode.ImageIndex;

                parentNode.Nodes.Add(childNode);

                if (node.IsDirectory)
                {
                    AddChildNodes(childNode, (ArchiveDirectory)node);
                }
                else
                {
                    childNode.Text = ((Models.ArchiveFile)node).SafeFilename;
                }
            }
        }
    }
}
