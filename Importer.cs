using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class Importer : Form
    {
        public MainForm mainForm;
        public Importer()
        {
            InitializeComponent();
            this.Text = LocRm.GetString("ImportObjects");
            label1.Text = LocRm.GetString("File");
            button2.Text = LocRm.GetString("OK");
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void Importer_Load(object sender, EventArgs e)
        {
            mainForm = (MainForm) Owner;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var itm in clbAdd.CheckedItems)
            {
                var o = itm as clb;
                if (o?.C != null)
                {
                    var a = _c.actions.entries.Where(p => p.objecttypeid == 2 && p.objectid == o.C.id).ToList();
                    o.C.id = MainForm.NextCameraId;
                    o.C.settings.micpair = -1;
                    MainForm.AddObject(o.C);

                    foreach (var ent in a)
                    {
                        ent.objectid = o.C.id;
                        MainForm.AddObject(ent);
                    }
                                       
                    mainForm.DisplayCamera(o.C, true);
                }
                //if (o?.M != null)
                //{
                //    o.M.id = MainForm.NextMicrophoneId;
                //    MainForm.AddObject(o.M);
                //    mainForm.DisplayMicrophone(o.M);
                //}
            }
            if (MainForm.Conf.AutoLayout)
                mainForm._pnlCameras.LayoutObjects(0, 0);
            Close();
        }

        private string _lastPath = "";

        private void button1_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = _lastPath;
                ofd.Filter = "iSpy Files (*.ispy)|*.ispy|XML Files (*.xml)|*.xml";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string fileName = ofd.FileName;
                    try
                    {
                        var fi = new FileInfo(fileName);
                        _lastPath = fi.DirectoryName;
                    }
                    catch
                    {
                    }


                    if (fileName.Trim() != "")
                    {

                        try
                        {
                            _c = MainForm.GetObjects(fileName.Trim());
                            ListObjects(_c);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, LocRm.GetString("Error"));
                        }
                    }
                }
            }
        }

        private objects _c;

        private void ListObjects(objects c)
        {
            clbAdd.Items.Clear();
            foreach (var cam in c.cameras)
            {
                clbAdd.Items.Add(new clb(cam));
            }
        }

        private class clb
        {
            public clb(objectsCamera c)
            {
                C = c;
            }

            //public clb(objectsMicrophone m)
            //{
            //    M = m;
            //}
            public objectsCamera C;
            //public objectsMicrophone M;
            public override string ToString()
            {
                return C.name;//!=null?C.name:M.name;
            }
        }
    }
}
