using Microsoft.Extensions.Localization;

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
            this.btnCustomization = new System.Windows.Forms.ToolStripMenuItem();
            this.imageRipperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tvDirectoryDisplay = new System.Windows.Forms.TreeView();
            this.DirectoryDisplayImageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Name = "statusStrip1";
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
            resources.ApplyResources(this.toolStripDropDownButton1, "toolStripDropDownButton1");
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Text = global::FusionExplorer.Properties.FusionExplorer.DROP_DOWN_BUTTON_FILE;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Name = "btnOpenFile";
            resources.ApplyResources(this.btnOpenFile, "btnOpenFile");
            this.btnOpenFile.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_OPEN;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // saveToolStripMenuItem
            // 
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_SAVE;
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // closeFileToolStripMenuItem
            // 
            resources.ApplyResources(this.closeFileToolStripMenuItem, "closeFileToolStripMenuItem");
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_CLOSE;
            this.closeFileToolStripMenuItem.Click += new System.EventHandler(this.closeFileToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnArchiveBuilder,
            this.btnTrackTool,
            this.btnOpenObjectCollection,
            this.btnEditorFavouritesTool,
            this.btnCustomization,
            this.imageRipperToolStripMenuItem,
            this.toolStripMenuItem1});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            this.toolsToolStripMenuItem.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_TOOLS;
            // 
            // btnArchiveBuilder
            // 
            this.btnArchiveBuilder.Name = "btnArchiveBuilder";
            resources.ApplyResources(this.btnArchiveBuilder, "btnArchiveBuilder");
            this.btnArchiveBuilder.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_TOOLS_ARCHIVE_BUILDER;
            this.btnArchiveBuilder.Click += new System.EventHandler(this.btnArchiveBuilder_Click);
            // 
            // btnTrackTool
            // 
            this.btnTrackTool.Name = "btnTrackTool";
            resources.ApplyResources(this.btnTrackTool, "btnTrackTool");
            this.btnTrackTool.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_TOOLS_TRACK_TOOL;
            this.btnTrackTool.Click += new System.EventHandler(this.btnTrackTool_Click);
            // 
            // btnOpenObjectCollection
            // 
            this.btnOpenObjectCollection.Name = "btnOpenObjectCollection";
            resources.ApplyResources(this.btnOpenObjectCollection, "btnOpenObjectCollection");
            this.btnOpenObjectCollection.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_TOOLS_OBJECT_COLLECTION;
            this.btnOpenObjectCollection.Click += new System.EventHandler(this.btnOpenObjectCollection_Click);
            // 
            // btnEditorFavouritesTool
            // 
            this.btnEditorFavouritesTool.Name = "btnEditorFavouritesTool";
            resources.ApplyResources(this.btnEditorFavouritesTool, "btnEditorFavouritesTool");
            this.btnEditorFavouritesTool.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_TOOLS_FAVOURITES_EDITOR;
            this.btnEditorFavouritesTool.Click += new System.EventHandler(this.btnEditorFavouritesTool_Click);
            // 
            // btnCustomization
            // 
            this.btnCustomization.Name = "btnCustomization";
            resources.ApplyResources(this.btnCustomization, "btnCustomization");
            this.btnCustomization.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_TOOLS_CUSTOMIZATION;
            this.btnCustomization.Click += new System.EventHandler(this.btnCustomization_Click);
            // 
            // imageRipperToolStripMenuItem
            // 
            this.imageRipperToolStripMenuItem.Name = "imageRipperToolStripMenuItem";
            resources.ApplyResources(this.imageRipperToolStripMenuItem, "imageRipperToolStripMenuItem");
            this.imageRipperToolStripMenuItem.Click += new System.EventHandler(this.imageRipperToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Text = global::FusionExplorer.Properties.FusionExplorer.FILE_ABOUT;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
            // 
            // tvDirectoryDisplay
            // 
            resources.ApplyResources(this.tvDirectoryDisplay, "tvDirectoryDisplay");
            this.tvDirectoryDisplay.ImageList = this.DirectoryDisplayImageList;
            this.tvDirectoryDisplay.Name = "tvDirectoryDisplay";
            this.tvDirectoryDisplay.DoubleClick += new System.EventHandler(this.tvDirectoryDisplay_DoubleClick);
            // 
            // DirectoryDisplayImageList
            // 
            this.DirectoryDisplayImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DirectoryDisplayImageList.ImageStream")));
            this.DirectoryDisplayImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.DirectoryDisplayImageList.Images.SetKeyName(0, "folder.png");
            this.DirectoryDisplayImageList.Images.SetKeyName(1, "icon.png");
            // 
            // FusionExplorer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tvDirectoryDisplay);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FusionExplorer";
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
        private System.Windows.Forms.ToolStripMenuItem imageRipperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}

