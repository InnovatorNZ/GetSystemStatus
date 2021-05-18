using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace GetSystemStatusWeb {
    public partial class CPUForm : System.Web.UI.Page {
        private const int historyLength = 30;
        private Color chartColor = Color.FromArgb(120, Color.DodgerBlue);
        private Color borderColor = Color.FromArgb(180, Color.DodgerBlue);
        private Color gridColor = ColorTranslator.FromHtml("#905baeff");
        private CPUInfo cpuInfo = new CPUInfo();

        protected void Page_Load(object sender, EventArgs e) {
            cpuName.Text = cpuInfo.CpuName;
            Thread.Sleep(200);
            float[] cCpuLoads = new float[cpuInfo.ProcessorCount];
            for(int i = 0; i < cpuInfo.ProcessorCount; i++) {
                cCpuLoads[i] = cpuInfo.CpuCoreLoad(i);
			}
            List<float>[] ys;
            try {
                ys = Session["ys"] as List<float>[];
                if (ys == null) throw new Exception();
            }
            catch {
                ys = new List<float>[cpuInfo.ProcessorCount];
                for (int i = 0; i < cpuInfo.ProcessorCount; i++) {
                    ys[i] = new List<float>();
                    for (int j = 0; j < historyLength; j++) {
                        ys[i].Add(0);
                    }
                }
                Session["ys"] = ys;
            }

            int rows = 1, columns = 1;
            Utility.FactorDecompose(cpuInfo.ProcessorCount, ref columns, ref rows);

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    Chart chart = new Chart();
                    chart.Palette = ChartColorPalette.None;
                    chart.PaletteCustomColors = new Color[] { chartColor };
                    chart.Series.Add(cid.ToString());
                    ys[cid].RemoveAt(0);
                    float cload = cCpuLoads[cid];
                    ys[cid].Add(cload);
                    chart.Series[0].Points.DataBindY(ys[cid]);
                    chart.Series[0].ChartType = SeriesChartType.Area;
                    chart.Series[0].BorderColor = borderColor;
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
                    chart_panel.Controls.Add(chart);
                }
            }
		}
    }

    public class CPUInfo {
        private string cpuName = string.Empty;  //CPU名称
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
                return pcCpuLoad.NextValue();
            }
        }
        public float CpuCoreLoad(int core_num) {
            return pcCpuCoreLoads[core_num].NextValue();
        }
    }
}