using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class ShareCamera : Form
    {
        public ShareCamera()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string make = txtMake.Text.Trim();
            string model = txtModel.Text.Trim();

            if (make=="" || model=="")
            {
                MessageBox.Show(this, LocRm.GetString("EnterMakeAndModel"));
                return;
            }
            btnAdd.Enabled = false;
            AddCameraToDatabase(make, model, FindCameras.LastConfig.Prefix, FindCameras.LastConfig.Source,
                       FindCameras.LastConfig.URL, FindCameras.LastConfig.Cookies, FindCameras.LastConfig.Flags, FindCameras.LastConfig.Port);
            Close();
        }

        private static void AddCameraToDatabase(string type, string model, string prefix, string source, string url, string cookies, string flags, int port)
        {
            try
            {
                var r = new Reporting.Reporting { Timeout = 8000 };
                r.AddCamera2(type, model, prefix, source, url, cookies, flags, port);
                r.Dispose();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);

            }
        }

        private void ShareCamera_Load(object sender, EventArgs e)
        {
            txtMake.Text = FindCameras.LastConfig.Iptype;
            txtModel.Text = FindCameras.LastConfig.Ipmodel;
            lblType.Text = FindCameras.LastConfig.Source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
