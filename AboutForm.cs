using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace iSpyApplication
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class AboutForm : Form
    {
        private Label _lblCopyright;
        private PictureBox _pictureBox1;
        private LinkLabel _linkLabel2;
        private Label _lblVersion;
        private Button _btnOk;
        private LinkLabel linkLabel1;
        private TableLayoutPanel tableLayoutPanel1;
        
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		public AboutForm( )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            RenderResources();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
        }

        private void RenderResources()
        {
            _lblVersion.Text = string.Format("iSpy v{0}", Application.ProductVersion);
            if (Program.Platform != "x86")
                _lblVersion.Text = string.Format("iSpy 64 v{0}", Application.ProductVersion);

            Helper.SetTitle(this);

            _lblCopyright.Text = "Copyright " + Helper.Now.Year;

            linkLabel1.Visible = false;

            if (!String.IsNullOrEmpty(MainForm.Conf.Vendor))
            {
                linkLabel1.Visible = true;
                try
                {
                    linkLabel1.Text = MainForm.Conf.Vendor;
                }
                catch
                {
                }
            }
        }

       

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._lblCopyright = new System.Windows.Forms.Label();
            this._pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this._btnOk = new System.Windows.Forms.Button();
            this._linkLabel2 = new System.Windows.Forms.LinkLabel();
            this._lblVersion = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _lblCopyright
            // 
            this._lblCopyright.Location = new System.Drawing.Point(139, 22);
            this._lblCopyright.Name = "_lblCopyright";
            this._lblCopyright.Size = new System.Drawing.Size(165, 16);
            this._lblCopyright.TabIndex = 13;
            this._lblCopyright.Text = "Copyright © 2011 iSpyConnect.com";
            this._lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _pictureBox1
            // 
            this._pictureBox1.Image = global::iSpyApplication.Properties.Resources.ispy;
            this._pictureBox1.Location = new System.Drawing.Point(3, 3);
            this._pictureBox1.Name = "_pictureBox1";
            this.tableLayoutPanel1.SetRowSpan(this._pictureBox1, 6);
            this._pictureBox1.Size = new System.Drawing.Size(127, 123);
            this._pictureBox1.TabIndex = 17;
            this._pictureBox1.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(139, 44);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Padding = new System.Windows.Forms.Padding(3);
            this.linkLabel1.Size = new System.Drawing.Size(67, 19);
            this.linkLabel1.TabIndex = 21;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "llblBusiness";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // _btnOk
            // 
            this._btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnOk.Location = new System.Drawing.Point(263, 103);
            this._btnOk.Name = "_btnOk";
            this._btnOk.Size = new System.Drawing.Size(75, 23);
            this._btnOk.TabIndex = 19;
            this._btnOk.Text = "OK";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // _linkLabel2
            // 
            this._linkLabel2.AutoSize = true;
            this._linkLabel2.Location = new System.Drawing.Point(139, 66);
            this._linkLabel2.Name = "_linkLabel2";
            this._linkLabel2.Padding = new System.Windows.Forms.Padding(3);
            this._linkLabel2.Size = new System.Drawing.Size(151, 19);
            this._linkLabel2.TabIndex = 20;
            this._linkLabel2.TabStop = true;
            this._linkLabel2.Text = "http://www.ispyconnect.com";
            this._linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel2LinkClicked);
            // 
            // _lblVersion
            // 
            this._lblVersion.AutoSize = true;
            this._lblVersion.Location = new System.Drawing.Point(139, 0);
            this._lblVersion.Name = "_lblVersion";
            this._lblVersion.Size = new System.Drawing.Size(42, 13);
            this._lblVersion.TabIndex = 18;
            this._lblVersion.Text = "Version";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.17595F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.82405F));
            this.tableLayoutPanel1.Controls.Add(this._pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._lblVersion, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this._linkLabel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this._lblCopyright, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this._btnOk, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.linkLabel1, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 8);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(341, 129);
            this.tableLayoutPanel1.TabIndex = 22;
            // 
            // AboutForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(357, 145);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.Load += new System.EventHandler(this.AboutFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this._pictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

        private void AboutFormLoad(object sender, EventArgs e)
        {

        }

        private void LinkLabel2LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.StartBrowser(MainForm.Website+"/");
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.StartBrowser(MainForm.PurchaseLink);
        }

        
	}
}
