using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public partial class downloader : Form
    {
        public string Url;
        public string SaveLocation;
        public string Format;

        private bool success;
        private bool aborting;
        //private bool cancel;

        public downloader()
        {
            InitializeComponent();
        }

        private void downloader_Load(object sender, EventArgs e)
        {
            UISync.Init(this);
            backgroundWorker1.RunWorkerAsync();
            Text = LocRm.GetString("Updating");

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string sUrlToReadFileFrom = Url;
            var url = new Uri(sUrlToReadFileFrom);
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            // gets the size of the file in bytes
            
            Int64 iSize = response.ContentLength;

            // keeps track of the total bytes downloaded so we can update the progress bar
            int iRunningByteTotal = 0;

            // use the webclient object to download the file

            using (Stream streamRemote = response.GetResponseStream())
            {
                if (streamRemote != null)
                {
                    streamRemote.ReadTimeout = 8000;
                    // loop the stream and get the file into the byte buffer
                    var byteBuffer = new byte[iSize];
                    int iByteSize;
                    while ((iByteSize = streamRemote.Read(byteBuffer, iRunningByteTotal, byteBuffer.Length - iRunningByteTotal)) > 0 && !backgroundWorker1.CancellationPending)
                    {
                        iRunningByteTotal += iByteSize;

                        // calculate the progress out of a base "100"
                        var dIndex = (double) (iRunningByteTotal);
                        var dTotal = (double) byteBuffer.Length;
                        var dProgressPercentage = (dIndex/dTotal);
                        var iProgressPercentage = (int) (dProgressPercentage*100);

                        // update the progress bar
                        backgroundWorker1.ReportProgress(iProgressPercentage);
                        int total = iRunningByteTotal;
                        UISync.Execute(() => lblProgress.Text = total + " / " + iSize);
                    }
                    if (!backgroundWorker1.CancellationPending)
                    {
                        if (SaveLocation.EndsWith(".xml"))
                        {
                            var ms = new MemoryStream(byteBuffer);
                            var doc = new XmlDocument();
                            try
                            {                               
                                doc.Load(ms);
                                doc.Save(SaveLocation);
                                success = true;

                            }
                            catch (Exception ex)
                            {
                                success = false;
                                Logger.LogException(ex);
                                DialogResult = DialogResult.Cancel;
                                aborting = true;
                            }
                            ms.Dispose();
                        }
                    }
                    else
                    {
                        Logger.LogMessage("Update cancelled");
                    }
                }
                else
                {
                    Logger.LogError("Response stream from " + Url + " failed");
                }
            }
            response.Close();
            
        }

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try { _sync.BeginInvoke(action, null); }
                catch { }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbDownloading.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (success)
            {
                DialogResult = DialogResult.OK;
            }
            Close();
        }

        private void downloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy && !aborting)
            {
                if (MessageBox.Show(this, LocRm.GetString("CancelUpdate"),LocRm.GetString("Confirm"), MessageBoxButtons.YesNo)==DialogResult.Yes)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
