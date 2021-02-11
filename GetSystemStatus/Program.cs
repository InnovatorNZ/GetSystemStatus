using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;
using System.Web;
//using Amazon.EC2.Model;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace GetSystemStatus {
    class Program {
        static void Main(string[] args) {
            SystemInfo sysInfo = new SystemInfo();
            for (int _ = 0; _ < 3000; _++) {
                Console.Clear();
                Console.WriteLine("Task Manager Console Edition");
                //CPU利用率
                int cusage = (int)Math.Round(sysInfo.CpuLoad);
                Console.Write("CPU: {0} Total Usage: {1}%\n", sysInfo.cpu_name, cusage);
                for (int i = 0; i < sysInfo.ProcessorCount; i++) {
                    int ccusage = (int)Math.Round(sysInfo.CpuCoreLoad(i));
                    Console.Write("Core " + i + " Usage: " + ccusage + "%\t");
                }
                Console.WriteLine();
                //RAM占用
                int rusage = (int)Math.Round((1.0 - (double)sysInfo.MemoryAvailable / (double)sysInfo.PhysicalMemory) * 100.0);
                string[] scale_unit = { "Bytes", "KB", "MB", "GB", "TB" };
                int ramScale = (int)Math.Floor(Math.Log((double)sysInfo.MemoryAvailable, 1024));
                double memAvail = Math.Round((double)sysInfo.MemoryAvailable / Math.Pow(1024, ramScale), 1);
                double memTotal = Math.Round((double)sysInfo.PhysicalMemory / Math.Pow(1024, ramScale), 1);
                Console.WriteLine("RAM Usage: {0}/{1}{2} ({3}%)", memTotal - memAvail, memTotal, scale_unit[ramScale], rusage);
                //磁盘占用与速率
                //List<string> disk_load_display = new List<string>();
                string[] disk_load_display = new string[sysInfo.m_DiskNum];
                for (int i = 0; i < sysInfo.m_DiskNum; i++) {
                    //float fDiskRead = systemInfo.DiskReadTotal;
                    float fDiskRead = sysInfo.DiskRead(i);
                    //float fDiskWrite = systemInfo.DiskWriteTotal;
                    float fDiskWrite = sysInfo.DiskWrite(i);
                    int rscale = (int)Math.Max(Math.Floor(Math.Log(fDiskRead, 1024)), 0);
                    int wscale = (int)Math.Max(Math.Floor(Math.Log(fDiskWrite, 1024)), 0);
                    fDiskRead /= (float)Math.Pow(1024, rscale);
                    fDiskWrite /= (float)Math.Pow(1024, wscale);
                    string[] speed_units = { "Bytes/sec", "KB/s", "MB/s", "GB/s", "TB/s" };
                    fDiskRead = (float)Math.Round(fDiskRead, 1);
                    fDiskWrite = (float)Math.Round(fDiskWrite, 1);
                    string[] csplit = sysInfo.DiskInstanceNames[i].Split(' ');
                    int cid = int.Parse(csplit[0]);
                    string cDiskDesc = "Disk " + csplit[0] + " ";
                    if (csplit.Length > 1) {
                        cDiskDesc += "(";
                        for (int j = 1; j < csplit.Length - 1; j++)
                            cDiskDesc += csplit[j] + " ";
                        cDiskDesc += csplit[csplit.Length - 1];
                        cDiskDesc += ")";
                    }
                    string cdisk_display = string.Empty;
                    //Console.Write(cDiskDesc);
                    cdisk_display += cDiskDesc;
                    //Console.Write(" Load: " + (int)Math.Round(sysInfo.DiskLoad(i)) + "%");
                    cdisk_display += " Load: " + (int)Math.Round(sysInfo.DiskLoad(i)) + "%";
                    //Console.Write("\tRead: " + fDiskRead + speed_units[rscale]);
                    cdisk_display += "\tRead: " + fDiskRead + speed_units[rscale];
                    //Console.Write("\tWrite: " + fDiskWrite + speed_units[wscale]);
                    cdisk_display += "\tWrite: " + fDiskWrite + speed_units[wscale];
                    //disk_load_display.Add(cdisk_display);
                    disk_load_display[cid] = cdisk_display;
                }
                foreach (string cdiskload in disk_load_display) {
                    Console.WriteLine(cdiskload);
                }
                //网卡信息与速率
                foreach (NetworkInterface adapter in sysInfo.adapters) {
                    if (adapter.Speed > 0 && adapter.Speed != 1073741824) {
                        string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                        RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                        if (rk is null) continue;
                        //区分 PnpInstanceID，前三个字符指示连接方式，PCI为PCI/PCIe内置网卡，USB为USB网卡，BTH为蓝牙网卡，ROOT为虚拟网卡；MediaSubType 为 01 则是虚拟网卡，02为无线网卡。
                        string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                        int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                        if (fPnpInstanceID.Length <= 3) continue;
                        string connMethod = fPnpInstanceID.Substring(0, 3);
                        if ((connMethod == "PCI" || connMethod == "USB" || connMethod == "BTH" || fMediaSubType == 2) && fMediaSubType != 1 && connMethod != "ROOT") {
                            Console.Write(adapter.NetworkInterfaceType + " ");
                            Console.Write(adapter.Name + ":\t");
                            long linkSpeed = adapter.Speed / 1000 / 1000;
                            string nScale = "Mbps";
                            if (linkSpeed >= 1000) {
                                linkSpeed /= 1000;
                                nScale = "Gbps";
                            }
                            Console.Write("Link Speed: {0}{1}", linkSpeed, nScale);
                            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                            UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = adapterProperties.UnicastAddresses;
                            string IPv4Adrress = "Not Present", IPv6Address = "Not Present";
                            foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection) {
                                if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                                    IPv4Adrress = UnicastIPAddressInformation.Address.ToString();
                                else if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetworkV6)
                                    IPv6Address = UnicastIPAddressInformation.Address.ToString();
                            }
                            Console.Write("\tIPv4 Address: " + IPv4Adrress);
                            Console.Write("\tIPv6 Address: " + IPv6Address);
                            long receiveSpeed = (long)Math.Round(sysInfo.ReceiveSpeed(adapter.Description)) * 8;
                            long sendSpeed = (long)Math.Round(sysInfo.SendSpeed(adapter.Description)) * 8;
                            int rscale = (int)Math.Max(0, Math.Floor(Math.Log(receiveSpeed, 1000)));
                            int sscale = (int)Math.Max(0, Math.Floor(Math.Log(sendSpeed, 1000)));
                            receiveSpeed = (long)Math.Round(receiveSpeed / Math.Pow(1000, rscale));
                            sendSpeed = (long)Math.Round(sendSpeed / Math.Pow(1000, sscale));
                            string[] speed_units = { "bps", "Kbps", "Mbps", "Gbps" };
                            Console.Write("\tSend: {0}{1}", sendSpeed, speed_units[sscale]);
                            Console.Write("\tReceive: {0}{1}", receiveSpeed, speed_units[rscale]);
                            Console.WriteLine();
                        }
                    }
                }
                //专用GPU显存
                List<long> dediGPUMem = sysInfo.GPUDedicatedMemory;
                for (int i = 0; i < dediGPUMem.Count; i++) {
                    int gscale = (int)Math.Max(Math.Floor(Math.Log(dediGPUMem[i], 1024)), 0);
                    double memGPU = Math.Round((double)dediGPUMem[i] / Math.Pow(1024, gscale), 1);
                    string strscale = scale_unit[gscale];
                    string name = sysInfo.gpu_name[i];
                    Console.WriteLine("GPU {0} {3}: Dedicated Memory Usage: {1}{2}", i, memGPU, strscale, name);
                }
                Thread.Sleep(1500);
            }
        }
    }
    ///  
    /// 系统信息类 - 获取CPU、内存、磁盘、网络信息
    ///  
    public class SystemInfo {
        private int m_ProcessorCount = 0;   //CPU个数
        public string cpu_name { get; }     //CPU名称
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

        // 构造函数，初始化计数器
        public SystemInfo() {
            m_ProcessorCount = Environment.ProcessorCount;
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
            pcCpuCoreLoads = new PerformanceCounter[m_ProcessorCount];
            for (int i = 0; i < m_ProcessorCount; i++) {
                pcCpuCoreLoads[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            pcCpuLoad.MachineName = ".";
            pcDiskRead.MachineName = ".";
            pcCpuLoad.NextValue();
            pcDiskRead.NextValue();
            pcDiskWrite.NextValue();
            for (int i = 0; i < m_ProcessorCount; i++) pcCpuCoreLoads[i].NextValue();
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
            cpu_name = st;

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
                catch (Exception) { }
            }
        }

        // CPU
        public int ProcessorCount {
            get {
                return m_ProcessorCount;
            }
        }
        public float CpuLoad {
            get {
                return pcCpuLoad.NextValue();
            }
        }
        public float CpuCoreLoad(int core_num) {
            return pcCpuCoreLoads[core_num].NextValue();
        }

        // 磁盘占用
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
                /*
                 * WMI版获取可用内存
                long availablebytes = 0;
                //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfOS_Memory"); 
                //foreach (ManagementObject mo in mos.Get()) 
                //{ 
                //    availablebytes = long.Parse(mo["Availablebytes"].ToString()); 
                //} 
                ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject mo in mos.GetInstances()) {
                    if (mo["FreePhysicalMemory"] != null) {
                        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    }
                }
                return availablebytes;*/
            }
        }
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

        private string R(string str) {
            return str.Replace('#', '_').Replace('(', '[').Replace(')', ']');
        }

        /*
        // CPU温度
        public float GetCPUTemperature() {
            string str = "";
            ManagementObjectSearcher vManagementObjectSearcher = new ManagementObjectSearcher(@"root\WMI", @"select * from MSAcpi_ThermalZoneTemperature");
            foreach (ManagementObject managementObject in vManagementObjectSearcher.Get()) {
                str += managementObject.Properties["CurrentTemperature"].Value.ToString();
            }
            float temp = (float.Parse(str) - 2732) / 10;
            return temp;
        }

        private void ShowAdapterInfo() {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            lst_NetworkAdapter.Items.Add("适配器个数：" + adapters.Length);
            int index = 0;

            foreach (NetworkInterface adapter in adapters) {
                index++;
                //显示网络适配器描述信息、名称、类型、速度、MAC 地址  
                lst_NetworkAdapter.Items.Add("---------------------第" + index + "个适配器信息---------------------");
                lst_NetworkAdapter.Items.Add("描述信息：" + adapter.Name);
                lst_NetworkAdapter.Items.Add("类型：" + adapter.NetworkInterfaceType);
                lst_NetworkAdapter.Items.Add("速度：" + adapter.Speed / 1000 / 1000 + "MB");
                lst_NetworkAdapter.Items.Add("MAC 地址：" + adapter.GetPhysicalAddress());

                //获取IPInterfaceProperties实例  
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();

                //获取并显示DNS服务器IP地址信息  
                IPAddressCollection dnsServers = adapterProperties.DnsAddresses;
                if (dnsServers.Count > 0) {
                    foreach (IPAddress dns in dnsServers) {
                        lst_NetworkAdapter.Items.Add("DNS 服务器IP地址：" + dns + "\n");
                    }
                } else {
                    lst_NetworkAdapter.Items.Add("DNS 服务器IP地址：" + "\n");
                }
            }
        }
        #region 获得分区信息 
        /// 获取分区信息 
        public List GetLogicalDrives() {
            List drives = new List();
            ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = diskClass.GetInstances();
            foreach (ManagementObject disk in disks) {
                // DriveType.Fixed 为固定磁盘(硬盘) 
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed) {
                    drives.Add(new DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }
        /// 获取特定分区信息 
        /// 盘符 
        public List GetLogicalDrives(char DriverID) {
            List drives = new List();
            WqlObjectQuery wmiquery = new WqlObjectQuery("SELECT * FROM Win32_LogicalDisk WHERE DeviceID = ’" + DriverID + ":’");
            ManagementObjectSearcher wmifind = new ManagementObjectSearcher(wmiquery);
            foreach (ManagementObject disk in wmifind.Get()) {
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed) {
                    drives.Add(new DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }
        #endregion

        #region 获得进程列表 
        /// 获得进程列表 
        public List GetProcessInfo() {
            List pInfo = new List();
            Process[] processes = Process.GetProcesses();
            foreach (Process instance in processes) {
                try {
                    pInfo.Add(new ProcessInfo(instance.Id,
                        instance.ProcessName,
                        instance.TotalProcessorTime.TotalMilliseconds,
                        instance.WorkingSet64,
                        instance.MainModule.FileName));
                }
                catch { }
            }
            return pInfo;
        }
        /// 获得特定进程信息 
        /// 进程名称 
        public List GetProcessInfo(string ProcessName) {
            List pInfo = new List();
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (Process instance in processes) {
                try {
                    pInfo.Add(new ProcessInfo(instance.Id,
                        instance.ProcessName,
                        instance.TotalProcessorTime.TotalMilliseconds,
                        instance.WorkingSet64,
                        instance.MainModule.FileName));
                }
                catch { }
            }
            return pInfo;
        }
        #endregion

        #region 结束指定进程 
        /// 结束指定进程 
        /// 进程的 Process ID 
        public static void EndProcess(int pid) {
            try {
                Process process = Process.GetProcessById(pid);
                process.Kill();
            }
            catch { }
        }
        #endregion


        #region 查找所有应用程序标题 
        /// 查找所有应用程序标题 
        /// 应用程序标题范型 
        public static List FindAllApps(int Handle) {
            List Apps = new List();

            int hwCurr;
            hwCurr = GetWindow(Handle, GW_HWNDFIRST);

            while (hwCurr > 0) {
                int IsTask = (WS_VISIBLE | WS_BORDER);
                int lngStyle = GetWindowLongA(hwCurr, GWL_STYLE);
                bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                if (TaskWindow) {
                    int length = GetWindowTextLength(new IntPtr(hwCurr));
                    StringBuilder sb = new StringBuilder(2 * length + 1);
                    GetWindowText(hwCurr, sb, sb.Capacity);
                    string strTitle = sb.ToString();
                    if (!string.IsNullOrEmpty(strTitle)) {
                        Apps.Add(strTitle);
                    }
                }
                hwCurr = GetWindow(hwCurr, GW_HWNDNEXT);
            }

            return Apps;
        }
        #endregion
        */
    }
}