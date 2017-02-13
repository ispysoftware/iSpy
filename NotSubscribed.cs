using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class NotSubscribed : Form
    {
        public NotSubscribed()
        {
            InitializeComponent();
            Text = LocRm.GetString("AccessDenied");
            llblSubscribe.Text = LocRm.GetString("MoreInformation");
            lblInfo.Text = LocRm.GetString("NotSubscribed");
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void NotSubscribed_Load(object sender, EventArgs e)
        {

        }

        private void llblSubscribe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Webserver + "/subscribe.aspx");
        }
    }
}
