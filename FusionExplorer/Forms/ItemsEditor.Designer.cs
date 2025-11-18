namespace FusionExplorer.Forms
{
    partial class ItemsEditor
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbHelmetSets = new System.Windows.Forms.ListBox();
            this.lbBottomSets = new System.Windows.Forms.ListBox();
            this.lbTopSets = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbRiderIcon = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbRiderId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRiderGenKey = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbRiderName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbRiders = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.itemsEditorStatusBar = new System.Windows.Forms.StatusStrip();
            this.btnOpenItemsXml = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnSaveItemsXml = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.itemsEditorStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1226, 627);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.lbHelmetSets);
            this.tabPage1.Controls.Add(this.lbBottomSets);
            this.tabPage1.Controls.Add(this.lbTopSets);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.lbRiders);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1218, 601);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Riders";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 289);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "rider";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(417, 286);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "helmet";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(291, 289);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "bottom";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(168, 289);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "top";
            // 
            // lbHelmetSets
            // 
            this.lbHelmetSets.FormattingEnabled = true;
            this.lbHelmetSets.Location = new System.Drawing.Point(420, 6);
            this.lbHelmetSets.Name = "lbHelmetSets";
            this.lbHelmetSets.Size = new System.Drawing.Size(120, 277);
            this.lbHelmetSets.TabIndex = 4;
            // 
            // lbBottomSets
            // 
            this.lbBottomSets.FormattingEnabled = true;
            this.lbBottomSets.Location = new System.Drawing.Point(294, 6);
            this.lbBottomSets.Name = "lbBottomSets";
            this.lbBottomSets.Size = new System.Drawing.Size(120, 277);
            this.lbBottomSets.TabIndex = 3;
            // 
            // lbTopSets
            // 
            this.lbTopSets.FormattingEnabled = true;
            this.lbTopSets.Location = new System.Drawing.Point(168, 6);
            this.lbTopSets.Name = "lbTopSets";
            this.lbTopSets.Size = new System.Drawing.Size(120, 277);
            this.lbTopSets.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbRiderIcon);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbRiderId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbRiderGenKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbRiderName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(546, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 122);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rider Info";
            // 
            // tbRiderIcon
            // 
            this.tbRiderIcon.Location = new System.Drawing.Point(55, 91);
            this.tbRiderIcon.Name = "tbRiderIcon";
            this.tbRiderIcon.Size = new System.Drawing.Size(231, 20);
            this.tbRiderIcon.TabIndex = 7;
            this.tbRiderIcon.TextChanged += new System.EventHandler(this.tbRiderIcon_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "icon";
            // 
            // tbRiderId
            // 
            this.tbRiderId.Location = new System.Drawing.Point(55, 65);
            this.tbRiderId.Name = "tbRiderId";
            this.tbRiderId.Size = new System.Drawing.Size(231, 20);
            this.tbRiderId.TabIndex = 5;
            this.tbRiderId.TextChanged += new System.EventHandler(this.tbRiderId_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "id";
            // 
            // tbRiderGenKey
            // 
            this.tbRiderGenKey.Location = new System.Drawing.Point(55, 39);
            this.tbRiderGenKey.Name = "tbRiderGenKey";
            this.tbRiderGenKey.Size = new System.Drawing.Size(231, 20);
            this.tbRiderGenKey.TabIndex = 3;
            this.tbRiderGenKey.TextChanged += new System.EventHandler(this.tbRiderGenKey_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "genKey";
            // 
            // tbRiderName
            // 
            this.tbRiderName.Location = new System.Drawing.Point(55, 13);
            this.tbRiderName.Name = "tbRiderName";
            this.tbRiderName.Size = new System.Drawing.Size(231, 20);
            this.tbRiderName.TabIndex = 1;
            this.tbRiderName.TextChanged += new System.EventHandler(this.tbRiderName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "name";
            // 
            // lbRiders
            // 
            this.lbRiders.FormattingEnabled = true;
            this.lbRiders.Location = new System.Drawing.Point(8, 6);
            this.lbRiders.Name = "lbRiders";
            this.lbRiders.Size = new System.Drawing.Size(154, 277);
            this.lbRiders.TabIndex = 0;
            this.lbRiders.SelectedIndexChanged += new System.EventHandler(this.lbRiders_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1218, 601);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Bikes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1218, 601);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Sets";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button2);
            this.tabPage4.Controls.Add(this.listBox2);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1218, 601);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Items";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label5);
            this.tabPage5.Controls.Add(this.button1);
            this.tabPage5.Controls.Add(this.listBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(1218, 601);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Changes";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(228, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "label5";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 377);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(219, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(219, 368);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // itemsEditorStatusBar
            // 
            this.itemsEditorStatusBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.itemsEditorStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOpenItemsXml,
            this.btnSaveItemsXml});
            this.itemsEditorStatusBar.Location = new System.Drawing.Point(0, 0);
            this.itemsEditorStatusBar.Name = "itemsEditorStatusBar";
            this.itemsEditorStatusBar.Size = new System.Drawing.Size(1226, 22);
            this.itemsEditorStatusBar.TabIndex = 3;
            this.itemsEditorStatusBar.Text = "statusStrip1";
            // 
            // btnOpenItemsXml
            // 
            this.btnOpenItemsXml.Name = "btnOpenItemsXml";
            this.btnOpenItemsXml.Size = new System.Drawing.Size(57, 17);
            this.btnOpenItemsXml.Text = "Open File";
            this.btnOpenItemsXml.Click += new System.EventHandler(this.btnOpenItemsXml_Click);
            // 
            // btnSaveItemsXml
            // 
            this.btnSaveItemsXml.Name = "btnSaveItemsXml";
            this.btnSaveItemsXml.Size = new System.Drawing.Size(31, 17);
            this.btnSaveItemsXml.Text = "Save";
            this.btnSaveItemsXml.Click += new System.EventHandler(this.btnSaveItemsXml_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(8, 53);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(493, 394);
            this.listBox2.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 453);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Refresh";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ItemsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1226, 652);
            this.Controls.Add(this.itemsEditorStatusBar);
            this.Controls.Add(this.tabControl1);
            this.Name = "ItemsEditor";
            this.Text = "ItemsEditor";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.itemsEditorStatusBar.ResumeLayout(false);
            this.itemsEditorStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip itemsEditorStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel btnOpenItemsXml;
        private System.Windows.Forms.ToolStripStatusLabel btnSaveItemsXml;
        private System.Windows.Forms.ListBox lbRiders;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbRiderIcon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbRiderId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRiderGenKey;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbRiderName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox lbHelmetSets;
        private System.Windows.Forms.ListBox lbBottomSets;
        private System.Windows.Forms.ListBox lbTopSets;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox2;
    }
}