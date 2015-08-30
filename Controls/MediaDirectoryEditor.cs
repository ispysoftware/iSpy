using System;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class MediaDirectoryEditor : UserControl
    {
        public configurationDirectory[] Directories;

        public MediaDirectoryEditor()
        {
            InitializeComponent();
        }

        public void Init(configurationDirectory[] ae)
        {
            Directories = ae;
            Init();

        }

        private void Init() {
           
            flpDirectories.VerticalScroll.Visible = true;
            flpDirectories.HorizontalScroll.Visible = false;
            button1.Text = LocRm.GetString("Add");
            RenderDirectoryList();
        }

        void RenderDirectoryList()
        {
            flpDirectories.Controls.Clear();
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

            var w = flpDirectories.Width - 2;


            if (Directories != null)
            {
                if (Directories.Length * DirectoryEventRow.Height >= flpDirectories.Height)
                    w = flpDirectories.Width - vertScrollWidth - 2;
                int i = 0;
                foreach (var e in Directories)
                {
                    var c = new DirectoryEventRow(e, i) {Width = w};
                    c.DirectoryEntryDelete += CDirectoryEntryDelete;
                    c.DirectoryEntryEdit += CDirectoryEntryEdit;
                    c.MouseOver += CMouseOver;
                    flpDirectories.Controls.Add(c);
                    flpDirectories.SetFlowBreak(c, true);
                    i++;
                }
            }
            
            flpDirectories.PerformLayout();
            flpDirectories.HorizontalScroll.Visible = flpDirectories.HorizontalScroll.Enabled = false;
            
        }

        void CMouseOver(object sender, EventArgs e)
        {
            foreach (var c in flpDirectories.Controls)
            {
                var o = c as DirectoryEventRow;
                if (o!=sender)
                {
                    if (o != null) o.RevertBackground();
                }
            }
        }

        void CDirectoryEntryEdit(object sender, EventArgs e)
        {       
            var oe = ((DirectoryEventRow)sender);

            string d = oe.Directory.Entry;
            var c = new MediaDirectoryConfig {Config = oe.Directory};
            if (c.ShowDialog(this)==DialogResult.OK)
            {
                Directories[oe.Index] = c.Config;
                if (d!=c.Config.Entry)
                {
                    //directory changed
                    MessageBox.Show(this, LocRm.GetString("MediaDirectoryChanged"));
                }
            }
            c.Dispose();

            
            RenderDirectoryList();
        }


        void CDirectoryEntryDelete(object sender, EventArgs e)
        {
            if (Directories.Length==1)
            {
                MessageBox.Show(this, LocRm.GetString("NeedOneMediaDirectory"));
                return;
            }
            var oe = ((DirectoryEventRow)sender);
            var lname =
                MainForm.Cameras.Where(p => p.settings.directoryIndex == oe.Directory.ID).Select(p => p.name).ToList();
            lname.AddRange(MainForm.Microphones.Where(p => p.settings.directoryIndex == oe.Directory.ID).Select(p=>p.name));

            if (lname.Count>0)
            {
                var t = lname.Aggregate("", (current, s) => current + (s + Environment.NewLine));
                MessageBox.Show(this,LocRm.GetString("ReassignAllCamerasMedia")+Environment.NewLine+t);
                return;
            }
            var l = Directories.ToList();
            l.RemoveAt(oe.Index);
            Directories = l.ToArray();
            RenderDirectoryList();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var i = MainForm.Conf.MediaDirectories.Max(p => p.ID) + 1;
            var mdc = new MediaDirectoryConfig
                          {
                              Config = new configurationDirectory {Entry = "", DeleteFilesOlderThanDays = MainForm.Conf.DeleteFilesOlderThanDays, Enable_Storage_Management = MainForm.Conf.Enable_Storage_Management, MaxMediaFolderSizeMB = MainForm.Conf.MaxMediaFolderSizeMB,StopSavingOnStorageLimit = MainForm.Conf.StopSavingOnStorageLimit, ID=i}
                          };
            if (mdc.ShowDialog(this) == DialogResult.OK)
            {
                var l = Directories.ToList();
                l.Add(mdc.Config);
                Directories = l.ToArray();
                RenderDirectoryList();
            }
        }

        private void flpDirectories_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
