using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace iSpyApplication.Controls
{
    public partial class ParamConfig : Form
    {
        public string TypeName;
        public string Param1, Param2, Param3, Param4;
        public string Param1Value, Param2Value, Param3Value, Param4Value;
        int _iPanel;

        public ParamConfig()
        {
            InitializeComponent();
            button1.Text = LocRm.GetString("OK");

        }

        private void ParamConfigcs_Load(object sender, EventArgs e)
        {
            Text = TypeName;
            AddControl(Param1, Param1Value);
            AddControl(Param2, Param2Value);
            AddControl(Param3, Param3Value);
            AddControl(Param4, Param4Value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pi = 0;
            foreach(Panel p in flpParams.Controls)
            {
                string val = "";
                foreach(Control c in p.Controls)
                {
                    if (c is TextBox)
                    {
                        val = c.Text;
                        break;
                        //...   
                    }
                    var numericUpDown = c as NumericUpDown;
                    if (numericUpDown != null)
                    {
                        val = numericUpDown.Value.ToString(CultureInfo.InvariantCulture);
                        break;
                    }
                    var comboBox = c as ComboBox;
                    if (comboBox != null)
                    {
                        val = ((ListItem)comboBox.SelectedItem).Value;
                        break;
                    }

                    var checkbox = c as CheckBox;
                    if (checkbox != null)
                    {
                        val = checkbox.Checked.ToString();
                        break;
                    }

                }
                switch (pi)
                {
                    case 0:
                        Param1Value = val;
                        break;
                    case 1:
                        Param2Value = val;
                        break;
                    case 2:
                        Param3Value = val;
                        break;
                    case 3:
                        Param4Value = val;
                        break;
                }
                pi++;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void AddControl(string paramName, string paramValue)
        {
            if (String.IsNullOrWhiteSpace(paramName))
                return;

            string[] cfg = paramName.Split('|');
            string lbl = cfg[0];
            string typ = "Textbox";
            const int coloffset = 95;
            const int elewidth = 120;
            const int elewidthlong = 160;
            string optval = "";
            if (cfg.Length==2)
            {
                string[] typcfg = cfg[1].Split(':');
                typ = typcfg[0];
                if (typcfg.Length > 1)
                    optval = typcfg[1];
            }
            var p = new Panel {Dock = DockStyle.Top, Height = 31, Tag = _iPanel, Width = 300};
            _iPanel++;
            var l = new Label {Text = lbl, Margin = new Padding(0), Padding = new Padding(4), AutoSize = true};
            p.Controls.Add(l);
            l.Location = new Point(3, 3);
            switch (typ)
            {
                case "Textbox":
                    {
                        var tb = new TextBox { Text = paramValue, Width = elewidthlong, Location = new Point(coloffset, 3) };
                        p.Controls.Add(tb);
                    }
                    break;
                case "FBD":
                    {
                        var tb = new TextBox { Text = paramValue, Width = elewidth, Tag = optval, Location = new Point(coloffset, 3) };

                        var btn = new Button { Text = "...", Location = new Point(elewidth+coloffset+10, 3), Width = 40 };

                        btn.Click += btn_Click;
                        btn.Tag = tb;
                        
                        p.Controls.Add(tb);
                        p.Controls.Add(btn);
                    }
                    break;
                case "DDL":
                    {
                        //optval="TCP,UDP"
                        var cb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = elewidthlong, Location = new Point(coloffset, 3) };
                        string[] opts = optval.Split(',');
                        foreach(string opt in opts)
                        {
                            cb.Items.Add(new ListItem(opt, opt));
                        }
                        
                        for(int i=0;i<cb.Items.Count;i++)
                        {
                            if (((ListItem)cb.Items[i]).Value == paramValue)
                            {
                                cb.SelectedIndex = i;
                                break;
                            }
                        }
                        if (cb.SelectedIndex == -1)
                            cb.SelectedIndex = 0;
                        p.Controls.Add(cb);

                    }
                    break;
                case "Object":
                    {
                        var cb = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = elewidthlong, Location = new Point(coloffset, 3) };

                        foreach (var c in MainForm.Cameras)
                        {
                            cb.Items.Add(new ListItem(c.name, "2," + c.id));
                        }
                        foreach (var c in MainForm.Microphones)
                        {
                            cb.Items.Add(new ListItem(c.name, "1," + c.id));
                        }
                        foreach (ListItem li in cb.Items)
                        {
                            if (li.Value == paramValue)
                                cb.SelectedItem = li;
                        }
                        if (cb.SelectedIndex == -1)
                            cb.SelectedIndex = 0;
                        p.Controls.Add(cb);
                    }
                    break;
                case "Numeric":
                    {
                        var n = new NumericUpDown { Width = elewidth, Location = new Point(coloffset, 3) };
                        string[] opts = optval.Split(',');
                        if (opts.Length==2)
                        {
                            n.Minimum = Convert.ToDecimal(opts[0]);
                            n.Maximum = Convert.ToDecimal(opts[1]);
                        }
                        try
                        {
                            n.Value = Convert.ToDecimal(paramValue);
                        }
                        catch
                        {
                            
                        }
                        p.Controls.Add(n);
                    }
                    break;
                case "SMS":
                    {
                        var tb = new TextBox { Text = paramValue, Width = elewidth, Location = new Point(coloffset, 3) };
                        var ll = new LinkLabel { Text = "How to Enter", Location = new Point(elewidth + coloffset + 10, 3) };
                        ll.Click += ll_Click;

                        p.Controls.Add(tb);
                        p.Controls.Add(ll);

                    }
                    break;
                case "Checkbox":
                    {
                        var cb = new CheckBox { Location = new Point(coloffset, 3), Checked = Convert.ToBoolean(paramValue) };
                        p.Controls.Add(cb);
                    }
                    break;
                case "Link":
                    {
                        var la = optval.Split(',');
                        var ll = new LinkLabel { Text = la[0], Location = new Point(coloffset, 3), Tag = paramValue, AutoSize = true };
                        ll.Click += ll_Click2;
                        p.Controls.Add(ll);

                    }
                    break;
            }
            flpParams.Controls.Add(p);
        }

        private struct ListItem
        {
            private readonly string _name;
            internal readonly string Value;

            public ListItem(string name, string value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }


        void ll_Click(object sender, EventArgs e)
        {
            MainForm.OpenUrl(MainForm.Website + "/countrycodes.aspx");
        }

        void ll_Click2(object sender, EventArgs e)
        {
            MainForm.OpenUrl(((LinkLabel)sender).Tag.ToString());
        }

        void btn_Click(object sender, EventArgs e)
        {
            var ofdDetect = new OpenFileDialog {FileName = "", Filter = ""};

            var o = (TextBox)(((Button) sender).Tag);

            string initpath = "";
            string path = o.Text.Trim();
            if (!String.IsNullOrWhiteSpace(path))
            {
                try
                {
                    var fi = new FileInfo(path);
                    initpath = fi.DirectoryName;
                }
                catch {}
            }
            else
            {
                if (TypeName == "Sound")
                {
                    initpath = Program.ExecutableDirectory + "Sounds";
                }
            }
            ofdDetect.Filter = "Files|" + o.Tag;
            ofdDetect.InitialDirectory = initpath;
            ofdDetect.ShowDialog(this);
            if (ofdDetect.FileName != "")
            {
                ((TextBox) ((Button) sender).Tag).Text = ofdDetect.FileName;
            }
        }
    }
}
