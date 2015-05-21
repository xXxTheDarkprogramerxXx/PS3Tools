namespace PARAM.SFO_Editor
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbVersion = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.gbAdvanced = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblkey_1 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbldata_1_offset = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbldata_1_max_len = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbldata_1_len = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbldata_1_fmt = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblkey_1_offset = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblTE = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDTS = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblKTS = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gbAdvanced.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Version:";
            // 
            // cbVersion
            // 
            this.cbVersion.FormattingEnabled = true;
            this.cbVersion.Location = new System.Drawing.Point(77, 84);
            this.cbVersion.Name = "cbVersion";
            this.cbVersion.Size = new System.Drawing.Size(91, 21);
            this.cbVersion.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(660, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Advanced";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // gbAdvanced
            // 
            this.gbAdvanced.Controls.Add(this.groupBox4);
            this.gbAdvanced.Controls.Add(this.groupBox3);
            this.gbAdvanced.Controls.Add(this.groupBox2);
            this.gbAdvanced.Location = new System.Drawing.Point(611, 89);
            this.gbAdvanced.Name = "gbAdvanced";
            this.gbAdvanced.Size = new System.Drawing.Size(200, 320);
            this.gbAdvanced.TabIndex = 5;
            this.gbAdvanced.TabStop = false;
            this.gbAdvanced.Text = "Advanced";
            this.gbAdvanced.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblkey_1);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Location = new System.Drawing.Point(6, 216);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(188, 68);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "key_table";
            // 
            // lblkey_1
            // 
            this.lblkey_1.AutoSize = true;
            this.lblkey_1.Location = new System.Drawing.Point(36, 36);
            this.lblkey_1.Name = "lblkey_1";
            this.lblkey_1.Size = new System.Drawing.Size(0, 13);
            this.lblkey_1.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(32, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 16);
            this.label11.TabIndex = 15;
            this.label11.Text = "key_1:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbldata_1_offset);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.lbldata_1_max_len);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lbldata_1_len);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.lbldata_1_fmt);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lblkey_1_offset);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Location = new System.Drawing.Point(6, 106);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 104);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "index_table";
            // 
            // lbldata_1_offset
            // 
            this.lbldata_1_offset.AutoSize = true;
            this.lbldata_1_offset.Location = new System.Drawing.Point(156, 82);
            this.lbldata_1_offset.Name = "lbldata_1_offset";
            this.lbldata_1_offset.Size = new System.Drawing.Size(0, 13);
            this.lbldata_1_offset.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(32, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(91, 16);
            this.label9.TabIndex = 21;
            this.label9.Text = "data_1_offset";
            // 
            // lbldata_1_max_len
            // 
            this.lbldata_1_max_len.AutoSize = true;
            this.lbldata_1_max_len.Location = new System.Drawing.Point(156, 65);
            this.lbldata_1_max_len.Name = "lbldata_1_max_len";
            this.lbldata_1_max_len.Size = new System.Drawing.Size(0, 13);
            this.lbldata_1_max_len.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(32, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 16);
            this.label7.TabIndex = 19;
            this.label7.Text = "data_1_max_len:";
            // 
            // lbldata_1_len
            // 
            this.lbldata_1_len.AutoSize = true;
            this.lbldata_1_len.Location = new System.Drawing.Point(156, 49);
            this.lbldata_1_len.Name = "lbldata_1_len";
            this.lbldata_1_len.Size = new System.Drawing.Size(0, 13);
            this.lbldata_1_len.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(32, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 16);
            this.label6.TabIndex = 17;
            this.label6.Text = "data_1_len:";
            // 
            // lbldata_1_fmt
            // 
            this.lbldata_1_fmt.AutoSize = true;
            this.lbldata_1_fmt.Location = new System.Drawing.Point(156, 33);
            this.lbldata_1_fmt.Name = "lbldata_1_fmt";
            this.lbldata_1_fmt.Size = new System.Drawing.Size(0, 13);
            this.lbldata_1_fmt.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(32, 30);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 16);
            this.label8.TabIndex = 15;
            this.label8.Text = "data_1_fmt:";
            // 
            // lblkey_1_offset
            // 
            this.lblkey_1_offset.AutoSize = true;
            this.lblkey_1_offset.Location = new System.Drawing.Point(156, 14);
            this.lblkey_1_offset.Name = "lblkey_1_offset";
            this.lblkey_1_offset.Size = new System.Drawing.Size(0, 13);
            this.lblkey_1_offset.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(32, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 16);
            this.label10.TabIndex = 13;
            this.label10.Text = "key_1_offset:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTE);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblDTS);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.lblKTS);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(188, 70);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Header";
            // 
            // lblTE
            // 
            this.lblTE.AutoSize = true;
            this.lblTE.Location = new System.Drawing.Point(154, 51);
            this.lblTE.Name = "lblTE";
            this.lblTE.Size = new System.Drawing.Size(0, 13);
            this.lblTE.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(30, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "table_entries";
            // 
            // lblDTS
            // 
            this.lblDTS.AutoSize = true;
            this.lblDTS.Location = new System.Drawing.Point(154, 35);
            this.lblDTS.Name = "lblDTS";
            this.lblDTS.Size = new System.Drawing.Size(0, 13);
            this.lblDTS.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(30, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "data_table_start:";
            // 
            // lblKTS
            // 
            this.lblKTS.AutoSize = true;
            this.lblKTS.Location = new System.Drawing.Point(154, 16);
            this.lblKTS.Name = "lblKTS";
            this.lblKTS.Size = new System.Drawing.Size(0, 13);
            this.lblKTS.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(30, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 16);
            this.label2.TabIndex = 13;
            this.label2.Text = "key_table_start:";
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::PARAM.SFO_Editor.Properties.Resources.Save;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.Location = new System.Drawing.Point(730, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 48);
            this.button2.TabIndex = 6;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PARAM.SFO_Editor.Properties.Resources.cooltext118235191746368;
            this.pictureBox1.Location = new System.Drawing.Point(15, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(638, 71);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::PARAM.SFO_Editor.Properties.Resources.Open;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(659, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 48);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "TitleID :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(77, 120);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(142, 20);
            this.textBox1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(823, 421);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.gbAdvanced);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cbVersion);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PARAM.SFO EDITOR v1.0";
            this.gbAdvanced.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbVersion;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox gbAdvanced;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbldata_1_len;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbldata_1_fmt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblkey_1_offset;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblTE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDTS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblKTS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbldata_1_offset;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbldata_1_max_len;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblkey_1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
    }
}

