namespace PS4_PS2_Classis_GUI
{
    partial class Advanced
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Advanced));
            this.txtAllInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtAllInfo
            // 
            this.txtAllInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAllInfo.Location = new System.Drawing.Point(0, 0);
            this.txtAllInfo.Multiline = true;
            this.txtAllInfo.Name = "txtAllInfo";
            this.txtAllInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAllInfo.Size = new System.Drawing.Size(578, 253);
            this.txtAllInfo.TabIndex = 0;
            // 
            // Advanced
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 253);
            this.Controls.Add(this.txtAllInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Advanced";
            this.Text = "Advanced";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAllInfo;
    }
}