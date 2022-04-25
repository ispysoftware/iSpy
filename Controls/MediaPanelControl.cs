using System;
using System.Drawing;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class MediaPanelControl : UserControl
    {
        public MainForm MainClass;

        public MediaPanelControl()
        {
            InitializeComponent();
        }

        private void lblPage_Click(object sender, EventArgs e)
        {
            MainClass.MediaPage();
        }

        public void RenderResources()
        {
            toolTip1.SetToolTip(mpcbSelect,LocRm.GetString("SelectAll"));
            toolTip1.SetToolTip(mpcbArchive,LocRm.GetString("Archive"));
            toolTip1.SetToolTip(mpcbDelete,LocRm.GetString("Delete"));
            toolTip1.SetToolTip(mpcbFilter,LocRm.GetString("Filter"));
            toolTip1.SetToolTip(mpcbNext, LocRm.GetString("Next"));
            toolTip1.SetToolTip(mpcbPrevious, LocRm.GetString("Previous"));
            toolTip1.SetToolTip(mpcbCloud, LocRm.GetString("UploadToCloud"));
            toolTip1.SetToolTip(mpcbMerge, LocRm.GetString("Merge"));
            mpcbMerge.Visible = Helper.HasFeature(Enums.Features.Recording);
        }

        private void mpcbSelect_Click(object sender, EventArgs e)
        {
            MainClass.MediaSelectAll();
        }

        private void mpcbDelete_Click(object sender, EventArgs e)
        {
            MainClass.MediaDeleteSelected();
        }

        private void mpcbArchive_Click(object sender, EventArgs e)
        {
            MainClass.MediaArchiveSelected();
        }

        private void mpcbFilter_Click(object sender, EventArgs e)
        {
            MainClass.MediaFilter();
        }

        private void mpcbPrevious_Click(object sender, EventArgs e)
        {
            MainClass.MediaBack();
        }

        private void mpcbNext_Click(object sender, EventArgs e)
        {
            MainClass.MediaNext();
        }

        private void mpcbCloud_Click(object sender, EventArgs e)
        {
            MainClass.MediaUploadCloud();
        }

        private void mpcbMerge_Click(object sender, EventArgs e)
        {
            MainClass.MergeMedia();
        }

        private void mpcbYouTube_Click(object sender, EventArgs e)
        {
            //MainClass.MediaUploadYouTube();
        }
    }
}
