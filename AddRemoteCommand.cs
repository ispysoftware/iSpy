using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class AddRemoteCommand : Form
    {
        public objectsCommand OC = null;

        public AddRemoteCommand()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            button3.Text = "...";
            label1.Text = LocRm.GetString("Name");
            btnAddCommand.Text = LocRm.GetString("Add");
            label83.Text = LocRm.GetString("ExecuteFile");
            llblHelp.Text = LocRm.GetString("help");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var ofdDetect = new OpenFileDialog { FileName = "", InitialDirectory = Program.AppPath + @"sounds\" };
            ofdDetect.ShowDialog(this);
            if (ofdDetect.FileName != "")
            {
                txtExecute.Text = ofdDetect.FileName;
            }
            ofdDetect.Dispose();
        }

        private void btnAddCommand_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string execute = txtExecute.Text.Trim();

            if (OC != null)
            {
                OC.name = name;
                OC.command = execute;
                OC.emitshortcut = txtShortcutKeys.Text.Trim();
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (MainForm.RemoteCommands.SingleOrDefault(p => p.name == name) != null)
            {
                MessageBox.Show(LocRm.GetString("UniqueNameCommand"));
                return;
            }

            var oc = new objectsCommand { name = name, command = execute, id = MainForm.NextCommandId, emitshortcut = txtShortcutKeys.Text.Trim()};

            MainForm.RemoteCommands.Add(oc);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtShortcutKeys_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Back)
            {
                Keys modifierKeys = e.Modifiers;
                Keys pressedKey = e.KeyData ^ modifierKeys;

                if (modifierKeys != Keys.None && pressedKey != Keys.None)
                {
                    var converter = new KeysConverter();
                    txtShortcutKeys.Text = converter.ConvertToString(e.KeyData);
                }
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
                e.SuppressKeyPress = true;

                txtShortcutKeys.Text = "";
            }
            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-remotecommands.aspx");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-commandline.aspx");
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void AddRemoteCommand_Load(object sender, EventArgs e)
        {
            if (OC != null)
            {
                txtExecute.Text = OC.command;
                txtShortcutKeys.Text = OC.emitshortcut;
                if (OC.name.StartsWith("cmd_"))
                    txtName.Text = LocRm.GetString(OC.name);
                else
                    txtName.Text = OC.name;

                Text = btnAddCommand.Text = LocRm.GetString("Update");
                
            }
        }
    }
}
