
namespace GetSystemStatusGUI
{
    partial class CPUForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
			this.label1 = new System.Windows.Forms.Label();
			this.cpuName = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 32.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(50, 50);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(139, 70);
			this.label1.TabIndex = 0;
			this.label1.Text = "CPU";
			// 
			// cpuName
			// 
			this.cpuName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cpuName.Font = new System.Drawing.Font("微软雅黑 Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cpuName.Location = new System.Drawing.Point(320, 71);
			this.cpuName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.cpuName.Name = "cpuName";
			this.cpuName.Size = new System.Drawing.Size(732, 49);
			this.cpuName.TabIndex = 1;
			this.cpuName.Text = "cpuName";
			this.cpuName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chart1
			// 
			this.chart1.BorderSkin.BorderColor = System.Drawing.Color.White;
			chartArea2.AxisX.LabelStyle.Enabled = false;
			chartArea2.AxisX.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea2.AxisX.LineWidth = 2;
			chartArea2.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
			chartArea2.AxisX.MajorTickMark.Enabled = false;
			chartArea2.AxisX.MinorGrid.Enabled = true;
			chartArea2.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
			chartArea2.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea2.AxisX2.LabelStyle.Enabled = false;
			chartArea2.AxisX2.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea2.AxisX2.LineWidth = 2;
			chartArea2.AxisX2.MajorGrid.Enabled = false;
			chartArea2.AxisX2.MajorTickMark.Enabled = false;
			chartArea2.AxisY.IsMarginVisible = false;
			chartArea2.AxisY.LabelStyle.Enabled = false;
			chartArea2.AxisY.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea2.AxisY.LineWidth = 2;
			chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
			chartArea2.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
			chartArea2.AxisY.Maximum = 100D;
			chartArea2.AxisY.Minimum = 0D;
			chartArea2.AxisY.ScaleBreakStyle.LineColor = System.Drawing.Color.Transparent;
			chartArea2.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea2.AxisY2.LabelStyle.Enabled = false;
			chartArea2.AxisY2.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea2.AxisY2.LineWidth = 2;
			chartArea2.AxisY2.MajorGrid.Enabled = false;
			chartArea2.AxisY2.MajorTickMark.Enabled = false;
			chartArea2.BackColor = System.Drawing.Color.Transparent;
			chartArea2.BorderColor = System.Drawing.Color.Transparent;
			chartArea2.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea2);
			legend2.Enabled = false;
			legend2.Name = "Legend1";
			this.chart1.Legends.Add(legend2);
			this.chart1.Location = new System.Drawing.Point(12, 150);
			this.chart1.Margin = new System.Windows.Forms.Padding(2);
			this.chart1.Name = "chart1";
			this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
			this.chart1.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.DodgerBlue};
			series2.ChartArea = "ChartArea1";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
			series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			series2.IsVisibleInLegend = false;
			series2.LabelForeColor = System.Drawing.Color.Silver;
			series2.Legend = "Legend1";
			series2.Name = "Series1";
			series2.YValuesPerPoint = 6;
			this.chart1.Series.Add(series2);
			this.chart1.Size = new System.Drawing.Size(1059, 161);
			this.chart1.TabIndex = 2;
			title2.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
			title2.DockedToChartArea = "ChartArea1";
			title2.ForeColor = System.Drawing.SystemColors.GrayText;
			title2.IsDockedInsideChartArea = false;
			title2.Name = "Title1";
			title2.Text = "Total Utilizations in 60 secs %";
			this.chart1.Titles.Add(title2);
			// 
			// CPUForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1082, 801);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.cpuName);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "CPUForm";
			this.Text = "CPU";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CPUForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPUForm_FormClosed);
			this.Load += new System.EventHandler(this.CPUForm_Load);
			this.Resize += new System.EventHandler(this.CPUForm_Resize);
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label cpuName;
		private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
	}
}