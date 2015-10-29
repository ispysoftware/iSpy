using System;
using System.Windows.Forms;


namespace iSpyApplication.Controls
{
    public partial class IntervalConfig : UserControl
    {
        public CameraWindow CW;
        public VolumeLevel VL;

        public IntervalConfig()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var aset = new AlertSettings();
            if (CW != null)
                aset.CamalertSettings = CW.Camobject.alerts;
            if (VL != null)
                aset.MicalertSettings = VL.Micobject.alerts;

            aset.ShowDialog(this);
            aset.Dispose();
            SetText();
        }

        private void IntervalConfig_Load(object sender, EventArgs e)
        {
            
        }

        public void Init(CameraWindow cw)
        {
            CW = cw;
            SetText();
        }

        public void Init(VolumeLevel vl)
        {
            VL = vl;
            SetText();
        }

        private void SetText()
        {
            string t = "";
            string gn = "";
            int ri = 0;
            int di = 0;
            if (CW != null)
            {
                gn = CW.Camobject.alerts.groupname;
                ri = CW.Camobject.alerts.resetinterval;
                di = CW.Camobject.alerts.minimuminterval;
            }
            if (VL != null)
            {
                gn = VL.Micobject.alerts.groupname;
                ri = VL.Micobject.alerts.resetinterval;
                di = VL.Micobject.alerts.minimuminterval;
            }

            if (!string.IsNullOrEmpty(gn))
            {
                if (ri > 0)
                {
                    t += gn+ " ("+ri+"s "+LocRm.GetString("Reset")+"), ";
                }
            }
            t += di + "s "+LocRm.GetString("Interval");

            lblInterval.Text = t;

        }


    }
}
