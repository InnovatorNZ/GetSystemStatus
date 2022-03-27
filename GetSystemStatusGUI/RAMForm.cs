using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static GetSystemStatusGUI.ModuleEnum;

namespace GetSystemStatusGUI {
    public partial class RAMForm : Form {
        private const int historyLength = 60;
        private RAMInfo ramInfo;
        private static string[] scale_unit = { "Bytes", "KB", "MB", "GB", "TB" };
        private Color chartColor = Color.FromArgb(105, 139, 0, 139);
        private Color borderColor = Color.FromArgb(180, 139, 0, 139);
        private Form1 mainform;
        private float fLineWidth = 2;
        private float fGridWidth = 1;

        public RAMForm(Form1 mainform) {
            ramInfo = new RAMInfo();
            InitializeComponent();
            this.mainform = mainform;
        }

        private void RAMForm_Load(object sender, EventArgs e) {
            List<int> list = new List<int>();
            for (int i = 0; i < historyLength; i++) list.Add(0);
            chart1.Series[0].Points.DataBindY(list);
            chart1.PaletteCustomColors[0] = chartColor;
            chart1.Series[0].BorderColor = borderColor;
            fLineWidth = chart1.ChartAreas[0].AxisX.LineWidth;
            fGridWidth = chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth;
            new Action(ram_update_thread).BeginInvoke(null, null);
        }

        public new void Show() {
            base.Show();
            this.TopMost = mainform.TopMostChecked(FormType.RAM);
        }

        private void ram_update_thread() {
            List<int> usageList = new List<int>();
            for (int i = 0; i < historyLength; i++) usageList.Add(0);
            while (!chart1.IsDisposed && !lblRAM.IsDisposed) {
                if (this.Visible) {
                    int rusage = (int)Math.Round((1.0 - (double)ramInfo.MemoryAvailable / (double)ramInfo.PhysicalMemory) * 100.0);
                    int ramScale = (int)Math.Floor(Math.Log(ramInfo.PhysicalMemory - ramInfo.MemoryAvailable, 1024));
                    double memAvail = Math.Round((double)ramInfo.MemoryAvailable / Math.Pow(1024, ramScale), 1);
                    double memTotal = Math.Round((double)ramInfo.PhysicalMemory / Math.Pow(1024, ramScale), 1);
                    usageList.RemoveAt(0);
                    usageList.Add(rusage);
                    Action updateChart = new Action(
                        delegate () {
                            if (ramScale == 2)
                                lblRAM.Text = string.Format("{0:f0} / {1:f0}{2} ({3}%)", memTotal - memAvail, memTotal, scale_unit[ramScale], rusage);
                            else if (ramScale == 3)
                                lblRAM.Text = string.Format("{0:f1} / {1:f1}{2} ({3}%)", memTotal - memAvail, memTotal, scale_unit[ramScale], rusage);
                            else if (ramScale == 4)
                                lblRAM.Text = string.Format("{0:f2} / {1:f2}{2} ({3}%)", memTotal - memAvail, memTotal, scale_unit[ramScale], rusage);
                            chart1.Series[0].Points.DataBindY(usageList);
                        }
                    );
                    try { Invoke(updateChart); } catch { break; }
                }
                Thread.Sleep(Global.interval_ms);
            }
        }

        private void RAMForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainform.DisableChecked("RAM");
        }

        private void RAMForm_Deactivate(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
                mainform.DisableChecked("RAM");
            }
        }

        private void RAMForm_DpiChanged(object sender, DpiChangedEventArgs e) {
            if (e.DeviceDpiNew != e.DeviceDpiOld) {
                float scale = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
                ChangeScale(scale);
            }
        }

        private void ChangeScale(float scale) {
            this.fLineWidth *= scale;
            this.fGridWidth *= scale;
            foreach (var control in this.Controls) {
                if (control is Label) {
                    Label label = control as Label;
                    label.Font = Utility.ScaleFont(label.Font, scale);
                } else if (control is Chart) {
                    Chart subchart = control as Chart;
                    foreach (var title in subchart.Titles) {
                        title.Font = Utility.ScaleFont(title.Font, scale);
                    }
                    foreach (var chartarea in subchart.ChartAreas) {
                        int lineWidth = (int)Math.Round(fLineWidth);
                        int gridLineWidth = (int)Math.Round(fGridWidth);
                        chartarea.AxisX.LineWidth = lineWidth;
                        chartarea.AxisY.LineWidth = lineWidth;
                        chartarea.AxisX2.LineWidth = lineWidth;
                        chartarea.AxisY2.LineWidth = lineWidth;
                        chartarea.AxisX.MajorGrid.LineWidth = gridLineWidth;
                        chartarea.AxisY.MajorGrid.LineWidth = gridLineWidth;
                        chartarea.AxisX.MinorGrid.LineWidth = gridLineWidth;
                    }
                    foreach (var series in subchart.Series) {
                        int borderWidth = (int)Math.Floor(series.BorderWidth * scale);
                        series.BorderWidth = borderWidth;
                    }
                }
            }
        }

        public void EnableLowDPI(float scale) {
            this.ChangeScale(scale);
            label1.Left = (int)Math.Round(label1.Left * scale);
            label1.Top = (int)Math.Round(label1.Top * scale);
            chart1.Left = (int)Math.Round(chart1.Left * scale);
            chart1.Top = (int)Math.Round(chart1.Top * scale);
            chart1.Height = (int)Math.Round(chart1.Height * scale);
            chart1.Width = (int)Math.Round(chart1.Width * scale);
            lblRAM.Left = this.Width - (int)Math.Round((this.Width - lblRAM.Right) * scale) - lblRAM.Width;
            lblRAM.Top = (int)Math.Round(lblRAM.Top * scale);
            this.Width = (int)Math.Round(this.Width * scale);
            this.Height = (int)Math.Round(this.Height * scale);
        }

        public void DisableLowDPI(float scale) {
            this.ChangeScale(1 / scale);
            label1.Left = (int)Math.Round(label1.Left / scale);
            label1.Top = (int)Math.Round(label1.Top / scale);
            chart1.Left = (int)Math.Round(chart1.Left / scale);
            chart1.Top = (int)Math.Round(chart1.Top / scale);
            chart1.Height = (int)Math.Round(chart1.Height / scale);
            chart1.Width = (int)Math.Round(chart1.Width / scale);
            lblRAM.Left = this.Width - (int)Math.Round((this.Width - lblRAM.Right) / scale) - lblRAM.Width;
            lblRAM.Top = (int)Math.Round(lblRAM.Top / scale);
            this.Width = (int)Math.Round(this.Width / scale);
            this.Height = (int)Math.Round(this.Height / scale);
        }
    }

    public class RAMInfo {
        private long m_PhysicalMemory = 0;   //物理内存
        private PerformanceCounter pcAvailMemory;   //可用内存（性能计数器版）

        // 构造函数，初始化计数器
        public RAMInfo() {
            //初始化计数器
            pcAvailMemory = new PerformanceCounter("Memory", "Available Bytes");

            //获得物理内存
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc) {
                if (mo["TotalPhysicalMemory"] != null) {
                    m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
        }

        // 可用内存
        public long MemoryAvailable {
            get {
                return (long)Math.Round(pcAvailMemory.NextValue());
            }
        }
        // 物理内存
        public long PhysicalMemory {
            get {
                return m_PhysicalMemory;
            }
        }
    }
}