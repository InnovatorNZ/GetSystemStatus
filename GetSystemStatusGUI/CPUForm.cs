using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static GetSystemStatusGUI.ModuleEnum;

namespace GetSystemStatusGUI {
    public partial class CPUForm : DarkAwareForm {
        private const int historyLength = 60;
        private Color chartColor = Color.FromArgb(120, Color.DodgerBlue);
        private Color borderColor = Color.FromArgb(180, Color.DodgerBlue);
        private Color gridColor = ColorTranslator.FromHtml("#905baeff");
        private float fLineWidth = 2;
        private float fGridWidth = 1;
        private Form1 mainForm;
        private CPUInfo cpuInfo;
        private Chart[] subCharts;
        private int rows = 1, columns = 1;
        private int ProcessorCount = 0;

        public CPUForm(Form1 mainForm, int processorCount = 0) {
            InitializeComponent();
            this.mainForm = mainForm;
            cpuInfo = new CPUInfo();
            if (processorCount == 0)
                this.ProcessorCount = cpuInfo.ProcessorCount;
            else
                this.ProcessorCount = processorCount;
        }

        private void CPUForm_Load(object sender, EventArgs e) {
            cpuName.Text = cpuInfo.CpuName;
            List<float> y = new List<float>();
            for (int i = 0; i < historyLength; i++) y.Add(0);
            chart1.Series[0].Points.DataBindY(y);
            chart1.Series[0].IsVisibleInLegend = false;
            chart1.PaletteCustomColors[0] = chartColor;
            chart1.Series[0].BorderColor = borderColor;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = gridColor;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = gridColor;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineColor = gridColor;
            int lineWidth = (int)Math.Round(this.fLineWidth * GetWinScaling());
            int gridWidth = (int)Math.Round(this.fGridWidth * GetWinScaling());
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = gridWidth;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = gridWidth;
            chart1.ChartAreas[0].AxisX.LineWidth = lineWidth;
            chart1.ChartAreas[0].AxisY.LineWidth = lineWidth;
            chart1.ChartAreas[0].AxisX2.LineWidth = lineWidth;
            chart1.ChartAreas[0].AxisY2.LineWidth = lineWidth;

            Utility.FactorDecompose(this.ProcessorCount, out columns, out rows);

            if (columns / rows > 100) {
                MessageBox.Show("The CPU logical core number is very strange and maybe odd. Reset logical cores to default.", "Error occured when loading", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ProcessorCount = cpuInfo.ProcessorCount;
                Utility.FactorDecompose(cpuInfo.ProcessorCount, out columns, out rows);
            }

            subCharts = new Chart[this.ProcessorCount];
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    Chart chart = new Chart();
                    chart.Palette = ChartColorPalette.None;
                    chart.PaletteCustomColors = new Color[] { chartColor };

                    chart.ChartAreas.Add(cid.ToString());
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 100;
                    chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY.MajorGrid.LineColor = gridColor;
                    chart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = gridColor;
                    chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineColor = gridColor;
                    chart.ChartAreas[0].AxisX.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisY.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisX.LineWidth = lineWidth;
                    chart.ChartAreas[0].AxisY.LineWidth = lineWidth;
                    chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisX2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX2.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisX2.LineWidth = lineWidth;
                    chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY2.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisY2.LineWidth = lineWidth;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineWidth = gridWidth;
                    chart.ChartAreas[0].AxisY.MajorGrid.LineWidth = gridWidth;

                    chart.Series.Add(cid.ToString());
                    chart.Series[0].Points.DataBindY(y);
                    chart.Series[0].ChartType = SeriesChartType.SplineArea;
                    chart.Series[0].BorderColor = borderColor;

                    chart.Titles.Add(cid.ToString());
                    chart.Titles[0].Text = "CPU " + cid.ToString();
                    chart.Titles[0].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[0].DockedToChartArea = cid.ToString();
                    chart.Titles[0].IsDockedInsideChartArea = false;
                    chart.Titles[0].ForeColor = SystemColors.GrayText;

                    subCharts[cid] = chart;
                    this.Controls.Add(subCharts[cid]);
                }
            }
            InitialSize();

