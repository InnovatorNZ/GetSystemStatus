using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GetSystemStatusGUI {
	public class SystemInfo {
		public string CpuName { get; }     //CPU名称
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private PerformanceCounter[] pcCpuCoreLoads;   //每CPU核心的利用率
        private PerformanceCounter[] pcDisksRead;   //每磁盘读速率
        private PerformanceCounter[] pcDisksWrite;  //每磁盘写速率
        private PerformanceCounter[] pcDisksLoad;   //磁盘占用率
        private PerformanceCounter pcDiskRead;  //总磁盘读速率
        private PerformanceCounter pcDiskWrite; //总磁盘写速率
        private long m_PhysicalMemory = 0;   //物理内存
        private PerformanceCounter pcAvailMemory;   //可用内存（性能计数器版）
        public int m_DiskNum = 0;    //磁盘个数
        public List<string> DiskInstanceNames = new List<string>();
        public NetworkInterface[] adapters;
        private Dictionary<string, PerformanceCounter> pcNetworkReceive;
        private Dictionary<string, PerformanceCounter> pcNetworkSend;
        private List<PerformanceCounter> pcDedicateGPUMemory;   //专用GPU显存占用率
        public List<int> gpu_memory { get; }    //GPU显存
        public List<string> gpu_name { get; }   //GPU名称
        private List<PerformanceCounter> pcGPUEngine;

        // 构造函数，初始化计数器
        public SystemInfo() {
            ProcessorCount = Environment.ProcessorCount;
            //初始化计数器
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcAvailMemory = new PerformanceCounter("Memory", "Available Bytes");
            pcDiskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            pcDiskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            PerformanceCounterCategory diskPfc = new PerformanceCounterCategory("PhysicalDisk");
            string[] diskInstanceNames = diskPfc.GetInstanceNames();
            m_DiskNum = diskInstanceNames.Length - 1;
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
            pcCpuCoreLoads = new PerformanceCounter[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++) {
                pcCpuCoreLoads[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            pcCpuLoad.MachineName = ".";
            pcDiskRead.MachineName = ".";
            pcCpuLoad.NextValue();
            pcDiskRead.NextValue();
            pcDiskWrite.NextValue();

            for (int i = 0; i < ProcessorCount; i++) pcCpuCoreLoads[i].NextValue();
            for (int i = 0; i < m_DiskNum; i++) { pcDisksRead[i].NextValue(); pcDisksWrite[i].NextValue(); pcDisksLoad[i].NextValue(); }

            //获得物理内存
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc) {
                if (mo["TotalPhysicalMemory"] != null) {
                    m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }

            //CPU名称
            var st = string.Empty;
            var driveId = new ManagementObjectSearcher("Select * from Win32_Processor");
            foreach (var o in driveId.Get()) {
                var mo = (ManagementObject)o;
                st = mo["Name"].ToString();
            }
            CpuName = st;

            //网卡性能计数器
            adapters = NetworkInterface.GetAllNetworkInterfaces();
            pcNetworkReceive = new Dictionary<string, PerformanceCounter>();
            pcNetworkSend = new Dictionary<string, PerformanceCounter>();
            foreach (NetworkInterface adapter in adapters) {
                pcNetworkReceive.Add(adapter.Description, new PerformanceCounter("Network Adapter", "Bytes Received/sec", R(adapter.Description), "."));
                pcNetworkSend.Add(adapter.Description, new PerformanceCounter("Network Adapter", "Bytes Sent/sec", R(adapter.Description), "."));
            }

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

        // 网卡上传、下载速率
        public float ReceiveSpeed(string AdapterDesc) {
            PerformanceCounter selectedPC;
            if (!pcNetworkReceive.TryGetValue(AdapterDesc, out selectedPC)) return -1;
            return selectedPC.NextValue();
        }
        public float SendSpeed(string AdapterDesc) {
            PerformanceCounter selectedPC;
            if (!pcNetworkSend.TryGetValue(AdapterDesc, out selectedPC)) return -1;
            return selectedPC.NextValue();
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

        // 专用GPU显存
        public List<long> GPUDedicatedMemory {
            get {
                List<long> ret = new List<long>();
                foreach (PerformanceCounter pc in pcDedicateGPUMemory) {
                    ret.Add((long)Math.Floor(pc.NextValue()));
                }
                ret.Remove(0);
                return ret;
            }
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

        //GPU利用率
        public List<int> GPUUtilization {
            get {
                List<int> uti = new List<int>();
                string l_deviceId = pcGPUEngine[0].InstanceName.Split('_')[4];
                float value = 0;
                foreach (PerformanceCounter pc in pcGPUEngine) {
                    string c_deviceId = pc.InstanceName.Split('_')[4];
                    if (c_deviceId != l_deviceId) {
                        uti.Add((int)Math.Round(value));
                        value = 0;
                    }
                    value += pc.NextValue();
                    l_deviceId = c_deviceId;
                }
                uti.Add((int)Math.Round(value));
                return uti;
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

        private string R(string str) {
            return str.Replace('#', '_').Replace('(', '[').Replace(')', ']');
        }

    }
}
