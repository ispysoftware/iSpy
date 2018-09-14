namespace iSpyApplication.Controls
{
    partial class ONVIFWizard
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlStep1 = new System.Windows.Forms.TableLayoutPanel();
            this.ddlConnectWith = new System.Windows.Forms.ComboBox();
            this.lblConnectWith = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblDeviceURL = new System.Windows.Forms.Label();
            this.lblTransport = new System.Windows.Forms.Label();
            this.txtOnvifUsername = new System.Windows.Forms.TextBox();
            this.txtOnvifPassword = new System.Windows.Forms.TextBox();
            this.ddlTransport = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.ddlDeviceURL = new System.Windows.Forms.ComboBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numRTSP = new System.Windows.Forms.NumericUpDown();
            this.chkOverrideRTSPPort = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlStep2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.lbOnvifURLs = new System.Windows.Forms.ListBox();
            this.lblURL = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblStep = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlStep1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRTSP)).BeginInit();
            this.pnlStep2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(711, 318);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pnlStep1);
            this.panel3.Controls.Add(this.pnlStep2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(711, 296);
            this.panel3.TabIndex = 4;
            // 
            // pnlStep1
            // 
            this.pnlStep1.ColumnCount = 4;
            this.pnlStep1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.pnlStep1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlStep1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlStep1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.pnlStep1.Controls.Add(this.ddlConnectWith, 1, 5);
            this.pnlStep1.Controls.Add(this.lblConnectWith, 0, 5);
            this.pnlStep1.Controls.Add(this.lblUsername, 0, 0);
            this.pnlStep1.Controls.Add(this.lblDeviceURL, 0, 2);
            this.pnlStep1.Controls.Add(this.lblTransport, 0, 4);
            this.pnlStep1.Controls.Add(this.txtOnvifUsername, 1, 0);
            this.pnlStep1.Controls.Add(this.txtOnvifPassword, 1, 1);
            this.pnlStep1.Controls.Add(this.ddlTransport, 1, 4);
            this.pnlStep1.Controls.Add(this.btnConnect, 1, 6);
            this.pnlStep1.Controls.Add(this.ddlDeviceURL, 1, 2);
            this.pnlStep1.Controls.Add(this.lblPassword, 0, 1);
            this.pnlStep1.Controls.Add(this.label2, 2, 1);
            this.pnlStep1.Controls.Add(this.numRTSP, 3, 1);
            this.pnlStep1.Controls.Add(this.chkOverrideRTSPPort, 3, 0);
            this.pnlStep1.Controls.Add(this.label1, 1, 3);
            this.pnlStep1.Location = new System.Drawing.Point(29, 12);
            this.pnlStep1.Name = "pnlStep1";
            this.pnlStep1.RowCount = 7;
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlStep1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.pnlStep1.Size = new System.Drawing.Size(639, 269);
            this.pnlStep1.TabIndex = 1;
            // 
            // ddlConnectWith
            // 
            this.ddlConnectWith.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlConnectWith.FormattingEnabled = true;
            this.ddlConnectWith.Location = new System.Drawing.Point(153, 207);
            this.ddlConnectWith.Name = "ddlConnectWith";
            this.ddlConnectWith.Size = new System.Drawing.Size(94, 21);
            this.ddlConnectWith.TabIndex = 43;
            // 
            // lblConnectWith
            // 
            this.lblConnectWith.AutoSize = true;
            this.lblConnectWith.Location = new System.Drawing.Point(3, 204);
            this.lblConnectWith.Name = "lblConnectWith";
            this.lblConnectWith.Size = new System.Drawing.Size(72, 13);
            this.lblConnectWith.TabIndex = 6;
            this.lblConnectWith.Text = "Connect With";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(3, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Username";
            // 
            // lblDeviceURL
            // 
            this.lblDeviceURL.AutoSize = true;
            this.lblDeviceURL.Location = new System.Drawing.Point(3, 62);
            this.lblDeviceURL.Name = "lblDeviceURL";
            this.lblDeviceURL.Size = new System.Drawing.Size(66, 13);
            this.lblDeviceURL.TabIndex = 4;
            this.lblDeviceURL.Text = "Device URL";
            // 
            // lblTransport
            // 
            this.lblTransport.AutoSize = true;
            this.lblTransport.Location = new System.Drawing.Point(3, 173);
            this.lblTransport.Name = "lblTransport";
            this.lblTransport.Size = new System.Drawing.Size(52, 13);
            this.lblTransport.TabIndex = 5;
            this.lblTransport.Text = "Transport";
            // 
            // txtOnvifUsername
            // 
            this.txtOnvifUsername.Location = new System.Drawing.Point(153, 3);
            this.txtOnvifUsername.Name = "txtOnvifUsername";
            this.txtOnvifUsername.Size = new System.Drawing.Size(137, 20);
            this.txtOnvifUsername.TabIndex = 38;
            // 
            // txtOnvifPassword
            // 
            this.txtOnvifPassword.Location = new System.Drawing.Point(153, 34);
            this.txtOnvifPassword.Name = "txtOnvifPassword";
            this.txtOnvifPassword.PasswordChar = '*';
            this.txtOnvifPassword.Size = new System.Drawing.Size(137, 20);
            this.txtOnvifPassword.TabIndex = 39;
            this.txtOnvifPassword.UseSystemPasswordChar = true;
            // 
            // ddlTransport
            // 
            this.ddlTransport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlTransport.FormattingEnabled = true;
            this.ddlTransport.Items.AddRange(new object[] {
            "Auto",
            "TCP",
            "UDP",
            "UDP Multicast",
            "HTTP Tunneling"});
            this.ddlTransport.Location = new System.Drawing.Point(153, 176);
            this.ddlTransport.Name = "ddlTransport";
            this.ddlTransport.Size = new System.Drawing.Size(94, 21);
            this.ddlTransport.TabIndex = 42;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(153, 238);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 44;
            this.btnConnect.Text = ">>";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // ddlDeviceURL
            // 
            this.pnlStep1.SetColumnSpan(this.ddlDeviceURL, 3);
            this.ddlDeviceURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlDeviceURL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.ddlDeviceURL.FormattingEnabled = true;
            this.ddlDeviceURL.Location = new System.Drawing.Point(150, 62);
            this.ddlDeviceURL.Margin = new System.Windows.Forms.Padding(0);
            this.ddlDeviceURL.Name = "ddlDeviceURL";
            this.ddlDeviceURL.Size = new System.Drawing.Size(489, 80);
            this.ddlDeviceURL.TabIndex = 45;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(3, 31);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 47;
            this.lblPassword.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(296, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Port";
            // 
            // numRTSP
            // 
            this.numRTSP.Enabled = false;
            this.numRTSP.Location = new System.Drawing.Point(328, 34);
            this.numRTSP.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numRTSP.Name = "numRTSP";
            this.numRTSP.Size = new System.Drawing.Size(137, 20);
            this.numRTSP.TabIndex = 48;
            this.numRTSP.Value = new decimal(new int[] {
            554,
            0,
            0,
            0});
            // 
            // chkOverrideRTSPPort
            // 
            this.chkOverrideRTSPPort.AutoSize = true;
            this.chkOverrideRTSPPort.Location = new System.Drawing.Point(328, 3);
            this.chkOverrideRTSPPort.Name = "chkOverrideRTSPPort";
            this.chkOverrideRTSPPort.Size = new System.Drawing.Size(120, 17);
            this.chkOverrideRTSPPort.TabIndex = 50;
            this.chkOverrideRTSPPort.Text = "Override RTSP Port";
            this.chkOverrideRTSPPort.UseVisualStyleBackColor = true;
            this.chkOverrideRTSPPort.CheckedChanged += new System.EventHandler(this.chkOverrideRTSPPort_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.pnlStep1.SetColumnSpan(this.label1, 3);
            this.label1.Location = new System.Drawing.Point(153, 145);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 13);
            this.label1.TabIndex = 46;
            this.label1.Text = "http://ipaddress/onvif/device_service";
            // 
            // pnlStep2
            // 
            this.pnlStep2.Controls.Add(this.tableLayoutPanel2);
            this.pnlStep2.Location = new System.Drawing.Point(364, 19);
            this.pnlStep2.Name = "pnlStep2";
            this.pnlStep2.Size = new System.Drawing.Size(247, 191);
            this.pnlStep2.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnBack, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lbOnvifURLs, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblURL, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(247, 191);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(3, 163);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(44, 23);
            this.btnBack.TabIndex = 45;
            this.btnBack.Text = "<<";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbOnvifURLs
            // 
            this.lbOnvifURLs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOnvifURLs.FormattingEnabled = true;
            this.lbOnvifURLs.Location = new System.Drawing.Point(153, 3);
            this.lbOnvifURLs.Name = "lbOnvifURLs";
            this.tableLayoutPanel2.SetRowSpan(this.lbOnvifURLs, 2);
            this.lbOnvifURLs.Size = new System.Drawing.Size(91, 185);
            this.lbOnvifURLs.TabIndex = 5;
            // 
            // lblURL
            // 
            this.lblURL.AutoSize = true;
            this.lblURL.Location = new System.Drawing.Point(3, 0);
            this.lblURL.Name = "lblURL";
            this.lblURL.Size = new System.Drawing.Size(29, 13);
            this.lblURL.TabIndex = 4;
            this.lblURL.Text = "URL";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblStep);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(711, 22);
            this.panel2.TabIndex = 2;
            // 
            // lblStep
            // 
            this.lblStep.AutoSize = true;
            this.lblStep.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblStep.Location = new System.Drawing.Point(0, 0);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(38, 13);
            this.lblStep.TabIndex = 0;
            this.lblStep.Text = "Step 1";
            // 
            // ONVIFWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ONVIFWizard";
            this.Size = new System.Drawing.Size(711, 318);
            this.Load += new System.EventHandler(this.ONVIFWizard_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlStep1.ResumeLayout(false);
            this.pnlStep1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRTSP)).EndInit();
            this.pnlStep2.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TableLayoutPanel pnlStep1;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblDeviceURL;
        private System.Windows.Forms.Label lblTransport;
        private System.Windows.Forms.Label lblConnectWith;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlStep2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblURL;
        private System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.ComboBox ddlConnectWith;
        public System.Windows.Forms.TextBox txtOnvifUsername;
        public System.Windows.Forms.TextBox txtOnvifPassword;
        public System.Windows.Forms.ComboBox ddlTransport;
        public System.Windows.Forms.ListBox lbOnvifURLs;
        public System.Windows.Forms.ComboBox ddlDeviceURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkOverrideRTSPPort;
        public System.Windows.Forms.NumericUpDown numRTSP;
    }
}
