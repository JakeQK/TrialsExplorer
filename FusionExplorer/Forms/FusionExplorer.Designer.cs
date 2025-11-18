using System.Windows.Forms;
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
            this.btnEditorFavouritesTool = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tvDirectoryDisplay = new System.Windows.Forms.TreeView();
            this.DirectoryDisplayImageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.ProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.mDLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2});
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
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
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Name = "btnOpenFile";
            resources.ApplyResources(this.btnOpenFile, "btnOpenFile");
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // saveToolStripMenuItem
            // 
            resources.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // closeFileToolStripMenuItem
            // 
            resources.ApplyResources(this.closeFileToolStripMenuItem, "closeFileToolStripMenuItem");
            this.closeFileToolStripMenuItem.Name = "closeFileToolStripMenuItem";
            this.closeFileToolStripMenuItem.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnEditorFavouritesTool,
            this.toolStripMenuItem2,
            this.mDLToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            resources.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
            // 
            // btnEditorFavouritesTool
            // 
            this.btnEditorFavouritesTool.Name = "btnEditorFavouritesTool";
            resources.ApplyResources(this.btnEditorFavouritesTool, "btnEditorFavouritesTool");
            this.btnEditorFavouritesTool.Click += new System.EventHandler(this.btnEditorFavouritesTool_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            resources.ApplyResources(this.btnAbout, "btnAbout");
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripMenuItem1});
            resources.ApplyResources(this.toolStripDropDownButton2, "toolStripDropDownButton2");
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            // 
            // toolStripTextBox1
            // 
            resources.ApplyResources(this.toolStripTextBox1, "toolStripTextBox1");
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Click += new System.EventHandler(this.toolStripTextBox1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // tvDirectoryDisplay
            // 
            resources.ApplyResources(this.tvDirectoryDisplay, "tvDirectoryDisplay");
            this.tvDirectoryDisplay.ImageList = this.DirectoryDisplayImageList;
            this.tvDirectoryDisplay.Name = "tvDirectoryDisplay";
            this.tvDirectoryDisplay.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvDirectoryDisplay_NodeMouseClick);
            this.tvDirectoryDisplay.DoubleClick += new System.EventHandler(this.archiveTreeView_DoubleClick);
            // 
            // DirectoryDisplayImageList
            // 
            this.DirectoryDisplayImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DirectoryDisplayImageList.ImageStream")));
            this.DirectoryDisplayImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.DirectoryDisplayImageList.Images.SetKeyName(0, "folder.png");
            this.DirectoryDisplayImageList.Images.SetKeyName(1, "icon.png");
            // 
            // statusStrip2
            // 
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgressBar1,
            this.StatusLabel1,
            this.StatusLabel2});
            resources.ApplyResources(this.statusStrip2, "statusStrip2");
            this.statusStrip2.Name = "statusStrip2";
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Name = "ProgressBar1";
            resources.ApplyResources(this.ProgressBar1, "ProgressBar1");
            this.ProgressBar1.Step = 1;
            // 
            // StatusLabel1
            // 
            this.StatusLabel1.Name = "StatusLabel1";
            resources.ApplyResources(this.StatusLabel1, "StatusLabel1");
            // 
            // StatusLabel2
            // 
            this.StatusLabel2.Name = "StatusLabel2";
            resources.ApplyResources(this.StatusLabel2, "StatusLabel2");
            // 
            // mDLToolStripMenuItem
            // 
            this.mDLToolStripMenuItem.Name = "mDLToolStripMenuItem";
            resources.ApplyResources(this.mDLToolStripMenuItem, "mDLToolStripMenuItem");
            // 
            // FusionExplorer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip2);
            this.Controls.Add(this.tvDirectoryDisplay);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FusionExplorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FusionExplorer_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FusionExplorer_KeyDown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnEditorFavouritesTool;
        private System.Windows.Forms.ToolStripMenuItem btnAbout;
        private StatusStrip statusStrip2;
        private ToolStripStatusLabel StatusLabel1;
        private ToolStripStatusLabel StatusLabel2;
        private ToolStripProgressBar ProgressBar1;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem mDLToolStripMenuItem;
    }
}

