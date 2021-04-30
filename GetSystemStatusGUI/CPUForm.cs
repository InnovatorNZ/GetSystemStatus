using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GetSystemStatusGUI {
    public partial class CPUForm : Form {
        const int historyLength = 60;
        int beginTop = 311;
        const int ratioChartMargin = 10;
        Color chartColor = Color.FromArgb(160, 30, 144, 255);
        int fixHeight = 40;   //修正高度
        Form1 mainForm;
        CPUInfo cpuInfo;
        private Chart[] subCharts;
        int rows = 1, columns = 1;

        public CPUForm(Form1 mainForm) {
            InitializeComponent();
            this.mainForm = mainForm;
            cpuInfo = new CPUInfo();
        }

        private void CPUForm_Load(object sender, EventArgs e) {
            cpuName.Text = cpuInfo.CpuName;
            this.beginTop = chart1.Location.Y + chart1.Size.Height + 5;
            List<int> x = new List<int>();
            List<float> y = new List<float>();
            for (int i = 0; i < historyLength; i++) {
                x.Add(i);
                y.Add(0);
            }
            chart1.Series["Series1"].Points.DataBindY(y);
            chart1.Series["Series1"].IsVisibleInLegend = false;
            chart1.PaletteCustomColors[0] = chartColor;

            FactorDecompose(cpuInfo.ProcessorCount, ref columns, ref rows);

            subCharts = new Chart[cpuInfo.ProcessorCount];
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    Chart chart = new Chart();
                    chart.Palette = ChartColorPalette.None;
                    chart.PaletteCustomColors = new Color[] { chartColor };
                    chart.Series.Add(cid.ToString());
                    chart.Series[0].Points.DataBindY(y);
                    chart.Series[0].ChartType = SeriesChartType.SplineArea;
                    chart.ChartAreas.Add(cid.ToString());
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 100;
                    chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#5baeff");
                    chart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#5baeff");
                    chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineColor = ColorTranslator.FromHtml("#5baeff");
                    chart.ChartAreas[0].AxisX.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisY.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisX.LineWidth = 2;
                    chart.ChartAreas[0].AxisY.LineWidth = 2;
                    chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisX2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX2.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisX2.LineWidth = 2;
                    chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY2.LineColor = Color.DodgerBlue;
                    chart.ChartAreas[0].AxisY2.LineWidth = 2;
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
            CPUForm_Resize(null, null);

            new Action(cpu_load_thread).BeginInvoke(null, null);
        }

        private void CPUForm_FormClosed(object sender, FormClosedEventArgs e) { }

        private void CPUForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainForm.DisableChecked("CPU");
            this.Hide();
        }

        private void cpu_load_thread() {
            List<float> y = new List<float>();
            List<float>[] ys = new List<float>[cpuInfo.ProcessorCount];
            for (int i = 0; i < historyLength; i++) y.Add(0);
            for (int i = 0; i < cpuInfo.ProcessorCount; i++) {
                ys[i] = new List<float>();
                for (int j = 0; j < historyLength; j++) {
                    ys[i].Add(0);
                }
            }
            while (true) {
                y.RemoveAt(0);
                y.Add(cpuInfo.CpuLoad);
                for (int i = 0; i < cpuInfo.ProcessorCount; i++) {
                    ys[i].RemoveAt(0);
                    ys[i].Add(cpuInfo.CpuCoreLoad(i));
                }
                Action updateChart = new Action(
                    delegate () {
                        chart1.Series["Series1"].Points.DataBindY(y);
                        for (int i = 0; i < cpuInfo.ProcessorCount; i++)
                            subCharts[i].Series[0].Points.DataBindY(ys[i]);
                    }
                );
                if (chart1.IsDisposed) break;
                Invoke(updateChart);
                Thread.Sleep(1000);
            }
        }

        private void FactorDecompose(int original, ref int bigger, ref int smaller) {
            double sqrt = Math.Sqrt(original);
            int a = (int)Math.Ceiling(sqrt), b = (int)Math.Floor(sqrt);
            while (a * b != original) {
                if (a * b > original) b--;
                else a++;
            }
            bigger = a;
            smaller = b;
        }

        private void CPUForm_Resize(object sender, EventArgs e) {
            //int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop - fixHeight) / (double)(rows + (rows + 1) / (double)ratioChartMargin));
            //int chartWidth = (int)Math.Round((double)this.Size.Width / (double)(columns + (columns + 1) / (double)ratioChartMargin));
            //int marginVertical = (int)Math.Round((double)chartHeight / (double)ratioChartMargin);
            //int marginHorizontal = (int)Math.Round((double)chartWidth / (double)ratioChartMargin);
            int marginVertical = (int)Math.Round((double)Math.Min(this.Size.Height, this.Size.Width) / 50.0);
            int marginHorizontal = marginVertical;
            int endRight = (int)Math.Round((double)marginHorizontal * 1.1);
            fixHeight = Math.Max(40, marginHorizontal * 3);
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
    }

    public class CPUInfo {
        public string CpuName { get; }     //CPU名称
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private PerformanceCounter[] pcCpuCoreLoads;   //每CPU核心的利用率

        public CPUInfo() {
            ProcessorCount = Environment.ProcessorCount;
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuCoreLoads = new PerformanceCounter[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++) {
                pcCpuCoreLoads[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
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
            CpuName = st;
        }

        // CPU利用率、核心数
        public int ProcessorCount { get; } = 0;
        public float CpuLoad {
            get {
                return pcCpuLoad.NextValue();
            }
        }
        public float CpuCoreLoad(int core_num) {
            return pcCpuCoreLoads[core_num].NextValue();
        }
    }
}