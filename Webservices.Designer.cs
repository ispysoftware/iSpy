namespace iSpyApplication
{
    partial class Webservices
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Webservices));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.Next = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbIPv4Address = new System.Windows.Forms.ListBox();
            this.chkReroute = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.llblHelp = new System.Windows.Forms.LinkLabel();
            this.tcIPMode = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLANPort = new System.Windows.Forms.NumericUpDown();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkuPNP = new System.Windows.Forms.CheckBox();
            this.txtWANPort = new System.Windows.Forms.NumericUpDown();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.lbIPv6Address = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnTroubleshooting = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkBindSpecific = new System.Windows.Forms.CheckBox();
            this.chkEnableIPv6 = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tcIPMode.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLANPort)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWANPort)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label1.Location = new System.Drawing.Point(73, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label2.Location = new System.Drawing.Point(73, 114);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(172, 83);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(100, 22);
            this.txtUsername.TabIndex = 2;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(172, 111);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(100, 22);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 3);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(6, 159);
            this.label4.Margin = new System.Windows.Forms.Padding(6);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(3);
            this.label4.Size = new System.Drawing.Size(379, 40);
            this.label4.TabIndex = 7;
            this.label4.Text = "To view your recorded and live content locally and remotely you need to configure" +
    " the built in web server. \r\n";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(172, 136);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(145, 17);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Create a new account";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1LinkClicked);
            // 
            // Next
            // 
            this.Next.AutoSize = true;
            this.Next.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Next.Location = new System.Drawing.Point(333, 3);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(55, 27);
            this.Next.TabIndex = 5;
            this.Next.Text = "Finish";
            this.Next.UseVisualStyleBackColor = true;
            this.Next.Click += new System.EventHandler(this.Button1Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label3.Location = new System.Drawing.Point(154, 0);
            this.label3.Name = "label3";
            this.label3.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label3.Size = new System.Drawing.Size(70, 23);
            this.label3.TabIndex = 25;
            this.label3.Text = "WAN Port";
            this.toolTip1.SetToolTip(this.label3, "This is the port that is accessible externally (from the internet)");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 0);
            this.label10.Name = "label10";
            this.label10.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label10.Size = new System.Drawing.Size(65, 23);
            this.label10.TabIndex = 39;
            this.label10.Text = "LAN Port";
            this.toolTip1.SetToolTip(this.label10, "This is the port that is accessible internally over your LAN");
            // 
            // lbIPv4Address
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.lbIPv4Address, 4);
            this.lbIPv4Address.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbIPv4Address.FormattingEnabled = true;
            this.lbIPv4Address.ItemHeight = 16;
            this.lbIPv4Address.Location = new System.Drawing.Point(3, 31);
            this.lbIPv4Address.Name = "lbIPv4Address";
            this.lbIPv4Address.Size = new System.Drawing.Size(359, 56);
            this.lbIPv4Address.TabIndex = 44;
            this.toolTip1.SetToolTip(this.lbIPv4Address, "Select the IP address you want to use");
            this.lbIPv4Address.SelectedIndexChanged += new System.EventHandler(this.lbIPv4Address_SelectedIndexChanged);
            // 
            // chkReroute
            // 
            this.chkReroute.AutoSize = true;
            this.chkReroute.Location = new System.Drawing.Point(201, 3);
            this.chkReroute.Name = "chkReroute";
            this.chkReroute.Size = new System.Drawing.Size(123, 21);
            this.chkReroute.TabIndex = 49;
            this.chkReroute.Text = "DHCP Reroute";
            this.toolTip1.SetToolTip(this.chkReroute, "iSpy can monitor your connection and re-configure your router if your LAN IP addr" +
        "ess changes");
            this.chkReroute.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label8.Size = new System.Drawing.Size(34, 23);
            this.label8.TabIndex = 47;
            this.label8.Text = "Port";
            this.toolTip1.SetToolTip(this.label8, "This is the port that is accessible internally over your LAN");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label6, 3);
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(6, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(379, 68);
            this.label6.TabIndex = 28;
            this.label6.Text = "To access your cameras, microphones and captured content over the web or with mob" +
    "ile devices and to use iSpy alerting services  you will need an iSpy Connect acc" +
    "ount.";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // llblHelp
            // 
            this.llblHelp.AutoSize = true;
            this.llblHelp.Location = new System.Drawing.Point(102, 8);
            this.llblHelp.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.llblHelp.Name = "llblHelp";
            this.llblHelp.Size = new System.Drawing.Size(37, 17);
            this.llblHelp.TabIndex = 62;
            this.llblHelp.TabStop = true;
            this.llblHelp.Text = "Help";
            this.llblHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblHelp_LinkClicked);
            // 
            // tcIPMode
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tcIPMode, 3);
            this.tcIPMode.Controls.Add(this.tabPage1);
            this.tcIPMode.Controls.Add(this.tabPage2);
            this.tcIPMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcIPMode.Location = new System.Drawing.Point(3, 266);
            this.tcIPMode.Name = "tcIPMode";
            this.tcIPMode.SelectedIndex = 0;
            this.tcIPMode.Size = new System.Drawing.Size(385, 195);
            this.tcIPMode.TabIndex = 45;
            this.tcIPMode.SelectedIndexChanged += new System.EventHandler(this.tcIPMode_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage1.Size = new System.Drawing.Size(377, 166);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "UPNP (IPv4)";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtLANPort, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbIPv4Address, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel2, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.flowLayoutPanel2, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtWANPort, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(365, 154);
            this.tableLayoutPanel2.TabIndex = 51;
            // 
            // txtLANPort
            // 
            this.txtLANPort.Location = new System.Drawing.Point(74, 3);
            this.txtLANPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtLANPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtLANPort.Name = "txtLANPort";
            this.txtLANPort.Size = new System.Drawing.Size(74, 22);
            this.txtLANPort.TabIndex = 50;
            this.txtLANPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.linkLabel2, 4);
            this.linkLabel2.Location = new System.Drawing.Point(3, 119);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(234, 17);
            this.linkLabel2.TabIndex = 42;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "... or manually configure your router";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2LinkClicked);
            // 
            // flowLayoutPanel2
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.flowLayoutPanel2, 4);
            this.flowLayoutPanel2.Controls.Add(this.chkuPNP);
            this.flowLayoutPanel2.Controls.Add(this.chkReroute);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 93);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(359, 23);
            this.flowLayoutPanel2.TabIndex = 51;
            // 
            // chkuPNP
            // 
            this.chkuPNP.AutoSize = true;
            this.chkuPNP.Location = new System.Drawing.Point(3, 3);
            this.chkuPNP.Name = "chkuPNP";
            this.chkuPNP.Size = new System.Drawing.Size(192, 21);
            this.chkuPNP.TabIndex = 43;
            this.chkuPNP.Text = "Auto configure with UPNP";
            this.chkuPNP.UseVisualStyleBackColor = true;
            this.chkuPNP.CheckedChanged += new System.EventHandler(this.ChkuPnpCheckedChanged);
            // 
            // txtWANPort
            // 
            this.txtWANPort.Location = new System.Drawing.Point(230, 3);
            this.txtWANPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtWANPort.Name = "txtWANPort";
            this.txtWANPort.Size = new System.Drawing.Size(71, 22);
            this.txtWANPort.TabIndex = 52;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(6);
            this.tabPage2.Size = new System.Drawing.Size(377, 166);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tunneling (IPv6)";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtPort, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lbIPv6Address, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(365, 154);
            this.tableLayoutPanel3.TabIndex = 52;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.tableLayoutPanel3.SetColumnSpan(this.label7, 2);
            this.label7.Location = new System.Drawing.Point(6, 109);
            this.label7.Margin = new System.Windows.Forms.Padding(6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(434, 17);
            this.label7.TabIndex = 46;
            this.label7.Text = "Using IPv6 iSpy *might* be able to configure your NAT automatically";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(43, 3);
            this.txtPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.txtPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(74, 22);
            this.txtPort.TabIndex = 51;
            this.txtPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbIPv6Address
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.lbIPv6Address, 2);
            this.lbIPv6Address.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbIPv6Address.FormattingEnabled = true;
            this.lbIPv6Address.ItemHeight = 16;
            this.lbIPv6Address.Location = new System.Drawing.Point(3, 31);
            this.lbIPv6Address.Name = "lbIPv6Address";
            this.lbIPv6Address.Size = new System.Drawing.Size(440, 69);
            this.lbIPv6Address.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 3);
            this.label5.Location = new System.Drawing.Point(6, 470);
            this.label5.Margin = new System.Windows.Forms.Padding(6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(372, 34);
            this.label5.TabIndex = 41;
            this.label5.Text = "If you are connecting multiple instances of iSpy, you must select a different por" +
    "t combination for each instance. ";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // button2
            // 
            this.button2.AutoSize = true;
            this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.button2.Location = new System.Drawing.Point(145, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(61, 27);
            this.button2.TabIndex = 31;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // btnTroubleshooting
            // 
            this.btnTroubleshooting.AutoSize = true;
            this.btnTroubleshooting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnTroubleshooting.Location = new System.Drawing.Point(212, 3);
            this.btnTroubleshooting.Name = "btnTroubleshooting";
            this.btnTroubleshooting.Size = new System.Drawing.Size(115, 27);
            this.btnTroubleshooting.TabIndex = 64;
            this.btnTroubleshooting.Text = "Troubleshooter";
            this.btnTroubleshooting.UseVisualStyleBackColor = true;
            this.btnTroubleshooting.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUsername, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.chkBindSpecific, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tcIPMode, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.linkLabel1, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkEnableIPv6, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(391, 510);
            this.tableLayoutPanel1.TabIndex = 65;
            // 
            // chkBindSpecific
            // 
            this.chkBindSpecific.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkBindSpecific, 2);
            this.chkBindSpecific.Location = new System.Drawing.Point(76, 208);
            this.chkBindSpecific.Name = "chkBindSpecific";
            this.chkBindSpecific.Size = new System.Drawing.Size(199, 21);
            this.chkBindSpecific.TabIndex = 47;
            this.chkBindSpecific.Text = "Bind to Specific IP Address";
            this.chkBindSpecific.UseVisualStyleBackColor = true;
            // 
            // chkEnableIPv6
            // 
            this.chkEnableIPv6.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkEnableIPv6, 2);
            this.chkEnableIPv6.Location = new System.Drawing.Point(76, 239);
            this.chkEnableIPv6.Name = "chkEnableIPv6";
            this.chkEnableIPv6.Size = new System.Drawing.Size(159, 21);
            this.chkEnableIPv6.TabIndex = 46;
            this.chkEnableIPv6.Text = "Enable IPv6 Support";
            this.chkEnableIPv6.UseVisualStyleBackColor = true;
            this.chkEnableIPv6.CheckedChanged += new System.EventHandler(this.chkEnableIPv6_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.Next);
            this.flowLayoutPanel1.Controls.Add(this.btnTroubleshooting);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.llblHelp);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 558);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(391, 37);
            this.flowLayoutPanel1.TabIndex = 46;
            // 
            // Webservices
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(403, 601);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Webservices";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Web Access";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Webservices_FormClosing);
            this.Load += new System.EventHandler(this.WebservicesLoad);
            this.tcIPMode.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLANPort)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtWANPort)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.CheckBox chkuPNP;
        private System.Windows.Forms.TabControl tcIPMode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lbIPv4Address;
        private System.Windows.Forms.ListBox lbIPv6Address;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkReroute;
        private System.Windows.Forms.LinkLabel llblHelp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown txtLANPort;
        private System.Windows.Forms.NumericUpDown txtPort;
        private System.Windows.Forms.Button btnTroubleshooting;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.NumericUpDown txtWANPort;
        private System.Windows.Forms.CheckBox chkEnableIPv6;
        private System.Windows.Forms.CheckBox chkBindSpecific;
    }
}