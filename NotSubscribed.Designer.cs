namespace iSpyApplication
{
    partial class NotSubscribed
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
            this.lblInfo = new System.Windows.Forms.Label();
            this.llblSubscribe = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(12, 19);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(234, 13);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "This functionality is only available to subscribers.";
            // 
            // llblSubscribe
            // 
            this.llblSubscribe.AutoSize = true;
            this.llblSubscribe.Location = new System.Drawing.Point(13, 45);
            this.llblSubscribe.Name = "llblSubscribe";
            this.llblSubscribe.Size = new System.Drawing.Size(86, 13);
            this.llblSubscribe.TabIndex = 2;
            this.llblSubscribe.TabStop = true;
            this.llblSubscribe.Text = "More Information";
            this.llblSubscribe.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblSubscribe_LinkClicked);
            // 
            // NotSubscribed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 92);
            this.Controls.Add(this.llblSubscribe);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "NotSubscribed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Access Denied";
            this.Load += new System.EventHandler(this.NotSubscribed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.LinkLabel llblSubscribe;
    }
}