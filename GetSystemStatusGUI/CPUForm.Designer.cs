
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
			this.cpuName.Font = new System.Drawing.Font("微软雅黑 Light", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
			chartArea1.AxisX.LabelStyle.Enabled = false;
			chartArea1.AxisX.LineColor = System.Drawing.Color.DodgerBlue;
			chartArea1.AxisX.LineWidth = 2;
			chartArea1.AxisX.MajorGrid.Enabled = false;
			chartArea1.AxisX.MajorTickMark.Enabled = false;
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
			chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.Transparent;
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
			chartArea1.BorderColor = System.Drawing.Color.Transparent;
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(12, 150);
			this.chart1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
			series1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
			series1.IsVisibleInLegend = false;
			series1.LabelForeColor = System.Drawing.Color.Silver;
			series1.Legend = "Legend1";
			series1.Name = "Series1";
			series1.YValuesPerPoint = 6;
			this.chart1.Series.Add(series1);
			this.chart1.Size = new System.Drawing.Size(1059, 161);
			this.chart1.TabIndex = 2;
			// 
			// CPUForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1083, 801);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.cpuName);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4);
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