using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AForge.Imaging.Filters;
using System.Linq;
using iSpyApplication.Controls;
using iSpyApplication.Utilities;

namespace iSpyApplication
{
    public partial class AddFloorPlan : Form
    {
        public FloorPlanControl Fpc;
        private List<objectsFloorplanObjectsEntry> _floorPlanEntries;
        private objectsFloorplanObjectsEntry _movingObject;
        private objectsFloorplanObjectsEntry _fovingObject;
        private objectsFloorplanObjectsEntry _radialObject;

        public MainForm MainClass;
        
        private Bitmap _floorPlanImage;
        private Controls.PictureBox _pnlPlan;

        public AddFloorPlan()
        {
            InitializeComponent();
            RenderResources();
        }

        private void AddFloorPlanLoad(object sender, EventArgs e)
        {
            txtName.Text = Fpc.Fpobject.name;

            if (Fpc.Fpobject.image != "")
            {
                try
                {
                    _floorPlanImage = (Bitmap)Image.FromFile(Fpc.Fpobject.image);
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                }
            }

            lbObjects.AllowDrop = true;
            _pnlPlan = new Controls.PictureBox {Location = new Point(64, 0), Size = new Size(533, 400), AllowDrop = true};
            pnlFloorPlan.Controls.Add(_pnlPlan);
            
            _floorPlanEntries = new List<objectsFloorplanObjectsEntry>();

            foreach (objectsFloorplanObjectsEntry fpobj in Fpc.Fpobject.objects.@object)
            {
                _floorPlanEntries.Add(fpobj);
            }

            ShowObjects();

            _pnlPlan.DragDrop += PnlPlanDragDrop;
            _pnlPlan.DragEnter += PnlPlanDragEnter;
            _pnlPlan.DragOver += PnlPlanDragOver;
            _pnlPlan.MouseDown += PMouseDown;
            _pnlPlan.MouseUp += PMouseUp;
            _pnlPlan.MouseMove += PMouseMove;
            _pnlPlan.Paint += PnlPlanPaint;

            chkOriginalSize.Checked = Fpc.Fpobject.originalsize;
            txtAccessGroups.Text = Fpc.Fpobject.accessgroups;

            _pnlPlan.Invalidate();
            if (Fpc.Fpobject.id > -1)
                Text = LocRm.GetString("EditFloorPlan");
        }

        void PnlPlanPaint(object sender, PaintEventArgs pe)
        {
            // lock
            Graphics gPlan = pe.Graphics;

            lock (this)
            {
                var alertBrush = new SolidBrush(Color.FromArgb(200, 255, 0, 0));
                var noalertBrush = new SolidBrush(Color.FromArgb(200, 75, 172, 21));
                var offlineBrush = new SolidBrush(Color.FromArgb(200, 0, 0, 0));

                var alertBrushScanner = new SolidBrush(Color.FromArgb(50, 255, 0, 0));
                var noalertBrushScanner = new SolidBrush(Color.FromArgb(50, 75, 172, 21));

                if (_floorPlanImage!=null)
                    gPlan.DrawImage(_floorPlanImage, 0, 0, 533, 400);

                foreach (objectsFloorplanObjectsEntry fpoe in _floorPlanEntries)
                {
                    var p = new Point(fpoe.x, fpoe.y);
                    switch (fpoe.type)
                    {
                        case "camera":
                            {
                                double drad = (fpoe.angle - 180) * Math.PI / 180;
                                var points = new[]
                                        {
                                            new Point(p.X + 11+Convert.ToInt32(20*Math.Cos(drad)), p.Y + 11 + Convert.ToInt32((20* Math.Sin(drad)))),
                                            new Point(p.X + 11+Convert.ToInt32(20*Math.Cos(drad+(135*Math.PI/180))), p.Y + 11 + Convert.ToInt32((20* Math.Sin(drad+(135*Math.PI/180))))),
                                            new Point(p.X + 11+Convert.ToInt32(10*Math.Cos(drad+(180*Math.PI/180))), p.Y + 11 + Convert.ToInt32((10* Math.Sin(drad+(180*Math.PI/180))))),
                                            new Point(p.X + 11+Convert.ToInt32(20*Math.Cos(drad-(135*Math.PI/180))), p.Y + 11 + Convert.ToInt32((20* Math.Sin(drad-(135*Math.PI/180)))))
                                        };
                                gPlan.FillPolygon(noalertBrush, points);

                                int offset = (fpoe.radius/2) - 11;
                                gPlan.FillPie(noalertBrushScanner, p.X - offset, p.Y - offset, fpoe.radius, fpoe.radius, (float)(fpoe.angle - 180 - (fpoe.fov / 2)), fpoe.fov);
                            }
                            break;
                        case "microphone":
                            {
                               gPlan.FillEllipse(noalertBrush, p.X-15 , p.Y-15 , 30, 30);
                            }
                            break;
                    }
                }

                alertBrush.Dispose();
                noalertBrush.Dispose();
                alertBrushScanner.Dispose();
                noalertBrushScanner.Dispose();
                offlineBrush.Dispose();

            }
            OnPaint(pe);
        }

