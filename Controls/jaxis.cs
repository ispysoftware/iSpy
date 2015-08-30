using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class jaxis : UserControl
    {
        private int _id;
        private bool _input, _invert;
        public bool SupportDPad;
        public bool Invert
        {
            get { return _invert; }
            set { 
                _invert = value;
                chkInvert.Checked = value;
            }
        }

        public event EventHandler GetInput;
        public int ID
        {
            get { return _id; }
            set { 
                _id = value;
                if (_id > 0)
                {
                    lblButton.Text = LocRm.GetString("Axis")+" " + _id;
                    _input = false;
                    button1.Text = "...";
                }
                if (_id <0)
                {
                    lblButton.Text = "DPad " + (0-_id);
                    _input = false;
                    button1.Text = "...";
                }
                if (_id==0)
                    lblButton.Text = "";
            }
        }

        public jaxis()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            ID = _id;
            _input = false;
            button1.Text = "...";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_input)
            {
                ID = 0;
                Reset();
                GetInput(null, EventArgs.Empty);
                return;
            }

            if (GetInput!=null)
                GetInput(this, EventArgs.Empty);

            LocRm.SetString(lblButton,"MoveAnAxis");
            if (SupportDPad)
                LocRm.SetString(lblButton, "MoveOrPress");
            LocRm.SetString(button1, "Clear");
            _input = true;

        }

        private void jaxis_Load(object sender, EventArgs e)
        {
            LocRm.SetString(chkInvert, "Invert");
        }

        private void chkInvert_CheckedChanged(object sender, EventArgs e)
        {
            _invert = chkInvert.Checked;
        }
    }
}
