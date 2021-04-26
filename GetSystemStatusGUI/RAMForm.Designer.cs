namespace GetSystemStatusGUI {
	partial class RAMForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.label1 = new System.Windows.Forms.Label();
			this.lblRamTotal = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(51, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 52);
			this.label1.TabIndex = 0;
			this.label1.Text = "RAM";
			// 
			// lblRamTotal
			// 
			this.lblRamTotal.Font = new System.Drawing.Font("微软雅黑 Light", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.lblRamTotal.Location = new System.Drawing.Point(339, 67);
			this.lblRamTotal.Name = "lblRamTotal";
			this.lblRamTotal.Size = new System.Drawing.Size(425, 27);
			this.lblRamTotal.TabIndex = 1;
			this.lblRamTotal.Text = "label2";
			this.lblRamTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chart1
			// 
			chartArea2.AxisX.LabelStyle.Enabled = false;
			chartArea2.AxisX.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea2.AxisX.LineWidth = 2;
			chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.Thistle;
			chartArea2.AxisX.MajorTickMark.Enabled = false;
			chartArea2.AxisX.MinorGrid.Enabled = true;
			chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.Thistle;
			chartArea2.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea2.AxisX2.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea2.AxisX2.LineWidth = 2;
			chartArea2.AxisX2.MajorGrid.Enabled = false;
			chartArea2.AxisX2.MajorTickMark.Enabled = false;
			chartArea2.AxisY.LabelStyle.Enabled = false;
			chartArea2.AxisY.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea2.AxisY.LineWidth = 2;
			chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.Thistle;
			chartArea2.AxisY.MajorTickMark.Enabled = false;
			chartArea2.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea2.AxisY2.LabelStyle.Enabled = false;
			chartArea2.AxisY2.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea2.AxisY2.LineWidth = 2;
			chartArea2.AxisY2.MajorGrid.Enabled = false;
			chartArea2.AxisY2.MajorTickMark.Enabled = false;
			chartArea2.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea2.AxisY2.MajorTickMark.LineWidth = 2;
			chartArea2.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea2);
			legend2.Enabled = false;
			legend2.Name = "Legend1";
			this.chart1.Legends.Add(legend2);
			this.chart1.Location = new System.Drawing.Point(60, 102);
			this.chart1.Name = "chart1";
			this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
			this.chart1.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.DarkMagenta};
			series2.ChartArea = "ChartArea1";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
			series2.Legend = "Legend1";
			series2.Name = "Series1";
			this.chart1.Series.Add(series2);
			this.chart1.Size = new System.Drawing.Size(704, 396);
			this.chart1.TabIndex = 2;
			this.chart1.Text = "chart1";
			// 
			// RAMForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(822, 546);
			this.Controls.Add(this.lblRamTotal);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chart1);
			this.Name = "RAMForm";
			this.Text = "RAMForm";
			this.Load += new System.EventHandler(this.RAMForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblRamTotal;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
	}
}