        private void RenderResources()
        {
            btnChooseFile.Text = "...";
            btnFinish.Text = LocRm.GetString("Finish");
            label1.Text = LocRm.GetString("Name");
            label2.Text = LocRm.GetString("Image");
            label3.Text = LocRm.GetString("addremoveObjectsdr");
            label6.Text = LocRm.GetString("FloorPlanControlInstructions");
            llblHelp.Text = linkLabel14.Text = LocRm.GetString("help");
            lblAccessGroups.Text = LocRm.GetString("AccessGroups");
            LocRm.SetString(chkOriginalSize,"OriginalSize");
        }


        private void ShowObjects()
        {
            lbObjects.Items.Clear();
            foreach (objectsCamera oc in MainForm.Cameras)
            {
                objectsCamera oc1 = oc;
                if (_floorPlanEntries.SingleOrDefault(p=>p.id==oc1.id && p.type=="camera")==null)
                    lbObjects.Items.Add(new ListItem(oc.name, oc.id, "camera"));
            }

            foreach (objectsMicrophone om in MainForm.Microphones)
            {
                objectsMicrophone om1 = om;
                if (_floorPlanEntries.SingleOrDefault(p => p.id == om1.id && p.type == "microphone") == null)
                    lbObjects.Items.Add(new ListItem(om.name, om.id, "microphone"));
            }
        }

        private void BtnFinishClick(object sender, EventArgs e)
        {
            if (txtName.Text.Trim()=="")
            {
                MessageBox.Show(LocRm.GetString("ValidateName"));
                return;
            }
            DialogResult = DialogResult.OK;
            Fpc.Fpobject.name = txtName.Text;
            Fpc.Fpobject.originalsize = chkOriginalSize.Checked;
            Fpc.Fpobject.accessgroups = txtAccessGroups.Text;
            Fpc.NeedsRefresh = true;
            Fpc.RefreshImage = true;
            Close();
        }

