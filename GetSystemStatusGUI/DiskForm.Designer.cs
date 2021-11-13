namespace GetSystemStatusGUI {
	partial class DiskForm {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiskForm));
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 27F);
			this.label1.Location = new System.Drawing.Point(38, 38);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 46);
			this.label1.TabIndex = 0;
			this.label1.Text = "Disk";
			// 
			// DiskForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(662, 467);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "DiskForm";
			this.ShowInTaskbar = false;
			this.Text = "Disks";
			this.Activated += new System.EventHandler(this.DiskForm_Activated);
			this.Deactivate += new System.EventHandler(this.DiskForm_Deactivate);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiskForm_FormClosing);
			this.Load += new System.EventHandler(this.DiskForm_Load);
			this.DpiChanged += new System.Windows.Forms.DpiChangedEventHandler(this.DiskForm_DpiChanged);
			this.Resize += new System.EventHandler(this.DiskForm_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
	}
}