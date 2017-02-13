using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    partial class CommandButtonControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private readonly Timer _tmrRefresh = new Timer();

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                _tmrRefresh.Stop();
                _tmrRefresh.Dispose();
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
            // CommandButtonControl
            // 
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandButtonsMouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CommandButtonControl_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion



    }
}
