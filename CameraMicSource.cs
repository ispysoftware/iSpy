using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class CameraMicSource : Form
    {
        public CameraWindow CameraControl;
        private bool _loaded = false;

        public CameraMicSource()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            radioButton1.Text = LocRm.GetString("None");
            radioButton2.Text = LocRm.GetString("Current");
            radioButton3.Text = LocRm.GetString("New");
            Text = LocRm.GetString("Addmicrophone");

        }

        private void CameraMicSource_Load(object sender, EventArgs e)
        {
            int micind = 0, ind = 0;
            foreach (objectsMicrophone om in MainForm.Microphones)
            {
                objectsMicrophone om1 = om;
                if (
                    MainForm.Cameras.Count(p => p.settings.micpair == om1.id && p.id != CameraControl.Camobject.id) == 0)
                {
                    ddlMic.Items.Add(new ListItem(om.name, om.id.ToString()));
                    if (CameraControl.Camobject.settings.micpair == om.id)
                    {
                        micind = ind;
                        radioButton2.Checked = true;
                    }
                    ind++;
                }
            }
            if (ddlMic.Items.Count == 0)
            {
                ddlMic.Enabled = false;
                radioButton2.Enabled = false;
            }
            else
                ddlMic.SelectedIndex = micind;

            if (!radioButton2.Checked)
                radioButton1.Checked = true;
            _loaded = true;
        }



        #region Nested type: ListItem

        private struct ListItem
        {
            private readonly string _name;
            internal readonly string Value;

            public ListItem(string name, string value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            VolumeLevel vl = CameraControl.VolumeControl;
            
            if (radioButton1.Checked)
            {
                CameraControl.Camobject.settings.micpair = -1;
            }
            if (radioButton2.Checked)
            {
                var li = (ListItem)ddlMic.SelectedItem;
                CameraControl.Camobject.settings.micpair = Convert.ToInt32(li.Value);
                LayoutPanel.NeedsRedraw = true;
            }
            if (radioButton3.Checked)
            {
                int micid = ((MainForm)this.Owner.Owner).AddMicrophone(0);
                if (micid!=-1)
                {
                    CameraControl.Camobject.settings.micpair = micid;
                    LayoutPanel.NeedsRedraw = true;
                }
            }
            if (vl!=null && vl!=CameraControl.VolumeControl)
            {
                vl.IsEdit = false;
            }
            if (CameraControl.VolumeControl!=null)
                CameraControl.VolumeControl.IsEdit = true;
            Close();
            return;
        }

        private void ddlMic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loaded && ddlMic.SelectedIndex > -1)
                    radioButton2.Checked = true;
        }
    }
}
