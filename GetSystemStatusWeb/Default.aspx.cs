using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Diagnostics;

namespace GetSystemStatusWeb {
	public partial class _Default : Page {
		protected void Page_Load(object sender, EventArgs e) {
			Chart1.Series[0].ChartType = SeriesChartType.SplineArea;
			List<int> y = new List<int> { 0, 1, 2, 3, 2 };
			Chart1.Series[0].Points.DataBindY(y);
			for (int i = 0; i < 2; i++) {
				Chart chart = new Chart();
				for (int j = 0; j < 2; j++) {
					chart.Series.Add(j.ToString());
					chart.ChartAreas.Add(j.ToString());
					chart.Series[j].Points.DataBindY(y);
					chart.Series[j].ChartArea = j.ToString();
				}
				//this.Controls.Add(chart);
				chartrow.Controls.Add(chart);
			}

			for (int i = 0; i < 3; i++) {
				Panel pnl = new Panel() { ID = "div" + i, CssClass = "input-group" }; //div
				pnl.Style["margin"] = "10px";
				Chart chart1 = new Chart();
				chart1.Series.Add("0");
				chart1.ChartAreas.Add("0");
				chart1.Series[0].Points.DataBindY(y);
				chart1.Series[0].ChartType = SeriesChartType.Bubble;
				pnl.Controls.Add(chart1);
				morechartrow.Controls.Add(pnl);
			}

		}
	}
}