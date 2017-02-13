using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iSpyApplication.Kinect
{
    public partial class EditDepthLine : Form
    {
        private readonly DepthLine _dl;

        public EditDepthLine(DepthLine dl)
        {
            _dl = dl;
            InitializeComponent();
        }

        private void EditDepthLine_Load(object sender, EventArgs e)
        {
            numDepthMax.Value = _dl.DepthMax;
            numDepthMin.Value = _dl.DepthMin;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _dl.DepthMax = Convert.ToInt32(numDepthMax.Value);
            _dl.DepthMin = Convert.ToInt32(numDepthMin.Value);
            _dl.WidthChanged = true;
            Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           TripWireEditor.TripWires.Remove(_dl);
           Close();
        }
    }
}
