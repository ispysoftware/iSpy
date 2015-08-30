using System;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class Prompt : Form
    {
        public string Val;
        public Prompt()
        {
            InitializeComponent();
            button1.Text = LocRm.GetString("OK");

        }

        public Prompt(string label, string prefill="", bool isPassword=false)
        {
            InitializeComponent();
            Text = label;
            textBox1.Text = prefill;
            if (isPassword)
                textBox1.PasswordChar = '*';
        }


        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Go();
        }

        private void Go()
        {
            DialogResult = DialogResult.OK;
            Val = textBox1.Text;
            Close();
        }

        private void Prompt_Load(object sender, EventArgs e)
        {

        }

        private void Prompt_Shown(object sender, EventArgs e)
        {
            this.Activate();
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Go();
            }
        }
    }
}
