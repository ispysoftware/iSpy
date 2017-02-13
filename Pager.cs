using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class Pager : Form
    {
        public Pager()
        {
            InitializeComponent();
        }

        private void Pager_Load(object sender, EventArgs e)
        {
            Text = LocRm.GetString("Page");
            numPage.Minimum = 1;
            numPage.Maximum = (MainForm.MasterFileList.Count - 1)/MainForm.Conf.PreviewItems + 1;
            if (MainForm.MediaPanelPage + 1<=numPage.Maximum)
                numPage.Value = MainForm.MediaPanelPage+1;
        }

        private void Pager_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.MediaPanelPage = ((int)numPage.Value)-1;
        }

        private void numPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
                Close();
        }
    }
}
