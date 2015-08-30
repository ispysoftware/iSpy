using System;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class Features : Form
    {
        public Features()
        {
            InitializeComponent();
        }

        private void Features_Load(object sender, EventArgs e)
        {
            initgroups();
            Text = LocRm.GetString("Permissions");
            btnDelete.Text = LocRm.GetString("Delete");
            button2.Text = LocRm.GetString("AddGroup");
            button1.Text = LocRm.GetString("OK");
        }

        private void initgroups()
        {
            tabControl1.TabPages.Clear();
            foreach (var group in MainForm.Conf.Permissions)
            {
                tabControl1.TabPages.Add(group.name);
                int i = tabControl1.TabPages.Count;
                var pf = new PermissionsForm();
                pf.Init(group);
                tabControl1.TabPages[i-1].Controls.Add(pf);
                pf.Dock = DockStyle.Fill;
            }
            btnDelete.Enabled = false;
        }
        private void Features_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!savepages())
                e.Cancel = true;

        }

        private bool savepages()
        {
            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                var c = tabControl1.TabPages[i].Controls[0] as PermissionsForm;
                if (c != null)
                {
                    if (!c.Save())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (savepages())
            {
                var p = new Prompt(LocRm.GetString("Name"), "");
                if (p.ShowDialog(this) == DialogResult.OK)
                {
                    var t = p.Val.Replace(",","").Trim();
                    if (t != "")
                    {
                        var cg = MainForm.Conf.Permissions.ToList();
                        cg.Add(new configurationGroup {featureset = 0, name = t, password = ""});
                        MainForm.Conf.Permissions = cg.ToArray();
                        initgroups();
                    }
                }
                p.Dispose();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = tabControl1.SelectedIndex > 0;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (savepages())
            {
                var l = MainForm.Conf.Permissions.ToList();
                l.RemoveAt(tabControl1.SelectedIndex);
                MainForm.Conf.Permissions = l.ToArray();
                initgroups();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
