
namespace GetSystemStatusGUI
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
			this.showCPU = new System.Windows.Forms.CheckBox();
			this.showRAM = new System.Windows.Forms.CheckBox();
			this.showDisk = new System.Windows.Forms.CheckBox();
			this.showNetwork = new System.Windows.Forms.CheckBox();
			this.showGPU = new System.Windows.Forms.CheckBox();
			this.buttonExit = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// showCPU
			// 
			this.showCPU.AutoSize = true;
			this.showCPU.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showCPU.Location = new System.Drawing.Point(105, 102);
			this.showCPU.Name = "showCPU";
			this.showCPU.Size = new System.Drawing.Size(186, 24);
			this.showCPU.TabIndex = 0;
			this.showCPU.Text = "Show CPU Utilizations";
			this.showCPU.UseVisualStyleBackColor = true;
			this.showCPU.CheckedChanged += new System.EventHandler(this.showCPU_CheckedChanged);
			// 
			// showRAM
			// 
			this.showRAM.AutoSize = true;
			this.showRAM.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showRAM.Location = new System.Drawing.Point(105, 132);
			this.showRAM.Name = "showRAM";
			this.showRAM.Size = new System.Drawing.Size(157, 24);
			this.showRAM.TabIndex = 1;
			this.showRAM.Text = "Show RAM Usage";
			this.showRAM.UseVisualStyleBackColor = true;
			this.showRAM.CheckedChanged += new System.EventHandler(this.showRAM_CheckedChanged);
			// 
			// showDisk
			// 
			this.showDisk.AutoSize = true;
			this.showDisk.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showDisk.Location = new System.Drawing.Point(105, 162);
			this.showDisk.Name = "showDisk";
			this.showDisk.Size = new System.Drawing.Size(331, 24);
			this.showDisk.TabIndex = 2;
			this.showDisk.Text = "Show Disk Utilizations and Transfer Speed";
			this.showDisk.UseVisualStyleBackColor = true;
			// 
			// showNetwork
			// 
			this.showNetwork.AutoSize = true;
			this.showNetwork.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showNetwork.Location = new System.Drawing.Point(105, 192);
			this.showNetwork.Name = "showNetwork";
			this.showNetwork.Size = new System.Drawing.Size(361, 24);
			this.showNetwork.TabIndex = 3;
			this.showNetwork.Text = "Show Network Speed and Adapter Properties";
			this.showNetwork.UseVisualStyleBackColor = true;
			// 
			// showGPU
			// 
			this.showGPU.AutoSize = true;
			this.showGPU.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showGPU.Location = new System.Drawing.Point(105, 222);
			this.showGPU.Name = "showGPU";
			this.showGPU.Size = new System.Drawing.Size(192, 24);
			this.showGPU.TabIndex = 4;
			this.showGPU.Text = "Show GPU Information";
			this.showGPU.UseVisualStyleBackColor = true;
			// 
			// buttonExit
			// 
			this.buttonExit.Location = new System.Drawing.Point(451, 324);
			this.buttonExit.Name = "buttonExit";
			this.buttonExit.Size = new System.Drawing.Size(80, 23);
			this.buttonExit.TabIndex = 5;
			this.buttonExit.Text = "Exit";
			this.buttonExit.UseVisualStyleBackColor = true;
			this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(544, 359);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.showGPU);
			this.Controls.Add(this.showNetwork);
			this.Controls.Add(this.showDisk);
			this.Controls.Add(this.showRAM);
			this.Controls.Add(this.showCPU);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Control Console";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox showCPU;
        private System.Windows.Forms.CheckBox showRAM;
        private System.Windows.Forms.CheckBox showDisk;
        private System.Windows.Forms.CheckBox showNetwork;
        private System.Windows.Forms.CheckBox showGPU;
        private System.Windows.Forms.Button buttonExit;
    }
}

