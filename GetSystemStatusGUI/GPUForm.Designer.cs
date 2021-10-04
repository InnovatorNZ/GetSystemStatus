
namespace GetSystemStatusGUI {
	partial class GPUForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPUForm));
			this.label1 = new System.Windows.Forms.Label();
			this.chartGPU = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.lblGPUName = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.chartGPU)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 24F);
			this.label1.Location = new System.Drawing.Point(31, 34);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 41);
			this.label1.TabIndex = 0;
			this.label1.Text = "GPU";
			// 
			// chartGPU
			// 
			this.chartGPU.Location = new System.Drawing.Point(11, 106);
			this.chartGPU.Margin = new System.Windows.Forms.Padding(2);
			this.chartGPU.Name = "chartGPU";
			this.chartGPU.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
			this.chartGPU.Size = new System.Drawing.Size(725, 428);
			this.chartGPU.TabIndex = 1;
			this.chartGPU.Text = "GPUChart";
			// 
			// lblGPUName
			// 
			this.lblGPUName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblGPUName.BackColor = System.Drawing.Color.Transparent;
			this.lblGPUName.Font = new System.Drawing.Font("微软雅黑 Light", 15F);
			this.lblGPUName.Location = new System.Drawing.Point(121, 44);
			this.lblGPUName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblGPUName.Name = "lblGPUName";
			this.lblGPUName.Size = new System.Drawing.Size(608, 31);
			this.lblGPUName.TabIndex = 2;
			this.lblGPUName.Text = "GPU Name";
			this.lblGPUName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// GPUForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(759, 560);
			this.Controls.Add(this.chartGPU);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblGPUName);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "GPUForm";
			this.ShowInTaskbar = false;
			this.Text = "GPU";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GPUForm_FormClosing);
			this.Load += new System.EventHandler(this.GPUForm_Load);
			this.DpiChanged += new System.Windows.Forms.DpiChangedEventHandler(this.GPUForm_DpiChanged);
			this.Resize += new System.EventHandler(this.GPUForm_Resize);
			((System.ComponentModel.ISupportInitialize)(this.chartGPU)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartGPU;
		private System.Windows.Forms.Label lblGPUName;
	}
}