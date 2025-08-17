using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Diagnostics;
using static GetSystemStatusGUI.ModuleEnum;

namespace GetSystemStatusGUI {
    public partial class Form1 : DarkAwareForm {
        public CPUForm cpuForm;
        public RAMForm ramForm;
        public DiskForm diskForm;
        public NetworkForm networkForm;
        public GPUForm gpuForm;
        private AboutBox1 aboutBox;
        private string iniFile = ".\\config.ini";
        public bool supportGPU = Environment.OSVersion.Version.Major >= 10;
        private bool showVirtual = false;
        private string[] startArgs;
        public const float lowDPIScale = 0.8f;
        public bool lowDPIEnabled {
            get { return this.lowDPIModeToolStripMenuItem.Checked; }
            set { this.lowDPIModeToolStripMenuItem.Checked = value; }
        }

        public Form1(string[] args) {
            this.startArgs = args;
            SetProcessorAffinity();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            if (!supportGPU) {
                showGPU.Enabled = false;
                showGPU.Text = "Show GPU (Only available in Windows 10)";
            }

            string darkModeStr = INIHelper.Read("DarkMode", "Mode", "0", iniFile);
            int darkMode = int.Parse(darkModeStr);
            switch (darkMode) {
                case 0:
                    Global.IsDarkMode = SystemThemeHelper.IsDarkModeEnabled();
                    checkDarkSystemDefault();
                    break;
                case 1:
                    Global.IsDarkMode = true;
                    checkDarkEnabled();
                    break;
                case 2:
                    Global.IsDarkMode = false;
                    checkDarkDisabled();
                    break;
            }

            bool doNotShowGPU = bool.Parse(INIHelper.Read("DoNotShow", "GPU", "false", iniFile));
            doNotShowGPUAtStartToolStripMenuItem.Checked = doNotShowGPU;

            showCPU.Checked = true;
            showRAM.Checked = true;
            showDisk.Checked = true;
            showNetwork.Checked = true;

            if (startArgs.Length > 0) {
                bool showcpu = false, showram = false, showdisk = false, shownetwork = false, showgpu = false, lowdpi = false;
                for (int i = 0; i < startArgs.Length; i++) {
                    string arg = startArgs[i];
                    if (arg.EndsWith("-cpu")) showcpu = true;
                    else if (arg.EndsWith("-ram")) showram = true;
                    else if (arg.EndsWith("-disk")) showdisk = true;
                    else if (arg.EndsWith("-network")) shownetwork = true;
                    else if (arg.EndsWith("-gpu") && supportGPU) showgpu = true;
                    else if (arg.EndsWith("-lowdpi")) lowdpi = true;
                    else if (arg.EndsWith("-interval")) {
                        if (i < startArgs.Length - 1) {
                            if (float.TryParse(startArgs[i + 1], out float intervalSec)) {
                                // Global.interval_ms = (int)Math.Round(intervalSec * 1000);
                                cbUpdateInterval.Text = intervalSec.ToString() + " sec";
                                i++;
                            }
                        }
                    }
                }
                showCPU.Checked = showcpu;
                showRAM.Checked = showram;
                showDisk.Checked = showdisk;
                showNetwork.Checked = shownetwork;
                showGPU.Checked = showgpu;
                lowDPIEnabled = lowdpi;
            } else {
                if (supportGPU) showGPU.Checked = !doNotShowGPU;
            }

            bool loadLocation = bool.Parse(INIHelper.Read("LoadAtStartup", "Location", "true", iniFile));
            loadAtStartup.Checked = loadLocation;
            bool loadSize = bool.Parse(INIHelper.Read("LoadAtStartup", "Size", "false", iniFile));
            loadSizeAtStartup.Checked = loadSize;
            if (loadLocation) LoadSavedLocation();
            if (loadSize) LoadSavedSize();
            LoadTopMost();
            FixMainFormDPI();
            FixToolStripDPI();

            ApplyDarkMode();

            foreach (Form form in Application.OpenForms) {
                if (form != this && !form.IsDisposed && form.Visible && DoWindowsOverlap(this, form)) {
                    this.WindowState = FormWindowState.Minimized;
                    break;
                }
            }
        }

        public override void ApplyDarkMode() {
            base.ApplyDarkMode();

            if (cpuForm != null && !cpuForm.IsDisposed)
                cpuForm.ApplyDarkMode();
            if (ramForm != null && !ramForm.IsDisposed)
                ramForm.ApplyDarkMode();
            if (diskForm != null && !diskForm.IsDisposed)
                diskForm.ApplyDarkMode();
            if (networkForm != null && !networkForm.IsDisposed)
                networkForm.ApplyDarkMode();
            if (gpuForm != null && !gpuForm.IsDisposed)
                gpuForm.ApplyDarkMode();
        }