            ApplyDarkMode();
            CPUForm_Resize(null, null);

            new Action(cpu_load_thread).BeginInvoke(null, null);
        }

        private void CPUForm_FormClosed(object sender, FormClosedEventArgs e) { }

        private void CPUForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainForm.DisableChecked("CPU");
            this.Hide();
        }

        public new void Show() {
            base.Show();
            this.TopMost = mainForm.TopMostChecked(FormType.CPU);
        }

        private void cpu_load_thread() {
            List<float> y = new List<float>();
            List<float>[] ys = new List<float>[cpuInfo.ProcessorCount];
            for (int i = 0; i < historyLength * columns; i++) y.Add(0);
            for (int i = 0; i < cpuInfo.ProcessorCount; i++) {
                ys[i] = new List<float>();
                for (int j = 0; j < historyLength; j++) {
                    ys[i].Add(0);
                }
            }

            int currentInterval = Global.interval_ms;
            float previousCpuLoad = 0;
            while (!this.IsDisposed && !chart1.IsDisposed) {
                if (this.Visible) {
                    float currentCpuLoad = cpuInfo.CpuLoad;

                    y.RemoveAt(0);
                    y.Add(currentCpuLoad);
                    for (int i = 0; i < cpuInfo.ProcessorCount; i++) {
                        ys[i].RemoveAt(0);
                        ys[i].Add(cpuInfo.CpuCoreLoad(i));
                    }
                    Action updateChart = new Action(
                        delegate () {
                            chart1.Series["Series1"].Points.DataBindY(y);
                            for (int i = 0; i < this.ProcessorCount; i++)
                                subCharts[i].Series[0].Points.DataBindY(ys[i % cpuInfo.ProcessorCount]);
                        }
                    );

                    try {
                        Invoke(updateChart);
                    } catch { break; }

                    if (Global.interval_ms > Global.MIN_INTERVAL_MS) {
                        float loadChange = Math.Abs(currentCpuLoad - previousCpuLoad);

                        if (loadChange > Global.CHANGE_THRESHOLD_PERCENT) {
                            currentInterval = Global.MIN_INTERVAL_MS;
                        } else {
                            // 逐渐延长间隔，但不超过用户设定的全局间隔
                            currentInterval = Math.Min(currentInterval + Global.INTERVAL_INCREMENT_MS, Global.interval_ms);
                        }

                        previousCpuLoad = currentCpuLoad;
                    } else {
                        currentInterval = Global.interval_ms;
                    }
                }

                Thread.Sleep(currentInterval);
            }
        }

        private void CPUForm_Resize(object sender, EventArgs e) {
            //const int ratioChartMargin = 10;
            //int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop - fixHeight) / (double)(rows + (rows + 1) / (double)ratioChartMargin));
            //int chartWidth = (int)Math.Round((double)this.Size.Width / (double)(columns + (columns + 1) / (double)ratioChartMargin));
            //int marginVertical = (int)Math.Round((double)chartHeight / (double)ratioChartMargin);
            //int marginHorizontal = (int)Math.Round((double)chartWidth / (double)ratioChartMargin);
            int margin_ratio = 50;
            if (this.ProcessorCount >= 48) margin_ratio = 65;
            int marginVertical = (int)Math.Round((double)Math.Min(this.Size.Height, this.Size.Width) / (double)margin_ratio);
            int marginHorizontal = marginVertical;
            int endRight = (int)Math.Round((double)marginHorizontal * 1.1);
            int fixHeight = Math.Max(40, marginHorizontal * 3);
            int beginTop = chart1.Location.Y + chart1.Size.Height + 5;
            int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop - fixHeight - (rows + 1) * marginVertical) / (double)rows);
            int chartWidth = (int)Math.Round((double)(this.Size.Width - endRight - (columns + 1) * marginHorizontal) / (double)columns);
            if (chartHeight <= 0 || chartWidth <= 0 || subCharts is null) return;
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    int startX = (j + 1) * marginHorizontal + j * chartWidth;
                    int startY = beginTop + (i + 1) * marginVertical + i * chartHeight;
                    subCharts[cid].Size = new Size(chartWidth, chartHeight);
                    subCharts[cid].Location = new Point(startX, startY);
                }
            }
            chart1.Width = this.Size.Width - marginHorizontal - endRight;
            chart1.Left = (int)Math.Round((double)marginHorizontal / 2.5);
        }

        private void CPUForm_Deactivate(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
                mainForm.DisableChecked("CPU");
            }
        }

        private void InitialSize() {
            const int iSize = 145;
            int beginTop = chart1.Location.Y + chart1.Size.Height + 5;
            float iHeight = beginTop + (rows * iSize + (rows + 1) * 3) * GetWinScaling();
            float iWidth = (columns * iSize + (columns + 1) * 7) * GetWinScaling();
            iHeight = Math.Max(iHeight, this.Size.Height);
            iWidth = Math.Max(iWidth, this.Size.Width);
            if (columns == 2) iWidth = (int)Math.Min(iWidth, (columns * 302 + (columns + 1) * 18) * GetWinScaling());    // 专为四核、双核优化
            this.Size = new Size((int)iWidth, (int)iHeight);
        }

        private void CPUForm_DpiChanged(object sender, DpiChangedEventArgs e) {
            if (e.DeviceDpiNew != e.DeviceDpiOld) {
                float scale = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
                ChangeScale(scale);
                new Action(delegate () {
                    Thread.Sleep(150);
                    Invoke(new Action(delegate () {
                        this.CPUForm_Resize(sender, e);
                    }));
                }).BeginInvoke(null, null);
            }
        }

        protected void ChangeScale(float scale) {
            fLineWidth *= scale;
            fGridWidth *= scale;
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
            cpuName.Left = this.Width - (int)Math.Round((this.Width - cpuName.Right) * scale) - cpuName.Width;
            cpuName.Top = (int)Math.Round(cpuName.Top * scale);
            this.Width = (int)Math.Round(this.Width * scale);
            this.Height = (int)Math.Round(this.Height * scale);
        }

        public void DisableLowDPI(float scale) {
            this.EnableLowDPI(1 / scale);
        }

        private float GetWinScaling() {
            Graphics graphics = this.CreateGraphics();
            /*
            int dpiX = (Int32)graphics.DpiX;
            if (dpiX == 96) { return 1; }
            else if (dpiX == 120) { return 1.25f; }
            else if (dpiX == 144) { return 1.5f; }
            else if (dpiX == 192) { return 2f; }
            else { return 1; }
            */
            float scale = graphics.DpiX / 96.0f;
            return scale;
        }
    }

    public class CPUInfo {
        private string cpuName;                 //CPU名称
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private PerformanceCounter[] pcCpuCoreLoads;   //每CPU核心的利用率

        public CPUInfo() {
            ProcessorCount = Environment.ProcessorCount;
            pcCpuLoad = new PerformanceCounter("Processor", "% Idle Time", "_Total");
            pcCpuCoreLoads = new PerformanceCounter[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++) {
                pcCpuCoreLoads[i] = new PerformanceCounter("Processor", "% Idle Time", i.ToString());
            }
            pcCpuLoad.MachineName = ".";
            pcCpuLoad.NextValue();

            for (int i = 0; i < ProcessorCount; i++) pcCpuCoreLoads[i].NextValue();

            //CPU名称
            var st = string.Empty;
            var driveId = new ManagementObjectSearcher("Select Name from Win32_Processor");
            foreach (var o in driveId.Get()) {
                var mo = (ManagementObject)o;
                st = mo["Name"].ToString();
            }
            cpuName = st;
        }

        // CPU名称
        public string CpuName {
            get { return cpuName.Trim(); }
        }
        // CPU利用率、核心数
        public int ProcessorCount { get; } = 0;
        public float CpuLoad {
            get {
                return 100 - pcCpuLoad.NextValue();
            }
        }
        public float CpuCoreLoad(int core_num) {
            return 100 - pcCpuCoreLoads[core_num].NextValue();
        }
    }
}