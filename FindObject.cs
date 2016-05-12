using System;
using System.Linq;
using System.Windows.Forms;
namespace iSpyApplication
{
    public partial class FindObject : Form
    {
        public FindObject()
        {
            InitializeComponent();
        }

        private void FindObject_Activated(object sender, EventArgs e)
        {
            LoadSources();
        }

        private void LoadSources()
        {
            ddlObject.Items.Clear();
            ddlObject.Items.Add(LocRm.GetString("PleaseSelect"));
            var l = ((MainForm)Owner).ControlList;
            foreach (var c in l)
            {
                string name = c.ObjectName.Trim();
                ddlObject.Items.Add(name);
            }
            ddlObject.SelectedIndex = 0;
        }

        private void FindObject_Load(object sender, EventArgs e)
        {
            Text = LocRm.GetString("Find");
        }

        private void ddlObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlObject.SelectedIndex > 0)
            {
                var l = ((MainForm)Owner).ControlList;
                var t = l.FirstOrDefault(p => String.Equals(p.ObjectName, ddlObject.SelectedItem.ToString(), StringComparison.CurrentCultureIgnoreCase));
                if (t != null)
                {
                    ((Control)t).Focus();
                    ((MainForm)Owner)._pnlCameras.ClearHighlights();
                    t.Highlighted = true;
                }

                
                ddlObject.SelectedIndex = 0;
            }
        }
    }
}
