using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class CommandButtons : Form
    {
        public CommandButtons()
        {
            InitializeComponent();
            
            Resize += CommandButtonsResize;
        }

        void CommandButtonsResize(object sender, EventArgs e)
        {
            cb.Invalidate();
        }

        private void CommandButtons_Load(object sender, EventArgs e)
        {
            int maxx = 0, maxy = 0;
            foreach (var btn in MainForm.RemoteCommands)
            {
                if (btn.inwindow)
                {
                    var loc = btn.location.Split(',');
                    var sz = btn.size.Split('x');
                    var x = Convert.ToInt32(loc[0]);
                    var y = Convert.ToInt32(loc[1]);
                    var w = Convert.ToInt32(sz[0]);
                    var h = Convert.ToInt32(sz[1]);

                    if (x + w > maxx)
                        maxx = x + w + 20;
                    if (y + h > maxy)
                        maxy = y + h + 40;
                }
            }
            if (Width < maxx)
                Width = maxx;
            if (Height < maxy)
                Height = maxy;
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cb.Add(p);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cb.Edit();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cb.Remove();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            editToolStripMenuItem.Visible = removeToolStripMenuItem.Visible = repositionToolStripMenuItem.Visible = cb.CurButton != null;
        }

        private Point p;
        private void cb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                p = e.Location;
            }
        }

        private void cb_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void repositionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cb.Reposition();
        }
    }
}
