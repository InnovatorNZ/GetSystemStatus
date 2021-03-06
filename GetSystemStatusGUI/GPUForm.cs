using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GetSystemStatusGUI {
    public partial class GPUForm : Form {
        private GPUInfo gpuInfo;
        public List<GPUForm> moreGPUForms { get; private set; }
        private Form1 mainForm;
        private readonly int id;
        private Color baseColor = Color.DeepSkyBlue;
        private Color chartColor = Color.FromArgb(120, Color.DeepSkyBlue);
        private Color borderColor = Color.FromArgb(180, Color.DeepSkyBlue);
        private Color lineColor = Color.FromArgb(150, Color.DeepSkyBlue);

        public GPUForm(Form1 mainForm, int id = 0) {
            InitializeComponent();
            moreGPUForms = new List<GPUForm>();
            gpuInfo = new GPUInfo(id);
            if (gpuInfo.Count == 0) {
                mainForm.DisableChecked("noGPU");
                this.Dispose();
                return;
            }
            this.id = id;
            this.mainForm = mainForm;
            if (id == 0 && gpuInfo.Count > 1) {
                for (int nid = 1; nid < gpuInfo.Count; nid++) {
                    moreGPUForms.Add(new GPUForm(mainForm, nid));
                    moreGPUForms[nid - 1].Show();
                }
            }
        }

        private void GPUForm_Load(object sender, EventArgs e) {
            chartGPU.PaletteCustomColors = new Color[] { chartColor };
            lblGPUName.Text = gpuInfo.getGpuName(id);
            label1.Text += " " + id.ToString();
            this.Text += " " + id.ToString();
            List<string> cGpuEngines = gpuInfo.GetGPUEngines(id);
            long cGPUTotalMemory = gpuInfo.getAdapterTotalMemory(id);
            if (cGPUTotalMemory > 0) cGpuEngines.Add("GPU Memory");
            foreach (string engine in cGpuEngines) {
                chartGPU.Series.Add(engine);
                chartGPU.ChartAreas.Add(engine);
                chartGPU.Titles.Add(new Title { Name = engine });
                chartGPU.Series[engine].ChartType = SeriesChartType.SplineArea;
                chartGPU.Series[engine].BorderColor = borderColor;
                chartGPU.Series[engine].ChartArea = engine;
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
                chartGPU.Titles[engine].Font = new Font(FontFamily.GenericSansSerif, 10);
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
            List<float> dediUsage = new List<float>();
            for (int i = 0; i < Global.history_length; i++) dediUsage.Add(0);
            int t = 0;
            while (!chartGPU.IsDisposed && !mainForm.IsDisposed) {
                if (t % 15 == 0 && t != 0) gpuInfo.RefreshGPUEnginePerfCnt(id);
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
                        long dediMem = gpuInfo.GetGPUDedicatedMemory(id);
                        long totalMem = gpuInfo.getAdapterTotalMemory(id);
                        if (totalMem > 0) {
                            float cusage = (float)dediMem / (float)totalMem * 100;
                            dediUsage.RemoveAt(0);
                            dediUsage.Add(cusage);
                            chartGPU.Series["GPU Memory"].Points.DataBindY(dediUsage);
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

        public new void Dispose() {
            if (id == 0) {
                foreach (var subform in moreGPUForms) {
                    subform.Dispose();
                }
            }
            base.Dispose();
        }

        private void GPUForm_Resize(object sender, EventArgs e) {
            int width = this.Width - (int)(chartGPU.Location.X * 2.5);
            int height = this.Height - chartGPU.Location.Y - chartGPU.Location.X * 6;
            if (width > 0 && height > 0)
                this.chartGPU.Size = new Size(width, height);
        }

        private void GPUForm_FormClosing(object sender, FormClosingEventArgs e) {
            mainForm.DisableChecked("GPU");
        }
    }

    public class GPUInfo {
        private List<PerformanceCounter> pcDedicateGPUMemory;
        private List<PerformanceCounter> pcGPUEngine;
        private List<string> gpu_name;
        public int Count { get; private set; }
        private List<PairedAdapterInfo> pairedAdapterInfos;

        [DllImport(@"..\..\..\..\x64\Debug\GetNVGPUMemoryDLL.dll")]
        private extern static long InitAndGetNVGPUMemory(int deviceId);

        struct PairedAdapterInfo : IComparable {
            public string name { get; }
            public string pcid { get; }
            public int nvid { get; }        //仅针对NVIDIA显卡有效的ID
            public long totalMemory { get; }
            public PairedAdapterInfo(string name, string pcid) {
                this.name = name;
                this.pcid = pcid;
                this.nvid = -1;
                this.totalMemory = 0;
            }
            public PairedAdapterInfo(string name, string pcid, int nvid, long totalMemory) {
                this.name = name;
                this.pcid = pcid;
                this.nvid = nvid;
                this.totalMemory = totalMemory;
            }
            public int CompareTo(object obj) {
                PairedAdapterInfo target = (PairedAdapterInfo)obj;
                return this.pcid.CompareTo(target.pcid);
            }
        }

        private string getGpuPcId(int id) {
            return this.pairedAdapterInfos[id].pcid;
        }

        public string getGpuName(int id) {      //GPU型号名称
            return this.pairedAdapterInfos[id].name;
        }

        public long getAdapterTotalMemory(int id) {     //GPU总显存
            return this.pairedAdapterInfos[id].totalMemory;
        }

        // 构造函数，初始化计数器
        public GPUInfo(int id = -1) {
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
            this.RemoveUnnecessaryPC(id);
        }

        private void FilterValidGPU() {
            List<string> GpuPcId = new List<string>();
            foreach (PerformanceCounter pc in pcDedicateGPUMemory) {
                string cDeviceId = pc.InstanceName.Split('_')[2];
                float cVRAM = pc.NextValue();
                if (cVRAM != 0) {
                    GpuPcId.Add(cDeviceId);
                } else {
                    PerformanceCounterCategory gpuPfc = new PerformanceCounterCategory("GPU Local Adapter Memory", "Local Usage");
                    gpuPfc.MachineName = ".";
                    string[] instanceNames = gpuPfc.GetInstanceNames();
                    foreach (string instanceName in instanceNames) {
                        string tDeviceId = instanceName.Split('_')[2];
                        if (cDeviceId == tDeviceId) {
                            PerformanceCounter lpc = new PerformanceCounter("GPU Local Adapter Memory", "Local Usage", instanceName);
                            lpc.MachineName = ".";
                            float nvalue = lpc.NextValue();
                            Debug.Assert(nvalue != 0);
                            if (nvalue > 0 && nvalue != 8192) {
                                GpuPcId.Add(cDeviceId);
                            }
                        }
                    }
                }
            }
            Debug.Assert(GpuPcId.Count == this.Count);
            this.Count = Math.Min(GpuPcId.Count, this.Count);
            this.pairedAdapterInfos = new List<PairedAdapterInfo>();
            int nvcnt = 0;
            for (int i = 0; i < this.Count; i++) {
                string manufactor = gpu_name[i].Substring(0, 6);
                if (manufactor == "NVIDIA") {
                    try {
                        long cTotalMemory = InitAndGetNVGPUMemory(nvcnt);
                        this.pairedAdapterInfos.Add(new PairedAdapterInfo(gpu_name[i], GpuPcId[i], nvcnt, cTotalMemory));
                        nvcnt++;
                    }
                    catch {
                        this.pairedAdapterInfos.Add(new PairedAdapterInfo(gpu_name[i], GpuPcId[i]));
                    }
                } else {
                    this.pairedAdapterInfos.Add(new PairedAdapterInfo(gpu_name[i], GpuPcId[i]));
                }
            }
            this.pairedAdapterInfos.Sort();
        }

        private void RemoveUnnecessaryPC(int id) {
            if (id < 0 || this.Count == 0) return;
            string cDeviceId = this.getGpuPcId(id);
            for (int i = pcGPUEngine.Count - 1; i >= 0; i--) {
                PerformanceCounter epc = pcGPUEngine[i];
                string[] esplit = epc.InstanceName.Split('_');
                string eDeviceId = esplit[4];
                if (eDeviceId != cDeviceId) {
                    pcGPUEngine.Remove(epc);
                }
            }
        }

        // 专用GPU显存
        public long GetGPUDedicatedMemory(int id) {
            string deviceId = this.getGpuPcId(id);
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
            string deviceId = this.getGpuPcId(id);
            try {
                Dictionary<string, float> result = new Dictionary<string, float>();
                foreach (PerformanceCounter pc in pcGPUEngine) {
                    string[] csplit = pc.InstanceName.Split('_');
                    string cDeviceId = csplit[4];
                    if (cDeviceId == deviceId) {
                        string cEngine = getEngineString(csplit);
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
                this.RefreshGPUEnginePerfCnt(id);
                return this.GetGPUUtilization(id);
            }
        }
        public Dictionary<string, Dictionary<string, float>> GetGPUUtilization() {
            Dictionary<string, Dictionary<string, float>> ret = new Dictionary<string, Dictionary<string, float>>();
            foreach (PerformanceCounter pc in pcGPUEngine) {
                string[] csplit = pc.InstanceName.Split('_');
                string cDeviceId = csplit[4];
                string cEngine = getEngineString(csplit);
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
            string deviceId = this.getGpuPcId(id);
            List<string> result = new List<string>();
            foreach (PerformanceCounter pc in pcGPUEngine) {
                string[] csplit = pc.InstanceName.Split('_');
                string cDeviceId = csplit[4];
                if (cDeviceId == deviceId) {
                    string cEngine = getEngineString(csplit);
                    if (!result.Contains(cEngine)) {
                        result.Add(cEngine);
                    }
                }
            }
            result.Sort();
            return result;
        }

        private string getEngineString(string[] splitInstanceName) {
            string cEngine = string.Empty;
            for (int i = 10; i < splitInstanceName.Length; i++) {
                cEngine += splitInstanceName[i];
                if (i != splitInstanceName.Length - 1) cEngine += " ";
            }
            if (cEngine == string.Empty) {
                cEngine = "Engine " + splitInstanceName[8];
            }
            return cEngine;
        }

        public void RefreshGPUEnginePerfCnt() {
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
        public void RefreshGPUEnginePerfCnt(int id) {
            PerformanceCounterCategory pidGpuPfc = new PerformanceCounterCategory("GPU Engine", "Utilization Percentage");
            pidGpuPfc.MachineName = ".";
            string[] pidGpuInstanceNames = pidGpuPfc.GetInstanceNames();
            pcGPUEngine.Clear();
            string c_device_id = this.getGpuPcId(id);
            foreach (string pidInstanceName in pidGpuInstanceNames) {
                string c_pid_deviceId = pidInstanceName.Split('_')[4];
                if (c_pid_deviceId == c_device_id) {
                    PerformanceCounter pc = new PerformanceCounter("GPU Engine", "Utilization Percentage", pidInstanceName);
                    try {
                        pc.NextValue();
                        pcGPUEngine.Add(pc);
                    }
                    catch { }
                }
            }
        }
    }
}