using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using iSpyApplication.Onvif;
using iSpyApplication.Utilities;

namespace iSpyApplication.Controls
{
    public partial class ONVIFWizard : UserControl
    {
        public CameraWindow CameraControl;

        public ONVIFWizard()
        {
            InitializeComponent();

            btnFind.Text = LocRm.GetString("json.discover");
            lblUsername.Text = LocRm.GetString("USername");
            lblPassword.Text = LocRm.GetString("Password");
            lblDeviceURL.Text = LocRm.GetString("NetworkAddress");
            lblTransport.Text = LocRm.GetString("json.transport");
            lblConnectWith.Text = LocRm.GetString("json.connectwith");
            btnConnect.Text = LocRm.GetString("Next");
            btnBack.Text = LocRm.GetString("Back");
            lblURL.Text = LocRm.GetString("URL");

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void SetPanel(Panel p)
        {
            HidePanel(pnlStep1);
            HidePanel(pnlStep2);

            p.Visible = true;
            p.Dock = DockStyle.Fill;
            lblStep.Text = "1.";
            if (p == pnlStep2)
                lblStep.Text = "2.";

        }

        private void HidePanel(Panel p)
        {
            p.Visible = false;
            p.Dock = DockStyle.None;
        }

        private void ONVIFWizard_Load(object sender, EventArgs e)
        {
            UISync.Init(this);
            SetPanel(pnlStep1);
            ddlTransport.SelectedIndex = 0;
            ddlConnectWith.Items.Add("FFMPEG");
            if (VlcHelper.VlcInstalled)
                ddlConnectWith.Items.Add("VLC");
            ddlConnectWith.SelectedIndex = 0;
            BindDevices();

            //Discovery.DiscoveryComplete += Discovery_DiscoveryComplete;

            if (!Discovery.DiscoveryFinished)
            {
                btnFind.Enabled = false;
                btnFind.Text = "...";
            }

            if (CameraControl != null)
            {
                txtOnvifUsername.Text = CameraControl.Camobject.settings.login;
                txtOnvifPassword.Text = CameraControl.Camobject.settings.password;
            }
        }
        #region Nested type: UISync

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try
                {
                    _sync.BeginInvoke(action, null);
                }
                catch
                {
                }
            }
        }

        #endregion

        public void Deinit()
        {
            //Discovery.DiscoveryComplete -= Discovery_DiscoveryComplete;
        }

        private void Discovery_DiscoveryComplete(object sender, EventArgs e)
        {
            UISync.Execute(() =>
                           {
                               btnFind.Enabled = true;
                               BindDevices();
                               btnFind.Text = LocRm.GetString("Find");
                           });
        }

        private void BindDevices()
        {
            ddlDeviceURL.Items.Clear();
            foreach (var s in Discovery.DiscoveredDevices)
            {
                ddlDeviceURL.Items.Add(s);
            }
            if (ddlDeviceURL.Items.Count > 0)
                ddlDeviceURL.SelectedIndex = 0;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (Discovery.DiscoveryFinished)
            {
                Discovery.FindDevices();
                SetPanel(pnlStep1);
                btnFind.Text = "...";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbOnvifURLs.Items.Clear();
            Uri uri;
            if (!Uri.TryCreate(ddlDeviceURL.Text, UriKind.Absolute, out uri))
            {
                MessageBox.Show("Invalid Address");
            }
            var url = uri.ToString();
            var paq = uri.PathAndQuery;
            if (string.IsNullOrEmpty(paq.Trim('/')))
            {
                url += "onvif/device_service";
            }
            

            var dev = url;
            var urls = new List<object>();
            var od = new ONVIFDevice(dev, txtOnvifUsername.Text, txtOnvifPassword.Text);

            if (od.Profiles == null)
            {
                MessageBox.Show(this, LocRm.GetString("ConnectFailed"), LocRm.GetString("Failed"));
            }
            else
            {
                foreach (var murl in od.MediaEndpoints)
                {
                    urls.Add(murl);
                }

                if (urls.Count > 0)
                {
                    lbOnvifURLs.Items.Clear();
                    lbOnvifURLs.Items.AddRange(urls.ToArray());
                    lbOnvifURLs.SelectedIndex = 0;
                    SetPanel(pnlStep2);
                }
                else
                {
                    MessageBox.Show(this, "No media endpoints found", LocRm.GetString("Failed"));
                }
            }
        }
    }
}
