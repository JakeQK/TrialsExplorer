using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using FusionExplorer.src;
using FusionExplorer.Services;
using FusionExplorer.Models;
using Microsoft.WindowsAPICodePack.Dialogs;
using static FusionExplorer.Services.ArchiveService;
using FusionExplorer.Forms;
using System.Text;

namespace FusionExplorer 
{
    public partial class FusionExplorer : Form
    {
        private string app_version = "2.0";

        private ArchiveService service;

        private ContextMenuStrip fileContextMenu;
        private ContextMenuStrip directoryContextMenu;

        public FusionExplorer() 
        {
            InitializeComponent();

            InitializeArchiveService();

            InitializeContextMenus();
        }

        private void InitializeArchiveService()
        {
            service = new ArchiveService();
            service.DirtyStateChanged += Service_DirtyStateChanged;
            service.ArchiveLoaded += Service_ArchiveLoaded;
            service.ArchiveUnloaded += Service_ArchiveUnloaded;
            service.ArchiveSaved += Service_ArchiveSaved;
        }

        private void InitializeContextMenus()
        {
            fileContextMenu = new ContextMenuStrip();
            fileContextMenu.Items.Add("Quick extract", null, QuickExtractFile_Click);
            fileContextMenu.Items.Add("Extract to selected path", null, ExtractFileToSelectedPath_Click);
            fileContextMenu.Items.Add("Replace", null, ReplaceFile_Click);
            fileContextMenu.Items.Add(new ToolStripSeparator());
            fileContextMenu.Items.Add("Properties", null, DisplayProperties_Click);

            directoryContextMenu = new ContextMenuStrip();
            directoryContextMenu.Items.Add("Quick extract directory", null, QuickExtractDirectory_Click);
            directoryContextMenu.Items.Add("Extract directory to selected path", null, ExtractDirectoryToSelectedPath_Click);
        }

        private void Service_DirtyStateChanged(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = service.IsDirty;
        }

        private void Service_ArchiveLoaded(object sender, EventArgs e)
        {
            FusionExplorer.ActiveForm.Text = $"Fusion Explorer | {service.CurrentFilePath}";
            closeFileToolStripMenuItem.Enabled = service.IsArchiveLoaded;
        }

        private void Service_ArchiveUnloaded(object sender, EventArgs e)
        {
            FusionExplorer.ActiveForm.Text = $"Fusion Explorer";
            closeFileToolStripMenuItem.Enabled = service.IsArchiveLoaded;
        }

        private void Service_ArchiveSaved(object sender, EventArgs e)
        {
            saveToolStripMenuItem.Enabled = false;
        }

