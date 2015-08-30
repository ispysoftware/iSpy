using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using iSpyApplication.Sources.Audio;

namespace iSpyApplication.Controls
{
    public partial class TextToSpeech : Form
    {
        public CameraWindow CW;

        public TextToSpeech(CameraWindow cw = null)
        {
            InitializeComponent();

            CW = cw;
            Text = LocRm.GetString("TextToSpeech");
            button1.Text = LocRm.GetString("OK");
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void TextToSpeech_Load(object sender, EventArgs e)
        {
           PopSentences();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Say();
        }

        private void Say()
        {
            var t = ddlSay.Text;
            if (!String.IsNullOrEmpty(t))
            {
                SpeechSynth.Say(t, CW);
                CW.LogToPlugin("Text: " + t);
                var p = new List<string> { t };
                foreach (var i in ddlSay.Items)
                {
                    if (!p.Contains(i) && !String.IsNullOrEmpty(i.ToString()))
                        p.Add(i.ToString());
                }

                var x = "";
                int j = 0;
                foreach (string s in p)
                {
                    if (j < 10)
                        x += s + "|";
                    else
                    {
                        break;
                    }
                    j++;
                }
                x = x.Trim('|');
                MainForm.Conf.TextSentences = x;
                PopSentences();
            }
        }

        private void PopSentences()
        {
            ddlSay.Items.Clear();
            var s = MainForm.Conf.TextSentences.Split('|');
            foreach (var t in s)
            {
                if (!String.IsNullOrEmpty(t))
                    ddlSay.Items.Add(t);
            }
        }

        private void ddlSay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Return)
            {
                Say();
                e.Handled = true;
            }
        }

        private void ddlSay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
