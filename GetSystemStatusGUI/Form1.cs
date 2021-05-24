using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSystemStatusGUI {
    public partial class Form1 : Form {
        protected CPUForm cpuForm;
        protected RAMForm ramForm;
        protected DiskForm diskForm;
        protected NetworkForm networkForm;

        public Form1() {
            InitializeComponent();
        }

        private void showCPU_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (cpuForm == null || cpuForm.IsDisposed)
                    cpuForm = new CPUForm(this);
                cpuForm.Show();
            } else {
                cpuForm.Hide();
            }
        }

        public void DisableChecked(string target) {
            switch (target) {
                case "CPU":
                    showCPU.Checked = false;
                    break;
                case "RAM":
                    showRAM.Checked = false;
                    break;
                case "Disk":
                    showDisk.Checked = false;
                    break;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e) {
            //Application.Exit();
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e) {
            showCPU.Checked = true;
            showRAM.Checked = true;
            showDisk.Checked = true;
            showNetwork.Checked = true;
        }

        private void showRAM_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (ramForm == null || ramForm.IsDisposed) ramForm = new RAMForm(this);
                ramForm.Show();
            } else {
                ramForm.Hide();
            }
        }

        private void showDisk_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (diskForm == null || diskForm.IsDisposed) diskForm = new DiskForm(this);
                diskForm.Show();
            } else {
                diskForm.Hide();
            }
        }

        private void showNetwork_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (networkForm == null || networkForm.IsDisposed) networkForm = new NetworkForm();
                networkForm.Show();
            } else {
                networkForm.Hide();
            }
        }

        private void cbUpdateInterval_SelectedIndexChanged(object sender, EventArgs e) {
            ComboBox comboBox = (ComboBox)sender;
            string cselect = comboBox.Text;
            switch (cselect) {
                case "1 sec":
                    Global.interval_ms = 1000;
                    break;
                case "2 sec":
                    Global.interval_ms = 2000;
                    break;
                case "0.5 sec":
                    Global.interval_ms = 500;
                    break;
                case "0.25 sec":
                    Global.interval_ms = 250;
                    break;
                default:
                    cbUpdateInterval_TextChanged(sender, e);
                    break;
            }
        }

        private void cbUpdateInterval_TextChanged(object sender, EventArgs e) {
            ComboBox comboBox = (ComboBox)sender;
            string cselect = comboBox.Text;
            try {
                string[] csplit = cselect.Split(' ');
                float ims = float.Parse(csplit[0]);
                if (ims == 0) return;
                string unit = csplit[1];
                if (unit == "ms") Global.interval_ms = (int)Math.Round(ims);
                else if (unit == "s" || unit == "sec" || unit == "secs") Global.interval_ms = (int)Math.Round(ims * 1000);
            }
            catch { }
        }

		public void btnDiskRefresh_Click(object sender, EventArgs e) {
            diskForm.Hide();
            diskForm = new DiskForm(this);
            diskForm.Show();
		}
	}
}