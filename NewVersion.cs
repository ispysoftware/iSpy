using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace iSpyApplication
{
    /// <summary>
    /// Summary description for AboutForm.
    /// </summary>
    public class NewVersion : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private Button _button1;
        private Button _button2;
        private Panel _panel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox chkCheckForUpdates;
        private WebBrowser _wbProductHistory;

        public NewVersion()
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

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void NewVersionLoad(object sender, EventArgs e)
        {
            chkCheckForUpdates.Checked = MainForm.Conf.Enable_Update_Check;
            _wbProductHistory.Navigate(MainForm.Website+"/producthistory.aspx?productid=11");
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("About");
            _button1.Text = LocRm.GetString("GetLatestVersion");
            _button2.Text = LocRm.GetString("NoThanks");
            Text = LocRm.GetString("NewVersion");
            chkCheckForUpdates.Text = LocRm.GetString("AutomaticallyCheckForUpda");
        }

        private void Button2Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/download.aspx");
            //MessageBox.Show(LocRm.GetString("ExportWarning"), LocRm.GetString("Note"));
            Close();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewVersion));
            this._wbProductHistory = new System.Windows.Forms.WebBrowser();
            this._panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this._button1 = new System.Windows.Forms.Button();
            this._button2 = new System.Windows.Forms.Button();
            this.chkCheckForUpdates = new System.Windows.Forms.CheckBox();
            this._panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _wbProductHistory
            // 
            this._wbProductHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this._wbProductHistory.Location = new System.Drawing.Point(0, 0);
            this._wbProductHistory.MinimumSize = new System.Drawing.Size(20, 20);
            this._wbProductHistory.Name = "_wbProductHistory";
            this._wbProductHistory.Size = new System.Drawing.Size(540, 301);
            this._wbProductHistory.TabIndex = 0;
            // 
            // _panel1
            // 
            this._panel1.Controls.Add(this.flowLayoutPanel1);
            this._panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panel1.Location = new System.Drawing.Point(0, 301);
            this._panel1.Name = "_panel1";
            this._panel1.Size = new System.Drawing.Size(540, 34);
            this._panel1.TabIndex = 1;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this._button1);
            this.flowLayoutPanel1.Controls.Add(this._button2);
            this.flowLayoutPanel1.Controls.Add(this.chkCheckForUpdates);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(540, 34);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // _button1
            // 
            this._button1.AutoSize = true;
            this._button1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._button1.Location = new System.Drawing.Point(438, 3);
            this._button1.Name = "_button1";
            this._button1.Size = new System.Drawing.Size(99, 23);
            this._button1.TabIndex = 0;
            this._button1.Text = "Get latest version";
            this._button1.UseVisualStyleBackColor = true;
            this._button1.Click += new System.EventHandler(this.Button1Click);
            // 
            // _button2
            // 
            this._button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._button2.AutoSize = true;
            this._button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this._button2.Location = new System.Drawing.Point(362, 3);
            this._button2.Name = "_button2";
            this._button2.Size = new System.Drawing.Size(70, 23);
            this._button2.TabIndex = 1;
            this._button2.Text = "No Thanks";
            this._button2.UseVisualStyleBackColor = true;
            this._button2.Click += new System.EventHandler(this.Button2Click);
            // 
            // chkCheckForUpdates
            // 
            this.chkCheckForUpdates.AutoSize = true;
            this.chkCheckForUpdates.Location = new System.Drawing.Point(176, 6);
            this.chkCheckForUpdates.Margin = new System.Windows.Forms.Padding(6);
            this.chkCheckForUpdates.Name = "chkCheckForUpdates";
            this.chkCheckForUpdates.Size = new System.Drawing.Size(177, 17);
            this.chkCheckForUpdates.TabIndex = 2;
            this.chkCheckForUpdates.Text = "Automatically check for updates";
            this.chkCheckForUpdates.UseVisualStyleBackColor = true;
            this.chkCheckForUpdates.CheckedChanged += new System.EventHandler(this.chkCheckForUpdates_CheckedChanged);
            // 
            // NewVersion
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(540, 335);
            this.Controls.Add(this._wbProductHistory);
            this.Controls.Add(this._panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewVersion";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Version";
            this.Load += new System.EventHandler(this.NewVersionLoad);
            this._panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private void chkCheckForUpdates_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.Conf.Enable_Update_Check = chkCheckForUpdates.Checked;
        }


    }
}