        private void BtnChooseFileClick(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                          {
                              InitialDirectory = Program.AppPath + @"backgrounds\",
                              Filter = "Image Files|*.jpg;*.gif;*.bmp;*.png;*.jpeg",
                              FilterIndex = 1
                          };
            if (ofd.ShowDialog(this) == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                Fpc.Fpobject.image = fileName;
                Image img = Image.FromFile(fileName);
                try
                {                   
                    Fpc.ImgPlan = (Bitmap) img.Clone();// 
                    var rf = new ResizeBilinear(533, 400);
                    _floorPlanImage = rf.Apply((Bitmap)img);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Fpc.NeedSizeUpdate = true;
                Fpc.NeedsRefresh = true;
                if (txtName.Text.Trim() == "")
                    txtName.Text = "Plan " + MainForm.NextFloorPlanId;
                _pnlPlan.Invalidate();
            }
            ofd.Dispose();
        }
        
        private void PnlPlanDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof (ListItem)))
            {
                Point local = _pnlPlan.PointToClient(new Point(e.X, e.Y));

                int x = local.X - 16;
                int y = local.Y - 16;

                var li = ((ListItem) e.Data.GetData(typeof (ListItem)));
                var fpobj = new objectsFloorplanObjectsEntry {type = li.Type, id = li.Id, x = x, y = y, angle = 135, fov = 120, radius=80};

                _floorPlanEntries.Add(fpobj);
                Fpc.Fpobject.objects.@object = _floorPlanEntries.ToArray();

                _pnlPlan.Invalidate();
                ShowObjects();
            }

