using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class PiPConfig : Form
    {
        public objectsCameraSettingsPip pip;
        public CameraWindow CW;
        public PiPConfig()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("PictureInPicture");
            label1.Text = LocRm.GetString("Camera");
            label2.Text = LocRm.GetString("Areas");
            chkEnable.Text = LocRm.GetString("Enable");
            btnOK.Text = LocRm.GetString("OK");
            linkLabel1.Text = LocRm.GetString("Help");

        }

        private void PiPConfig_Load(object sender, EventArgs e)
        {
            chkEnable.Checked = pip.enabled;
            tableLayoutPanel1.Enabled = chkEnable.Checked;
            areaSelector1.Areas = pip.config;

            foreach (var cam in MainForm.Cameras)
            {
                ddlCamera.Items.Add(new MainForm.ListItem(cam.name, cam.id));
            }
            if (ddlCamera.Items.Count > 0)
                ddlCamera.SelectedIndex = 0;
            areaSelector1.BoundsChanged += areaSelector1_BoundsChanged;
        }

        void areaSelector1_BoundsChanged(object sender, EventArgs e)
        {
            pip.config = areaSelector1.Areas;
            if (CW.Camera != null)
                CW.Camera.PiPConfig = pip.config;
        }

        

        private void btnOK_Click(object sender, EventArgs e)
        {
            pip.enabled = chkEnable.Checked;
            pip.config = areaSelector1.Areas;
            Close();
        }

        private void ddlCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            var li = (MainForm.ListItem) ddlCamera.SelectedItem;
            areaSelector1.CurrentCameraID = (int)li.Value;
        }

        private void chkEnable_CheckedChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.Enabled = chkEnable.Checked;
            pip.enabled = chkEnable.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-picture-in-picture.aspx");
        }
    }
}
