using System;
using System.IO;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class SSLConfig : Form
    {
        public SSLConfig()
        {
            InitializeComponent();
            RenderResources();
        }

        private void SSLConfig_Load(object sender, EventArgs e)
        {
            _lastPath = txtSSLCertificate.Text = MainForm.Conf.SSLCertificate;
            chkRequireClientCertificate.Checked = MainForm.Conf.SSLClientRequired;
            chkIgnorePolicyErrors.Checked = MainForm.Conf.SSLIgnoreErrors;
            chkCheckRevocation.Checked = MainForm.Conf.SSLCheckRevocation;
            tlpSSL.Enabled = chkEnableSSL.Checked = MainForm.Conf.SSLEnabled;
        }

        private void RenderResources()
        {
            label1.Text = LocRm.GetString("Certificate");
            chkRequireClientCertificate.Text = LocRm.GetString("RequireClientCertificate");
            chkIgnorePolicyErrors.Text = LocRm.GetString("IgnoreSSLErrors");
            btnOK.Text = LocRm.GetString("OK");
            Text = LocRm.GetString("SSLConfiguration");
            chkCheckRevocation.Text = LocRm.GetString("SSLCheckRevocation");
            chkEnableSSL.Text = LocRm.GetString("EnableSSL");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSSLCertificate.Text))
                chkEnableSSL.Checked = false;
            MainForm.Conf.SSLEnabled = chkEnableSSL.Checked;
            MainForm.Conf.SSLCertificate = txtSSLCertificate.Text;
            MainForm.Conf.SSLClientRequired = chkRequireClientCertificate.Checked;
            MainForm.Conf.SSLIgnoreErrors = chkIgnorePolicyErrors.Checked;
            MainForm.Conf.SSLCheckRevocation = chkCheckRevocation.Checked;
            Close();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private string _lastPath = "";

        private void button2_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "Certificate Files|*.cer";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {
                        string res = X509.LoadCertificate(fileName);
                        if (res=="OK")
                            txtSSLCertificate.Text = fileName;
                        else
                        {
                            MessageBox.Show(this, res);
                        }
                    }
                }
            }
        }

        private void chkEnableSSL_CheckedChanged(object sender, EventArgs e)
        {
            tlpSSL.Enabled = chkEnableSSL.Checked;
        }
    }
}
