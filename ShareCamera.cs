using System;
using System.Collections.Generic;
using System.Windows.Forms;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public partial class ShareCamera : Form
    {
        public ShareCamera()
        {
            InitializeComponent();
            LoadSources();
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
                Logger.LogException(ex);

            }
        }
        private HashSet<string> _hashdata;
        private void LoadSources()
        {
            var camDb = new List<AutoCompleteTextbox.TextEntry>();

            _hashdata = new HashSet<string>();

            foreach (var source in MainForm.Sources)
            {
                string name = source.name.Trim();
                if (!_hashdata.Contains(name.ToUpper()))
                {
                    camDb.Add(new AutoCompleteTextbox.TextEntry(name));
                    _hashdata.Add(name.ToUpper());
                }
                
            }


            txtMake.AutoCompleteList = camDb;
            txtMake.MinTypedCharacters = 1;
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