        private void Service_ExtractionProgressChanged(object sender, ExtractionProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateProgressUI(e)));
            }
            else
            {
                UpdateProgressUI(e);
            }
        }

        private void UpdateProgressUI(ExtractionProgressEventArgs e)
        {
            ProgressBar1.Value = (int)e.ProgressPercentage;
            StatusLabel1.Text = $"{e.ProcessedFiles} of {e.TotalFiles}";
            StatusLabel2.Text = $"Current file: {Path.GetFileName(e.CurrentFileName)}";
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pak files (*.pak)|*.pak|All files (*.*)|*.*";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                service.OpenArchive(ofd.FileName);
                experimentalPopulateTreeView();

                FusionExplorer.ActiveForm.Text = $"Fusion Explorer | {ofd.SafeFileName}";
                closeFileToolStripMenuItem.Enabled = true;
            }
        }

        private void experimentalPopulateTreeView()
        {
            tvDirectoryDisplay.Nodes.Clear();

            ArchiveDirectory rootDirectory = service.GetRootDirectory;

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

                    //childNode.Text = fileNode.SafeFilename;

                    childNode.Text =  $"{fileNode.SafeFilename} {fileNode.ArchiveFileEntry.CompressionFlag}";
                }

                parentNode.Nodes.Add(childNode);
            }
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

        private async void QuickExtractDirectory_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveDirectory directory)
            {
                ProgressBar1.Value = 0;
                ProgressBar1.Visible = true;

                service.ExtractionProgress -= Service_ExtractionProgressChanged;
                service.ExtractionProgress += Service_ExtractionProgressChanged;

                await service.QuickExtractDirectoryAsync(directory);

                ProgressBar1.Visible = false;

                StatusLabel2.Text = "";
                StatusLabel1.Text = "Extraction completed successfully";
            }
        }

        private async void ExtractDirectoryToSelectedPath_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveDirectory directory)
            {
                service.ExtractionProgress -= Service_ExtractionProgressChanged;
                service.ExtractionProgress += Service_ExtractionProgressChanged;

                await service.ExtractDirectoryToSelectedPathAsync(directory);
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

        private bool SaveChanges()
        {
            try
            {
                if (service.IsDirty & service.IsArchiveLoaded)
                {
                    DialogResult res = MessageBox.Show("Are you sure you want to save changes?\n\nChanges made are irreversible, make sure you have a backup.", "Trials Explorer", MessageBoxButtons.OKCancel);
                    if (res == DialogResult.OK)
                    {
                        if (service.SaveArchive())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save changes: {ex.Message}");

                return false;
            }
        }

        private void DisplayProperties_Click(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveFile file)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"ID: {file.ArchiveFileEntry.Id}");
                sb.AppendLine($"Compression Flag: {file.ArchiveFileEntry.CompressionFlag}");
                sb.AppendLine($"Size (Compressed): {file.ArchiveFileEntry.CompressedSize}");
                sb.AppendLine($"Size (Decompressed): {file.ArchiveFileEntry.DecompressedSize}");
                sb.AppendLine($"Offset: 0x{file.ArchiveFileEntry.OffsetToData.ToString("X")}");

                MessageBox.Show(sb.ToString());
            }
        }

        /*private void view_texture(object sender, EventArgs e)
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

        private void export_dds(object sender, EventArgs e)
        {
            if (tvDirectoryDisplay.SelectedNode?.Tag is Models.ArchiveFile file)
            {

                byte[] tex = service.ExtractFile(file);
                byte[] dds = Texture.TEXToDDS(tex);

                string filename = service
                string safeFilename = filename.Remove(0, filename.LastIndexOf("/") + 1);
                safeFilename = safeFilename.Remove(safeFilename.IndexOf("."), safeFilename.Length - safeFilename.IndexOf("."));

                VistaSaveFileDialog sfd = new VistaSaveFileDialog();
                sfd.Filter = "dds image (*.dds)|*.dds|all files (*.*)|*.*";
                sfd.FileName = safeFilename + ".dds";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (BinaryWriter bw = new BinaryWriter(File.Create(sfd.FileName)))
                    {
                        bw.Write(dds);
                    }
                }
            }
        }

        private void import_dds(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ArchiveFile selected_file = get_selected_archive_file();
                byte[] data = File.ReadAllBytes(ofd.FileName);

                byte[] tex = get_file_data();
                Texture.TextureVersion textureVersion = Texture.TextureVersion.T5X;
                switch (tex[1])
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
        }*/

        

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (service.IsArchiveLoaded)
            {
                tvDirectoryDisplay.Nodes.Clear();
                service.CloseArchive();
            }
        }

        private void archiveTreeView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void FusionExplorer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (service.IsDirty & service.IsArchiveLoaded)
            {
                DialogResult result = MessageBox.Show("Do you want to save changes to " + service.CurrentFilePath, "Fusion Explorer", MessageBoxButtons.YesNoCancel);
                switch(result)
                {
                    case DialogResult.Yes:
                        service.SaveArchive();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
            else if (service.IsArchiveLoaded)
            {
                e.Cancel = MessageBox.Show("Are you sure you want to exit?", "Fusion Explorer", MessageBoxButtons.YesNo) == DialogResult.No;
            }
        }

        private void FusionExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control && e.KeyCode == Keys.S)
            {
                SaveChanges();
            }
        }

        private void btnEditorFavouritesTool_Click(object sender, EventArgs e)
        {
            EditorFavouritesTool _editorFavouritesTool = new EditorFavouritesTool();
            _editorFavouritesTool.Show();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Version: {app_version}\nCreated by: JakeQK (formerly Raon Hook)", "About", MessageBoxButtons.OK);
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (service.IsArchiveLoaded)
            {
                string search = toolStripTextBox1.Text;

                string filename = "";

                foreach (var file in service.Files)
                {
                    if (file.ArchiveFileEntry.Id == int.Parse(search))
                        filename = file.SafeFilename;
                }

                MessageBox.Show(filename);
            }
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ItemsEditor _itemsEditor = new ItemsEditor();
            _itemsEditor.Show();
        }
    }
}
