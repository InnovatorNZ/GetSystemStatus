﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSystemStatusGUI {
    public partial class Form1 : Form {
        protected CPUForm cpuForm;
        protected RAMForm ramForm;
        protected DiskForm diskForm;
        protected NetworkForm networkForm;
        protected GPUForm gpuForm;
        private string iniFile = ".\\config.ini";

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
                case "Network":
                    showNetwork.Checked = false;
                    break;
                case "GPU":
                    showGPU.Checked = false;
                    break;
                case "noGPU":
                    showGPU.Checked = false;
                    showGPU.Enabled = false;
                    showGPU.Text += " (No discrete graphics detected)";
                    break;
                case "noNetwork":
                    showNetwork.Checked = false;
                    showNetwork.Enabled = false;
                    showNetwork.Text = "Show Network (No connections)";
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
            bool ifShowGPU = !bool.Parse(INIHelper.Read("DoNotShow", "GPU", "true", iniFile));
            doNotShowGPUAtStartToolStripMenuItem.Checked = !ifShowGPU;
            if (Environment.OSVersion.Version.Major < 10) {
                showGPU.Enabled = false;
                showGPU.Text += " (Only available in Windows 10)";
            } else {
                showGPU.Checked = ifShowGPU;
            }
            LoadSavedLocation();
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
                if (networkForm == null || networkForm.IsDisposed) networkForm = new NetworkForm(this);
                if (!networkForm.IsDisposed) networkForm.Show();
            } else {
                if (networkForm != null) networkForm.Hide();
            }
        }

        private void showGPU_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (gpuForm == null || gpuForm.IsDisposed) gpuForm = new GPUForm(this);
                if (!gpuForm.IsDisposed) {
                    gpuForm.Show();
                    loadGPUFormLocation();
                }
            } else {
                if (gpuForm != null) gpuForm.Dispose();
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
            this.showDisk.Checked = false;
            if (!diskForm.IsDisposed) diskForm.Dispose();
            diskForm = new DiskForm(this);
            this.showDisk.Checked = true;
        }

        private void btnNetworkRefresh_Click(object sender, EventArgs e) {
            this.showNetwork.Enabled = true;
            this.showNetwork.Text = "Show Network and Adapter Speed";
            this.showNetwork.Checked = false;
            if (!networkForm.IsDisposed) networkForm.Dispose();
            networkForm = new NetworkForm(this);
            this.showNetwork.Checked = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (cpuForm != null) cpuForm.Dispose();
            if (ramForm != null) ramForm.Dispose();
            if (diskForm != null) diskForm.Dispose();
            if (networkForm != null) networkForm.Dispose();
            if (gpuForm != null) gpuForm.Dispose();
            this.Dispose();
        }

        private void btnDiskRefresh_MouseEnter(object sender, EventArgs e) {
            this.toolTip1.Show("Reload disks if you have new disk inserted", (Button)sender);
        }

        private void btnNetworkRefresh_MouseEnter(object sender, EventArgs e) {
            this.toolTip1.Show("Reload networks if you have connections established or removed", (Button)sender);
        }

        private void btnDiskRefresh_MouseLeave(object sender, EventArgs e) {
            this.toolTip1.Hide((Button)sender);
        }

        private void saveOpenedWindowLocationsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (cpuForm != null && !cpuForm.IsDisposed) {
                INIHelper.Write("CPUForm", "X", cpuForm.Location.X.ToString(), iniFile);
                INIHelper.Write("CPUForm", "Y", cpuForm.Location.Y.ToString(), iniFile);
            }
            if (ramForm != null && !ramForm.IsDisposed) {
                INIHelper.Write("RAMForm", "X", ramForm.Location.X.ToString(), iniFile);
                INIHelper.Write("RAMForm", "Y", ramForm.Location.Y.ToString(), iniFile);
            }
            if (diskForm != null && !diskForm.IsDisposed) {
                INIHelper.Write("DiskForm", "X", diskForm.Location.X.ToString(), iniFile);
                INIHelper.Write("DiskForm", "Y", diskForm.Location.Y.ToString(), iniFile);
            }
            if (networkForm != null && !networkForm.IsDisposed) {
                INIHelper.Write("NetworkForm", "X", networkForm.Location.X.ToString(), iniFile);
                INIHelper.Write("NetworkForm", "Y", networkForm.Location.Y.ToString(), iniFile);
            }
            if (gpuForm != null && !gpuForm.IsDisposed) {
                INIHelper.Write("GPUForm0", "X", gpuForm.Location.X.ToString(), iniFile);
                INIHelper.Write("GPUForm0", "Y", gpuForm.Location.Y.ToString(), iniFile);
                int i = 1;
                foreach (var subGpuForm in gpuForm.moreGPUForms) {
                    INIHelper.Write("GPUForm" + i.ToString(), "X", subGpuForm.Location.X.ToString(), iniFile);
                    INIHelper.Write("GPUForm" + i.ToString(), "Y", subGpuForm.Location.Y.ToString(), iniFile);
                    i++;
                }
            }
        }

        private void loadSavedLocationsToolStripMenuItem_Click(object sender, EventArgs e) {
            LoadSavedLocation();
        }

        private void LoadSavedLocation() {
            if (cpuForm != null && !cpuForm.IsDisposed) {
                string sX = INIHelper.Read("CPUForm", "X", cpuForm.Location.X.ToString(), iniFile);
                string sY = INIHelper.Read("CPUForm", "Y", cpuForm.Location.Y.ToString(), iniFile);
                int x = int.Parse(sX), y = int.Parse(sY);
                cpuForm.Location = new Point(x, y);
            }
            if (ramForm != null && !ramForm.IsDisposed) {
                string sX = INIHelper.Read("RAMForm", "X", ramForm.Location.X.ToString(), iniFile);
                string sY = INIHelper.Read("RAMForm", "Y", ramForm.Location.Y.ToString(), iniFile);
                int x = int.Parse(sX), y = int.Parse(sY);
                ramForm.Location = new Point(x, y);
            }
            if (diskForm != null && !diskForm.IsDisposed) {
                string sX = INIHelper.Read("DiskForm", "X", diskForm.Location.X.ToString(), iniFile);
                string sY = INIHelper.Read("DiskForm", "Y", diskForm.Location.Y.ToString(), iniFile);
                int x = int.Parse(sX), y = int.Parse(sY);
                diskForm.Location = new Point(x, y);
            }
            if (networkForm != null && !networkForm.IsDisposed) {
                string sX = INIHelper.Read("NetworkForm", "X", networkForm.Location.X.ToString(), iniFile);
                string sY = INIHelper.Read("NetworkForm", "Y", networkForm.Location.Y.ToString(), iniFile);
                int x = int.Parse(sX), y = int.Parse(sY);
                networkForm.Location = new Point(x, y);
            }
            loadGPUFormLocation();
        }

        private void loadGPUFormLocation() {
            if (gpuForm != null && !gpuForm.IsDisposed) {
                string sX = INIHelper.Read("GPUForm0", "X", gpuForm.Location.X.ToString(), iniFile);
                string sY = INIHelper.Read("GPUForm0", "Y", gpuForm.Location.Y.ToString(), iniFile);
                int x = int.Parse(sX), y = int.Parse(sY);
                gpuForm.Location = new Point(x, y);
                int i = 1;
                foreach (var subGpuForm in gpuForm.moreGPUForms) {
                    sX = INIHelper.Read("GPUForm" + i.ToString(), "X", subGpuForm.Location.X.ToString(), iniFile);
                    sY = INIHelper.Read("GPUForm" + i.ToString(), "Y", subGpuForm.Location.Y.ToString(), iniFile);
                    x = int.Parse(sX);
                    y = int.Parse(sY);
                    subGpuForm.Location = new Point(x, y);
                }
            }
        }

        private void doNotShowGPUAtStartToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("DoNotShow", "GPU", "true", iniFile);
            } else {
                INIHelper.Write("DoNotShow", "GPU", "false", iniFile);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
		}
	}
}