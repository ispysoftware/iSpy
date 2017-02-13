using System;
using System.Windows.Forms;
using iSpyApplication.Controls;
using iSpyApplication.Pelco;

namespace iSpyApplication
{
    public partial class PTZConfig : Form
    {
        public CameraWindow CameraControl;
        public PTZConfig()
        {
            InitializeComponent();
            chkPTZFlipX.Text = LocRm.GetString("Flipx");
            chkPTZFlipY.Text = LocRm.GetString("Flipy");
            chkPTZRotate90.Text = LocRm.GetString("Rotate90");
            label22.Text = LocRm.GetString("Username");
            label42.Text = LocRm.GetString("Password");
            button2.Text = LocRm.GetString("OK");
            button3.Text = LocRm.GetString("Cancel");
        }

        private void chkPTZFlipX_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chkPTZFlipY_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void chkPTZRotate90_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void PTZConfig_Load(object sender, EventArgs e)
        {
            txtPTZChannel.Text = CameraControl.Camobject.settings.ptzchannel;
            txtPTZURL.Text = CameraControl.Camobject.settings.ptzurlbase;
            numPort.Value = CameraControl.Camobject.settings.ptzport;
            txtPTZUsername.Text = CameraControl.Camobject.settings.ptzusername;
            txtPTZPassword.Text = CameraControl.Camobject.settings.ptzpassword;

            chkPTZFlipX.Checked = CameraControl.Camobject.settings.ptzflipx;
            chkPTZFlipY.Checked = CameraControl.Camobject.settings.ptzflipy;
            chkPTZRotate90.Checked = CameraControl.Camobject.settings.ptzrotate90;

            bool bPelco = CameraControl.Camobject.ptz == -3 || CameraControl.Camobject.ptz == -4;
            button1.Enabled = bPelco;
        }

        private void PTZConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                CameraControl.Camobject.settings.ptzusername = txtPTZUsername.Text;
                CameraControl.Camobject.settings.ptzpassword = txtPTZPassword.Text;
                CameraControl.Camobject.settings.ptzchannel = txtPTZChannel.Text;
                CameraControl.Camobject.settings.ptzport = (int) numPort.Value;
                CameraControl.Camobject.settings.ptzflipx = chkPTZFlipX.Checked;
                CameraControl.Camobject.settings.ptzflipy = chkPTZFlipY.Checked;
                CameraControl.Camobject.settings.ptzrotate90 = chkPTZRotate90.Checked;
                CameraControl.Camobject.settings.ptzurlbase = txtPTZURL.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CameraControl.Camobject.settings.ptzpelcoconfig))
            {
                //default
                CameraControl.Camobject.settings.ptzpelcoconfig = "COM1|9600|8|One|Odd|1";
            }
            using (var pc = new PelcoConfig { Config = CameraControl.Camobject.settings.ptzpelcoconfig })
            {
                if (pc.ShowDialog(this) == DialogResult.OK)
                {
                    CameraControl.Camobject.settings.ptzpelcoconfig = pc.Config;
                    CameraControl.PTZ.ConfigurePelco();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
