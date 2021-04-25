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
        const int beginTop = 311;
        const int ratioChartMargin = 10;
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
            /*List<int> x = new List<int>() { 0, 1, 2, 3, 4 };
            List<int> y = new List<int>() { 5, 7, 25, 94, 3 };
            //chart1.Series.Add("_Total");
            //chart1.Series["Series1"].Points.DataBindXY(x, y);
            chart1.Series["Series1"].Points.DataBindY(y);
            chart1.Series["Series1"].IsVisibleInLegend = false;
            cpuName.Text = sysInfo.CpuName;
            List<int> x = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> y = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };*/
            //TODO：单独开个线程更新CPU负载值
            //Thread cpuThread = new Thread(new ThreadStart(cpu_load_thread));
            //cpuThread.Start();
            cpuName.Text = cpuInfo.CpuName;
            List<int> x = new List<int>();
            List<float> y = new List<float>();
            for (int i = 0; i < historyLength; i++) {
                x.Add(i);
                y.Add(0);
            }
            chart1.Series["Series1"].Points.DataBindY(y);
            chart1.Series["Series1"].IsVisibleInLegend = false;

            FactorDecompose(cpuInfo.ProcessorCount, ref columns, ref rows);

            int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop) / (double)(rows + (rows + 1) / (double)ratioChartMargin));
            int chartWidth = (int)Math.Round((double)this.Size.Width / (double)(columns + (columns + 1) / (double)ratioChartMargin));
            int marginVertical = (int)Math.Round((double)chartHeight / (double)ratioChartMargin);
            int marginHorizontal = (int)Math.Round((double)chartWidth / (double)ratioChartMargin);

            subCharts = new Chart[cpuInfo.ProcessorCount];
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    Chart chart = new Chart();
                    chart.Series.Add(cid.ToString());
                    chart.Series[0].IsVisibleInLegend = false;
                    chart.Series[0].Points.DataBindY(y);
                    chart.Series[0].ChartType = SeriesChartType.SplineArea;
                    chart.ChartAreas.Add(cid.ToString());
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 100;
                    chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
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
                    int startX = (j + 1) * marginHorizontal + j * chartWidth;
                    int startY = beginTop + (i + 1) * marginVertical + i * chartHeight;
                    chart.Location = new Point(startX, startY);
                    chart.Size = new Size(chartWidth, chartHeight);
                    subCharts[cid] = chart;
                    this.Controls.Add(subCharts[cid]);
                }
            }

            new Action(cpu_load_thread).BeginInvoke(null, null);
        }

        private void CPUForm_FormClosed(object sender, FormClosedEventArgs e) {

        }

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
                //chart1.Update();
                //Action updateChart = new Action(delegate () { chart1.Update(); });
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
            int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop) / (double)(rows + (rows + 1) / (double)ratioChartMargin));
            int chartWidth = (int)Math.Round((double)this.Size.Width / (double)(columns + (columns + 1) / (double)ratioChartMargin));
            int marginVertical = (int)Math.Round((double)chartHeight / (double)ratioChartMargin);
            int marginHorizontal = (int)Math.Round((double)chartWidth / (double)ratioChartMargin);
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    int startX = (j + 1) * marginHorizontal + j * chartWidth;
                    int startY = beginTop + (i + 1) * marginVertical + i * chartHeight;
                    subCharts[cid].Size = new Size(chartWidth, chartHeight);
                    subCharts[cid].Location = new Point(startX, startY);
                }
            }
            chart1.Width = this.Size.Width - 2 * marginHorizontal;
            chart1.Left = marginHorizontal;
        }
    }

    public class CPUInfo {
        public string CpuName { get; }     //CPU名称
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private PerformanceCounter[] pcCpuCoreLoads;   //每CPU核心的利用率

        // 构造函数，初始化计数器
        public CPUInfo() {
            ProcessorCount = Environment.ProcessorCount;
            //初始化计数器
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