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
        public CPUForm(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void CPUForm_Load(object sender, EventArgs e)
        {

        }

        private void CPUForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.DisableChecked("CPU");
        }

    }
}