        public static bool DoWindowsOverlap(Form form1, Form form2) {
            Rectangle form1Bounds = form1.RectangleToScreen(form1.ClientRectangle);
            Rectangle form2Bounds = form2.RectangleToScreen(form2.ClientRectangle);
            return Rectangle.Intersect(form1Bounds, form2Bounds) != Rectangle.Empty;
        }

        private void SetProcessorAffinity() {
            if (!Global.enableAffinity) return;
            Process proc = Process.GetCurrentProcess();
            long affinityMask = (long)proc.ProcessorAffinity;
            int cpuCnt = Environment.ProcessorCount;
            int doNotUseFirstCores = Global.doNotUseFirstCores;
            if (doNotUseFirstCores >= cpuCnt) doNotUseFirstCores = cpuCnt / 2;
            long secondMask = affinityMask << doNotUseFirstCores;
            affinityMask &= secondMask;
            proc.ProcessorAffinity = (IntPtr)affinityMask;
        }

        private void showCPU_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (cpuForm == null || cpuForm.IsDisposed)
                    cpuForm = new CPUForm(this);
                cpuForm.Show();
                btnFocusCPU.Enabled = true;
            } else {
                cpuForm.Hide();
                btnFocusCPU.Enabled = false;
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
                    showGPU.Text += " (No graphics detected)";
                    break;
                case "noNetwork":
                    showNetwork.Checked = false;
                    showNetwork.Enabled = false;
                    showNetwork.Text = "Show Network (No connections)";
                    break;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void FixMainFormDPI() {
            Point cLocation = this.Location;
            this.Location = new Point(0, 0);
            this.Location = cLocation;
        }

        private void FixToolStripDPI() {
            foreach (var control in this.Controls) {
                if (control is ToolStrip) {
                    ToolStrip ts = control as ToolStrip;
                    float dpi = this.GetWinScaling();
                    for (int i = 0; i < ts.Items.Count; i++) {
                        var item = ts.Items[i] as ToolStripMenuItem;
                        item.Height = (int)Math.Round(item.Height * dpi);
                        item.Width = (int)Math.Round(item.Width * dpi);
                    }
                }
            }
        }

        private float GetWinScaling() {
            Graphics graphics = this.CreateGraphics();
            float scale = graphics.DpiX / 96.0f;
            return scale;
        }

        private void showRAM_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (ramForm == null || ramForm.IsDisposed) ramForm = new RAMForm(this);
                ramForm.Show();
                btnFocusRAM.Enabled = true;
            } else {
                ramForm.Hide();
                btnFocusRAM.Enabled = false;
            }
        }

        private void showDisk_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (diskForm == null || diskForm.IsDisposed) diskForm = new DiskForm(this);
                diskForm.Show();
                btnFocusDisk.Enabled = true;
            } else {
                diskForm.Hide();
                btnFocusDisk.Enabled = false;
            }
        }

        private void showNetwork_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (networkForm == null || networkForm.IsDisposed) networkForm = new NetworkForm(this);
                if (!networkForm.IsDisposed) {
                    networkForm.Show();
                    btnFocusNetwork.Enabled = true;
                }
            } else {
                if (networkForm != null) networkForm.Hide();
                btnFocusNetwork.Enabled = false;
            }
        }

        private void showGPU_CheckedChanged(object sender, EventArgs e) {
            CheckBox self = (CheckBox)sender;
            if (self.Checked) {
                if (gpuForm == null || gpuForm.IsDisposed) gpuForm = new GPUForm(this);
                if (!gpuForm.IsDisposed) {
                    gpuForm.Show();
                    btnFocusGPU.Enabled = true;
                    loadGPUFormLocation();
                    gpuForm.TopMost = gPUFormToolStripMenuItem.Checked;
                }
            } else {
                if (gpuForm != null) gpuForm.Dispose();
                btnFocusGPU.Enabled = false;
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
            } catch { }
        }

        public void btnDiskRefresh_Click(object sender, EventArgs e) {
            var diskLocation = diskForm.Location;
            List<Point> diskLocations = new List<Point>();
            foreach (var subdiskform in diskForm.moreDiskForms) {
                diskLocations.Add(subdiskform.Location);
            }
            this.showDisk.Checked = false;
            if (!diskForm.IsDisposed) diskForm.Dispose();
            diskForm = new DiskForm(this);
            this.showDisk.Checked = true;
            this.diskForm.Location = diskLocation;
            for (int i = 0; i < diskForm.moreDiskForms.Count; i++) {
                diskForm.moreDiskForms[i].Location = diskLocations[i];
            }
        }

        private void btnNetworkRefresh_Click(object sender, EventArgs e) {
            this.showNetwork.Enabled = true;
            this.showNetwork.Text = "Show Network and Adapter Speed";
            var networkLocation = networkForm.Location;
            this.showNetwork.Checked = false;
            if (!networkForm.IsDisposed) networkForm.Dispose();
            networkForm = new NetworkForm(this, this.showVirtual);
            this.showNetwork.Checked = true;
            this.networkForm.Location = networkLocation;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (aboutBox != null) aboutBox.Dispose();
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
                INIHelper.Write("CPUForm", "Width", cpuForm.Width.ToString(), iniFile);
                INIHelper.Write("CPUForm", "Height", cpuForm.Height.ToString(), iniFile);
            }
            if (ramForm != null && !ramForm.IsDisposed) {
                INIHelper.Write("RAMForm", "X", ramForm.Location.X.ToString(), iniFile);
                INIHelper.Write("RAMForm", "Y", ramForm.Location.Y.ToString(), iniFile);
            }
            if (diskForm != null && !diskForm.IsDisposed) {
                INIHelper.Write("DiskForm", "X", diskForm.Location.X.ToString(), iniFile);
                INIHelper.Write("DiskForm", "Y", diskForm.Location.Y.ToString(), iniFile);
                INIHelper.Write("DiskForm", "Width", diskForm.Width.ToString(), iniFile);
                INIHelper.Write("DiskForm", "Height", diskForm.Height.ToString(), iniFile);
                for (int i = 0; i < diskForm.moreDiskForms.Count; i++) {
                    var cSubDiskForm = diskForm.moreDiskForms[i];
                    INIHelper.Write("DiskForm" + (i + 1).ToString(), "X", cSubDiskForm.Location.X.ToString(), iniFile);
                    INIHelper.Write("DiskForm" + (i + 1).ToString(), "Y", cSubDiskForm.Location.Y.ToString(), iniFile);
                    INIHelper.Write("DiskForm" + (i + 1).ToString(), "Width", cSubDiskForm.Width.ToString(), iniFile);
                    INIHelper.Write("DiskForm" + (i + 1).ToString(), "Height", cSubDiskForm.Height.ToString(), iniFile);
                }
            }
            if (networkForm != null && !networkForm.IsDisposed) {
                INIHelper.Write("NetworkForm", "X", networkForm.Location.X.ToString(), iniFile);
                INIHelper.Write("NetworkForm", "Y", networkForm.Location.Y.ToString(), iniFile);
                INIHelper.Write("NetworkForm", "Width", networkForm.Width.ToString(), iniFile);
                INIHelper.Write("NetworkForm", "Height", networkForm.Height.ToString(), iniFile);
            }
            if (gpuForm != null && !gpuForm.IsDisposed) {
                INIHelper.Write("GPUForm0", "X", gpuForm.Location.X.ToString(), iniFile);
                INIHelper.Write("GPUForm0", "Y", gpuForm.Location.Y.ToString(), iniFile);
                //INIHelper.Write("GPUForm0", "Width", gpuForm.Width.ToString(), iniFile);
                //INIHelper.Write("GPUForm0", "Height", gpuForm.Height.ToString(), iniFile);
                int i = 1;
                foreach (var subGpuForm in gpuForm.moreGPUForms) {
                    INIHelper.Write("GPUForm" + i.ToString(), "X", subGpuForm.Location.X.ToString(), iniFile);
                    INIHelper.Write("GPUForm" + i.ToString(), "Y", subGpuForm.Location.Y.ToString(), iniFile);
                    //INIHelper.Write("GPUForm" + i.ToString(), "Width", subGpuForm.Width.ToString(), iniFile);
                    //INIHelper.Write("GPUForm" + i.ToString(), "Height", subGpuForm.Height.ToString(), iniFile);
                    i++;
                }
            }
        }

        private void loadSavedLocationsToolStripMenuItem_Click(object sender, EventArgs e) {
            LoadSavedLocation();
            LoadSavedSize();
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
                for (int i = 1; i <= diskForm.moreDiskForms.Count; i++) {
                    var cSubDiskForm = diskForm.moreDiskForms[i - 1];
                    sX = INIHelper.Read("DiskForm" + i.ToString(), "X", cSubDiskForm.Location.X.ToString(), iniFile);
                    sY = INIHelper.Read("DiskForm" + i.ToString(), "Y", cSubDiskForm.Location.Y.ToString(), iniFile);
                    x = int.Parse(sX);
                    y = int.Parse(sY);
                    cSubDiskForm.Location = new Point(x, y);
                }
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
                    i++;
                }
            }
        }

        private void LoadSavedSize() {
            if (cpuForm != null && !cpuForm.IsDisposed) {
                string sWidth = INIHelper.Read("CPUForm", "Width", cpuForm.Width.ToString(), iniFile);
                string sHeight = INIHelper.Read("CPUForm", "Height", cpuForm.Height.ToString(), iniFile);
                int width = int.Parse(sWidth), height = int.Parse(sHeight);
                cpuForm.Size = new Size(width, height);
            }
            if (diskForm != null && !diskForm.IsDisposed) {
                string sWidth = INIHelper.Read("DiskForm", "Width", diskForm.Width.ToString(), iniFile);
                string sHeight = INIHelper.Read("DiskForm", "Height", diskForm.Height.ToString(), iniFile);
                int width = int.Parse(sWidth), height = int.Parse(sHeight);
                diskForm.Size = new Size(width, height);
                for (int i = 1; i <= diskForm.moreDiskForms.Count; i++) {
                    var cSubDiskForm = diskForm.moreDiskForms[i - 1];
                    sWidth = INIHelper.Read("DiskForm" + i.ToString(), "Width", diskForm.Width.ToString(), iniFile);
                    sHeight = INIHelper.Read("DiskForm" + i.ToString(), "Height", diskForm.Height.ToString(), iniFile);
                    width = int.Parse(sWidth);
                    height = int.Parse(sHeight);
                    cSubDiskForm.Size = new Size(width, height);
                }
            }
            if (networkForm != null && !networkForm.IsDisposed) {
                string sWidth = INIHelper.Read("NetworkForm", "Width", networkForm.Width.ToString(), iniFile);
                string sHeight = INIHelper.Read("NetworkForm", "Height", networkForm.Height.ToString(), iniFile);
                int width = int.Parse(sWidth), height = int.Parse(sHeight);
                networkForm.Size = new Size(width, height);
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
            if (this.aboutBox == null || this.aboutBox.IsDisposed)
                this.aboutBox = new AboutBox1();
            this.aboutBox.Show();
            this.aboutBox.Focus();
        }

        private void loadAtStartup_Click(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("LoadAtStartup", "Location", "true", iniFile);
            } else {
                INIHelper.Write("LoadAtStartup", "Location", "false", iniFile);
            }
        }

        private void loadSizeAtStartup_Click(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("LoadAtStartup", "Size", "true", iniFile);
            } else {
                INIHelper.Write("LoadAtStartup", "Size", "false", iniFile);
            }
        }

        private void btnFocus_Click(object sender, EventArgs e) {
            if (showRAM.Checked) ramForm.Focus();
            if (showNetwork.Checked) networkForm.Focus();
            if (showDisk.Checked) diskForm.Focus();
            if (showGPU.Checked) gpuForm.Focus();
            if (showCPU.Checked) cpuForm.Focus();
        }

        private void fakeToolStripMenuItem_Click(object sender, EventArgs e) {
            string str = Interaction.InputBox("Input the number of logical processor cores you want (0 for default):", "Fake CPU cores");
            try {
                if (str != string.Empty) {
                    int core_num = int.Parse(str);
                    if (core_num < 0 || core_num > 448) throw new Exception("Number of cores is too large or below 0");
                    this.showCPU.Text = "Loading CPU...";
                    var cpuLocation = cpuForm.Location;
                    this.showCPU.Checked = false;
                    if (!cpuForm.IsDisposed) cpuForm.Dispose();
                    cpuForm = new CPUForm(this, core_num);
                    this.showCPU.Text = "Show CPU Utilizations";
                    this.showCPU.Checked = true;
                    this.cpuForm.Location = cpuLocation;
                }
            } catch (Exception ex) {
                MessageBox.Show("Not valid: " + ex.Message, "Invalid core number", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void showVirtualNetworkToolStrip_Click(object sender, EventArgs e) {
            ToolStripMenuItem sd = sender as ToolStripMenuItem;
            this.showVirtual = sd.Checked;
            this.showNetwork.Enabled = true;
            this.showNetwork.Text = "Show Network and Adapter Speed";
            var networkLocation = networkForm.Location;
            this.showNetwork.Checked = false;
            if (!networkForm.IsDisposed) networkForm.Dispose();
            networkForm = new NetworkForm(this, this.showVirtual);
            this.showNetwork.Checked = true;
            this.networkForm.Location = networkLocation;
        }

        private void btnFocusCPU_Click(object sender, EventArgs e) {
            cpuForm.Focus();
        }

        private void btnFocusRAM_Click(object sender, EventArgs e) {
            ramForm.Focus();
        }

        private void btnFocusDisk_Click(object sender, EventArgs e) {
            diskForm.Focus();
        }

        private void btnFocusNetwork_Click(object sender, EventArgs e) {
            networkForm.Focus();
        }

        private void btnFocusGPU_Click(object sender, EventArgs e) {
            gpuForm.Focus();
        }

        private void Form1_DpiChanged(object sender, DpiChangedEventArgs e) {
            if (e.DeviceDpiNew != e.DeviceDpiOld) {
                float scale = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
                ChangeScale(scale);
            }
        }

        private void ChangeScale(float scale) {
            foreach (var control in this.Controls) {
                if (control is CheckBox) {
                    CheckBox cb = control as CheckBox;
                    cb.Font = Utility.ScaleFont(cb.Font, scale);
                } else if (control is ComboBox) {
                    ComboBox cb = control as ComboBox;
                    cb.Font = Utility.ScaleFont(cb.Font, scale);
                } else if (control is Label) {
                    Label lbl = control as Label;
                    lbl.Font = Utility.ScaleFont(lbl.Font, scale);
                } else if (control is ToolStrip) {
                    ToolStrip ts = control as ToolStrip;
                    for (int i = 0; i < ts.Items.Count; i++) {
                        var item = ts.Items[i] as ToolStripMenuItem;
                        item.Height = (int)Math.Round(item.Height * scale);
                        item.Width = (int)Math.Round(item.Width * scale);
                        item.Font = Utility.ScaleFont(item.Font, scale);
                    }
                } else if (control is Button) {
                    Button button = control as Button;
                    button.Font = Utility.ScaleFont(button.Font, scale);
                }
            }
        }

        public void EnableLowDPI(float scale) {
            this.ChangeScale(scale);
            foreach (var control in this.Controls) {
                if (control is CheckBox) {
                    CheckBox checkBox = control as CheckBox;
                    checkBox.Top = (int)Math.Round(checkBox.Top * scale);
                    checkBox.Left = (int)Math.Round(checkBox.Left * scale);
                } else if (control is ComboBox) {
                    ComboBox comboBox = control as ComboBox;
                    comboBox.Top = (int)Math.Round(comboBox.Top * scale);
                    comboBox.Left = (int)Math.Round(comboBox.Left * scale);
                } else if (control is Button) {
                    Button button = control as Button;
                    button.Top = (int)Math.Round(button.Top * scale);
                    button.Left = (int)Math.Round(button.Left * scale);
                    button.Width = (int)Math.Round(button.Width * scale);
                    button.Height = (int)Math.Round(button.Height * scale);
                } else if (control is Label) {
                    Label label = control as Label;
                    label.Top = (int)Math.Round(label.Top * scale);
                    label.Left = (int)Math.Round(label.Left * scale);
                }
            }
            this.Width = (int)Math.Round(this.Width * scale);
            this.Height = (int)Math.Round(this.Height * scale);
        }

        public void DisableLowDPI(float scale) {
            this.EnableLowDPI(1 / scale);
        }

        private void CPUFormToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("CPUForm", "TopMost", "true", iniFile);
                cpuForm.TopMost = true;
            } else {
                INIHelper.Write("CPUForm", "TopMost", "false", iniFile);
                cpuForm.TopMost = false;
            }
        }

        private void ramFormToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("RAMForm", "TopMost", "true", iniFile);
                ramForm.TopMost = true;
            } else {
                INIHelper.Write("RAMForm", "TopMost", "false", iniFile);
                ramForm.TopMost = false;
            }
        }

        private void diskFormToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("DiskForm", "TopMost", "true", iniFile);
                diskForm.TopMost = true;
            } else {
                INIHelper.Write("DiskForm", "TopMost", "false", iniFile);
                diskForm.TopMost = false;
            }
        }

        private void networkFormToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("NetworkForm", "TopMost", "true", iniFile);
                networkForm.TopMost = true;
            } else {
                INIHelper.Write("NetworkForm", "TopMost", "false", iniFile);
                networkForm.TopMost = false;
            }
        }

        private void gPUFormToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            ToolStripMenuItem here = sender as ToolStripMenuItem;
            if (here.Checked) {
                INIHelper.Write("GPUForm0", "TopMost", "true", iniFile);
                if (gpuForm != null && !gpuForm.IsDisposed)
                    gpuForm.TopMost = true;
            } else {
                INIHelper.Write("GPUForm0", "TopMost", "false", iniFile);
                if (gpuForm != null && !gpuForm.IsDisposed)
                    gpuForm.TopMost = false;
            }
        }

        private void LoadTopMost() {
            string topMost = INIHelper.Read("CPUForm", "TopMost", "false", iniFile);
            CPUFormToolStripMenuItem.Checked = bool.Parse(topMost);
            topMost = INIHelper.Read("RAMForm", "TopMost", "false", iniFile);
            ramFormToolStripMenuItem.Checked = bool.Parse(topMost);
            topMost = INIHelper.Read("DiskForm", "TopMost", "false", iniFile);
            diskFormToolStripMenuItem.Checked = bool.Parse(topMost);
            topMost = INIHelper.Read("NetworkForm", "TopMost", "false", iniFile);
            networkFormToolStripMenuItem.Checked = bool.Parse(topMost);
            topMost = INIHelper.Read("GPUForm0", "TopMost", "false", iniFile);
            gPUFormToolStripMenuItem.Checked = bool.Parse(topMost);
        }

        public bool TopMostChecked(FormType formType) {
            switch (formType) {
                case FormType.CPU:
                    return CPUFormToolStripMenuItem.Checked;
                case FormType.RAM:
                    return ramFormToolStripMenuItem.Checked;
                case FormType.Disk:
                    return diskFormToolStripMenuItem.Checked;
                case FormType.Network:
                    return networkFormToolStripMenuItem.Checked;
                case FormType.GPU:
                    return gPUFormToolStripMenuItem.Checked;
                default:
                    return false;
            }
        }

        private void lowDPIModeToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            const float scale = lowDPIScale;
            bool enableLowDPI = lowDPIModeToolStripMenuItem.Checked;
            if (enableLowDPI) {
                this.EnableLowDPI(scale);
                cpuForm.EnableLowDPI(scale);
                ramForm.EnableLowDPI(scale);
                diskForm.EnableLowDPI(scale);
                networkForm.EnableLowDPI(scale);
                if (gpuForm != null && !gpuForm.IsDisposed) gpuForm.EnableLowDPI(scale);
            } else {
                this.DisableLowDPI(scale);
                cpuForm.DisableLowDPI(scale);
                ramForm.DisableLowDPI(scale);
                diskForm.DisableLowDPI(scale);
                networkForm.DisableLowDPI(scale);
                if (gpuForm != null && !gpuForm.IsDisposed) gpuForm.DisableLowDPI(scale);
            }
        }

        private void enableToolStripMenuItem_Click(object sender, EventArgs e) {
            Global.IsDarkMode = true;
            ApplyDarkMode();

            INIHelper.Write("DarkMode", "Mode", "1", iniFile);
            checkDarkEnabled();
        }

        private void checkDarkEnabled() {
            enableToolStripMenuItem.Checked = true;
            systemDefaultToolStripMenuItem.Checked = false;
            disableToolStripMenuItem.Checked = false;
        }

        private void systemDefaultToolStripMenuItem_Click(object sender, EventArgs e) {
            Global.IsDarkMode = SystemThemeHelper.IsDarkModeEnabled();
            if (Global.IsDarkMode)
                ApplyDarkMode();

            INIHelper.Write("DarkMode", "Mode", "0", iniFile);
            checkDarkSystemDefault();
        }

        private void checkDarkSystemDefault() {
            systemDefaultToolStripMenuItem.Checked = true;
            enableToolStripMenuItem.Checked = false;
            disableToolStripMenuItem.Checked = false;
        }

        private void disableToolStripMenuItem_Click(object sender, EventArgs e) {
            Global.IsDarkMode = false;

            INIHelper.Write("DarkMode", "Mode", "2", iniFile);
            checkDarkDisabled();
        }

        private void checkDarkDisabled() {
            disableToolStripMenuItem.Checked = true;
            enableToolStripMenuItem.Checked = false;
            systemDefaultToolStripMenuItem.Checked = false;
        }
    }
}
