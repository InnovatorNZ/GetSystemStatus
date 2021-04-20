using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSystemStatusGUI
{
    public partial class CPUForm : Form
    {
        Form1 mainForm;
        SystemInfo sysInfo;
        public CPUForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void CPUForm_Load(object sender, EventArgs e)
        {
            sysInfo = new SystemInfo();
            List<int> x = new List<int>() { 0, 1, 2, 3, 4 };
            List<int> y = new List<int>() { 5, 7, 25, 94, 3 };
            //chart1.Series.Add("_Total");
            //chart1.Series["Series1"].Points.DataBindXY(x, y);
            chart1.Series["Series1"].Points.DataBindY(y);
            chart1.Series["Series1"].IsVisibleInLegend = false;
            cpuName.Text = sysInfo.CpuName;
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
}