            Fpc.NeedsRefresh = true;
        }


        private void PMouseMove(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;

            if (e.Button == MouseButtons.Left)
            {
                bool invalidate = false;
                if (_anglingObject != null && ((ModifierKeys & Keys.Shift) == Keys.Shift))
                {
                    double angle = Math.Atan2(_anglingObject.y + 11 - e.Y, _anglingObject.x+11- e.X);
                    _anglingObject.angle = Convert.ToInt32(angle*(180/Math.PI));
                    invalidate = true;
                }
                if (_fovingObject != null && ((ModifierKeys & Keys.Alt) == Keys.Alt))
                {
                    double angle = Math.Atan2(_fovingObject.y + 11 - e.Y, _fovingObject.x + 11 - e.X);
                    _fovingObject.fov = Convert.ToInt32(angle * (180 / Math.PI)) * 2;
                    invalidate = true;
                }
                if (_radialObject != null && ((ModifierKeys & Keys.Control) == Keys.Control))
                {
                    double distance =
                        Math.Sqrt(Math.Pow((_radialObject.x + 11 - e.X), 2) + Math.Pow((_radialObject.y + 11 - e.Y), 2))*
                        2;
                    _radialObject.radius = Convert.ToInt32(distance);
                    if (_radialObject.radius < 10)
                        _radialObject.radius = 10;
                    invalidate = true;
                }
                if (_movingObject != null)
                {
                    if (_movingObject.type == "camera")
                    {
                        _movingObject.x = e.X - 11;
                        _movingObject.y = e.Y - 11;
                    }
                    if (_movingObject.type == "microphone")
                    {
                        _movingObject.x = e.X;
                        _movingObject.y = e.Y;
                    }
                    invalidate = true;
                }         
                if (invalidate)
                    _pnlPlan.Invalidate();
            }
        }

        private objectsFloorplanObjectsEntry _anglingObject;

        private  void PMouseDown(object sender, MouseEventArgs e)
        { 
            switch (e.Button)
            {
                case MouseButtons.Left:
                    bool handled = false;
                    if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                    {
                        handled = true;
                        _anglingObject = null;
                        foreach (objectsFloorplanObjectsEntry fpoe in Fpc.Fpobject.objects.@object)
                        {
                            if ((fpoe.x - 22) <= e.X && (fpoe.x + 22) > e.X &&
                                (fpoe.y - 22) <= e.Y && (fpoe.y + 22) > e.Y)
                            {
                                _anglingObject = fpoe;
                                break;
                            }
                        }
                    }
                    if ((ModifierKeys & Keys.Alt) == Keys.Alt)
                    {
                        handled = true;
                        _fovingObject = null;
                        foreach (objectsFloorplanObjectsEntry fpoe in Fpc.Fpobject.objects.@object)
                        {
                            if ((fpoe.x - 44) <= e.X && (fpoe.x + 44) > e.X &&
                                (fpoe.y - 44) <= e.Y && (fpoe.y + 44) > e.Y)
                            {
                                _fovingObject = fpoe;
                                break;
                            }
                        }
                    }
                    if ((ModifierKeys & Keys.Control) == Keys.Control)
                    {
                        handled = true;
                        _radialObject = null;
                        foreach (objectsFloorplanObjectsEntry fpoe in Fpc.Fpobject.objects.@object)
                        {
                            if ((fpoe.x - 44) <= e.X && (fpoe.x + 44) > e.X &&
                                (fpoe.y - 44) <= e.Y && (fpoe.y + 44) > e.Y)
                            {
                                _radialObject = fpoe;
                                break;
                            }
                        }
                    }
                    if (!handled)
                    {
                        _movingObject = null;
                        foreach (objectsFloorplanObjectsEntry fpoe in Fpc.Fpobject.objects.@object)
                        {
                            if ((fpoe.x - 22) <= e.X && (fpoe.x + 22) > e.X &&
                                (fpoe.y - 22) <= e.Y && (fpoe.y + 22) > e.Y)
                            {
                                _movingObject = fpoe;
                                break;
                            }
                        }
                    }
                    
                    break;
            }
            

        }
        private  void PMouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (_movingObject!=null)
                    {
                        if (_movingObject.x<0 || _movingObject.x>_pnlPlan.Width || _movingObject.y<0 || _movingObject.y>_pnlPlan.Height)
                        {
                            _floorPlanEntries.RemoveAll(p => p.id == _movingObject.id && p.type == _movingObject.type);
                            Fpc.Fpobject.objects.@object = _floorPlanEntries.ToArray();
                            _pnlPlan.Invalidate();
                            ShowObjects();
                        }
                    }
                    _movingObject = null;
                    Fpc.NeedsRefresh = true;
                    break;
            }
        }

        private void LbObjectsQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
        }

        private void LbObjectsMouseDown(object sender, MouseEventArgs e)
        {
            int indexOfItem = lbObjects.IndexFromPoint(e.X, e.Y);
            if (indexOfItem >= 0 && indexOfItem < lbObjects.Items.Count) // check we clicked down on a string
            {
                var li = (ListItem) lbObjects.Items[indexOfItem];
                lbObjects.DoDragDrop(li, DragDropEffects.Move);
            }
        }

        private void PnlPlanDragEnter(object sender, DragEventArgs e)
        {
            if ((e.Data.GetDataPresent(typeof (ListItem)) ||
                 e.Data.GetDataPresent(typeof (objectsFloorplanObjectsEntry))) &&
                (e.AllowedEffect == DragDropEffects.Move))
                e.Effect = DragDropEffects.Move;
        }

        private void PnlPlanDragOver(object sender, DragEventArgs e)
        {
        }

        private void AddFloorPlanPaint(object sender, PaintEventArgs e)
        {
        }

        private void TxtNameKeyUp(object sender, KeyEventArgs e)
        {
            Fpc.Fpobject.name = txtName.Text;
        }

        #region Nested type: ListItem

        private struct ListItem
        {
            internal readonly int Id;
            private readonly string Name;
            internal readonly string Type;

            public ListItem(string name, int id, string type)
            {
                Name = name;
                Id = id;
                Type = type;
            }

            public override string ToString()
            {
                if (Name != "")
                    return Type + ": " + Name;
                return Type + ": " + LocRm.GetString("NoName");
            }
        }

        #endregion

        private void AddFloorPlan_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pnlPlan.Dispose();
        }

        private void llblHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl( MainForm.Website+"/userguide-floorplans.aspx");
        }

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/userguide-grant-access.aspx");
        }

        private void chkOriginalSize_CheckedChanged(object sender, EventArgs e)
        {

        }

        //private void pnlPlan_MouseDown(object sender, MouseEventArgs e)
        //{

        //}

        //private void pnlPlan_MouseMove(object sender, MouseEventArgs e)
        //{
            
        //}

        //private void pnlPlan_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Middle)
        //    {
        //        _anglingObject = null;
        //    }
        //}
    }
}