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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
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
            this.splitContainer1.Panel2.Controls.Add(this.label4);
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
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Image = global::PS4_PS2_Classis_GUI.Properties.Resources.icon0;
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
            // 
            // changeImageToolStripMenuItem
            // 
            this.changeImageToolStripMenuItem.Name = "changeImageToolStripMenuItem";
            this.changeImageToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.changeImageToolStripMenuItem.Text = "Change Icon";
            // 
            // restoreBackgroundToolStripMenuItem1
            // 
            this.restoreBackgroundToolStripMenuItem1.Name = "restoreBackgroundToolStripMenuItem1";
            this.restoreBackgroundToolStripMenuItem1.Size = new System.Drawing.Size(211, 24);
            this.restoreBackgroundToolStripMenuItem1.Text = "Restore Background";
            // 
            // resotreIconToolStripMenuItem
            // 
            this.resotreIconToolStripMenuItem.Name = "resotreIconToolStripMenuItem";
            this.resotreIconToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.resotreIconToolStripMenuItem.Text = "Resotre Icon";
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
            this.pictureBox2.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = global::PS4_PS2_Classis_GUI.Properties.Resources.pic1;
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
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem1.Text = "File";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(20, 332);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(573, 23);
            this.progressBar1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 290);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Running";
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(192, 249);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(161, 23);
            this.btnConvert.TabIndex = 10;
            this.btnConvert.Text = "Create PS2 Classic";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
    }
}

