namespace iSpyApplication.Controls
{
    sealed partial class MediaPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MediaPanel
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MediaPanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MediaPanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MediaPanel_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
