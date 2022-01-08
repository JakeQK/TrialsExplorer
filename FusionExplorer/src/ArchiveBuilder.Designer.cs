
namespace FusionExplorer
{
    partial class ArchiveBuilder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArchiveBuilder));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoadCompressionCSV = new System.Windows.Forms.Button();
            this.btnCreateCompressionCSV = new System.Windows.Forms.Button();
            this.lLoadedCSV = new System.Windows.Forms.Label();
            this.cbGameSelection = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBuildArchive = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.cbGameSelection);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(281, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Archive Builder Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLoadCompressionCSV);
            this.groupBox2.Controls.Add(this.btnCreateCompressionCSV);
            this.groupBox2.Controls.Add(this.lLoadedCSV);
            this.groupBox2.Location = new System.Drawing.Point(9, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 68);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Compression Values";
            // 
            // btnLoadCompressionCSV
            // 
            this.btnLoadCompressionCSV.Location = new System.Drawing.Point(6, 19);
            this.btnLoadCompressionCSV.Name = "btnLoadCompressionCSV";
            this.btnLoadCompressionCSV.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCompressionCSV.TabIndex = 3;
            this.btnLoadCompressionCSV.Text = "Load";
            this.btnLoadCompressionCSV.UseVisualStyleBackColor = true;
            this.btnLoadCompressionCSV.Click += new System.EventHandler(this.btnLoadCompressionCSV_Click);
            // 
            // btnCreateCompressionCSV
            // 
            this.btnCreateCompressionCSV.Location = new System.Drawing.Point(87, 19);
            this.btnCreateCompressionCSV.Name = "btnCreateCompressionCSV";
            this.btnCreateCompressionCSV.Size = new System.Drawing.Size(75, 23);
            this.btnCreateCompressionCSV.TabIndex = 4;
            this.btnCreateCompressionCSV.Text = "Create";
            this.btnCreateCompressionCSV.UseVisualStyleBackColor = true;
            this.btnCreateCompressionCSV.Click += new System.EventHandler(this.btnCreateCompressionCSV_Click);
            // 
            // lLoadedCSV
            // 
            this.lLoadedCSV.AutoSize = true;
            this.lLoadedCSV.Location = new System.Drawing.Point(3, 45);
            this.lLoadedCSV.Name = "lLoadedCSV";
            this.lLoadedCSV.Size = new System.Drawing.Size(73, 13);
            this.lLoadedCSV.TabIndex = 2;
            this.lLoadedCSV.Text = "Loaded CSV: ";
            // 
            // cbGameSelection
            // 
            this.cbGameSelection.FormattingEnabled = true;
            this.cbGameSelection.Items.AddRange(new object[] {
            "Trials Fusion",
            "Trials Evolution Gold Edition"});
            this.cbGameSelection.Location = new System.Drawing.Point(53, 22);
            this.cbGameSelection.Name = "cbGameSelection";
            this.cbGameSelection.Size = new System.Drawing.Size(212, 21);
            this.cbGameSelection.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Game:";
            // 
            // btnBuildArchive
            // 
            this.btnBuildArchive.Location = new System.Drawing.Point(12, 153);
            this.btnBuildArchive.Name = "btnBuildArchive";
            this.btnBuildArchive.Size = new System.Drawing.Size(281, 23);
            this.btnBuildArchive.TabIndex = 1;
            this.btnBuildArchive.Text = "Build Archive";
            this.btnBuildArchive.UseVisualStyleBackColor = true;
            this.btnBuildArchive.Click += new System.EventHandler(this.btnBuildArchive_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 189);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(304, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(45, 17);
            this.toolStripStatusLabel1.Text = "Status: ";
            // 
            // ArchiveBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 211);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnBuildArchive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ArchiveBuilder";
            this.Text = "Archive Builder";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnLoadCompressionCSV;
        private System.Windows.Forms.Button btnCreateCompressionCSV;
        private System.Windows.Forms.Label lLoadedCSV;
        private System.Windows.Forms.ComboBox cbGameSelection;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBuildArchive;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}