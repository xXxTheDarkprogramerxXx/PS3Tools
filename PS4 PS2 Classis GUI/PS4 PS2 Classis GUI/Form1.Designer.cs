namespace PS4_PS2_Classis_GUI
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.changeBacgroToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreBackgroundToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.resotreIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblContentName = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openGP4ProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGP4ProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.logAnIssueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.addCustomPs2ConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblTask = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblPS2ID = new System.Windows.Forms.Label();
            this.txtTitleId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtContentID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnISO = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackgroundImage = global::PS4_PS2_Classis_GUI.Properties.Resources.pic1;
            this.splitContainer1.Panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel1.Controls.Add(this.lblContentName);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox2);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.progressBar1);
            this.splitContainer1.Panel2.Controls.Add(this.lblTask);
            this.splitContainer1.Panel2.Controls.Add(this.btnConvert);
            this.splitContainer1.Panel2.Controls.Add(this.lblPS2ID);
            this.splitContainer1.Panel2.Controls.Add(this.txtTitleId);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.txtContentID);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.btnISO);
            this.splitContainer1.Panel2.Controls.Add(this.txtPath);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1326, 407);
            this.splitContainer1.SplitterDistance = 717;
            this.splitContainer1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::PS4_PS2_Classis_GUI.Properties.Resources.icon0;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Location = new System.Drawing.Point(39, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(141, 124);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.Color.White;
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBacgroToolStripMenuItem,
            this.changeImageToolStripMenuItem,
            this.restoreBackgroundToolStripMenuItem1,
            this.resotreIconToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(212, 100);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // changeBacgroToolStripMenuItem
            // 
            this.changeBacgroToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.changeBacgroToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Silver;
            this.changeBacgroToolStripMenuItem.Name = "changeBacgroToolStripMenuItem";
            this.changeBacgroToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.changeBacgroToolStripMenuItem.Text = "Change Bacground ";
            this.changeBacgroToolStripMenuItem.Click += new System.EventHandler(this.changeBacgroToolStripMenuItem_Click);
            // 
            // changeImageToolStripMenuItem
            // 
            this.changeImageToolStripMenuItem.Name = "changeImageToolStripMenuItem";
            this.changeImageToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.changeImageToolStripMenuItem.Text = "Change Icon";
            this.changeImageToolStripMenuItem.Click += new System.EventHandler(this.changeImageToolStripMenuItem_Click);
            // 
            // restoreBackgroundToolStripMenuItem1
            // 
            this.restoreBackgroundToolStripMenuItem1.Name = "restoreBackgroundToolStripMenuItem1";
            this.restoreBackgroundToolStripMenuItem1.Size = new System.Drawing.Size(211, 24);
            this.restoreBackgroundToolStripMenuItem1.Text = "Restore Background";
            this.restoreBackgroundToolStripMenuItem1.Click += new System.EventHandler(this.restoreBackgroundToolStripMenuItem1_Click);
            // 
            // resotreIconToolStripMenuItem
            // 
            this.resotreIconToolStripMenuItem.Name = "resotreIconToolStripMenuItem";
            this.resotreIconToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.resotreIconToolStripMenuItem.Text = "Resotre Icon";
            this.resotreIconToolStripMenuItem.Click += new System.EventHandler(this.resotreIconToolStripMenuItem_Click);
            // 
            // lblContentName
            // 
            this.lblContentName.AutoSize = true;
            this.lblContentName.BackColor = System.Drawing.Color.Transparent;
            this.lblContentName.ForeColor = System.Drawing.Color.White;
            this.lblContentName.Location = new System.Drawing.Point(54, 191);
            this.lblContentName.Name = "lblContentName";
            this.lblContentName.Size = new System.Drawing.Size(35, 17);
            this.lblContentName.TabIndex = 0;
            this.lblContentName.Text = "Title";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::PS4_PS2_Classis_GUI.Properties.Resources.pic1;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(0, 28);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(717, 379);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(717, 28);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openGP4ProjectToolStripMenuItem,
            this.saveGP4ProjectToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.logAnIssueToolStripMenuItem,
            this.toolStripSeparator2,
            this.addCustomPs2ConfigToolStripMenuItem,
            this.creditsToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // openGP4ProjectToolStripMenuItem
            // 
            this.openGP4ProjectToolStripMenuItem.Name = "openGP4ProjectToolStripMenuItem";
            this.openGP4ProjectToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.openGP4ProjectToolStripMenuItem.Text = "Open GP4 Project";
            // 
            // saveGP4ProjectToolStripMenuItem
            // 
            this.saveGP4ProjectToolStripMenuItem.Name = "saveGP4ProjectToolStripMenuItem";
            this.saveGP4ProjectToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.saveGP4ProjectToolStripMenuItem.Text = "Save GP4 Project";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(236, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(239, 26);
            this.toolStripMenuItem1.Text = "Settings";
            // 
            // logAnIssueToolStripMenuItem
            // 
            this.logAnIssueToolStripMenuItem.Name = "logAnIssueToolStripMenuItem";
            this.logAnIssueToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.logAnIssueToolStripMenuItem.Text = "Log an issue";
            this.logAnIssueToolStripMenuItem.Click += new System.EventHandler(this.logAnIssueToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(236, 6);
            // 
            // addCustomPs2ConfigToolStripMenuItem
            // 
            this.addCustomPs2ConfigToolStripMenuItem.Name = "addCustomPs2ConfigToolStripMenuItem";
            this.addCustomPs2ConfigToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.addCustomPs2ConfigToolStripMenuItem.Text = "Add Custom ps2 config";
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(239, 26);
            this.creditsToolStripMenuItem.Text = "Credits";
            this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(20, 332);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(573, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 12;
            this.progressBar1.Visible = false;
            // 
            // lblTask
            // 
            this.lblTask.AutoSize = true;
            this.lblTask.Location = new System.Drawing.Point(20, 290);
            this.lblTask.Name = "lblTask";
            this.lblTask.Size = new System.Drawing.Size(61, 17);
            this.lblTask.TabIndex = 11;
            this.lblTask.Text = "Running";
            // 
            // btnConvert
            // 
            this.btnConvert.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnConvert.ImageIndex = 0;
            this.btnConvert.ImageList = this.imageList1;
            this.btnConvert.Location = new System.Drawing.Point(189, 201);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(245, 88);
            this.btnConvert.TabIndex = 10;
            this.btnConvert.Text = "Create PS2 Classic";
            this.btnConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "images.jpg");
            // 
            // lblPS2ID
            // 
            this.lblPS2ID.AutoSize = true;
            this.lblPS2ID.Location = new System.Drawing.Point(421, 74);
            this.lblPS2ID.Name = "lblPS2ID";
            this.lblPS2ID.Size = new System.Drawing.Size(59, 17);
            this.lblPS2ID.TabIndex = 9;
            this.lblPS2ID.Text = "PS2 ID :";
            // 
            // txtTitleId
            // 
            this.txtTitleId.Location = new System.Drawing.Point(17, 159);
            this.txtTitleId.Name = "txtTitleId";
            this.txtTitleId.Size = new System.Drawing.Size(532, 22);
            this.txtTitleId.TabIndex = 6;
            this.txtTitleId.TextChanged += new System.EventHandler(this.txtTitleId_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Title";
            // 
            // txtContentID
            // 
            this.txtContentID.Location = new System.Drawing.Point(17, 106);
            this.txtContentID.MaxLength = 9;
            this.txtContentID.Name = "txtContentID";
            this.txtContentID.Size = new System.Drawing.Size(532, 22);
            this.txtContentID.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Content ID";
            // 
            // btnISO
            // 
            this.btnISO.Location = new System.Drawing.Point(556, 32);
            this.btnISO.Name = "btnISO";
            this.btnISO.Size = new System.Drawing.Size(37, 23);
            this.btnISO.TabIndex = 2;
            this.btnISO.Text = "...";
            this.btnISO.UseVisualStyleBackColor = true;
            this.btnISO.Click += new System.EventHandler(this.btnISO_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(17, 34);
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(532, 22);
            this.txtPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "PS2 ISO:";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeBackgroundToolStripMenuItem,
            this.changeIconToolStripMenuItem,
            this.restoreBackgroundToolStripMenuItem,
            this.restoreIconToolStripMenuItem});
            this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // changeBackgroundToolStripMenuItem
            // 
            this.changeBackgroundToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.changeBackgroundToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.changeBackgroundToolStripMenuItem.Name = "changeBackgroundToolStripMenuItem";
            this.changeBackgroundToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.changeBackgroundToolStripMenuItem.Text = "Change Background";
            // 
            // changeIconToolStripMenuItem
            // 
            this.changeIconToolStripMenuItem.BackColor = System.Drawing.Color.White;
            this.changeIconToolStripMenuItem.ForeColor = System.Drawing.Color.Black;
            this.changeIconToolStripMenuItem.Name = "changeIconToolStripMenuItem";
            this.changeIconToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.changeIconToolStripMenuItem.Text = "Change Icon";
            // 
            // restoreBackgroundToolStripMenuItem
            // 
            this.restoreBackgroundToolStripMenuItem.Name = "restoreBackgroundToolStripMenuItem";
            this.restoreBackgroundToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.restoreBackgroundToolStripMenuItem.Text = "Restore Background";
            this.restoreBackgroundToolStripMenuItem.Click += new System.EventHandler(this.restoreBackgroundToolStripMenuItem_Click);
            // 
            // restoreIconToolStripMenuItem
            // 
            this.restoreIconToolStripMenuItem.Name = "restoreIconToolStripMenuItem";
            this.restoreIconToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.restoreIconToolStripMenuItem.Text = "Restore Icon";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 407);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PS2 Classic GUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblContentName;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnISO;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtContentID;
        private System.Windows.Forms.TextBox txtTitleId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPS2ID;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeIconToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem changeBacgroToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreBackgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreIconToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreBackgroundToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem resotreIconToolStripMenuItem;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblTask;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripMenuItem logAnIssueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openGP4ProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGP4ProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem addCustomPs2ConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem creditsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}

