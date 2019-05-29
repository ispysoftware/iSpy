using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class VideoSourceAdvanced : Form
    {
        public objectsCamera Camobject;
        public VideoSourceAdvanced()
        {
            InitializeComponent();
            RenderResources();
        }

        private void VideoSourceAdvanced_Load(object sender, EventArgs e)
        {
            txtUserAgent.Text = Camobject.settings.useragent;
            txtResizeWidth.Value = Camobject.settings.resizeWidth;
            txtResizeHeight.Value = Camobject.settings.resizeHeight;
            chkHttp10.Checked = Camobject.settings.usehttp10;
            chkFBA.Checked = Camobject.settings.forcebasic;
            txtReconnect.Value = Camobject.settings.reconnectinterval;
            txtCookies.Text = Camobject.settings.cookies;
            chkCalibrate.Checked = Camobject.settings.calibrateonreconnect;
            txtHeaders.Text = Camobject.settings.headers;

            tlpFishEye.Enabled = chkFishEyeActive.Checked = Camobject.settings.FishEyeCorrect;
            
            numFocalLength.Value = Camobject.settings.FishEyeFocalLengthPX;
            numLimit.Value = Camobject.settings.FishEyeLimit;
            numScale.Value = Convert.ToDecimal(Camobject.settings.FishEyeScale);
            
            
            numTimeout.Value = Camobject.settings.timeout;
        }

        private void RenderResources()
        {
            label5.Text = LocRm.GetString("ResizeTo");
            label2.Text = LocRm.GetString("UserAgent");
            label6.Text = "X";
            label4.Text = LocRm.GetString("Seconds");
            label3.Text = LocRm.GetString("ReconnectEvery");
            label7.Text = LocRm.GetString("Headers");
            LocRm.SetString(label1,"Cookies");
            LocRm.SetString(label2, "UserAgent");
            LocRm.SetString(lblTimeout, "TimeoutMS");

            chkFBA.Text = LocRm.GetString("ForceBasic");
            chkHttp10.Text = LocRm.GetString("UseHTTP10");
            LocRm.SetString(label1, "Cookies");
            LocRm.SetString(chkCalibrate, "CalibrateOnReconnect");
            Text = LocRm.GetString("Advanced");
            tabPage1.Text = LocRm.GetString("Options");
            tabPage2.Text = LocRm.GetString("FishEye Correction");
            chkFishEyeActive.Text = LocRm.GetString("ApplyCorrection");
            LocRm.SetString(label8, "FocalLength");
            LocRm.SetString(label9, "Limit");
            LocRm.SetString(label10, "ScaleSize");
            LocRm.SetString(label11, "SettingsAppliedLive");
            LocRm.SetString(button1, "OK");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var iReconnect = (int)txtReconnect.Value;
            if (iReconnect < 30 && iReconnect != 0)
            {
                MessageBox.Show(LocRm.GetString("Validate_ReconnectInterval"), LocRm.GetString("Note"));
                return;
            }

            Camobject.settings.reconnectinterval = iReconnect;
            Camobject.settings.calibrateonreconnect = chkCalibrate.Checked;

            //wh must be even for stride calculations
            int w = Convert.ToInt32(txtResizeWidth.Value);
            if (w % 2 != 0)
                w++;
            Camobject.settings.resizeWidth = w;

            int h = Convert.ToInt32(txtResizeHeight.Value);
            if (h % 2 != 0)
                h++;

            Camobject.settings.resizeHeight = h;


            Camobject.settings.usehttp10 = chkHttp10.Checked;
            Camobject.settings.cookies = txtCookies.Text;
            Camobject.settings.forcebasic = chkFBA.Checked;
            Camobject.settings.useragent = txtUserAgent.Text;
            Camobject.settings.headers = txtHeaders.Text;

            
            Camobject.settings.timeout = (int) numTimeout.Value;
            Close();
        }


        private void chkFishEyeActive_CheckedChanged(object sender, EventArgs e)
        {
            Camobject.settings.FishEyeCorrect = chkFishEyeActive.Checked;
            tlpFishEye.Enabled = Camobject.settings.FishEyeCorrect;
        }

        private void numFocalLength_ValueChanged(object sender, EventArgs e)
        {
            Camobject.settings.FishEyeFocalLengthPX = (int)numFocalLength.Value;
        }

        private void numLimit_ValueChanged(object sender, EventArgs e)
        {
            Camobject.settings.FishEyeLimit = (int)numLimit.Value;
        }

        private void numScale_ValueChanged(object sender, EventArgs e)
        {
            Camobject.settings.FishEyeScale = Convert.ToDouble(numScale.Value);
        }
    }
}
