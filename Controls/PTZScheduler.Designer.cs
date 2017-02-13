namespace iSpyApplication.Controls
{
    partial class PTZScheduler
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
            this.panel11 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel20 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPTZSchedule = new System.Windows.Forms.Panel();
            this.lbPTZSchedule = new System.Windows.Forms.ListBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel27 = new System.Windows.Forms.FlowLayoutPanel();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.btnDeletePTZ = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.flowPTZSchedule = new System.Windows.Forms.FlowLayoutPanel();
            this.chkSuspendOnMovement = new System.Windows.Forms.CheckBox();
            this.ddlScheduleCommand = new System.Windows.Forms.ComboBox();
            this.dtpSchedulePTZ = new System.Windows.Forms.DateTimePicker();
            this.button6 = new System.Windows.Forms.Button();
            this.panel15 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel28 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkSchedulePTZ = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel11.SuspendLayout();
            this.tableLayoutPanel20.SuspendLayout();
            this.pnlPTZSchedule.SuspendLayout();
            this.panel13.SuspendLayout();
            this.flowLayoutPanel27.SuspendLayout();
            this.panel14.SuspendLayout();
            this.flowPTZSchedule.SuspendLayout();
            this.panel15.SuspendLayout();
            this.flowLayoutPanel28.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.tableLayoutPanel20);
            this.panel11.Controls.Add(this.panel2);
            this.panel11.Controls.Add(this.panel15);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel11.Location = new System.Drawing.Point(0, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(635, 228);
            this.panel11.TabIndex = 83;
            // 
            // tableLayoutPanel20
            // 
            this.tableLayoutPanel20.ColumnCount = 2;
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.Controls.Add(this.pnlPTZSchedule, 1, 0);
            this.tableLayoutPanel20.Controls.Add(this.panel14, 0, 0);
            this.tableLayoutPanel20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel20.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanel20.Name = "tableLayoutPanel20";
            this.tableLayoutPanel20.RowCount = 1;
            this.tableLayoutPanel20.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel20.Size = new System.Drawing.Size(635, 156);
            this.tableLayoutPanel20.TabIndex = 0;
            // 
            // pnlPTZSchedule
            // 
            this.pnlPTZSchedule.Controls.Add(this.lbPTZSchedule);
            this.pnlPTZSchedule.Controls.Add(this.panel13);
            this.pnlPTZSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPTZSchedule.Location = new System.Drawing.Point(320, 3);
            this.pnlPTZSchedule.Name = "pnlPTZSchedule";
            this.pnlPTZSchedule.Size = new System.Drawing.Size(312, 150);
            this.pnlPTZSchedule.TabIndex = 1;
            // 
            // lbPTZSchedule
            // 
            this.lbPTZSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbPTZSchedule.FormattingEnabled = true;
            this.lbPTZSchedule.Location = new System.Drawing.Point(0, 0);
            this.lbPTZSchedule.Name = "lbPTZSchedule";
            this.lbPTZSchedule.Size = new System.Drawing.Size(312, 119);
            this.lbPTZSchedule.TabIndex = 0;
            // 
            // panel13
            // 
            this.panel13.Controls.Add(this.flowLayoutPanel27);
            this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel13.Location = new System.Drawing.Point(0, 119);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(312, 31);
            this.panel13.TabIndex = 1;
            // 
            // flowLayoutPanel27
            // 
            this.flowLayoutPanel27.Controls.Add(this.button7);
            this.flowLayoutPanel27.Controls.Add(this.button8);
            this.flowLayoutPanel27.Controls.Add(this.btnDeletePTZ);
            this.flowLayoutPanel27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel27.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel27.Name = "flowLayoutPanel27";
            this.flowLayoutPanel27.Size = new System.Drawing.Size(312, 31);
            this.flowLayoutPanel27.TabIndex = 3;
            // 
            // button7
            // 
            this.button7.AutoSize = true;
            this.button7.Location = new System.Drawing.Point(3, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(64, 23);
            this.button7.TabIndex = 1;
            this.button7.Text = "Repeat";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button8.AutoSize = true;
            this.button8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button8.Location = new System.Drawing.Point(73, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(62, 23);
            this.button8.TabIndex = 2;
            this.button8.Text = "Delete All";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnDeletePTZ
            // 
            this.btnDeletePTZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePTZ.AutoSize = true;
            this.btnDeletePTZ.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDeletePTZ.Location = new System.Drawing.Point(141, 3);
            this.btnDeletePTZ.Name = "btnDeletePTZ";
            this.btnDeletePTZ.Size = new System.Drawing.Size(48, 23);
            this.btnDeletePTZ.TabIndex = 0;
            this.btnDeletePTZ.Text = "Delete";
            this.btnDeletePTZ.UseVisualStyleBackColor = true;
            this.btnDeletePTZ.Click += new System.EventHandler(this.btnDeletePTZ_Click);
            // 
            // panel14
            // 
            this.panel14.Controls.Add(this.flowPTZSchedule);
            this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel14.Location = new System.Drawing.Point(0, 0);
            this.panel14.Margin = new System.Windows.Forms.Padding(0);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(317, 156);
            this.panel14.TabIndex = 2;
            // 
            // flowPTZSchedule
            // 
            this.flowPTZSchedule.Controls.Add(this.chkSuspendOnMovement);
            this.flowPTZSchedule.Controls.Add(this.ddlScheduleCommand);
            this.flowPTZSchedule.Controls.Add(this.dtpSchedulePTZ);
            this.flowPTZSchedule.Controls.Add(this.button6);
            this.flowPTZSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPTZSchedule.Location = new System.Drawing.Point(0, 0);
            this.flowPTZSchedule.Margin = new System.Windows.Forms.Padding(0);
            this.flowPTZSchedule.Name = "flowPTZSchedule";
            this.flowPTZSchedule.Size = new System.Drawing.Size(317, 156);
            this.flowPTZSchedule.TabIndex = 6;
            // 
            // chkSuspendOnMovement
            // 
            this.chkSuspendOnMovement.AutoSize = true;
            this.chkSuspendOnMovement.Location = new System.Drawing.Point(3, 3);
            this.chkSuspendOnMovement.Name = "chkSuspendOnMovement";
            this.chkSuspendOnMovement.Padding = new System.Windows.Forms.Padding(3);
            this.chkSuspendOnMovement.Size = new System.Drawing.Size(191, 23);
            this.chkSuspendOnMovement.TabIndex = 6;
            this.chkSuspendOnMovement.Text = "Suspend on Movement Detection";
            this.chkSuspendOnMovement.UseVisualStyleBackColor = true;
            // 
            // ddlScheduleCommand
            // 
            this.ddlScheduleCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.ddlScheduleCommand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlScheduleCommand.FormattingEnabled = true;
            this.ddlScheduleCommand.Location = new System.Drawing.Point(3, 32);
            this.ddlScheduleCommand.Name = "ddlScheduleCommand";
            this.ddlScheduleCommand.Size = new System.Drawing.Size(290, 21);
            this.ddlScheduleCommand.TabIndex = 3;
            // 
            // dtpSchedulePTZ
            // 
            this.dtpSchedulePTZ.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpSchedulePTZ.Location = new System.Drawing.Point(3, 59);
            this.dtpSchedulePTZ.Name = "dtpSchedulePTZ";
            this.dtpSchedulePTZ.ShowUpDown = true;
            this.dtpSchedulePTZ.Size = new System.Drawing.Size(109, 20);
            this.dtpSchedulePTZ.TabIndex = 4;
            // 
            // button6
            // 
            this.button6.AutoSize = true;
            this.button6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button6.Location = new System.Drawing.Point(118, 59);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(36, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "Add";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // panel15
            // 
            this.panel15.Controls.Add(this.flowLayoutPanel28);
            this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel15.Location = new System.Drawing.Point(0, 0);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(635, 31);
            this.panel15.TabIndex = 2;
            // 
            // flowLayoutPanel28
            // 
            this.flowLayoutPanel28.Controls.Add(this.chkSchedulePTZ);
            this.flowLayoutPanel28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel28.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel28.Name = "flowLayoutPanel28";
            this.flowLayoutPanel28.Size = new System.Drawing.Size(635, 31);
            this.flowLayoutPanel28.TabIndex = 1;
            // 
            // chkSchedulePTZ
            // 
            this.chkSchedulePTZ.AutoSize = true;
            this.chkSchedulePTZ.Location = new System.Drawing.Point(3, 3);
            this.chkSchedulePTZ.Name = "chkSchedulePTZ";
            this.chkSchedulePTZ.Padding = new System.Windows.Forms.Padding(3);
            this.chkSchedulePTZ.Size = new System.Drawing.Size(101, 23);
            this.chkSchedulePTZ.TabIndex = 0;
            this.chkSchedulePTZ.Text = "Schedule PTZ";
            this.chkSchedulePTZ.UseVisualStyleBackColor = true;
            this.chkSchedulePTZ.CheckedChanged += new System.EventHandler(this.chkSchedulePTZ_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 187);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(635, 41);
            this.panel2.TabIndex = 85;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(548, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // PTZScheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 228);
            this.Controls.Add(this.panel11);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PTZScheduler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PTZScheduler";
            this.Load += new System.EventHandler(this.PTZScheduler_Load);
            this.panel11.ResumeLayout(false);
            this.tableLayoutPanel20.ResumeLayout(false);
            this.pnlPTZSchedule.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.flowLayoutPanel27.ResumeLayout(false);
            this.flowLayoutPanel27.PerformLayout();
            this.panel14.ResumeLayout(false);
            this.flowPTZSchedule.ResumeLayout(false);
            this.flowPTZSchedule.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.flowLayoutPanel28.ResumeLayout(false);
            this.flowLayoutPanel28.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel20;
        private System.Windows.Forms.Panel pnlPTZSchedule;
        private System.Windows.Forms.ListBox lbPTZSchedule;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel27;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnDeletePTZ;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.FlowLayoutPanel flowPTZSchedule;
        private System.Windows.Forms.CheckBox chkSuspendOnMovement;
        private System.Windows.Forms.ComboBox ddlScheduleCommand;
        private System.Windows.Forms.DateTimePicker dtpSchedulePTZ;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel28;
        private System.Windows.Forms.CheckBox chkSchedulePTZ;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOK;
    }
}