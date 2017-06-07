using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class Authorizer : Form
    {
        public string AuthCode = "";
        public string URL = "";
        public Authorizer()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AuthCode = txtCode.Text;
            Close();
        }

        private void Authorizer_Load(object sender, EventArgs e)
        {
            lblCode.Text = LocRm.GetString("Code");
            btnOK.Text = LocRm.GetString("OK");
            llURL.Text = URL;
            label2.Text = LocRm.GetString("AuthoriseInstructions");
        }

        private void llURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(llURL.Text);
        }
    }
}
