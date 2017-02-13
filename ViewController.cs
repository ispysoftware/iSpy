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
    public sealed partial class ViewController : Form
    {
        private ViewControllerPanel vcp = null;
        public LayoutPanel LayoutTarget;

        public ViewController(LayoutPanel layoutTarget)
        {

            this.LayoutTarget = layoutTarget;
            InitializeComponent();
            Text = LocRm.GetString("ViewController");
        }

        private void ViewController_Load(object sender, EventArgs e)
        {
            vcp = new ViewControllerPanel(LayoutTarget) { Dock = DockStyle.Fill };
            this.Controls.Add(vcp);

        }

        public void Redraw()
        {
            vcp.Invalidate();
        }

        private void tmrRedraw_Tick(object sender, EventArgs e)
        {
            Redraw();
        }

        
        
    }
}
