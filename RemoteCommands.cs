using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class RemoteCommands : Form
    {
        public RemoteCommands()
        {
            InitializeComponent();
            RenderResources();
        }

        private void ManualAlertsLoad(object sender, EventArgs e)
        {
            RenderCommands();

            if (lbManualAlerts.Items.Count > 0)
                lbManualAlerts.SelectedIndex = 0;
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("RemoteCommands");
            btnAddCommand.Text = LocRm.GetString("Add");
            btnDelete.Text = LocRm.GetString("Delete");
            btnEditCommand.Text = LocRm.GetString("Edit");
            label45.Text = LocRm.GetString("forExamples");
            label82.Text = LocRm.GetString("YouCanTriggerRemoteComman");
            linkLabel3.Text = LocRm.GetString("Reset");
            
        }


        private void RenderCommands()
        {
            lbManualAlerts.Items.Clear();
            foreach (objectsCommand oc in MainForm.RemoteCommands)
            {
                string n = oc.name;
                if (n.StartsWith("cmd_"))
                {
                    n = LocRm.GetString(oc.name);
                }
                lbManualAlerts.Items.Add(new MainForm.ListItem(n,oc.id));
            }
        }

        private void BtnAddCommandClick(object sender, EventArgs e)
        {
            using (var arc = new AddRemoteCommand())
            {
                if (arc.ShowDialog(this) == DialogResult.OK)
                {
                    RenderCommands();
                }
                
            }           
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            if (lbManualAlerts.SelectedIndex > -1)
            {
                var c = (MainForm.ListItem) lbManualAlerts.SelectedItem;
                objectsCommand oc = MainForm.RemoteCommands.FirstOrDefault(p => p.id == (int)c.Value);
                if (oc != null)
                {
                    MainForm.RemoteCommands.Remove(oc);
                    RenderCommands();
                }
            }
        }

        private void lbManualAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbManualAlerts.SelectedIndex>-1)
            {
                var c = (MainForm.ListItem)lbManualAlerts.SelectedItem;
                objectsCommand oc = MainForm.RemoteCommands.FirstOrDefault(p => p.id == (int)c.Value);
                if (oc != null)
                {
                    string s = oc.command;
                    if (!string.IsNullOrEmpty(oc.emitshortcut))
                    {
                        if (oc.emitshortcut != "")
                            s = oc.emitshortcut + " & " + oc.command;
                    }
                    lblCommand.Text = s;
                }

            }
            btnDelete.Enabled = btnEditCommand.Enabled = lbManualAlerts.SelectedIndex > -1;
        }
        
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show(LocRm.GetString("AreYouSure"), LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            MainForm.RemoteCommands = MainForm.GenerateRemoteCommands().ToList();
            RenderCommands();
        }

        private void btnEditCommand_Click(object sender, EventArgs e)
        {
            if (lbManualAlerts.SelectedIndex > -1)
            {
                var c = (MainForm.ListItem)lbManualAlerts.SelectedItem;
                objectsCommand oc = MainForm.RemoteCommands.FirstOrDefault(p => p.id == (int)c.Value);
                if (oc != null)
                {
                    using (var arc = new AddRemoteCommand {OC = oc})
                    {
                        if (arc.ShowDialog(this) == DialogResult.OK)
                        {
                            RenderCommands();
                        }

                    }           
                }
            }
        }
    }
}