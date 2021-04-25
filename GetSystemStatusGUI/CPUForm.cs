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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSystemStatusGUI
{
    public partial class CPUForm : Form
    {
        Form1 mainForm;
        CPUInfo sysInfo;
        public CPUForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void CPUForm_Load(object sender, EventArgs e)
        {
            sysInfo = new CPUInfo();
            /*List<int> x = new List<int>() { 0, 1, 2, 3, 4 };
            List<int> y = new List<int>() { 5, 7, 25, 94, 3 };
            //chart1.Series.Add("_Total");
            //chart1.Series["Series1"].Points.DataBindXY(x, y);
            chart1.Series["Series1"].Points.DataBindY(y);
            chart1.Series["Series1"].IsVisibleInLegend = false;
            cpuName.Text = sysInfo.CpuName;*/
            List<int> x = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            List<int> y = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //TODO：单独开个线程更新CPU负载值
        }

        private void CPUForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

		private void CPUForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainForm.DisableChecked("CPU");
            this.Hide();
        }
	}

    public class CPUInfo
    {
        public string CpuName { get; }     //CPU名称
        private PerformanceCounter pcCpuLoad;   //CPU计数器
        private PerformanceCounter[] pcCpuCoreLoads;   //每CPU核心的利用率

        // 构造函数，初始化计数器
        public CPUInfo()
        {
            ProcessorCount = Environment.ProcessorCount;
            //初始化计数器
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuCoreLoads = new PerformanceCounter[ProcessorCount];
            for (int i = 0; i < ProcessorCount; i++)
            {
                pcCpuCoreLoads[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
            pcCpuLoad.MachineName = ".";
            pcCpuLoad.NextValue();

            for (int i = 0; i < ProcessorCount; i++) pcCpuCoreLoads[i].NextValue();

            //CPU名称
            var st = string.Empty;
            var driveId = new ManagementObjectSearcher("Select Name from Win32_Processor");
            foreach (var o in driveId.Get())
            {
                var mo = (ManagementObject)o;
                st = mo["Name"].ToString();
            }
            CpuName = st;
        }

        // CPU利用率、核心数
        public int ProcessorCount { get; } = 0;
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

    }

}
