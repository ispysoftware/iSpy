using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class LayoutEditor : Form
    {
        public int H;
        public int W;
        public int X, Y;

        public LayoutEditor()
        {
            InitializeComponent();
            RenderResources();
        }

        private void Button1Click(object sender, EventArgs e)
        {
            X = Convert.ToInt32(numX.Value);
            Y = Convert.ToInt32(numY.Value);
            W = Convert.ToInt32(numW.Value);
            H = Convert.ToInt32(numH.Value);

            DialogResult = DialogResult.OK;
        }

        private void LayoutEditorLoad(object sender, EventArgs e)
        {
            numX.Value = X;
            numY.Value = Y;
            numW.Value = W;
            numH.Value = H;
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("LayoutEditor");
            button1.Text = LocRm.GetString("Update");
            label1.Text = "X";
            label2.Text = "Y";
            label3.Text = LocRm.GetString("Width");
            label4.Text = LocRm.GetString("Height");
        }
    }
}