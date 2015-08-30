namespace iSpyApplication
{
    partial class PTZTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PTZTool));
            this.pnlPTZ = new System.Windows.Forms.Panel();
            this.ddlExtended = new System.Windows.Forms.ComboBox();
            this.pnlController = new System.Windows.Forms.Panel();
            this.tmrRepeater = new System.Windows.Forms.Timer(this.components);
            this.pnlController.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPTZ
            // 
            this.pnlPTZ.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlPTZ.BackgroundImage")));
            this.pnlPTZ.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlPTZ.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlPTZ.Location = new System.Drawing.Point(6, 9);
            this.pnlPTZ.Margin = new System.Windows.Forms.Padding(6);
            this.pnlPTZ.Name = "pnlPTZ";
            this.pnlPTZ.Size = new System.Drawing.Size(225, 169);
            this.pnlPTZ.TabIndex = 5;
            this.pnlPTZ.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlPTZ_MouseDown);
            this.pnlPTZ.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlPTZ_MouseMove);
            this.pnlPTZ.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlPTZ_MouseUp);
            // 
            // ddlExtended
            // 
            this.ddlExtended.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlExtended.FormattingEnabled = true;
            this.ddlExtended.Location = new System.Drawing.Point(6, 178);
            this.ddlExtended.Name = "ddlExtended";
            this.ddlExtended.Size = new System.Drawing.Size(225, 21);
            this.ddlExtended.TabIndex = 6;
            this.ddlExtended.SelectedIndexChanged += new System.EventHandler(this.ddlExtended_SelectedIndexChanged);
            // 
            // pnlController
            // 
            this.pnlController.AutoSize = true;
            this.pnlController.Controls.Add(this.pnlPTZ);
            this.pnlController.Controls.Add(this.ddlExtended);
            this.pnlController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlController.Location = new System.Drawing.Point(0, 0);
            this.pnlController.Name = "pnlController";
            this.pnlController.Size = new System.Drawing.Size(237, 202);
            this.pnlController.TabIndex = 7;
            // 
            // tmrRepeater
            // 
            this.tmrRepeater.Interval = 200;
            this.tmrRepeater.Tick += new System.EventHandler(this.tmrRepeater_Tick);
            // 
            // PTZTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(237, 202);
            this.ControlBox = false;
            this.Controls.Add(this.pnlController);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(243, 231);
            this.MinimizeBox = false;
            this.Name = "PTZTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PTZ Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PTZTool_FormClosing);
            this.Load += new System.EventHandler(this.PTZTool_Load);
            this.pnlController.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPTZ;
        private System.Windows.Forms.ComboBox ddlExtended;
        private System.Windows.Forms.Panel pnlController;
        private System.Windows.Forms.Timer tmrRepeater;
    }
}