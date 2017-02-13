using System;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class jbutton : UserControl
    {
        private int _id;
        private bool _input;
        public event EventHandler GetInput;
        public int ID
        {
            get { return _id; }
            set { 
                _id = value;
                if (_id > 0)
                {
                    lblButton.Text = "Button " + _id;
                    _input = false;
                    button1.Text = "...";
                }
                else
                    lblButton.Text = "";
            }
        }

        public jbutton()
        {
            InitializeComponent();
        }

        public void Reset()
        {
            ID = _id;
            _input = false;
            button1.Text = "...";
        }

        private void jbutton_Load(object sender, EventArgs e)
        {

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

            LocRm.SetString(lblButton, "PressButton");
            LocRm.SetString(button1, "Clear");
            _input = true;

        }
    }
}
