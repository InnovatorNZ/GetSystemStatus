
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
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CPUForm));
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
			this.label1.Location = new System.Drawing.Point(40, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(112, 57);
			this.label1.TabIndex = 0;
			this.label1.Text = "CPU";
			// 
			// cpuName
			// 
			this.cpuName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cpuName.BackColor = System.Drawing.Color.Transparent;
			this.cpuName.Font = new System.Drawing.Font("微软雅黑 Light", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cpuName.Location = new System.Drawing.Point(185, 57);
			this.cpuName.Name = "cpuName";
			this.cpuName.Size = new System.Drawing.Size(579, 39);
			this.cpuName.TabIndex = 1;
			this.cpuName.Text = "cpuName";
			this.cpuName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chart1
			// 
			this.chart1.BorderSkin.BorderColor = System.Drawing.Color.White;
			chartArea1.AxisX.LabelStyle.Enabled = false;
			chartArea1.AxisX.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea1.AxisX.LineWidth = 2;
			chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
			chartArea1.AxisX.MajorTickMark.Enabled = false;
			chartArea1.AxisX.MinorGrid.Enabled = true;
			chartArea1.AxisX.MinorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
			chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea1.AxisX2.LabelStyle.Enabled = false;
			chartArea1.AxisX2.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea1.AxisX2.LineWidth = 2;
			chartArea1.AxisX2.MajorGrid.Enabled = false;
			chartArea1.AxisX2.MajorTickMark.Enabled = false;
			chartArea1.AxisY.IsMarginVisible = false;
			chartArea1.AxisY.LabelStyle.Enabled = false;
			chartArea1.AxisY.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea1.AxisY.LineWidth = 2;
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(174)))), ((int)(((byte)(255)))));
			chartArea1.AxisY.MajorTickMark.LineColor = System.Drawing.Color.Transparent;
			chartArea1.AxisY.Maximum = 100D;
			chartArea1.AxisY.Minimum = 0D;
			chartArea1.AxisY.ScaleBreakStyle.LineColor = System.Drawing.Color.Transparent;
			chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
			chartArea1.AxisY2.LabelStyle.Enabled = false;
			chartArea1.AxisY2.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea1.AxisY2.LineWidth = 2;
			chartArea1.AxisY2.MajorGrid.Enabled = false;
			chartArea1.AxisY2.MajorTickMark.Enabled = false;
			chartArea1.BackColor = System.Drawing.Color.Transparent;
			chartArea1.BorderColor = System.Drawing.Color.Transparent;
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Enabled = false;
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(10, 120);
			this.chart1.Margin = new System.Windows.Forms.Padding(2);
			this.chart1.Name = "chart1";
			this.chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
			this.chart1.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.DodgerBlue};
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
			series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			series1.IsVisibleInLegend = false;
			series1.LabelForeColor = System.Drawing.Color.Silver;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			series1.YValuesPerPoint = 6;
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(754, 129);
			this.chart1.TabIndex = 2;
			title1.Alignment = System.Drawing.ContentAlignment.MiddleLeft;
			title1.DockedToChartArea = "ChartArea1";
			title1.ForeColor = System.Drawing.SystemColors.GrayText;
			title1.IsDockedInsideChartArea = false;
			title1.Name = "Title1";
			title1.Text = "Total Utilizations %";
			this.chart1.Titles.Add(title1);
			// 
			// CPUForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(795, 641);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cpuName);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CPUForm";
			this.ShowInTaskbar = false;
			this.Text = "CPU";
			this.Deactivate += new System.EventHandler(this.CPUForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CPUForm_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPUForm_FormClosed);
			this.Load += new System.EventHandler(this.CPUForm_Load);
			this.DpiChanged += new System.Windows.Forms.DpiChangedEventHandler(this.CPUForm_DpiChanged);
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