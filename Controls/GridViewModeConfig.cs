using System;
using System.Globalization;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class GridViewModeConfig : Form
    {
        public string ModeConfig;
        public GridViewModeConfig()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("Configuration");
            LocRm.SetString(label1,"DefaultCamera");
            LocRm.SetString(label2, "RemoveDelay");
            LocRm.SetString(label3, "MaxItems");
            LocRm.SetString(chkRestore, "MaximseAndRestore");
            LocRm.SetString(button1, "OK");
        }

        private void GridViewModeConfig_Load(object sender, EventArgs e)
        {
            int rd = 10;
            int id = -1;
            int mi = 16;
            bool sr = false;
            if (!string.IsNullOrEmpty(ModeConfig))
            {
                string[] cfg = ModeConfig.Split(',');
                if (cfg.Length >= 2)
                {
                    rd = Convert.ToInt32(cfg[0]);
                    if (cfg[1]!="")
                        id = Convert.ToInt32(cfg[1]);
                }
                if (cfg.Length >= 3)
                {
                    mi = Convert.ToInt32(cfg[2]);
                }
                if (cfg.Length >= 4)
                {
                    Boolean.TryParse(cfg[3], out sr);
                }
            }

            int i = 1, j = 0;
            foreach (var c in MainForm.Cameras)
            {
                ddlDefault.Items.Add(new MainForm.ListItem(c.name, c.id));
                if (c.id == id)
                    j = i;
                i++;
            }
            ddlDefault.SelectedIndex = j;
            numRemoveDelay.Value = rd;
            numMaxItems.Value = mi;
            chkRestore.Checked = sr;
        }

        private void GridViewModeConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cfg = numRemoveDelay.Value.ToString(CultureInfo.InvariantCulture) + ",";
            if (ddlDefault.SelectedIndex > 0)
            {
                var o = (MainForm.ListItem)ddlDefault.SelectedItem;
                cfg += o.Value;
            }
            else
            {
                cfg += ",";
            }
            cfg += numMaxItems.Value.ToString(CultureInfo.InvariantCulture);
            cfg += "," + chkRestore.Checked.ToString(CultureInfo.InvariantCulture);

            ModeConfig = cfg;
            Close();
        }
    }
}
