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
        private float fLineWidth = 2;
        private float fGridWidth = 1;
        public new bool TopMost {
            get { return base.TopMost; }
            set {
                if (id == 0) {
                    foreach (var subform in moreGPUForms) {
                        subform.TopMost = value;
                    }
                }
                base.TopMost = value;
            }
        }

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
                chartGPU.ChartAreas[engine].AxisX.LineWidth = (int)this.fLineWidth;
                chartGPU.ChartAreas[engine].AxisY.LineWidth = (int)this.fLineWidth;
                chartGPU.ChartAreas[engine].AxisX.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.Enabled = AxisEnabled.True;
                chartGPU.ChartAreas[engine].AxisX2.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.MajorGrid.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.MajorTickMark.Enabled = false;
                chartGPU.ChartAreas[engine].AxisX2.LineColor = baseColor;
                chartGPU.ChartAreas[engine].AxisX2.LineWidth = (int)this.fLineWidth;
                chartGPU.ChartAreas[engine].AxisY2.Enabled = AxisEnabled.True;
                chartGPU.ChartAreas[engine].AxisY2.LabelStyle.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY2.MajorGrid.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY2.MajorTickMark.Enabled = false;
                chartGPU.ChartAreas[engine].AxisY2.LineColor = baseColor;
                chartGPU.ChartAreas[engine].AxisY2.LineWidth = (int)this.fLineWidth;
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
                if (t % Global.refresh_gpupc_interval == 0 && t != 0)
                    gpuInfo.RefreshGPUEnginePerfCntLPL(id);
                Dictionary<string, float> cGpuUti = gpuInfo.GetGPUUtilizationLPL(id);
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

        public new void Focus() {
            if (id == 0) {
                foreach (var subform in moreGPUForms) {
                    subform.Focus();
                }
            }
            base.Focus();
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

        private void GPUForm_DpiChanged(object sender, DpiChangedEventArgs e) {
            if (e.DeviceDpiNew != e.DeviceDpiOld) {
                new Action(delegate () {
                    Thread.Sleep(150);
                    Invoke(new Action(delegate () {
                        this.GPUForm_Resize(sender, e);
                    }));
                }).BeginInvoke(null, null);
                float scale = (float)e.DeviceDpiNew / (float)e.DeviceDpiOld;
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
        }
    }

    public class GPUInfo {
        private List<PerformanceCounter> pcDedicateGPUMemory;
        private List<PerformanceCounter> pcGPUEngine;
        private List<string> gpu_name;
        public int Count { get; private set; }
        private List<PairedAdapterInfo> pairedAdapterInfos;

        [DllImport("GetGPUInfoDXDLL.dll")]
        private extern static int Init();
        [DllImport("GetGPUInfoDXDLL.dll")]
        private extern static IntPtr getAdapterName(int luid);
        [DllImport("GetGPUInfoDXDLL.dll")]
        private extern static long getDedicatedMemory(int luid);

        struct PairedAdapterInfo : IComparable {
            public string name { get; }
            public string luid { get; }
            public long totalMemory { get; }
            public PairedAdapterInfo(string name, string luid) {
                this.name = name;
                this.luid = luid;
                this.totalMemory = 0;
            }
            public PairedAdapterInfo(string name, string luid, long totalMemory) {
                this.name = name;
                this.luid = luid;
                this.totalMemory = totalMemory;
            }
            public int CompareTo(object obj) {
                PairedAdapterInfo target = (PairedAdapterInfo)obj;
                return this.luid.CompareTo(target.luid);
            }
        }

        private string getGpuPcId(int id) {
            return this.pairedAdapterInfos[id].luid;
        }

        public string getGpuName(int id) {      //GPU型号名称
            return this.pairedAdapterInfos[id].name;
        }

        public long getAdapterTotalMemory(int id) {     //GPU总显存
            return this.pairedAdapterInfos[id].totalMemory;
        }

        // 构造函数，初始化计数器
        public GPUInfo(int id = -1) {
            this.resetEvents = new ManualResetEvent[max_thread_num];
            for (int i = 0; i < max_thread_num; i++)
                this.resetEvents[i] = new ManualResetEvent(false);
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
                catch (NullReferenceException) { }
            }
            this.Count = gpu_name.Count;
            this.FilterValidGPU();
            this.RemoveUnnecessaryPC(id);
        }

        private void FilterValidGPU() {
            this.pairedAdapterInfos = new List<PairedAdapterInfo>();
            int i = 0;
            foreach (PerformanceCounter pc in pcDedicateGPUMemory) {
                string cDeviceId = pc.InstanceName.Split('_')[2];
                try {
                    int suc = Init();
                    if (suc != 0) throw new DllNotFoundException("Load DXGI DLL failed. Make sure DirectX is correctly installed.");
                    int cluid = Convert.ToInt32(cDeviceId, 16);
                    string c_gpu_name = Marshal.PtrToStringAnsi(getAdapterName(cluid));
                    if (c_gpu_name.Contains("Microsoft Basic")) throw new NotSupportedException("Microsoft basic renderer is not supported. Skipping.");
                    long dedicate_memory = 0;
                    if (!c_gpu_name.Contains("HD Graphics"))
                        dedicate_memory = getDedicatedMemory(cluid);
                    this.pairedAdapterInfos.Add(new PairedAdapterInfo(c_gpu_name, cDeviceId, dedicate_memory));
                }
                catch (DllNotFoundException) {
                    this.pairedAdapterInfos.Add(new PairedAdapterInfo(gpu_name[i], cDeviceId));
                }
                catch {
                    continue;
                }
                i++;
            }
            //Debug.Assert(pairedAdapterInfos.Count == this.Count);
            if (pairedAdapterInfos.Count != this.Count) {
                MessageBox.Show("Some graphics card(s) are too old and not supported. Be sure your graphics driver is up-to-date and support WDDM 2.x.", "Not supported graphics detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Count = Math.Min(pairedAdapterInfos.Count, this.Count);
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
        // 原始版本
        public Dictionary<string, float> GetGPUUtilization(int id) {
            string deviceId = this.getGpuPcId(id);
            try {
                Dictionary<string, float> result = new Dictionary<string, float>();
                foreach (PerformanceCounter pc in pcGPUEngine) {
                    string[] csplit = pc.InstanceName.Split('_');
                    string cDeviceId = csplit[4];
                    if (cDeviceId == deviceId) {
                        string cEngine = getEngineString(csplit);
                        if (result.ContainsKey(cEngine)) {
                            result[cEngine] += pc.NextValue();
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
        // 使用了System.Threading.Tasks.Parallel类的ForEach
        public Dictionary<string, float> GetGPUUtilizationPL(int id) {
            string deviceId = this.getGpuPcId(id);
            Dictionary<string, float> result = new Dictionary<string, float>();
            Parallel.ForEach(pcGPUEngine, pc => {
                string[] csplit = pc.InstanceName.Split('_');
                string cDeviceId = csplit[4];
                if (cDeviceId == deviceId) {
                    string cEngine = getEngineString(csplit);
                    float cvalue = 0;
                    try { cvalue = pc.NextValue(); }
                    catch (InvalidOperationException) { }
                    lock (result) {
                        if (result.ContainsKey(cEngine)) {
                            result[cEngine] += cvalue;
                        } else {
                            result[cEngine] = cvalue;
                        }
                    }
                }
            });
            return result;
        }
        // 使用了System.Threading.Tasks.Parallel类的For且限制了最大并发数量
        public Dictionary<string, float> GetGPUUtilizationLPL(int id) {
            const int max_parallel_num = 6;
            string deviceId = this.getGpuPcId(id);
            int per_pc_cnt = pcGPUEngine.Count / max_parallel_num;
            Dictionary<string, float> result = new Dictionary<string, float>();
            Parallel.For(0, max_parallel_num, tid => {
                int start_i = tid * per_pc_cnt;
                int end_i = (tid + 1) * per_pc_cnt;
                if (tid == max_parallel_num - 1) end_i = pcGPUEngine.Count;
                for (int i = start_i; i < end_i; i++) {
                    PerformanceCounter pc = pcGPUEngine[i];
                    string cDeviceId = pc.InstanceName.Split('_')[4];
                    if (cDeviceId == deviceId) {
                        string cEngine = getEngineString(pc);
                        float cvalue = 0;
                        try { cvalue = pc.NextValue(); }
                        catch (InvalidOperationException) { }
                        lock (result) {
                            if (result.ContainsKey(cEngine)) {
                                result[cEngine] += cvalue;
                            } else {
                                result[cEngine] = cvalue;
                            }
                        }
                    }
                }
            });
            return result;
        }

        private const int max_thread_num = 3;
        private ManualResetEvent[] resetEvents;
        private struct Para {
            public int tid;
            public List<PerformanceCounter> cPcGPUs;
            public ManualResetEvent reset_event;
            public Para(int id, ManualResetEvent resetEvent) {
                this.tid = id;
                this.cPcGPUs = new List<PerformanceCounter>();
                this.reset_event = resetEvent;
            }
        }
        private Dictionary<string, float>[] retDic = new Dictionary<string, float>[max_thread_num];
        // 使用了System.Threading.ThreadPool类创建线程池并使用WaitHandle与线程池通信且限制了最大线程数量
        public Dictionary<string, float> GetGPUUtilizationTP(int id) {
            string deviceId = this.getGpuPcId(id);
            int per_pc_cnt = pcGPUEngine.Count / max_thread_num;
            Para[] paras = new Para[max_thread_num];
            for (int tid = 0; tid < max_thread_num; tid++) {
                paras[tid] = new Para(tid, resetEvents[tid]);
                //this.retDic[tid] = new Dictionary<string, float>();
            }
            for (int i = 0; i < pcGPUEngine.Count; i++) {
                int tid = Math.Min(i / per_pc_cnt, max_thread_num - 1);
                PerformanceCounter pc = pcGPUEngine[i];
                string[] csplit = pc.InstanceName.Split('_');
                string cDeviceId = csplit[4];
                if (cDeviceId == deviceId) {
                    paras[tid].cPcGPUs.Add(pc);
                }
            }
            for (int tid = 0; tid < max_thread_num; tid++) {
                resetEvents[tid].Reset();
                ThreadPool.QueueUserWorkItem(new WaitCallback(getGPUUti_workingthread), paras[tid]);
            }
            WaitHandle.WaitAll(resetEvents);
            Dictionary<string, float> ret = new Dictionary<string, float>();
            for (int th = 0; th < max_thread_num; th++) {
                foreach (var kv in retDic[th]) {
                    string key = kv.Key;
                    float value = kv.Value;
                    if (ret.ContainsKey(key)) {
                        ret[key] += value;
                    } else {
                        ret[key] = value;
                    }
                }
            }
            return ret;
        }
        private void getGPUUti_workingthread(object obj) {
            Para para = (Para)obj;
            Dictionary<string, float> cret = new Dictionary<string, float>();
            foreach (var pc in para.cPcGPUs) {
                string cEngine = getEngineString(pc);
                float cvalue = 0;
                try {
                    cvalue = pc.NextValue();
                }
                catch (InvalidOperationException) { }
                if (cret.ContainsKey(cEngine)) {
                    cret[cEngine] += cvalue;
                } else {
                    cret[cEngine] = cvalue;
                }
            }
            this.retDic[para.tid] = cret;
            para.reset_event.Set();
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
        private string getEngineString(PerformanceCounter pc) {
            return getEngineString(pc.InstanceName.Split('_'));
        }

        public void RefreshGPUEnginePerfCnt() {
            //刷新GPU利用率计数器
            PerformanceCounterCategory pidGpuPfc = new PerformanceCounterCategory("GPU Engine", "Utilization Percentage");
            pidGpuPfc.MachineName = ".";
            string[] pidGpuInstanceNames = pidGpuPfc.GetInstanceNames();
            pcGPUEngine.Clear();
            foreach (var gpu in pcDedicateGPUMemory) {
                string c_device_id = gpu.InstanceName.Split('_')[2];
                //gpu.NextValue();      //TODO: TEST THIS LINE TO ENSURE IF IT IS REALLY UNNECESSARY
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
                    const bool nv_init = true;     //TEST FAILURE: Must NextValue() before use
                    if (nv_init) {
                        try {
                            pc.NextValue();
                            pcGPUEngine.Add(pc);
                        }
                        catch (InvalidOperationException) { }
                    } else {
                        pcGPUEngine.Add(pc);
                    }
                }
            }
        }
        public void RefreshGPUEnginePerfCntLPL(int id) {
            const int max_parallel_num = 4;
            PerformanceCounterCategory pidGpuPfc = new PerformanceCounterCategory("GPU Engine", "Utilization Percentage");
            pidGpuPfc.MachineName = ".";
            string[] pidGpuInstanceNames = pidGpuPfc.GetInstanceNames();
            pcGPUEngine.Clear();
            int per_pc_cnt = pidGpuInstanceNames.Length / max_parallel_num;
            string c_device_id = this.getGpuPcId(id);
            Parallel.For(0, max_parallel_num, tid => {
                int start_i = tid * per_pc_cnt;
                int end_i = (tid + 1) * per_pc_cnt;
                if (tid == max_parallel_num - 1) end_i = pidGpuInstanceNames.Length;
                for (int i = start_i; i < end_i; i++) {
                    string pidInstanceName = pidGpuInstanceNames[i];
                    string c_pid_deviceId = pidInstanceName.Split('_')[4];
                    if (c_pid_deviceId == c_device_id) {
                        PerformanceCounter pc = new PerformanceCounter("GPU Engine", "Utilization Percentage", pidInstanceName);
                        // Must NextValue() before use
                        try {
                            pc.NextValue();
                            lock (this.pcGPUEngine) {
                                this.pcGPUEngine.Add(pc);
                            }
                        }
                        catch (InvalidOperationException) { }
                    }
                }
            });
        }
    }
}