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
    public partial class GridViewManager : Form
    {
        public MainForm MainClass;

        public GridViewManager()
        {
            InitializeComponent();
            RenderResources();
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("GridViewManager");
            LocRm.SetString(button3,"Edit");
            LocRm.SetString(button2, "Delete");
            LocRm.SetString(button1, "New");
        }

        private void GridViewManager_Load(object sender, EventArgs e)
        {
            LoadGrids();
        }

        private void LoadGrids()
        {
            lbGridViews.Items.Clear();

            foreach (configurationGrid gv in MainForm.Conf.GridViews)
            {
                lbGridViews.Items.Add(gv.name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddGridView();
            LoadGrids();
        }

        private void AddGridView()
        {
            var gvc = new GridViewCustom();
            gvc.ShowDialog(this);
            if (gvc.DialogResult == DialogResult.OK)
            {
                var cg = new configurationGrid
                {
                    Columns = gvc.Cols,
                    Rows = gvc.Rows,
                    name = gvc.GridName,
                    FullScreen = gvc.FullScreen,
                    AlwaysOnTop = gvc.AlwaysOnTop,
                    Display = gvc.Display,
                    Framerate = gvc.Framerate,
                    ModeIndex = gvc.Mode,
                    Fill = gvc.Fill,
                    ModeConfig = gvc.ModeConfig,
                    ShowAtStartup = gvc.ShowAtStartup,
                    GridItem = new configurationGridGridItem[] { }
                };
                List<configurationGrid> l = MainForm.Conf.GridViews.ToList();
                l.Add(cg);
                MainForm.Conf.GridViews = l.ToArray();

                MainClass.ShowGridView(cg.name);

                LoadGrids();
            }
            gvc.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var l = MainForm.Conf.GridViews.ToList();
            foreach (var s in lbGridViews.SelectedItems)
            {
                l.RemoveAll(p => p.name == s.ToString());
            }
            MainForm.Conf.GridViews = l.ToArray();
            LoadGrids();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var s in lbGridViews.SelectedItems)
            {
                MainClass.EditGridView(s.ToString(),this);
                LoadGrids();
                break;
            }
        }

    }
}
