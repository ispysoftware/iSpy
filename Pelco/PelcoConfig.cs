using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iSpyApplication.Controls;

namespace iSpyApplication.Pelco
{
    public partial class PelcoConfig : Form
    {
        public string Config;

        public PelcoConfig()
        {
            InitializeComponent();
        }

        private void PelcoConfig_Load(object sender, EventArgs e)
        {
            string[] cfg = Config.Split('|');


            string[] ports = SerialPort.GetPortNames();

            foreach (string p in ports)
            {
                ddlComPort.Items.Add(p);
            }
            // "COM1|9600|8|One|Odd|1";
            SetSelected(ddlComPort, cfg[0]);
            SetSelected(ddlBaud, cfg[1]);
            SetSelected(ddlData, cfg[2]);
            SetSelected(ddlStop, cfg[3]);
            SetSelected(ddlParity, cfg[4]);
            numAddress.Value = Convert.ToInt32(cfg[5]);


        }

        private void SetSelected(ComboBox cb, string val)
        {
            int i = 0;
            foreach(var s in cb.Items)
            {
                if (s.ToString() == val)
                {
                    cb.SelectedIndex = i;
                    return;
                }
                i++;
            }
            if (cb.Items.Count>0)
                cb.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config = ddlComPort.SelectedItem + "|" + ddlBaud.SelectedItem + "|" + ddlData.SelectedItem + "|" +
                     ddlStop.SelectedItem + "|" + ddlParity.SelectedItem + "|" + (int) numAddress.Value;
            DialogResult = DialogResult.OK;
            Close();

        }
    }
}
