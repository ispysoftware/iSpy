using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class GridViewCamera : Form
    {
        public int Delay = 4;
        public List<GridViewItem> SelectedIDs;

        public GridViewCamera()
        {
            InitializeComponent();
            RenderResources();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Delay = Convert.ToInt32(numDelay.Value);
            SelectedIDs = new List<GridViewItem>();
            foreach (GridViewItem li in lbCameras.SelectedItems)
            {
                SelectedIDs.Add(li);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void GridViewCamera_Load(object sender, EventArgs e)
        {
            numDelay.Value = Delay;
            foreach(var c in MainForm.Cameras)
            {
                lbCameras.Items.Add(new GridViewItem(c.name, c.id, 2));
            }
            foreach (var c in MainForm.Microphones)
            {
                lbCameras.Items.Add(new GridViewItem(c.name, c.id, 1));
            }
            foreach (var c in MainForm.FloorPlans)
            {
                lbCameras.Items.Add(new GridViewItem(c.name, c.id, 3));
            }        

            for(int j=0;j<lbCameras.Items.Count;j++)
            {
                var li = (GridViewItem)lbCameras.Items[j];
                for(int i=0;i<SelectedIDs.Count;i++)
                {
                    if (SelectedIDs[i].ObjectID == li.ObjectID && SelectedIDs[i].TypeID == li.TypeID)
                    {
                        lbCameras.SetSelected(j, true);
                        break;
                    }
                }
            }
        }       

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedIDs = new List<GridViewItem>();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void RenderResources()
        {
            LocRm.SetString(this, "AddObjectToGrid");
            LocRm.SetString(label1, "SelectOneOrMoreObjects");
            LocRm.SetString(label2, "CycleDelay");
            LocRm.SetString(button1, "ClearAll");
            LocRm.SetString(btnOK, "OK");

        }
    }


}
