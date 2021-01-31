using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;
using System.Web;
using Amazon.EC2.Model;
using System.Runtime.CompilerServices;

namespace GetSystemStatus
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemInfo systemInfo = new SystemInfo();
            for (int _ = 0; _ < 30; _++)
            {
                Console.Clear();
                Console.WriteLine("C# Get System Status Task Manager Console Edition:");
                int cusage = (int)Math.Round(systemInfo.CpuLoad);
                Console.Write("CPU Total Usage: " + cusage + "%\t");
                for (int i = 0; i < systemInfo.ProcessorCount; i++)
                {
                    int ccusage = (int)Math.Round(systemInfo.CpuCoreLoad(i));
                    Console.Write("Core " + i + " Usage: " + ccusage + "%  ");
                }
                int rusage = (int)Math.Round((1.0 - (double)systemInfo.MemoryAvailable / (double)systemInfo.PhysicalMemory) * 100.0);
                Console.WriteLine();
                Console.WriteLine("RAM Usage: " + rusage + "%\t");
                for (int i = 0; i < systemInfo.m_DiskNum; i++)
                {
                    //float fDiskRead = systemInfo.DiskReadTotal;
                    float fDiskRead = systemInfo.DiskRead(i);
                    float fDiskWrite = systemInfo.DiskWriteTotal;
                    int rscale = (int)Math.Floor(Math.Log(fDiskRead, 1024));
                    int wscale = (int)Math.Floor(Math.Log(fDiskWrite, 1024));
                    if (rscale < 0) rscale = 0;
                    if (wscale < 0) wscale = 0;
                    fDiskRead /= (float)Math.Pow(1024, rscale);
                    fDiskWrite /= (float)Math.Pow(1024, wscale);
                    string[] speed_units = { "Bytes/sec", "KB/s", "MB/s", "GB/s", "TB/s" };
                    fDiskRead = (float)Math.Round(fDiskRead, 1);
                    fDiskWrite = (float)Math.Round(fDiskWrite, 1);
                    Console.Write("Disk " + i + " Read : " + fDiskRead + speed_units[rscale] + "\t");
                    Console.Write("Disk Write Total: " + fDiskWrite + speed_units[wscale] + "\t");
                    Console.WriteLine();
                }
                Console.WriteLine();
                Thread.Sleep(1000);
            }
        }
    }
    ///  
    /// 系统信息类 - 获取CPU、内存、磁盘、进程信息
    ///  
    public class SystemInfo
    {
        private int m_ProcessorCount = 0;   //CPU个数
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private PerformanceCounter[] pcCpuCoreLoads;   //每个CPU核心的利用率
        public PerformanceCounter[] pcDisksRead;
        private PerformanceCounter pcDiskRead;  //磁盘读速率
        private PerformanceCounter pcDiskWrite; //磁盘写速率
        private long m_PhysicalMemory = 0;   //物理内存
        public int m_DiskNum = 0;    //磁盘个数

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

        #region DLL引入
        [DllImport("IpHlpApi.dll")]
        extern static public uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        [DllImport("User32")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);
        #endregion

        // 构造函数，初始化计数器等
        public SystemInfo()
        {
            m_ProcessorCount = Environment.ProcessorCount;
            //初始化计数器
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcDiskRead = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            pcDiskWrite = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            PerformanceCounterCategory diskPfc = new PerformanceCounterCategory("PhysicalDisk");
            string[] diskInstanceNames = diskPfc.GetInstanceNames();
            m_DiskNum = diskInstanceNames.Length - 1;
            pcDisksRead = new PerformanceCounter[m_DiskNum];
            int c = 0;
            for(int i = 0; i < diskInstanceNames.Length; i++)
            {
                if (diskInstanceNames[i] != "_Total")
                {
                    pcDisksRead[c++] = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", diskInstanceNames[i]);
                }
            }
            pcCpuCoreLoads = new PerformanceCounter[m_ProcessorCount];
            for(int i = 0; i < m_ProcessorCount; i++)
            {
                pcCpuCoreLoads[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            pcCpuLoad.MachineName = ".";
            pcDiskRead.MachineName = ".";
            pcCpuLoad.NextValue();
            pcDiskRead.NextValue();
            pcDiskWrite.NextValue();
            for (int i = 0; i < m_ProcessorCount; i++) pcCpuCoreLoads[i].NextValue();
            for (int i = 0; i < m_DiskNum; i++) pcDisksRead[i].NextValue();

            //获得物理内存
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
        }

        #region CPU个数
        public int ProcessorCount
        {
            get
            {
                return m_ProcessorCount;
            }
        }
        #endregion

        #region CPU占用率
        public float CpuLoad
        {
            get
            {
                return pcCpuLoad.NextValue();
            }
        }
        public float CpuCoreLoad(int core_num)
        {
            return pcCpuCoreLoads[core_num].NextValue();
        }
        #endregion

        #region 磁盘占用
        public float DiskReadTotal {
            get { return pcDiskRead.NextValue(); }
        }
        public float DiskWriteTotal {
            get { return pcDiskWrite.NextValue(); }
        }
        public float DiskRead(int diskId)
        {
            return pcDisksRead[diskId].NextValue();
        }
        #endregion

        #region 可用内存 
        ///  
        /// 获取可用内存 
        ///  
        public long MemoryAvailable
        {
            get
            {
                long availablebytes = 0;
                //ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PerfRawData_PerfOS_Memory"); 
                //foreach (ManagementObject mo in mos.Get()) 
                //{ 
                //    availablebytes = long.Parse(mo["Availablebytes"].ToString()); 
                //} 
                ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject mo in mos.GetInstances())
                {
                    if (mo["FreePhysicalMemory"] != null)
                    {
                        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    }
                }
                return availablebytes;
            }
        }
        #endregion

        #region 物理内存 
        ///  
        /// 获取物理内存 
        ///  
        public long PhysicalMemory
        {
            get
            {
                return m_PhysicalMemory;
            }
        }
        #endregion
        
        /*
        #region 获得分区信息 
        ///  
        /// 获取分区信息 
        ///  
        public List GetLogicalDrives()
        {
            List drives = new List();
            ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = diskClass.GetInstances();
            foreach (ManagementObject disk in disks)
            {
                // DriveType.Fixed 为固定磁盘(硬盘) 
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed)
                {
                    drives.Add(new DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }
        ///  
        /// 获取特定分区信息 
        ///  
        /// 盘符 
        public List GetLogicalDrives(char DriverID)
        {
            List drives = new List();
            WqlObjectQuery wmiquery = new WqlObjectQuery("SELECT * FROM Win32_LogicalDisk WHERE DeviceID = ’" + DriverID + ":’");
            ManagementObjectSearcher wmifind = new ManagementObjectSearcher(wmiquery);
            foreach (ManagementObject disk in wmifind.Get())
            {
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed)
                {
                    drives.Add(new DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }
        #endregion

        #region 获得进程列表 
        ///  
        /// 获得进程列表 
        ///  
        public List GetProcessInfo()
        {
            List pInfo = new List();
            Process[] processes = Process.GetProcesses();
            foreach (Process instance in processes)
            {
                try
                {
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
        ///  
        /// 获得特定进程信息 
        ///  
        /// 进程名称 
        public List GetProcessInfo(string ProcessName)
        {
            List pInfo = new List();
            Process[] processes = Process.GetProcessesByName(ProcessName);
            foreach (Process instance in processes)
            {
                try
                {
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
        ///  
        /// 结束指定进程 
        ///  
        /// 进程的 Process ID 
        public static void EndProcess(int pid)
        {
            try
            {
                Process process = Process.GetProcessById(pid);
                process.Kill();
            }
            catch { }
        }
        #endregion


        #region 查找所有应用程序标题 
        ///  
        /// 查找所有应用程序标题 
        ///  
        /// 应用程序标题范型 
        public static List FindAllApps(int Handle)
        {
            List Apps = new List();

            int hwCurr;
            hwCurr = GetWindow(Handle, GW_HWNDFIRST);

            while (hwCurr > 0)
            {
                int IsTask = (WS_VISIBLE | WS_BORDER);
                int lngStyle = GetWindowLongA(hwCurr, GWL_STYLE);
                bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                if (TaskWindow)
                {
                    int length = GetWindowTextLength(new IntPtr(hwCurr));
                    StringBuilder sb = new StringBuilder(2 * length + 1);
                    GetWindowText(hwCurr, sb, sb.Capacity);
                    string strTitle = sb.ToString();
                    if (!string.IsNullOrEmpty(strTitle))
                    {
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
