using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TestGUI {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e) {
			Random rd = new Random();
			foreach (var control in this.Controls) {
				if (control is Chart) {
					Chart cChart = (Chart)control;
					for (int i = 1; i <= 6; i++) {
						cChart.Series[0].Points.AddY(rd.Next(0, 100));
					}
				}
			}
		}
	}
}