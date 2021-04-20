using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSystemStatusGUI
{
    public partial class Form1 : Form
    {
        protected CPUForm cpuForm;
        public Form1()
        {
            InitializeComponent();
        }

        private void showCPU_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox self = (CheckBox)sender;
            if (self.Checked)
            {
                if (cpuForm == null || cpuForm.IsDisposed)
                    cpuForm = new CPUForm(this);
                cpuForm.Show();
            }
            else
            {
                cpuForm.Hide();
            }
        }

        public void DisableChecked(string target)
        {
            switch (target)
            {
                case "CPU":
                    showCPU.Checked = false;
                    break;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            //Application.Exit();
            this.Close();
        }
    }
}