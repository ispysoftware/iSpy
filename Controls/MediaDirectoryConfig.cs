using System;
using System.IO;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class MediaDirectoryConfig : Form
    {
        public configurationDirectory Config;
        public FolderSelectDialog Fsd = new FolderSelectDialog();


        public MediaDirectoryConfig()
        {
            InitializeComponent();
            chkStorage.Text = LocRm.GetString("StorageManagement");
            gbStorage.Text = LocRm.GetString("StorageManagement");
            label9.Text = LocRm.GetString("Mb");
            label10.Text = LocRm.GetString("MaxMediaFolderSize");
            label11.Text = LocRm.GetString("WhenOver70FullDeleteFiles");
            label12.Text = LocRm.GetString("DaysOld0ForNoDeletions");
            label1.Text = LocRm.GetString("Directory");
            chkStopRecording.Text = LocRm.GetString("StopRecordingOnLimit");
            chkArchive.Text = LocRm.GetString("ArchiveInsteadOfDelete");
            Text = LocRm.GetString("MediaDirectoryConfiguration");
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void chkStorage_CheckedChanged(object sender, EventArgs e)
        {
            gbStorage.Enabled = chkStorage.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtMediaDirectory.Text))
            {
                Config.Enable_Storage_Management = chkStorage.Checked;
                Config.Entry = txtMediaDirectory.Text;
                Config.MaxMediaFolderSizeMB = (int) txtMaxMediaSize.Value;
                Config.DeleteFilesOlderThanDays = (int) txtDaysDelete.Value;
                Config.StopSavingOnStorageLimit = chkStopRecording.Checked;
                Config.archive = chkArchive.Checked;
                if (!Config.Enable_Storage_Management)
                    Config.StopSavingFlag = false;
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this, LocRm.GetString("MediaDirectoryNotFound"));
            }
            
        }

        private void MediaDirectoryConfig_Load(object sender, EventArgs e)
        {
            gbStorage.Enabled = chkStorage.Checked = Config.Enable_Storage_Management;
            txtMediaDirectory.Text = Config.Entry;
            txtMaxMediaSize.Value = Config.MaxMediaFolderSizeMB;
            txtDaysDelete.Value = Config.DeleteFilesOlderThanDays;
            chkStopRecording.Checked = Config.StopSavingOnStorageLimit;
            chkArchive.Checked = Config.archive;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string f = GetFolder(Config.Entry);
            if (f != "")
                txtMediaDirectory.Text = f;
        }

        private string GetFolder(string initialPath)
        {
            string f = "";
            if (!string.IsNullOrEmpty(initialPath))
            {
                try
                {
                    Fsd.InitialDirectory = initialPath;
                }
                catch
                {

                }
            }
            
            
            if (Fsd.ShowDialog(Handle))
            {
                bool success = false;
                try
                {
                    string path = Fsd.FileName;
                    if (!path.EndsWith("\\"))
                        path += "\\";
                    Directory.CreateDirectory(path + "video");
                    Directory.CreateDirectory(path + "audio");
                    success = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (success)
                {
                    f = Fsd.FileName;
                    if (!f.EndsWith(@"\"))
                        f += @"\";

                }
            }
            return f;
        }
    }
}
