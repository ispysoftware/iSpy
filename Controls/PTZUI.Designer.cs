namespace iSpyApplication.Controls
{
    partial class PTZUI
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
            this.components = new System.ComponentModel.Container();
            this.pnlPTZ = new System.Windows.Forms.Panel();
            this.tmrRepeater = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // pnlPTZ
            // 
            this.pnlPTZ.BackgroundImage = global::iSpyApplication.Properties.Resources.PTZ_Controller;
            this.pnlPTZ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlPTZ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlPTZ.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPTZ.Location = new System.Drawing.Point(0, 0);
            this.pnlPTZ.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.pnlPTZ.Name = "pnlPTZ";
            this.pnlPTZ.Size = new System.Drawing.Size(225, 176);
            this.pnlPTZ.TabIndex = 6;
            this.pnlPTZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlPTZ_MouseDown);
            this.pnlPTZ.MouseEnter += new System.EventHandler(this.pnlPTZ_MouseEnter);
            this.pnlPTZ.MouseLeave += new System.EventHandler(this.pnlPTZ_MouseLeave);
            this.pnlPTZ.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlPTZ_MouseMove);
            this.pnlPTZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlPTZ_MouseUp);
            // 
            // tmrRepeater
            // 
            this.tmrRepeater.Interval = 200;
            this.tmrRepeater.Tick += new System.EventHandler(this.tmrRepeater_Tick);
            // 
            // PTZUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlPTZ);
            this.Name = "PTZUI";
            this.Size = new System.Drawing.Size(225, 176);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPTZ;
        private System.Windows.Forms.Timer tmrRepeater;
    }
}
