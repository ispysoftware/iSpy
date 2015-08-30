using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;
using PictureBox = iSpyApplication.Controls.PictureBox;

namespace iSpyApplication
{
    public partial class MainForm
    {
        private void LayoutObjects(int w, int h)
        {
            _pnlCameras.HorizontalScroll.Value = 0;
            _pnlCameras.VerticalScroll.Value = 0;
            _pnlCameras.Refresh();
            int num = _pnlCameras.Controls.Count;
            if (num == 0)
                return;
            // Get data.
            var rectslist = new List<Rectangle>();

            foreach (Control c in _pnlCameras.Controls)
            {
                bool skip = false;
                if (!(c is CameraWindow) && !(c is VolumeLevel) && !(c is FloorPlanControl)) continue;
                var p = (PictureBox)c;
                if (w > 0)
                {
                    p.Width = w;
                    p.Height = h;
                }
                if (w == -1)
                {
                    var window = c as CameraWindow;
                    if (window != null)
                    {
                        var cw = window;
                            
                        if (cw.Camera != null)
                        {
                            var bmp = cw.LastFrame;
                            if (bmp != null)
                            {
                                p.Width = bmp.Width + 2;
                                p.Height = bmp.Height + 32;
                            }
                        }
                    }
                    else
                    {
                        p.Width = c.Width;
                        p.Height = c.Height;
                    }
                }
                int nh = p.Height;
                var cameraWindow = c as CameraWindow;
                if (cameraWindow?.VolumeControl != null)
                    nh += 40;
                var level = c as VolumeLevel;
                if (level != null)
                {
                    if (level.Paired)
                        skip = true;
                }
                if (!skip)
                {
                    rectslist.Add(new Rectangle(0, 0, p.Width, nh));
                }
            }
            // Arrange the rectangles.
            Rectangle[] rects = rectslist.ToArray();
            int binWidth = _pnlCameras.Width;
            var proc = new C2BpProcessor();
            proc.SubAlgFillOneColumn(binWidth, rects);
            rectslist = rects.ToList();
            bool assigned = true;
            var indexesassigned = new List<int>();
            while (assigned)
            {
                assigned = false;
                foreach (Rectangle r in rectslist)
                {
                    for (int i = 0; i < _pnlCameras.Controls.Count; i++)
                    {
                        Control c = _pnlCameras.Controls[i];
                        if (c is CameraWindow || c is VolumeLevel || c is FloorPlanControl)
                        {
                            bool skip = false;
                            int hoffset = 0;
                            if (!indexesassigned.Contains(i))
                            {
                                var window = c as CameraWindow;
                                var cw = window;
                                if (cw?.VolumeControl != null)
                                    hoffset = 40;
                                var level = c as VolumeLevel;
                                if (level != null)
                                {
                                    if (level.Paired)
                                        skip = true;
                                }
                                if (!skip && c.Width == r.Width && c.Height + hoffset == r.Height)
                                {
                                    PositionPanel((PictureBox)c, new Point(r.X, r.Y), r.Width, r.Height - hoffset);
                                    rectslist.Remove(r);
                                    assigned = true;
                                    indexesassigned.Add(i);
                                    break;
                                }
                            }
                        }
                    }
                    if (assigned)
                        break;
                }
            }
            NeedsRedraw = true;
        }

        private void ResetLayout()
        {
            foreach (LayoutItem li in SavedLayout)
            {
                switch (li.ObjectTypeId)
                {
                    case 1:
                        VolumeLevel vl = GetVolumeLevel(li.ObjectId);
                        if (vl != null)
                        {
                            vl.Location = new Point(li.LayoutRectangle.X, li.LayoutRectangle.Y);
                            vl.Size = new Size(li.LayoutRectangle.Width, li.LayoutRectangle.Height);
                        }
                        break;
                    case 2:
                        CameraWindow cw = GetCameraWindow(li.ObjectId);
                        if (cw != null)
                        {
                            cw.Location = new Point(li.LayoutRectangle.X, li.LayoutRectangle.Y);
                            cw.Size = new Size(li.LayoutRectangle.Width, li.LayoutRectangle.Height);
                        }
                        break;
                    case 3:
                        FloorPlanControl fp = GetFloorPlan(li.ObjectId);
                        if (fp != null)
                        {
                            fp.Location = new Point(li.LayoutRectangle.X, li.LayoutRectangle.Y);
                            fp.Size = new Size(li.LayoutRectangle.Width, li.LayoutRectangle.Height);
                        }
                        break;
                }
            }
        }

