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
    public partial class DiskForm : Form {
        private DiskInfo diskInfo;
        public List<DiskForm> moreDiskForms { get; private set; }
        private Form1 mainform;
        private readonly int startId;
        private Chart[] subCharts;
        private Color chartColor = Color.FromArgb(120, Color.LimeGreen);
        private Color borderColor = Color.FromArgb(180, Color.LimeGreen);
        private int rows = 1, columns = 1;
        private const double margin_ratio = 35;
        private const int history_length = 60;
        private float fLineWidth = 2;
        private float fGridWidth = 1;
        private int cDiskNum {
            get { return rows * columns; }
        }
        public new bool TopMost {
            get { return base.TopMost; }
            set {
                if (startId == 0) {
                    foreach (var subform in this.moreDiskForms) {
                        subform.TopMost = value;
                    }
                }
                base.TopMost = value;
            }
        }

        public DiskForm(Form1 mainform, int startId = 0) {
            InitializeComponent();
            moreDiskForms = new List<DiskForm>();
            diskInfo = new DiskInfo();
            this.mainform = mainform;
            this.startId = startId;
            if (startId == 0) {
                List<int> factors = Utility.FactorDisposeRecurse2(diskInfo.m_DiskNum);
                this.setColumnRow(factors[0], factors[1]);
                if (factors.Count > 2) {
                    for (int i = 2; i < factors.Count; i += 2) {
                        int nStartId = factors[i - 2] * factors[i - 1];
                        DiskForm nDiskForm = new DiskForm(mainform, nStartId);
                        nDiskForm.setColumnRow(factors[i], factors[i + 1]);
                        moreDiskForms.Add(nDiskForm);
                    }
                }
            }
        }

        protected void setColumnRow(int column, int row) {
            this.rows = row;
            this.columns = column;
        }

        private void DiskForm_Load(object sender, EventArgs e) {
            List<int> y = new List<int>();
            for (int i = 0; i < history_length; i++) y.Add(0);
            //Utility.FactorDecompose(diskInfo.m_DiskNum, out columns, out rows);
            subCharts = new Chart[rows * columns];
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
                    chart.ChartAreas[0].AxisX.LineWidth = (int)fLineWidth;
                    chart.ChartAreas[0].AxisY.LineWidth = (int)fLineWidth;
                    chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisX2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX2.LineColor = Color.LimeGreen;
                    chart.ChartAreas[0].AxisX2.LineWidth = (int)this.fLineWidth;
                    chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY2.LineColor = Color.LimeGreen;
                    chart.ChartAreas[0].AxisY2.LineWidth = (int)this.fLineWidth;
                    chart.Titles.Add(cid.ToString() + "_0");
                    chart.Titles[0].Text = getChartTitle(cid);
                    chart.Titles[0].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[0].DockedToChartArea = cid.ToString();
                    chart.Titles[0].IsDockedInsideChartArea = false;
                    if (chart.Titles[0].Text.Length <= 18)
                        chart.Titles[0].Font = new Font(FontFamily.GenericSansSerif, 14);
                    else
                        chart.Titles[0].Font = new Font(FontFamily.GenericSansSerif, 11);
                    chart.Titles.Add(cid.ToString() + "_1");
                    //chart.Titles[1].Text = "Load rate in 60 secs";
                    chart.Titles[1].Text = diskInfo.DiskModel(cid + startId);
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
            int beginTop = label1.Location.Y + label1.Size.Height;
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
            int beginTop = label1.Location.Y + label1.Size.Height;
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

        private void DiskForm_Deactivate(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
                mainform.DisableChecked("Disk");
            }
        }

        private void DiskForm_DpiChanged(object sender, DpiChangedEventArgs e) {
            if (e.DeviceDpiNew != e.DeviceDpiOld) {
                new Action(delegate () {
                    Thread.Sleep(150);
                    Invoke(new Action(delegate () {
                        this.DiskForm_Resize(sender, e);
                    }));
                }).BeginInvoke(null, null);
                float scale = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
                fLineWidth *= scale;
                fGridWidth *= scale;
                foreach (var control in this.Controls) {
                    if (control is Label) {
                        Label c = control as Label;
                        c.Font = Utility.ScaleFont(c.Font, scale);
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
                        }
                        foreach (var series in subchart.Series) {
                            int borderWidth = (int)Math.Floor(series.BorderWidth * scale);
                            series.BorderWidth = borderWidth;
                        }
                    }
                }
            }
        }

        public new void Show() {
            base.Show();
            if (startId == 0) {
                foreach (var subForm in moreDiskForms) {
                    subForm.Show();
                }
                this.TopMost = mainform.TopMostChecked(FormType.Disk);
            }
        }

        public new void Hide() {
            if (startId == 0) {
                foreach (var subForm in moreDiskForms) {
                    subForm.Hide();
                }
            }
            base.Hide();
        }

        public new void Dispose() {
            if (startId == 0) {
                foreach (var subform in moreDiskForms) {
                    subform.Dispose();
                }
            }
            base.Dispose();
        }

        public new void Focus() {
            if (startId == 0) {
                foreach (var subform in moreDiskForms) {
                    subform.Focus();
                }
            }
            base.Focus();
        }

        private void DiskForm_Activated(object sender, EventArgs e) {
            mainform.diskForm.Focus();
        }

        private void disk_load_thread() {
            List<float>[] ys = new List<float>[cDiskNum];
            for (int i = 0; i < cDiskNum; i++) {
                ys[i] = new List<float>();
                for (int j = 0; j < history_length; j++) ys[i].Add(0);
            }
            while (!this.IsDisposed && !subCharts[0].IsDisposed) {
                if (this.Visible) {
                    for (int i = 0; i < cDiskNum; i++) {
                        ys[i].RemoveAt(0);
                        try {
                            float cload = diskInfo.DiskLoad(i + startId);
                            ys[i].Add(cload);
                        }
                        catch {
                            Action reload = new Action(
                                delegate () {
                                    Thread.Sleep(100);
                                    mainform.btnDiskRefresh_Click(null, null);
                                }
                            );
                            Invoke(reload);
                            break;
                        }
                    }
                    Action updateChart = new Action(
                        delegate () {
                            for (int i = 0; i < cDiskNum; i++) {
                                subCharts[i].Series[0].Points.DataBindY(ys[i]);
                                float cRead = diskInfo.DiskRead(i + startId);
                                float cWrite = diskInfo.DiskWrite(i + startId);
                                string rw_speed = "Read -\nWrite -";
                                if (cRead >= 0 && cWrite >= 0) {
                                    rw_speed = Utility.FormatSpeedString("Read", cRead, "Write", cWrite, false);
                                    string cTitle = subCharts[i].Titles[0].Text;
                                    if (cTitle.Contains("Ejected"))
                                        subCharts[i].Titles[0].Text = getChartTitle(i);
                                } else {
                                    subCharts[i].Titles[0].Text = "Disk " + (i + startId).ToString() + " (Ejected)";
                                }
                                subCharts[i].Titles[2].Text = rw_speed;
                            }
                        }
                    );
                    try { Invoke(updateChart); }
                    catch { break; }
                }
                Thread.Sleep(Global.interval_ms);
            }
        }

        private string getChartTitle(int id) {
            string ret = "Disk " + (id + startId).ToString() + " (" + diskInfo.DriveLetters(id) + ")";
            return ret;
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
        private string[] diskDriveLetters;   //磁盘分区卷标

        // 构造函数，初始化计数器
        public DiskInfo() {
            pcDiskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            pcDiskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            PerformanceCounterCategory diskPfc = new PerformanceCounterCategory("PhysicalDisk");
            string[] _diskInstanceNames = diskPfc.GetInstanceNames();
            m_DiskNum = _diskInstanceNames.Length - 1;
            foreach (string disk in _diskInstanceNames) {
                string[] split = disk.Split(' ');
                try {
                    int cid = int.Parse(split[0]);
                    m_DiskNum = Math.Max(m_DiskNum, cid + 1);
                }
                catch (FormatException) { }
            }
            string[] diskInstanceNames = new string[m_DiskNum];
            foreach (string disk in _diskInstanceNames) {
                string[] split = disk.Split(' ');
                try {
                    int cid = int.Parse(split[0]);
                    diskInstanceNames[cid] = disk;
                }
                catch (FormatException) { }
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

            for (int i = 0; i < m_DiskNum; i++) {
                try {
                    pcDisksRead[i].NextValue();
                    pcDisksWrite[i].NextValue();
                    pcDisksLoad[i].NextValue();
                }
                catch (InvalidOperationException) { }
            }

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
            wmi_disk.Dispose();
            diskDriveLetters = new string[m_DiskNum];
            wmi_disk = new ManagementObjectSearcher("select * from Win32_LogicalDiskToPartition");
            foreach (var o in wmi_disk.Get()) {
                var mo = o as ManagementObject;
                string _relpath = mo["__RELPATH"].ToString();
                const string s_drvLetter = "Win32_LogicalDisk.DeviceID=\\\"";
                const string s_drvID = "Win32_DiskPartition.DeviceID=\\\"Disk #";
                int i_drvLetter = _relpath.IndexOf(s_drvLetter) + s_drvLetter.Length;
                string drvLetter = _relpath.Substring(i_drvLetter, 2);
                int i_drvID = _relpath.IndexOf(s_drvID) + s_drvID.Length;
                string whole_drvID = _relpath.Substring(i_drvID, 4);
                int drvID = int.Parse(whole_drvID.Split(',')[0]);
                diskDriveLetters[drvID] += drvLetter + " ";
            }
            wmi_disk.Dispose();
        }

        // 磁盘型号
        public string DiskModel(int id) {
            return diskModelCaption[id];
        }
        // 磁盘分区卷标
        public string DriveLetters(int id) {
            return diskDriveLetters[id].Trim();
        }

        // 磁盘占用、读写速率
        public float DiskReadTotal {
            get { return pcDiskRead.NextValue(); }
        }
        public float DiskWriteTotal {
            get { return pcDiskWrite.NextValue(); }
        }
        public float DiskRead(int diskId) {
            try { return pcDisksRead[diskId].NextValue(); }
            catch (InvalidOperationException) { return -1; }
        }
        public float DiskWrite(int diskId) {
            try { return pcDisksWrite[diskId].NextValue(); }
            catch (InvalidOperationException) { return -1; }
        }
        public float DiskLoad(int diskId) {
            try { return Math.Max(0, 100 - pcDisksLoad[diskId].NextValue()); }
            catch (InvalidOperationException) { return 0; }
        }
    }
}