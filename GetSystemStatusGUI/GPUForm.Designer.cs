
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
			this.label1 = new System.Windows.Forms.Label();
			this.chartGPU = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.lblGPUName = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.chartGPU)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 27F);
			this.label1.Location = new System.Drawing.Point(41, 42);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 58);
			this.label1.TabIndex = 0;
			this.label1.Text = "GPU";
			// 
			// chartGPU
			// 
			this.chartGPU.Location = new System.Drawing.Point(51, 133);
			this.chartGPU.Name = "chartGPU";
			this.chartGPU.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
			this.chartGPU.Size = new System.Drawing.Size(1004, 445);
			this.chartGPU.TabIndex = 1;
			this.chartGPU.Text = "GPUChart";
			// 
			// lblGPUName
			// 
			this.lblGPUName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblGPUName.Font = new System.Drawing.Font("微软雅黑 Light", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.lblGPUName.Location = new System.Drawing.Point(244, 61);
			this.lblGPUName.Name = "lblGPUName";
			this.lblGPUName.Size = new System.Drawing.Size(811, 39);
			this.lblGPUName.TabIndex = 2;
			this.lblGPUName.Text = "GPU Name";
			this.lblGPUName.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// GPUForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(1105, 644);
			this.Controls.Add(this.lblGPUName);
			this.Controls.Add(this.chartGPU);
			this.Controls.Add(this.label1);
			this.Name = "GPUForm";
			this.Text = "GPUForm";
			this.Load += new System.EventHandler(this.GPUForm_Load);
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