namespace iSpyApplication
{
    partial class SSLConfig
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
            this.txtSSLCertificate = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tlpSSL = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRequireClientCertificate = new System.Windows.Forms.CheckBox();
            this.chkIgnorePolicyErrors = new System.Windows.Forms.CheckBox();
            this.chkCheckRevocation = new System.Windows.Forms.CheckBox();
            this.chkEnableSSL = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tlpSSL.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSSLCertificate
            // 
            this.txtSSLCertificate.Location = new System.Drawing.Point(127, 6);
            this.txtSSLCertificate.Margin = new System.Windows.Forms.Padding(6);
            this.txtSSLCertificate.Name = "txtSSLCertificate";
            this.txtSSLCertificate.ReadOnly = true;
            this.txtSSLCertificate.Size = new System.Drawing.Size(119, 20);
            this.txtSSLCertificate.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(289, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(72, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(255, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(34, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // tlpSSL
            // 
            this.tlpSSL.ColumnCount = 3;
            this.tlpSSL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.94007F));
            this.tlpSSL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.05993F));
            this.tlpSSL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpSSL.Controls.Add(this.button2, 2, 0);
            this.tlpSSL.Controls.Add(this.txtSSLCertificate, 1, 0);
            this.tlpSSL.Controls.Add(this.label1, 0, 0);
            this.tlpSSL.Controls.Add(this.chkRequireClientCertificate, 1, 1);
            this.tlpSSL.Controls.Add(this.chkIgnorePolicyErrors, 1, 2);
            this.tlpSSL.Controls.Add(this.chkCheckRevocation, 1, 3);
            this.tlpSSL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSSL.Location = new System.Drawing.Point(0, 29);
            this.tlpSSL.Name = "tlpSSL";
            this.tlpSSL.RowCount = 4;
            this.tlpSSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpSSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpSSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpSSL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpSSL.Size = new System.Drawing.Size(373, 164);
            this.tlpSSL.TabIndex = 5;
            this.tlpSSL.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Certificate";
            // 
            // chkRequireClientCertificate
            // 
            this.chkRequireClientCertificate.AutoSize = true;
            this.tlpSSL.SetColumnSpan(this.chkRequireClientCertificate, 2);
            this.chkRequireClientCertificate.Location = new System.Drawing.Point(127, 37);
            this.chkRequireClientCertificate.Margin = new System.Windows.Forms.Padding(6);
            this.chkRequireClientCertificate.Name = "chkRequireClientCertificate";
            this.chkRequireClientCertificate.Size = new System.Drawing.Size(142, 17);
            this.chkRequireClientCertificate.TabIndex = 8;
            this.chkRequireClientCertificate.Text = "Require Client Certificate";
            this.chkRequireClientCertificate.UseVisualStyleBackColor = true;
            // 
            // chkIgnorePolicyErrors
            // 
            this.chkIgnorePolicyErrors.AutoSize = true;
            this.tlpSSL.SetColumnSpan(this.chkIgnorePolicyErrors, 2);
            this.chkIgnorePolicyErrors.Location = new System.Drawing.Point(127, 68);
            this.chkIgnorePolicyErrors.Margin = new System.Windows.Forms.Padding(6);
            this.chkIgnorePolicyErrors.Name = "chkIgnorePolicyErrors";
            this.chkIgnorePolicyErrors.Size = new System.Drawing.Size(109, 17);
            this.chkIgnorePolicyErrors.TabIndex = 9;
            this.chkIgnorePolicyErrors.Text = "Ignore SSL Errors";
            this.chkIgnorePolicyErrors.UseVisualStyleBackColor = true;
            // 
            // chkCheckRevocation
            // 
            this.chkCheckRevocation.AutoSize = true;
            this.tlpSSL.SetColumnSpan(this.chkCheckRevocation, 2);
            this.chkCheckRevocation.Location = new System.Drawing.Point(127, 99);
            this.chkCheckRevocation.Margin = new System.Windows.Forms.Padding(6);
            this.chkCheckRevocation.Name = "chkCheckRevocation";
            this.chkCheckRevocation.Size = new System.Drawing.Size(138, 17);
            this.chkCheckRevocation.TabIndex = 10;
            this.chkCheckRevocation.Text = "Check SSL Revocation";
            this.chkCheckRevocation.UseVisualStyleBackColor = true;
            // 
            // chkEnableSSL
            // 
            this.chkEnableSSL.AutoSize = true;
            this.chkEnableSSL.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkEnableSSL.Location = new System.Drawing.Point(0, 0);
            this.chkEnableSSL.Name = "chkEnableSSL";
            this.chkEnableSSL.Padding = new System.Windows.Forms.Padding(6);
            this.chkEnableSSL.Size = new System.Drawing.Size(373, 29);
            this.chkEnableSSL.TabIndex = 11;
            this.chkEnableSSL.Text = "Enable SSL";
            this.chkEnableSSL.UseVisualStyleBackColor = true;
            this.chkEnableSSL.CheckedChanged += new System.EventHandler(this.chkEnableSSL_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tlpSSL);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.chkEnableSSL);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 229);
            this.panel1.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 193);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(373, 36);
            this.panel2.TabIndex = 12;
            // 
            // SSLConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 229);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SSLConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SSLConfig";
            this.Load += new System.EventHandler(this.SSLConfig_Load);
            this.tlpSSL.ResumeLayout(false);
            this.tlpSSL.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSSLCertificate;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tlpSSL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRequireClientCertificate;
        private System.Windows.Forms.CheckBox chkIgnorePolicyErrors;
        private System.Windows.Forms.CheckBox chkCheckRevocation;
        private System.Windows.Forms.CheckBox chkEnableSSL;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}