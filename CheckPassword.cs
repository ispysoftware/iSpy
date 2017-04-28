using System;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public partial class CheckPassword : Form
    {
        public CheckPassword()
        {
            InitializeComponent();
            RenderResources();
        }
        
        private void DoCheckPassword()
        {
            var g = MainForm.Conf.Permissions.First(p => p.name == ddlAccount.SelectedItem.ToString());
            if (txtPassword.Text == EncDec.DecryptData(g.password, MainForm.Conf.EncryptCode))
            {
                if (MainForm.Group != g.name)
                    MainForm.NeedsResourceUpdate = true;
                MainForm.Group = g.name;
                DialogResult = DialogResult.OK;
                Logger.LogMessage("Login: "+g.name);
                Close();
                return;
            }
            
            DialogResult = DialogResult.Cancel;
            MessageBox.Show(LocRm.GetString("PasswordIncorrect"), LocRm.GetString("Note"));
            
            Close();
        }

        private void CheckPasswordLoad(object sender, EventArgs e)
        {
            txtPassword.Focus();
        }

        private void RenderResources() {
            
            Text = LocRm.GetString("ApplicationHasBeenLocked");
            button1.Text = LocRm.GetString("Unlock");
            lblPassword.Text = LocRm.GetString("Password");
            lblAccount.Text = LocRm.GetString("Account");
        }


        private void TxtPasswordKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoCheckPassword();
            }
        }

        private void CheckPassword_Shown(object sender, EventArgs e)
        {
            foreach (var g in MainForm.Conf.Permissions)
            {
                ddlAccount.Items.Add(g.name);
            }
            ddlAccount.SelectedItem = MainForm.Group;

            this.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoCheckPassword();
        }
    }
}