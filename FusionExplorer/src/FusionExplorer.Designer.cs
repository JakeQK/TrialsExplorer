namespace FusionExplorer {
    partial class FusionExplorer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FusionExplorer));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnArchiveBuilder = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTrackTool = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenObjectCollection = new System.Windows.Forms.ToolStripMenuItem();
            this.btnEditorFavouritesTool = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tvDirectoryDisplay = new System.Windows.Forms.TreeView();
            this.DirectoryDisplayImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnCustomization = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenFile,
            this.saveToolStripMenuItem,
            this.closeFileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.btnAbout});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 20);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(180, 22);
            this.btnOpenFile.Text = "Open";
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Enabled = false;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeFileToolStripMenuItem
            // 
            this.closeFileToolStripMenuItem.Enabled = false;
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeFileToolStripMenuItem.Text = "Close";
            this.closeFileToolStripMenuItem.Click += new System.EventHandler(this.closeFileToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnArchiveBuilder,
            this.btnTrackTool,
            this.btnOpenObjectCollection,
            this.btnEditorFavouritesTool,
            this.btnCustomization});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // btnArchiveBuilder
            // 
            this.btnArchiveBuilder.Name = "btnArchiveBuilder";
            this.btnArchiveBuilder.Size = new System.Drawing.Size(187, 22);
            this.btnArchiveBuilder.Text = "Archive Builder";
            this.btnArchiveBuilder.Click += new System.EventHandler(this.btnArchiveBuilder_Click);
            // 
            // btnTrackTool
            // 
            this.btnTrackTool.Name = "btnTrackTool";
            this.btnTrackTool.Size = new System.Drawing.Size(187, 22);
            this.btnTrackTool.Text = "Track Tool";
            this.btnTrackTool.Visible = false;
            this.btnTrackTool.Click += new System.EventHandler(this.btnTrackTool_Click);
            // 
            // btnOpenObjectCollection
            // 
            this.btnOpenObjectCollection.Name = "btnOpenObjectCollection";
            this.btnOpenObjectCollection.Size = new System.Drawing.Size(187, 22);
            this.btnOpenObjectCollection.Text = "Object Collection";
            this.btnOpenObjectCollection.Click += new System.EventHandler(this.btnOpenObjectCollection_Click);
            // 
            // btnEditorFavouritesTool
            // 
            this.btnEditorFavouritesTool.Name = "btnEditorFavouritesTool";
            this.btnEditorFavouritesTool.Size = new System.Drawing.Size(187, 22);
            this.btnEditorFavouritesTool.Text = "Editor Favourites Tool";
            this.btnEditorFavouritesTool.Click += new System.EventHandler(this.btnEditorFavouritesTool_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(180, 22);
            this.btnAbout.Text = "About";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // tvDirectoryDisplay
            // 
            this.tvDirectoryDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvDirectoryDisplay.ImageIndex = 0;
            this.tvDirectoryDisplay.ImageList = this.DirectoryDisplayImageList;
            this.tvDirectoryDisplay.Indent = 21;
            this.tvDirectoryDisplay.Location = new System.Drawing.Point(0, 22);
            this.tvDirectoryDisplay.Name = "tvDirectoryDisplay";
            this.tvDirectoryDisplay.SelectedImageIndex = 0;
            this.tvDirectoryDisplay.Size = new System.Drawing.Size(800, 428);
            this.tvDirectoryDisplay.TabIndex = 2;
            this.tvDirectoryDisplay.DoubleClick += new System.EventHandler(this.tvDirectoryDisplay_DoubleClick);
            // 
            // DirectoryDisplayImageList
            // 
            this.DirectoryDisplayImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DirectoryDisplayImageList.ImageStream")));
            this.DirectoryDisplayImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.DirectoryDisplayImageList.Images.SetKeyName(0, "folder.png");
            this.DirectoryDisplayImageList.Images.SetKeyName(1, "icon.png");
            // 
            // btnCustomization
            // 
            this.btnCustomization.Name = "btnCustomization";
            this.btnCustomization.Size = new System.Drawing.Size(187, 22);
            this.btnCustomization.Text = "Customization";
            this.btnCustomization.Click += new System.EventHandler(this.btnCustomization_Click);
            // 
            // FusionExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tvDirectoryDisplay);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FusionExplorer";
            this.Text = "Fusion Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem btnOpenFile;
        private System.Windows.Forms.TreeView tvDirectoryDisplay;
        private System.Windows.Forms.ImageList DirectoryDisplayImageList;
        private System.Windows.Forms.ToolStripMenuItem closeFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnArchiveBuilder;
        private System.Windows.Forms.ToolStripMenuItem btnTrackTool;
        private System.Windows.Forms.ToolStripMenuItem btnOpenObjectCollection;
        private System.Windows.Forms.ToolStripMenuItem btnEditorFavouritesTool;
        private System.Windows.Forms.ToolStripMenuItem btnAbout;
        private System.Windows.Forms.ToolStripMenuItem btnCustomization;
    }
}

