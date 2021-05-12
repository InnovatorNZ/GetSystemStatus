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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.label1 = new System.Windows.Forms.Label();
			this.lblRAM = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(51, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(116, 52);
			this.label1.TabIndex = 0;
			this.label1.Text = "RAM";
			// 
			// lblRAM
			// 
			this.lblRAM.Font = new System.Drawing.Font("微软雅黑 Light", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.lblRAM.Location = new System.Drawing.Point(339, 68);
			this.lblRAM.Name = "lblRAM";
			this.lblRAM.Size = new System.Drawing.Size(425, 28);
			this.lblRAM.TabIndex = 1;
			this.lblRAM.Text = "label2";
			this.lblRAM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chart1
			// 
			chartArea1.AxisX.LabelStyle.Enabled = false;
			chartArea1.AxisX.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea1.AxisX.LineWidth = 2;
			chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.Thistle;
			chartArea1.AxisX.MajorTickMark.Enabled = false;
			chartArea1.AxisX.MinorGrid.Enabled = true;
			chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.Thistle;
			chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea1.AxisX2.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea1.AxisX2.LineWidth = 2;
			chartArea1.AxisX2.MajorGrid.Enabled = false;
			chartArea1.AxisX2.MajorTickMark.Enabled = false;
			chartArea1.AxisY.LabelStyle.Enabled = false;
			chartArea1.AxisY.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea1.AxisY.LineWidth = 2;
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Thistle;
			chartArea1.AxisY.MajorTickMark.Enabled = false;
			chartArea1.AxisY.Maximum = 100D;
			chartArea1.AxisY.Minimum = 0D;
			chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea1.AxisY2.LabelStyle.Enabled = false;
			chartArea1.AxisY2.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea1.AxisY2.LineWidth = 2;
			chartArea1.AxisY2.MajorGrid.Enabled = false;
			chartArea1.AxisY2.MajorTickMark.Enabled = false;
			chartArea1.AxisY2.MajorTickMark.LineColor = System.Drawing.Color.DarkMagenta;
			chartArea1.AxisY2.MajorTickMark.LineWidth = 2;
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Enabled = false;
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(60, 102);
			this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chart1.Name = "chart1";
			this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
			this.chart1.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.DarkMagenta};
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(704, 396);
			this.chart1.TabIndex = 2;
			this.chart1.Text = "chart1";
			title1.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
			title1.DockedToChartArea = "ChartArea1";
			title1.DockingOffset = 2;
			title1.ForeColor = System.Drawing.SystemColors.GrayText;
			title1.IsDockedInsideChartArea = false;
			title1.Name = "Title1";
			title1.Text = "Total Usage %";
			this.chart1.Titles.Add(title1);
			// 
			// RAMForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(821, 546);
			this.Controls.Add(this.lblRAM);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chart1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.MaximizeBox = false;
			this.Name = "RAMForm";
			this.Text = "RAM";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RAMForm_FormClosing);
			this.Load += new System.EventHandler(this.RAMForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblRAM;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
	}
}