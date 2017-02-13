namespace iSpyApplication
{
    partial class RemoteCommands
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
            this.label45 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.lbManualAlerts = new System.Windows.Forms.ListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddCommand = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCommand = new System.Windows.Forms.Label();
            this.btnEditCommand = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label45
            // 
            this.label45.Location = new System.Drawing.Point(299, 19);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(265, 134);
            this.label45.TabIndex = 83;
            this.label45.Text = "For Example:\r\n\r\nSwitch on and off cameras, Execute a batch file for home automati" +
    "on,\r\nPlay an MP3 to stop your dogs barking, Sound an alarm";
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label82, 2);
            this.label82.Location = new System.Drawing.Point(3, 0);
            this.label82.Name = "label82";
            this.label82.Padding = new System.Windows.Forms.Padding(3);
            this.label82.Size = new System.Drawing.Size(432, 19);
            this.label82.TabIndex = 0;
            this.label82.Text = "You can trigger remote commands manually from the iSpy website or from mobile dev" +
    "ices.";
            // 
            // lbManualAlerts
            // 
            this.lbManualAlerts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbManualAlerts.FormattingEnabled = true;
            this.lbManualAlerts.Location = new System.Drawing.Point(3, 22);
            this.lbManualAlerts.Name = "lbManualAlerts";
            this.lbManualAlerts.Size = new System.Drawing.Size(290, 134);
            this.lbManualAlerts.TabIndex = 0;
            this.lbManualAlerts.SelectedIndexChanged += new System.EventHandler(this.lbManualAlerts_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(135, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 19);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDeleteClick);
            // 
            // btnAddCommand
            // 
            this.btnAddCommand.Location = new System.Drawing.Point(3, 3);
            this.btnAddCommand.Name = "btnAddCommand";
            this.btnAddCommand.Size = new System.Drawing.Size(60, 19);
            this.btnAddCommand.TabIndex = 6;
            this.btnAddCommand.Text = "Add";
            this.btnAddCommand.UseVisualStyleBackColor = true;
            this.btnAddCommand.Click += new System.EventHandler(this.BtnAddCommandClick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label82, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbManualAlerts, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label45, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(613, 205);
            this.tableLayoutPanel1.TabIndex = 91;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnAddCommand);
            this.flowLayoutPanel2.Controls.Add(this.btnEditCommand);
            this.flowLayoutPanel2.Controls.Add(this.btnDelete);
            this.flowLayoutPanel2.Controls.Add(this.linkLabel3);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 162);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(290, 38);
            this.flowLayoutPanel2.TabIndex = 89;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(219, 6);
            this.linkLabel3.Margin = new System.Windows.Forms.Padding(6);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(35, 13);
            this.linkLabel3.TabIndex = 2;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Reset";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblCommand);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(299, 162);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(311, 38);
            this.flowLayoutPanel1.TabIndex = 91;
            // 
            // lblCommand
            // 
            this.lblCommand.AutoSize = true;
            this.flowLayoutPanel1.SetFlowBreak(this.lblCommand, true);
            this.lblCommand.Location = new System.Drawing.Point(3, 0);
            this.lblCommand.Name = "lblCommand";
            this.lblCommand.Padding = new System.Windows.Forms.Padding(6);
            this.lblCommand.Size = new System.Drawing.Size(66, 25);
            this.lblCommand.TabIndex = 90;
            this.lblCommand.Text = "Command";
            // 
            // btnEditCommand
            // 
            this.btnEditCommand.Enabled = false;
            this.btnEditCommand.Location = new System.Drawing.Point(69, 3);
            this.btnEditCommand.Name = "btnEditCommand";
            this.btnEditCommand.Size = new System.Drawing.Size(60, 19);
            this.btnEditCommand.TabIndex = 7;
            this.btnEditCommand.Text = "Edit";
            this.btnEditCommand.UseVisualStyleBackColor = true;
            this.btnEditCommand.Click += new System.EventHandler(this.btnEditCommand_Click);
            // 
            // RemoteCommands
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(625, 223);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimizeBox = false;
            this.Name = "RemoteCommands";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remote Commands";
            this.Load += new System.EventHandler(this.ManualAlertsLoad);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.ListBox lbManualAlerts;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAddCommand;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label lblCommand;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnEditCommand;
    }
}