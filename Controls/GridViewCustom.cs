using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class GridViewCustom : Form
    {
        public int Cols = 1;
        public int Rows = 1;
        public string GridName;
        public bool FullScreen = false;
        public bool Overlays = true;
        public bool AlwaysOnTop = false;
        public string Display;
        public int Framerate = 5;
        public int Mode = 0;
        public bool Fill = true;
        public string ModeConfig = "";
        public bool ShowAtStartup = false;

        public GridViewCustom()
        {
            InitializeComponent();
            RenderResources();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (GridName == null && (string.IsNullOrEmpty(txtName.Text.Trim()) || MainForm.Conf.GridViews.Any(p => p.name.ToLower() == txtName.Text.Trim().ToLower())))
            {
                MessageBox.Show(this, LocRm.GetString("validate_uniquename"));
                return;
            }
            Rows = (int)numRows.Value;
            Cols = (int)numCols.Value;
            GridName = txtName.Text.Trim();
            FullScreen = chkFullScreen.Checked;
            Overlays = chkOverlays.Checked;
            AlwaysOnTop = chkAlwaysOnTop.Checked;
            Display = cmbDisplay.Text;
            Framerate = (int)numFramerate.Value;
            Mode = ddlMode.SelectedIndex;
            Fill = chkFill.Checked;
            ShowAtStartup = chkShowOnLoad.Checked;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void GridViewCustom_Load(object sender, EventArgs e)
        {
            cmbDisplay.DataSource = Screen.AllScreens;
            cmbDisplay.DisplayMember = "DeviceName";

            numRows.Value = Rows;
            numCols.Value = Cols;
            txtName.Text = GridName;
            chkFullScreen.Checked = FullScreen;
            chkAlwaysOnTop.Checked = AlwaysOnTop;
            cmbDisplay.Text = Display;
            chkOverlays.Checked = Overlays;
            numFramerate.Value = Framerate;
            ddlMode.SelectedIndex = Mode;
            chkFill.Checked = Fill;
            chkShowOnLoad.Checked = ShowAtStartup;

            if (!string.IsNullOrEmpty(txtName.Text))
                return;
            
            int i = 1;
            bool cont = false;
            while(!cont)
            {
                cont = true;
                foreach (var g in MainForm.Conf.GridViews)
                {
                    if (g.name == "Grid "+i)
                        cont = false;
                }
                if (!cont)
                    i++;
            }
            txtName.Text = "Grid " + i;
        }

        private void RenderResources()
        {
            LocRm.SetString(this,"CustomiseGrid");
            LocRm.SetString(label3, "Name");
            LocRm.SetString(label1, "Mode");
            LocRm.SetString(label6, "Columns");
            LocRm.SetString(label2, "Rows");
            LocRm.SetString(chkFullScreen, "FullScreen");
            LocRm.SetString(chkAlwaysOnTop, "AlwaysOnTop");
            LocRm.SetString(chkFill, "FillArea");
            LocRm.SetString(label5, "Framerate");
            LocRm.SetString(label4, "Display");
            LocRm.SetString(chkShowOnLoad, "ShowOnLoad");
            LocRm.SetString(chkOverlays,"Overlays");
            LocRm.SetString(button1,"OK");

        }

        private void ddlMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            numCols.Enabled = numRows.Enabled = ddlMode.SelectedIndex == 0;
            btnConfigure.Enabled = ddlMode.SelectedIndex > 0;
        }

        private void btnConfigure_Click(object sender, EventArgs e)
        {
            var gvmc = new GridViewModeConfig { ModeConfig = ModeConfig };
            gvmc.ShowDialog(this);
            ModeConfig = gvmc.ModeConfig;
            gvmc.Dispose();
        }
    }
}
