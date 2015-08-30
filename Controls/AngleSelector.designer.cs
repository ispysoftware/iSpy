namespace iSpyApplication.Controls
{
    sealed partial class AngleSelector
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
            // AngleSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "AngleSelector";
            this.Size = new System.Drawing.Size(40, 40);
            this.Load += new System.EventHandler(this.AngleSelector_Load);
            this.SizeChanged += new System.EventHandler(this.AngleSelector_SizeChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AngleSelectorMouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AngleSelectorMouseMove);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
