namespace iSpyApplication
{
    partial class PTZCommandButtons
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
            this.MinimizeBox = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            //this.ddlExtended = new System.Windows.Forms.ComboBox();
            this.pnlController = new System.Windows.Forms.Panel();
            this.pnlController.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlExtended
            // 
            //this.ddlExtended.Dock = System.Windows.Forms.DockStyle.Bottom;
            //this.ddlExtended.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.ddlExtended.FormattingEnabled = true;
            //this.ddlExtended.Location = new System.Drawing.Point(0, 176);
            //this.ddlExtended.Margin = new System.Windows.Forms.Padding(4);
            //this.ddlExtended.Name = "ddlExtended";
            //this.ddlExtended.Size = new System.Drawing.Size(225, 24);
            //this.ddlExtended.TabIndex = 6;
            //this.ddlExtended.Font = new System.Drawing.Font("Serif", 20);
            //this.ddlExtended.SelectedIndexChanged += new System.EventHandler(this.ddlExtended_SelectedIndexChanged);
            // 
            // pnlController
            // 
            //this.pnlController.Controls.Add(this.ptzui1);
            //this.pnlController.Controls.Add(this.ddlExtended);
            this.pnlController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlController.Location = new System.Drawing.Point(3, 3);
            this.pnlController.Margin = new System.Windows.Forms.Padding(4);
            this.pnlController.Name = "pnlController";
            this.pnlController.Size = new System.Drawing.Size(225, 200);
            this.pnlController.TabIndex = 7;
            // 
            // ptzui1
            // 
            //this.ptzui1.Location = new System.Drawing.Point(0, 0);
            //this.ptzui1.Name = "ptzui1";
            //this.ptzui1.Size = new System.Drawing.Size(225, 176);
            //this.ptzui1.TabIndex = 7;
            // 
            // PTZCommandButtons
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(231, 206);
            this.Controls.Add(this.pnlController);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PTZCommandButtons";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PTZ Command Buttons";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PTZCommandButtons_FormClosing);
            this.Load += new System.EventHandler(this.PTZCommandButtons_Load);
            this.pnlController.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        //private System.Windows.Forms.ComboBox ddlExtended;
        private System.Windows.Forms.Panel pnlController;
        //private Controls.PTZUI ptzui1;
    }
}