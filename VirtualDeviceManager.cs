using System;
using System.Globalization;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class VirtualDeviceManager : Form
    {
        private bool _loaded;
        public VirtualDeviceManager()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("DefaultDeviceManager");
            groupBox1.Text = LocRm.GetString("DefaultCameras");
            label1.Text = LocRm.GetString("SelectMultiple");
            button1.Text = LocRm.GetString("OK");
            linkLabel1.Text = LocRm.GetString("DownloadVirtualDeviceDriver");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-virtual-webcam-driver.aspx");
        }

        private void VirtualDeviceManager_Load(object sender, EventArgs e)
        {
            foreach (var cam in MainForm.Cameras)
            {
                lbCameras.Items.Add(new MainForm.ListItem(cam.name,cam.id));
            }

            var s = MainForm.Conf.DeviceDriverDefault.Split(',');
            foreach (var s2 in s)
            {
                for(int i=0;i<lbCameras.Items.Count;i++)
                {
                    var s3 = (MainForm.ListItem) lbCameras.Items[i];
                    if (((int)s3.Value).ToString(CultureInfo.InvariantCulture) == s2)
                    {
                        lbCameras.SetSelected(i,true);
                    }
                }
            }
            _loaded = true;
        }

        private void lbCameras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;
            var s = "";
            foreach (MainForm.ListItem s2 in lbCameras.SelectedItems)
            {
                s += s2.Value + ",";
            }
            MainForm.Conf.DeviceDriverDefault = s.Trim(',');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
