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
    public partial class GPUForm : Form {
        private GPUInfo gpuInfo;
        private List<GPUForm> moreGPUForms;
        private readonly int id;
        private Color baseColor = Color.DeepSkyBlue;
        private Color chartColor = Color.FromArgb(120, Color.DeepSkyBlue);
        private Color borderColor = Color.FromArgb(180, Color.DeepSkyBlue);
        private Color lineColor = Color.DeepSkyBlue;

        public GPUForm(int id = 0) {
            InitializeComponent();
            moreGPUForms = new List<GPUForm>();
            gpuInfo = new GPUInfo();
            this.id = id;
            if (id == 0 && gpuInfo.Count > 1) {
                for (int nid = 1; nid < gpuInfo.Count; nid++) {
                    moreGPUForms.Add(new GPUForm(nid));
                    moreGPUForms[nid - 1].Show();
                }
            }
        }

        private void GPUForm_Load(object sender, EventArgs e) {
            chartGPU.PaletteCustomColors = new Color[] { chartColor };
            List<string> cGpuEngines = gpuInfo.GetGPUEngines(id);
            foreach (string engine in cGpuEngines) {
                chartGPU.Series.Add(engine);
                chartGPU.ChartAreas.Add(engine);
                chartGPU.Titles.Add(new Title { Name = engine });
                chartGPU.Series[engine].ChartType = SeriesChartType.SplineArea;
                chartGPU.Series[engine].BorderColor = borderColor;
                chartGPU.ChartAreas[engine].AxisY.Enabled = AxisEnabled.True;
                chartGPU.ChartAreas[engine].AxisX.Enabled = AxisEnabled.True;
                chartGPU.ChartAreas[engine].AxisY.Minimum = 0;
                chartGPU.ChartAreas[engine].AxisY.Maximum = 100;
                chartGPU.ChartAreas[engine].AxisY.MajorGrid.Enabled = true;
                chartGPU.ChartAreas[engine].AxisY.MajorTickMark.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY.MajorGrid.LineColor = lineColor;
                chartGPU.ChartAreas[engine].AxisY.MinorGrid.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX.MajorGrid.Enabled = true;
                chartGPU.ChartAreas[engine].AxisX.MajorGrid.LineColor = lineColor;
                chartGPU.ChartAreas[engine].AxisX.MajorTickMark.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX.MinorGrid.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX.MinorGrid.LineColor = lineColor;
                chartGPU.ChartAreas[engine].AxisX.LineColor = baseColor;
                chartGPU.ChartAreas[engine].AxisY.LineColor = baseColor;
                chartGPU.ChartAreas[engine].AxisX.LineWidth = 2;
                chartGPU.ChartAreas[engine].AxisY.LineWidth = 2;
                chartGPU.ChartAreas[engine].AxisX.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.Enabled = AxisEnabled.True;
                chartGPU.ChartAreas[engine].AxisX2.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.MajorGrid.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.MajorTickMark.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.LineColor = baseColor;
                chartGPU.ChartAreas[engine].AxisX2.LineWidth = 2;
                chartGPU.ChartAreas[engine].AxisY2.Enabled = AxisEnabled.True;
                chartGPU.ChartAreas[engine].AxisY2.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY2.MajorGrid.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY2.MajorTickMark.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY2.LineColor = baseColor;
                chartGPU.ChartAreas[engine].AxisY2.LineWidth = 2;
                chartGPU.Titles[engine].Text = engine;
                chartGPU.Titles[engine].Alignment = ContentAlignment.MiddleLeft;
                chartGPU.Titles[engine].DockedToChartArea = engine;
                chartGPU.Titles[engine].IsDockedInsideChartArea = false;
                chartGPU.Titles[engine].ForeColor = SystemColors.GrayText;
            }
            GPUForm_Resize(null, null);
            Thread gpuThread = new Thread(new ThreadStart(GPUPCThread));
            gpuThread.Start();
        }

        private void GPUPCThread() {
            Dictionary<string, List<float>> ys = new Dictionary<string, List<float>>();
            List<string> engines = gpuInfo.GetGPUEngines(id);
            foreach (string engine in engines) {
                List<float> cy = new List<float>();
                for (int i = 0; i < Global.history_length; i++) cy.Add(0);
                ys.Add(engine, cy);
            }
            int t = 0;
            while (!chartGPU.IsDisposed) {
                if (t % 100 == 0 && t != 0) gpuInfo.RefreshGPUEnginePerformanceCounter();
                Dictionary<string, float> cGpuUti = gpuInfo.GetGPUUtilization(id);
                Action update = new Action(
                    delegate () {
                        foreach (var keyValuePair in cGpuUti) {
                            string cEngine = keyValuePair.Key;
                            float cUti = keyValuePair.Value;
                            ys[cEngine].RemoveAt(0);
                            ys[cEngine].Add(cUti);
                            chartGPU.Series[cEngine].Points.DataBindY(ys[cEngine]);
                        }
                    }
                );
                try { Invoke(update); }
                catch { break; }
                t++;
                Thread.Sleep(Global.interval_ms);
            }
        }

        public new void Hide() {
            base.Hide();
            foreach (var subForm in moreGPUForms) {
                subForm.Hide();
            }
        }

        private void GPUForm_Resize(object sender, EventArgs e) {
            int width = this.Width - chartGPU.Location.X - 68;
            int height = this.Height - chartGPU.Location.Y - 113;
            this.chartGPU.Size = new Size(width, height);
        }
    }

    public class GPUInfo {
        private List<PerformanceCounter> pcDedicateGPUMemory;
        private List<PerformanceCounter> pcGPUEngine;
        public List<string> gpu_name { get; }   //GPU名称
        public int Count { get; private set; }  //GPU个数
        private List<string> GpuPcId;

        // 构造函数，初始化计数器
        public GPUInfo() {
            //专用GPU显存计数器
            PerformanceCounterCategory gpuPfc = new PerformanceCounterCategory("GPU Adapter Memory", "Dedicated Usage");
            gpuPfc.MachineName = ".";
            string[] instanceNames = gpuPfc.GetInstanceNames();
            pcDedicateGPUMemory = new List<PerformanceCounter>();
            foreach (string gpu in instanceNames) {
                pcDedicateGPUMemory.Add(new PerformanceCounter("GPU Adapter Memory", "Dedicated Usage", gpu));
            }

            //GPU利用率计数器
            PerformanceCounterCategory pidGpuPfc = new PerformanceCounterCategory("GPU Engine", "Utilization Percentage");
            pidGpuPfc.MachineName = ".";
            string[] pidGpuInstanceNames = pidGpuPfc.GetInstanceNames();
            pcGPUEngine = new List<PerformanceCounter>();
            foreach (var gpu in pcDedicateGPUMemory) {
                string c_device_id = gpu.InstanceName.Split('_')[2];
                gpu.NextValue();
                //if (gpu.NextValue() < 0.5) continue;
                foreach (string pidInstanceName in pidGpuInstanceNames) {
                    string c_pid_deviceId = pidInstanceName.Split('_')[4];
                    if (c_pid_deviceId == c_device_id) {
                        pcGPUEngine.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", pidInstanceName));
                    }
                }
            }

            //GPU名称
            var gpumem = new ManagementObjectSearcher("Select * from Win32_VideoController");
            gpu_name = new List<string>();
            foreach (var o in gpumem.Get()) {
                var mo = (ManagementObject)o;
                string c_gpu_name = (string)mo["name"].ToString();
                try {
                    string mem = mo["AdapterRAM"].ToString();
                    gpu_name.Add(c_gpu_name);
                }
                catch { }
            }
            this.Count = gpu_name.Count;
            this.FilterValidGPU();
        }

        private void FilterValidGPU() {
            this.GpuPcId = new List<string>();
            foreach (PerformanceCounter pc in pcDedicateGPUMemory) {
                string cDeviceId = pc.InstanceName.Split('_')[2];
                float cVRAM = pc.NextValue();
                if (cVRAM != 0) {
                    this.GpuPcId.Add(cDeviceId);
                }
            }
            //Debug.Assert(this.GpuPcId.Count == this.Count);
            this.Count = Math.Min(this.GpuPcId.Count, this.Count);
        }

        // 专用GPU显存
        public long GetGPUDedicatedMemory(int id) {
            string deviceId = this.GpuPcId[id];
            foreach (PerformanceCounter pc in pcDedicateGPUMemory) {
                string cDeviceId = pc.InstanceName.Split('_')[2];
                if (cDeviceId == deviceId) {
                    long ret = (long)Math.Round(pc.NextValue());
                    return ret;
                }
            }
            return 0;
        }
        public Dictionary<string, long> GetGPUDedicatedMemory() {
            Dictionary<string, long> ret = new Dictionary<string, long>();
            foreach (PerformanceCounter pc in pcDedicateGPUMemory) {
                string cDeviceId = pc.InstanceName.Split('_')[2];
                ret.Add(cDeviceId, (long)Math.Round(pc.NextValue()));
            }
            bool RemoveZero = false;
            if (RemoveZero) {
                foreach (var kp in ret) {
                    if (kp.Value == 0) {
                        ret.Remove(kp.Key);
                        break;
                    }
                }
            }
            return ret;
        }

        // GPU各引擎利用率
        public Dictionary<string, float> GetGPUUtilization(int id) {
            string deviceId = this.GpuPcId[id];
            try {
                Dictionary<string, float> result = new Dictionary<string, float>();
                foreach (PerformanceCounter pc in pcGPUEngine) {
                    string[] csplit = pc.InstanceName.Split('_');
                    string cDeviceId = csplit[4];
                    if (cDeviceId == deviceId) {
                        string cEngine = string.Empty;
                        for (int i = 10; i < csplit.Length; i++) cEngine += csplit[i];
                        float cvalue;
                        if (result.TryGetValue(cEngine, out cvalue)) {
                            cvalue += pc.NextValue();
                            result[cEngine] = cvalue;
                        } else {
                            result.Add(cEngine, pc.NextValue());
                        }
                    }
                }
                return result;
            }
            catch {
                this.RefreshGPUEnginePerformanceCounter();
                return this.GetGPUUtilization(id);
            }
        }
        public Dictionary<string, Dictionary<string, float>> GetGPUUtilization() {
            Dictionary<string, Dictionary<string, float>> ret = new Dictionary<string, Dictionary<string, float>>();
            foreach (PerformanceCounter pc in pcGPUEngine) {
                string[] csplit = pc.InstanceName.Split('_');
                string cDeviceId = csplit[4];
                string cEngine = string.Empty;
                for (int i = 10; i < csplit.Length; i++) cEngine += csplit[i];
                Dictionary<string, float> cdic;
                if (ret.TryGetValue(cDeviceId, out cdic)) {
                    ret.Remove(cDeviceId);
                    float cvalue;
                    if (cdic.TryGetValue(cEngine, out cvalue)) {
                        cdic.Remove(cEngine);
                        cvalue += pc.NextValue();
                        cdic.Add(cEngine, cvalue);
                    } else {
                        cdic.Add(cEngine, pc.NextValue());
                    }
                    ret.Add(cDeviceId, cdic);
                } else {
                    cdic = new Dictionary<string, float>();
                    cdic.Add(cEngine, pc.NextValue());
                    ret.Add(cDeviceId, cdic);
                }
            }
            return ret;
        }
        public List<string> GetGPUEngines(int id) {
            string deviceId = this.GpuPcId[id];
            List<string> result = new List<string>();
            foreach (PerformanceCounter pc in pcGPUEngine) {
                string[] csplit = pc.InstanceName.Split('_');
                string cDeviceId = csplit[4];
                if (cDeviceId == deviceId) {
                    string cEngine = string.Empty;
                    for (int i = 10; i < csplit.Length; i++) cEngine += csplit[i];
                    if (!result.Contains(cEngine)) {
                        result.Add(cEngine);
                    }
                }
            }
            result.Sort();
            return result;
        }

        public void RefreshGPUEnginePerformanceCounter() {
            //刷新GPU利用率计数器
            PerformanceCounterCategory pidGpuPfc = new PerformanceCounterCategory("GPU Engine", "Utilization Percentage");
            pidGpuPfc.MachineName = ".";
            string[] pidGpuInstanceNames = pidGpuPfc.GetInstanceNames();
            pcGPUEngine.Clear();
            foreach (var gpu in pcDedicateGPUMemory) {
                string c_device_id = gpu.InstanceName.Split('_')[2];
                gpu.NextValue();
                //if (gpu.NextValue() < 0.5) continue;
                foreach (string pidInstanceName in pidGpuInstanceNames) {
                    string c_pid_deviceId = pidInstanceName.Split('_')[4];
                    if (c_pid_deviceId == c_device_id) {
                        pcGPUEngine.Add(new PerformanceCounter("GPU Engine", "Utilization Percentage", pidInstanceName));
                    }
                }
            }
        }
    }
}