        public void SaveLayout()
        {
            //save layout
            SavedLayout.Clear();

            foreach (Control c in _pnlCameras.Controls)
            {
                var r = new Rectangle(c.Location.X, c.Location.Y, c.Width, c.Height);
                var window = c as CameraWindow;
                if (window != null)
                {
                    SavedLayout.Add(new LayoutItem
                    {
                        LayoutRectangle = r,
                        ObjectId = window.Camobject.id,
                        ObjectTypeId = 2
                    });
                    continue;
                }
                var control = c as FloorPlanControl;
                if (control != null)
                {
                    SavedLayout.Add(new LayoutItem
                    {
                        LayoutRectangle = r,
                        ObjectId = control.Fpobject.id,
                        ObjectTypeId = 3
                    });
                    continue;
                }
                var level = c as VolumeLevel;
                if (level != null)
                {
                    SavedLayout.Add(new LayoutItem
                    {
                        LayoutRectangle = r,
                        ObjectId = level.Micobject.id,
                        ObjectTypeId = 1
                    });
                }

            }
            resetLayoutToolStripMenuItem1.Enabled = mnuResetLayout.Enabled = true;
        }

        public void Maximise(object obj)
        {
            Maximise(obj, true);
        }

        private delegate void MaximiseDelegate(object obj, bool minimiseIfMaximised);

        public void Maximise(object obj, bool minimiseIfMaximised)
        {
            if (obj == null)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new MaximiseDelegate(Maximise), obj, minimiseIfMaximised);
                return;
            }
            var window = obj as CameraWindow;
            if (window != null)
            {

                var cameraControl = window;
                cameraControl.BringToFront();


                try
                {
                    var r = cameraControl.RestoreRect;
                    if (r.IsEmpty)
                    {
                        var s = "320x240";
                        if (!String.IsNullOrEmpty(cameraControl.Camobject.resolution))
                            s = cameraControl.Camobject.resolution;
                        var wh = s.Split('x');

                        cameraControl.RestoreRect = new Rectangle(cameraControl.Location.X, cameraControl.Location.Y,
                                                                  cameraControl.Width, cameraControl.Height);

                        double wFact = Convert.ToDouble(_pnlCameras.Width)/Convert.ToDouble(wh[0]);
                        double hFact = Convert.ToDouble(_pnlCameras.Height)/Convert.ToDouble(wh[1]);
                        if (cameraControl.VolumeControl != null)
                            hFact = Convert.ToDouble((_pnlCameras.Height - 40))/Convert.ToDouble(wh[1]);
                        if (hFact <= wFact)
                        {
                            cameraControl.Width = Convert.ToInt32(((Convert.ToDouble(_pnlCameras.Width)*hFact)/wFact));
                            cameraControl.Height = _pnlCameras.Height;
                        }
                        else
                        {
                            cameraControl.Width = _pnlCameras.Width;
                            cameraControl.Height = Convert.ToInt32((Convert.ToDouble(_pnlCameras.Height)*wFact)/hFact);
                        }
                        cameraControl.Location = new Point(((_pnlCameras.Width - cameraControl.Width)/2),
                                                           ((_pnlCameras.Height - cameraControl.Height)/2));
                        if (cameraControl.VolumeControl != null)
                            cameraControl.Height -= 40;
                    }
                    else
                    {
                        if (minimiseIfMaximised)
                            Minimize(window, false);
                        cameraControl.RestoreRect = Rectangle.Empty;
                    }
                }
                catch(Exception ex)
                {
                    LogExceptionToFile(ex);
                }
            }

            var level = obj as VolumeLevel;
            if (level != null)
            {
                var vf = level;
                vf.BringToFront();
                if (vf.Paired)
                {
                    CameraWindow cw = GetCameraWindow(Cameras.Single(p => p.settings.micpair == vf.Micobject.id).id);
                    if (vf.Width == _pnlCameras.Width)
                    {
                        if (minimiseIfMaximised)
                            Minimize(cw, false);
                    }
                    else
                        Maximise(cw);
                }
                else
                {
                    var r = vf.RestoreRect;
                    if (r.IsEmpty)
                    {
                        vf.RestoreRect = new Rectangle(vf.Location.X, vf.Location.Y,
                                                                  vf.Width, vf.Height);
                        vf.Location = new Point(0, 0);
                        vf.Width = _pnlCameras.Width;
                        vf.Height = _pnlCameras.Height;

                    }
                    else
                    {

                        if (minimiseIfMaximised)
                            Minimize(vf, false);
                        vf.RestoreRect = Rectangle.Empty;
                    }                    
                }
            }

