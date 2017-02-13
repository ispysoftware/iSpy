namespace iSpyApplication
{
    partial class Pager
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
            this.numPage = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
            this.SuspendLayout();
            // 
            // numPage
            // 
            this.numPage.Location = new System.Drawing.Point(12, 12);
            this.numPage.Name = "numPage";
            this.numPage.Size = new System.Drawing.Size(100, 20);
            this.numPage.TabIndex = 0;
            this.numPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numPage_KeyDown);
            // 
            // Pager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(130, 49);
            this.Controls.Add(this.numPage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Pager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Page...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Pager_FormClosing);
            this.Load += new System.EventHandler(this.Pager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numPage;
    }
}