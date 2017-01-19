using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class GettingStarted : Form
    {
        public bool LangChanged = false;
        private WebBrowser _wbGettingStarted;
        public GettingStarted()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            _btnOk.Text = LocRm.GetString("Ok");
            chkShowGettingStarted.Text = LocRm.GetString("ShowGettingStarted");
            Text = LocRm.GetString("GettingStarted");
        }

        private void GettingStarted_Load(object sender, EventArgs e)
        {
            int i = 0;
            int selind = 0;
            foreach (TranslationsTranslationSet set in LocRm.TranslationSets.OrderBy(p => p.Name))
            {
                _ddlLanguage.Items.Add(new MainForm.ListItem(set.Name, new[] { set.CultureCode }));
                if (set.CultureCode == MainForm.Conf.Language)
                    selind = i;
                i++;
            }
            _ddlLanguage.SelectedIndex = selind;

            _wbGettingStarted = new WebBrowser();
            pnlWBContainer.Controls.Add(_wbGettingStarted);
            _wbGettingStarted.Location = new Point(0, 0);
            _wbGettingStarted.Dock = DockStyle.Fill;
            _wbGettingStarted.Navigate(MainForm.Website + "/getting_started.aspx");

            chkShowGettingStarted.Checked = MainForm.Conf.Enabled_ShowGettingStarted;
        }

        private void _ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lang = ((string[])((MainForm.ListItem)_ddlLanguage.SelectedItem).Value)[0];
            if (lang != MainForm.Conf.Language)
            {
                MainForm.Conf.Language = lang;
                LocRm.Reset();
                RenderResources();
                LangChanged = true;
            }
        }

        private void _btnOk_Click(object sender, EventArgs e)
        {
            _wbGettingStarted.Dispose();
            Close();
        }

        private void GettingStarted_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.Conf.Enabled_ShowGettingStarted = chkShowGettingStarted.Checked;
        }
    }
}
