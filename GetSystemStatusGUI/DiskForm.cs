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

namespace GetSystemStatusGUI {
    public partial class DiskForm : Form {
        private DiskInfo diskInfo;
        private Chart[] subCharts;
        private Color chartColor = Color.FromArgb(120, Color.LimeGreen);
        private Color borderColor = Color.FromArgb(180, Color.LimeGreen);
        private int beginTop;
        private int rows = 1, columns = 1;
        private const double margin_ratio = 35;
        private const int history_length = 60;
        private Form1 mainform;

        public DiskForm(Form1 mainform) {
            InitializeComponent();
            diskInfo = new DiskInfo();
            this.mainform = mainform;
        }

        private void DiskForm_Load(object sender, EventArgs e) {
            List<int> y = new List<int>();
            for (int i = 0; i < history_length; i++) y.Add(0);
            beginTop = label1.Location.Y + label1.Size.Height;
            Utility.FactorDecompose(diskInfo.m_DiskNum, ref columns, ref rows);
            subCharts = new Chart[diskInfo.m_DiskNum];
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    Chart chart = new Chart();
                    chart.Palette = ChartColorPalette.None;
                    chart.PaletteCustomColors = new Color[] { chartColor };
                    chart.Series.Add(cid.ToString());
                    chart.Series[0].Points.DataBindY(y);
                    chart.Series[0].ChartType = SeriesChartType.SplineArea;
                    chart.Series[0].BorderColor = borderColor;
                    chart.ChartAreas.Add(cid.ToString());
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 100;
                    chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGreen;
                    chart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGreen;
                    chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.LightGreen;
                    chart.ChartAreas[0].AxisX.LineColor = Color.LimeGreen;
                    chart.ChartAreas[0].AxisY.LineColor = Color.LimeGreen;
                    chart.ChartAreas[0].AxisX.LineWidth = 2;
                    chart.ChartAreas[0].AxisY.LineWidth = 2;
                    chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisX2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX2.LineColor = Color.LimeGreen;
                    chart.ChartAreas[0].AxisX2.LineWidth = 2;
                    chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY2.LineColor = Color.LimeGreen;
                    chart.ChartAreas[0].AxisY2.LineWidth = 2;
                    chart.Titles.Add(cid.ToString() + "_0");
                    chart.Titles[0].Text = "Disk " + cid.ToString();
                    chart.Titles[0].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[0].DockedToChartArea = cid.ToString();
                    chart.Titles[0].IsDockedInsideChartArea = false;
                    chart.Titles[0].Font = new Font(FontFamily.GenericSansSerif, 14);
                    chart.Titles.Add(cid.ToString() + "_1");
                    //chart.Titles[1].Text = "Load rate in 60 secs";
                    chart.Titles[1].Text = diskInfo.DiskModel(cid);
                    chart.Titles[1].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[1].DockedToChartArea = cid.ToString();
                    chart.Titles[1].IsDockedInsideChartArea = false;
                    chart.Titles[1].ForeColor = SystemColors.GrayText;
                    chart.Titles.Add(cid.ToString() + "_2");
                    chart.Titles[2].Text = "Read 0KB/s\tWrite 0KB/s";
                    chart.Titles[2].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[2].DockedToChartArea = cid.ToString();
                    chart.Titles[2].Docking = Docking.Bottom;
                    chart.Titles[2].IsDockedInsideChartArea = false;
                    chart.Titles[2].Font = new Font(FontFamily.GenericSansSerif, 11);
                    chart.Titles[2].ForeColor = ColorTranslator.FromHtml("#494949");
                    subCharts[cid] = chart;
                    this.Controls.Add(subCharts[cid]);
                }
            }
            InitialSize();
            DiskForm_Resize(null, null);
            new Action(disk_load_thread).BeginInvoke(null, null);
        }

        private void DiskForm_Resize(object sender, EventArgs e) {
            int marginHorizontal = (int)Math.Round((double)Math.Min(this.Size.Height, this.Size.Width) / (double)margin_ratio);
            int marginVertical = marginHorizontal / 3;
            int endRight = (int)Math.Round((double)marginHorizontal * 1.1);
            int fixHeight = Math.Max(40, marginHorizontal * 2);
            int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop - fixHeight - (rows + 1) * marginVertical) / (double)rows);
            int chartWidth = (int)Math.Round((double)(this.Size.Width - endRight - (columns + 1) * marginHorizontal) / (double)columns);
            if (chartHeight <= 0 || chartWidth <= 0 || subCharts == null) return;
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    int startX = (j + 1) * marginHorizontal + j * chartWidth;
                    int startY = beginTop + (i + 1) * marginVertical + i * chartHeight;
                    subCharts[cid].Size = new Size(chartWidth, chartHeight);
                    subCharts[cid].Location = new Point(startX, startY);
                }
            }
        }

        private void InitialSize() {
            int iHeight = beginTop + rows * 200 + (rows + 1) * 5;
            int iWidth = columns * 180 + (columns + 1) * 10;
            iHeight = Math.Max(iHeight, this.Size.Height);
            iWidth = Math.Max(iWidth, this.Size.Width);
            this.Size = new Size(iWidth, iHeight);
        }

        private void DiskForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainform.DisableChecked("Disk");
        }

        private void disk_load_thread() {
            List<float>[] ys = new List<float>[diskInfo.m_DiskNum];
            for (int i = 0; i < diskInfo.m_DiskNum; i++) {
                ys[i] = new List<float>();
                for (int j = 0; j < history_length; j++) ys[i].Add(0);
            }
            while (!subCharts[0].IsDisposed) {
                for (int i = 0; i < diskInfo.m_DiskNum; i++) {
                    ys[i].RemoveAt(0);
                    ys[i].Add(diskInfo.DiskLoad(i));
                }
                Action updateChart = new Action(
                    delegate () {
                        for (int i = 0; i < diskInfo.m_DiskNum; i++) {
                            subCharts[i].Series[0].Points.DataBindY(ys[i]);
                            string rw_speed = Utility.FormatDuplexString("Read", diskInfo.DiskRead(i), "Write", diskInfo.DiskWrite(i), 1024);
                            subCharts[i].Titles[2].Text = rw_speed;
                        }
                    }
                );
                try { Invoke(updateChart); }
				catch { break; }
                Thread.Sleep(Global.interval_ms);
            }
        }
    }

    public class DiskInfo {
        private PerformanceCounter[] pcDisksRead;   //每磁盘读速率
        private PerformanceCounter[] pcDisksWrite;  //每磁盘写速率
        private PerformanceCounter[] pcDisksLoad;   //磁盘占用率
        private PerformanceCounter pcDiskRead;  //总磁盘读速率
        private PerformanceCounter pcDiskWrite; //总磁盘写速率
        public int m_DiskNum = 0;    //磁盘个数
        public List<string> DiskInstanceNames = new List<string>();
        private string[] diskModelCaption;   //磁盘型号名称

        // 构造函数，初始化计数器
        public DiskInfo() {
            pcDiskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            pcDiskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            PerformanceCounterCategory diskPfc = new PerformanceCounterCategory("PhysicalDisk");
            string[] _diskInstanceNames = diskPfc.GetInstanceNames();
            m_DiskNum = _diskInstanceNames.Length - 1;
            string[] diskInstanceNames = new string[m_DiskNum];
            foreach (string disk in _diskInstanceNames) {
                string[] split = disk.Split(' ');
                try {
                    int cid = int.Parse(split[0]);
                    diskInstanceNames[cid] = disk;
                }
                catch { }
            }
            pcDisksRead = new PerformanceCounter[m_DiskNum];
            pcDisksWrite = new PerformanceCounter[m_DiskNum];
            pcDisksLoad = new PerformanceCounter[m_DiskNum];
            int c = 0;
            for (int i = 0; i < diskInstanceNames.Length; i++) {
                if (diskInstanceNames[i] != "_Total") {
                    pcDisksRead[c] = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", diskInstanceNames[i]);
                    pcDisksWrite[c] = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", diskInstanceNames[i]);
                    pcDisksLoad[c] = new PerformanceCounter("PhysicalDisk", "% Idle Time", diskInstanceNames[i]);
                    DiskInstanceNames.Add(diskInstanceNames[i]);
                    c++;
                }
            }
            pcDiskRead.MachineName = ".";
            pcDiskRead.NextValue();
            pcDiskWrite.NextValue();

            for (int i = 0; i < m_DiskNum; i++) { pcDisksRead[i].NextValue(); pcDisksWrite[i].NextValue(); pcDisksLoad[i].NextValue(); }

            diskModelCaption = new string[m_DiskNum];
            var wmi_disk = new ManagementObjectSearcher("select * from Win32_DiskDrive");
            foreach (var o in wmi_disk.Get()) {
                var mo = (ManagementObject)o;
                string cDeviceID = mo["DeviceID"].ToString();
                string cModel = mo["Model"].ToString();
                const string s_phydrv = @"\\.\PHYSICALDRIVE";
                int si = cDeviceID.IndexOf(s_phydrv) + s_phydrv.Length;
                string scid = cDeviceID.Substring(si);
                int cid = int.Parse(scid);
                diskModelCaption[cid] = cModel;
            }
        }

        // 磁盘型号
        public string DiskModel(int id) {
            return diskModelCaption[id];
        }

        // 磁盘占用、读写速率
        public float DiskReadTotal {
            get { return pcDiskRead.NextValue(); }
        }
        public float DiskWriteTotal {
            get { return pcDiskWrite.NextValue(); }
        }
        public float DiskRead(int diskId) {
            return pcDisksRead[diskId].NextValue();
        }
        public float DiskWrite(int diskId) {
            return pcDisksWrite[diskId].NextValue();
        }
        public float DiskLoad(int diskId) {
            return Math.Max(0, 100 - pcDisksLoad[diskId].NextValue());
        }
    }
}