            var control = obj as FloorPlanControl;
            if (control != null)
            {
                var fp = control;
                fp.BringToFront();
                var r = fp.RestoreRect;
                if (r.IsEmpty)
                {
                    fp.RestoreRect = new Rectangle(fp.Location.X, fp.Location.Y,
                                                              fp.Width, fp.Height);
                    var wFact = Convert.ToDouble(_pnlCameras.Width) / fp.Width;
                    var hFact = Convert.ToDouble(_pnlCameras.Height) / fp.Height;

                    if (hFact <= wFact)
                    {
                        fp.Width = (int)(_pnlCameras.Width / wFact * hFact);
                        fp.Height = _pnlCameras.Height;
                    }
                    else
                    {
                        fp.Width = _pnlCameras.Width;
                        fp.Height = (int)(_pnlCameras.Height / hFact * wFact);
                    }
                    fp.Location = new Point(((_pnlCameras.Width - fp.Width) / 2), ((_pnlCameras.Height - fp.Height) / 2));
                }
                else
                {
                    if (minimiseIfMaximised)
                        Minimize(control, false);
                    fp.RestoreRect = Rectangle.Empty;
                }
            }
        }

        private void Minimize(object obj, bool tocontents)
        {
            if (obj == null)
                return;
            var window = obj as CameraWindow;
            if (window != null)
            {
                var cw = window;
                Rectangle r = cw.RestoreRect;
                if (r != Rectangle.Empty && !tocontents)
                {
                    cw.Location = r.Location;
                    cw.Height = r.Height;
                    cw.Width = r.Width;
                }
                else
                {
                    if (cw.Camera != null)
                    {
                        Bitmap bmp = cw.LastFrame;
                        if (bmp != null)
                        {
                            cw.Width = bmp.Width + 2;
                            cw.Height = bmp.Height + 26;
                            bmp.Dispose();
                        }
                    }
                    else
                    {
                        cw.Width = 322;
                        cw.Height = 266;
                    }
                }
                cw.Invalidate();
            }

            var level = obj as VolumeLevel;
            if (level != null)
            {
                var cw = level;
                Rectangle r = cw.RestoreRect;
                if (r != Rectangle.Empty && !tocontents)
                {
                    cw.Location = r.Location;
                    cw.Height = r.Height;
                    cw.Width = r.Width;
                }
                else
                {
                    cw.Width = 160;
                    cw.Height = 40;
                }
                cw.Invalidate();
            }

            var control = obj as FloorPlanControl;
            if (control != null)
            {
                var fp = control;
                Rectangle r = fp.RestoreRect;
                if (r != Rectangle.Empty && !tocontents)
                {
                    fp.Location = r.Location;
                    fp.Height = r.Height;
                    fp.Width = r.Width;
                    fp.Invalidate();
                }
                else
                {
                    if (fp.ImgPlan != null)
                    {
                        fp.Width = fp.ImgPlan.Width + 2;
                        fp.Height = fp.ImgPlan.Height + 26;
                    }
                    else
                    {
                        fp.Width = 322;
                        fp.Height = 266;
                    }
                }
            }
        }

        private void MaxMin()
        {
            fullScreenToolStripMenuItem1.Checked = menuItem3.Checked = !fullScreenToolStripMenuItem1.Checked;
            if (fullScreenToolStripMenuItem1.Checked)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                WinApi.SetWinFullScreen(Handle);
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
            Conf.Fullscreen = fullScreenToolStripMenuItem1.Checked;
        }

        private void LayoutOptimised()
        {
            double numberCameras = Cameras.Count;
            int useX = 320, useY = 200;
            int dispArea = _pnlCameras.Width * _pnlCameras.Height;
            int lastArea = dispArea;


            for (int y = 1; y <= numberCameras; y++)
            {
                int camX = y;
                var camY = (int)Math.Round((numberCameras / y) + 0.499999999, 0);

                int dispWidth = _pnlCameras.Width / camX;
                int dispHeight = dispWidth / 4 * 3;
                int camArea = (int)numberCameras * (dispWidth * (dispHeight + 40));
                if (((dispArea - camArea) <= lastArea) && ((dispArea - camArea) > 0) && (((camY * (dispHeight + 40)) < _pnlCameras.Height)))
                {
                    useX = dispWidth;
                    useY = dispHeight;
                    lastArea = dispArea - camArea;
                }

                dispHeight = (_pnlCameras.Height - (camY * 40)) / camY;
                dispWidth = dispHeight * 4 / 3;
                camArea = (int)numberCameras * (dispWidth * (dispHeight + 40));
                if (((dispArea - camArea) <= lastArea) && ((dispArea - camArea) > 0) && (((camX * dispWidth) < _pnlCameras.Width)))
                {
                    useX = dispWidth;
                    useY = dispHeight;
                    lastArea = dispArea - camArea;
                }
            }
            LayoutObjects(useX, useY);
        }

        internal void ClearHighlights()
        {
            foreach (Control c in _pnlCameras.Controls)
            {
                var window = c as CameraWindow;
                if (window != null)
                {
                    window.Highlighted = false;
                }
                var control = c as FloorPlanControl;
                if (control != null)
                {
                    control.Highlighted = false;
                }
                var level = c as VolumeLevel;
                if (level != null)
                {
                    level.Highlighted = false;
                }

            }
        }


        private void UnlockLayout()
        {
            Conf.LockLayout = menuItem22.Checked = false;
        }
    }
}
