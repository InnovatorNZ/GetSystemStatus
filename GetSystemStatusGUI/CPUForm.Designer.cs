
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
            this.label1.Location = new System.Drawing.Point(39, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 57);
            this.label1.TabIndex = 0;
            this.label1.Text = "CPU";
            // 
            // cpuName
            // 
            this.cpuName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cpuName.Font = new System.Drawing.Font("微软雅黑 Light", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cpuName.Location = new System.Drawing.Point(256, 56);
            this.cpuName.Name = "cpuName";
            this.cpuName.Size = new System.Drawing.Size(506, 39);
            this.cpuName.TabIndex = 1;
            this.cpuName.Text = "cpuName";
            this.cpuName.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chart1
            // 
            chartArea2.BorderColor = System.Drawing.Color.DimGray;
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(73, 134);
            this.chart1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.SplineArea;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series2.LabelForeColor = System.Drawing.Color.Silver;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            series2.YValuesPerPoint = 6;
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(659, 386);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "CPU";
            // 
            // CPUForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(812, 600);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.cpuName);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "CPUForm";
            this.Text = "CPU";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CPUForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CPUForm_FormClosed);
            this.Load += new System.EventHandler(this.CPUForm_Load);
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