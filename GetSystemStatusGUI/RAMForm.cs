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

namespace GetSystemStatusGUI {
    public partial class RAMForm : Form {
        private const int historyLength = 60;
        private RAMInfo ramInfo;
        private static string[] scale_unit = { "Bytes", "KB", "MB", "GB", "TB" };
        private Color chartColor = Color.FromArgb(160, 139, 0, 139);
        private Form1 mainform;

        public RAMForm(Form1 mainform) {
            ramInfo = new RAMInfo();
            InitializeComponent();
            this.mainform = mainform;
        }

        private void RAMForm_Load(object sender, EventArgs e) {
            List<int> list = new List<int>();
            for (int i = 0; i < historyLength; i++) list.Add(0);
            chart1.Series[0].Points.DataBindY(list);
            chart1.PaletteCustomColors[0] = chartColor;
            new Action(ram_update_thread).BeginInvoke(null, null);
        }

        private void ram_update_thread() {
            List<int> usageList = new List<int>();
            for (int i = 0; i < historyLength; i++) usageList.Add(0);
            while (!chart1.IsDisposed && !lblRAM.IsDisposed) {
                int rusage = (int)Math.Round((1.0 - (double)ramInfo.MemoryAvailable / (double)ramInfo.PhysicalMemory) * 100.0);
                int ramScale = (int)Math.Floor(Math.Log((double)ramInfo.MemoryAvailable, 1024));
                double memAvail = Math.Round((double)ramInfo.MemoryAvailable / Math.Pow(1024, ramScale), 1);
                double memTotal = Math.Round((double)ramInfo.PhysicalMemory / Math.Pow(1024, ramScale), 1);
                usageList.RemoveAt(0);
                usageList.Add(rusage);
                Action updateChart = new Action(
                    delegate () {
                        lblRAM.Text = string.Format("{0} / {1}{2} ({3}%)", memTotal - memAvail, memTotal, scale_unit[ramScale], rusage);
                        chart1.Series[0].Points.DataBindY(usageList);
                    }
                );
                Invoke(updateChart);
                Thread.Sleep(1000);
            }
        }

		private void RAMForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainform.DisableChecked("RAM");
		}
	}

	public class RAMInfo {
        private long m_PhysicalMemory = 0;   //物理内存
        private PerformanceCounter pcAvailMemory;   //可用内存（性能计数器版）

        // 构造函数，初始化计数器
        public RAMInfo() {
            //初始化计数器
            pcAvailMemory = new PerformanceCounter("Memory", "Available Bytes");

            //获得物理内存
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc) {
                if (mo["TotalPhysicalMemory"] != null) {
                    m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
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
    }
}