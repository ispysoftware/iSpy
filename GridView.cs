using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication
{
    public partial class GridView : Form
    {
        private readonly Controls.GridView _gv;
        private readonly configurationGrid _layout;
        public configurationGrid Cg;


        public GridView(MainForm parent, ref configurationGrid layout)
        {
            InitializeComponent();
            _gv = new Controls.GridView(parent, ref layout, this);
            _gv.KeyDown += GridView_KeyDown;
            Controls.Add(_gv);
            _gv.Dock = DockStyle.Fill;
            _layout = layout;
            fullScreenToolStripMenuItem.Checked = layout.FullScreen;
            alwaysOnTopToolStripMenuItem.Checked = layout.AlwaysOnTop;
            Cg = layout;

        }



        private void GridView_Load(object sender, EventArgs e)
        {
            Text = _gv.Text;

            var screen = Screen.AllScreens.Where(s => s.DeviceName == _layout.Display).DefaultIfEmpty(Screen.PrimaryScreen).First();
            StartPosition = FormStartPosition.Manual;
            Location = screen.Bounds.Location;

            if (fullScreenToolStripMenuItem.Checked)
                MaxMin();

            if (alwaysOnTopToolStripMenuItem.Checked)
                OnTop();
        }

        private void MaxMin()
        {
            if (fullScreenToolStripMenuItem.Checked)
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
        }


        public void ShowForm()
        {
            if (InvokeRequired)
            {
                Invoke(new Delegates.SimpleDelegate(ShowForm));
                return;
            }

            Activate();
            Visible = true;
            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                WindowState = FormWindowState.Maximized;
            }
        }

        private void OnTop()
        {
            TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void Edit()
        {
            _gv.MainClass.EditGridView(_layout.name,this);
        }


        private void GridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.Enter)
            {
                fullScreenToolStripMenuItem.Checked = !fullScreenToolStripMenuItem.Checked;
                MaxMin();
            }
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaxMin();
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnTop();
        }

        private void closeGridViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Edit();
            _gv.Init();
        }

        private void switchFillModeAltFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _gv.Cg.Fill = !_gv.Cg.Fill;
        }

        private void GridView_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void quickSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var so = new SelectObjects())
            {
                so.ShowDialog(this);
                if (so.DialogResult == DialogResult.OK)
                {
                    var l = so.SelectedObjects;
                    int i = 0;
                    var lg = new List<configurationGridGridItem>();

                    foreach (var o in l)
                    {
                        int oid = -1, otid = -1;
                        var k = o as objectsCamera;
                        if (k != null)
                        {
                            oid = k.id;
                            otid = 2;
                        }
                        var j = o as objectsMicrophone;
                        if (j != null)
                        {
                            oid = j.id;
                            otid = 1;
                        }
                        var m = o as objectsFloorplan;
                        if (m != null)
                        {
                            oid = m.id;
                            otid = 3;
                        }
                        lg.Add(new configurationGridGridItem
                               {
                                   CycleDelay = 4,
                                   GridIndex = i,
                                   Item =
                                       new[]
                                       {
                                           new configurationGridGridItemItem
                                           {
                                               ObjectID = oid,
                                               TypeID = otid
                                           }
                                       }
                               });
                        i++;
                        if (i >= _gv.Cg.Columns*_gv.Cg.Rows)
                            break;

                    }
                    _gv.Cg.GridItem = lg.ToArray();
                    _gv.Init();
                }
            }
        }

        private void quickSelectToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            quickSelectToolStripMenuItem.Visible = _gv.Cg.ModeIndex == 0;
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void GridView_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void contextMenuStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void GridView_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
