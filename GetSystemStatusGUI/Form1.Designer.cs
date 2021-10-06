
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
			this.components = new System.ComponentModel.Container();
			this.showCPU = new System.Windows.Forms.CheckBox();
			this.showRAM = new System.Windows.Forms.CheckBox();
			this.showDisk = new System.Windows.Forms.CheckBox();
			this.showNetwork = new System.Windows.Forms.CheckBox();
			this.showGPU = new System.Windows.Forms.CheckBox();
			this.buttonExit = new System.Windows.Forms.Button();
			this.cbUpdateInterval = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnDiskRefresh = new System.Windows.Forms.Button();
			this.btnNetworkRefresh = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveOpenedWindowLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadSavedLocationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadAtStartup = new System.Windows.Forms.ToolStripMenuItem();
			this.loadSizeAtStartup = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.doNotShowGPUAtStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showVirtualNetworkToolStrip = new System.Windows.Forms.ToolStripMenuItem();
			this.fakeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnFocus = new System.Windows.Forms.Button();
			this.btnFocusCPU = new System.Windows.Forms.Button();
			this.btnFocusRAM = new System.Windows.Forms.Button();
			this.btnFocusDisk = new System.Windows.Forms.Button();
			this.btnFocusNetwork = new System.Windows.Forms.Button();
			this.btnFocusGPU = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// showCPU
			// 
			this.showCPU.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showCPU.Location = new System.Drawing.Point(126, 89);
			this.showCPU.Name = "showCPU";
			this.showCPU.Size = new System.Drawing.Size(323, 24);
			this.showCPU.TabIndex = 0;
			this.showCPU.Text = "Show CPU Utilizations";
			this.showCPU.UseVisualStyleBackColor = true;
			this.showCPU.CheckedChanged += new System.EventHandler(this.showCPU_CheckedChanged);
			// 
			// showRAM
			// 
			this.showRAM.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showRAM.Location = new System.Drawing.Point(126, 118);
			this.showRAM.Name = "showRAM";
			this.showRAM.Size = new System.Drawing.Size(323, 24);
			this.showRAM.TabIndex = 1;
			this.showRAM.Text = "Show RAM Usage";
			this.showRAM.UseVisualStyleBackColor = true;
			this.showRAM.CheckedChanged += new System.EventHandler(this.showRAM_CheckedChanged);
			// 
			// showDisk
			// 
			this.showDisk.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showDisk.Location = new System.Drawing.Point(126, 148);
			this.showDisk.Name = "showDisk";
			this.showDisk.Size = new System.Drawing.Size(323, 24);
			this.showDisk.TabIndex = 2;
			this.showDisk.Text = "Show Disk Load and Transfer Speed";
			this.showDisk.UseVisualStyleBackColor = true;
			this.showDisk.CheckedChanged += new System.EventHandler(this.showDisk_CheckedChanged);
			// 
			// showNetwork
			// 
			this.showNetwork.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showNetwork.Location = new System.Drawing.Point(126, 178);
			this.showNetwork.Name = "showNetwork";
			this.showNetwork.Size = new System.Drawing.Size(405, 24);
			this.showNetwork.TabIndex = 3;
			this.showNetwork.Text = "Show Network and Adapter Speed";
			this.showNetwork.UseVisualStyleBackColor = true;
			this.showNetwork.CheckedChanged += new System.EventHandler(this.showNetwork_CheckedChanged);
			// 
			// showGPU
			// 
			this.showGPU.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.showGPU.Location = new System.Drawing.Point(126, 209);
			this.showGPU.Name = "showGPU";
			this.showGPU.Size = new System.Drawing.Size(405, 24);
			this.showGPU.TabIndex = 4;
			this.showGPU.Text = "Show GPU Utilizations";
			this.showGPU.UseVisualStyleBackColor = true;
			this.showGPU.CheckedChanged += new System.EventHandler(this.showGPU_CheckedChanged);
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
			// cbUpdateInterval
			// 
			this.cbUpdateInterval.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.cbUpdateInterval.FormattingEnabled = true;
			this.cbUpdateInterval.ImeMode = System.Windows.Forms.ImeMode.Disable;
			this.cbUpdateInterval.Items.AddRange(new object[] {
            "1 sec",
            "2 sec",
            "0.5 sec",
            "0.25 sec"});
			this.cbUpdateInterval.Location = new System.Drawing.Point(248, 249);
			this.cbUpdateInterval.Margin = new System.Windows.Forms.Padding(2);
			this.cbUpdateInterval.Name = "cbUpdateInterval";
			this.cbUpdateInterval.Size = new System.Drawing.Size(98, 25);
			this.cbUpdateInterval.TabIndex = 6;
			this.cbUpdateInterval.Text = "1 sec";
			this.cbUpdateInterval.SelectedIndexChanged += new System.EventHandler(this.cbUpdateInterval_SelectedIndexChanged);
			this.cbUpdateInterval.TextChanged += new System.EventHandler(this.cbUpdateInterval_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.label1.Location = new System.Drawing.Point(121, 251);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 20);
			this.label1.TabIndex = 7;
			this.label1.Text = "Update interval";
			// 
			// btnDiskRefresh
			// 
			this.btnDiskRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnDiskRefresh.BackgroundImage = global::GetSystemStatusGUI.Properties.Resources.restart_icon;
			this.btnDiskRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnDiskRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnDiskRefresh.Location = new System.Drawing.Point(423, 145);
			this.btnDiskRefresh.Margin = new System.Windows.Forms.Padding(2);
			this.btnDiskRefresh.Name = "btnDiskRefresh";
			this.btnDiskRefresh.Size = new System.Drawing.Size(26, 27);
			this.btnDiskRefresh.TabIndex = 8;
			this.btnDiskRefresh.UseVisualStyleBackColor = true;
			this.btnDiskRefresh.Click += new System.EventHandler(this.btnDiskRefresh_Click);
			this.btnDiskRefresh.MouseEnter += new System.EventHandler(this.btnDiskRefresh_MouseEnter);
			this.btnDiskRefresh.MouseLeave += new System.EventHandler(this.btnDiskRefresh_MouseLeave);
			// 
			// btnNetworkRefresh
			// 
			this.btnNetworkRefresh.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnNetworkRefresh.BackgroundImage = global::GetSystemStatusGUI.Properties.Resources.restart_icon;
			this.btnNetworkRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnNetworkRefresh.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNetworkRefresh.Location = new System.Drawing.Point(423, 177);
			this.btnNetworkRefresh.Name = "btnNetworkRefresh";
			this.btnNetworkRefresh.Size = new System.Drawing.Size(26, 27);
			this.btnNetworkRefresh.TabIndex = 9;
			this.btnNetworkRefresh.UseVisualStyleBackColor = true;
			this.btnNetworkRefresh.Click += new System.EventHandler(this.btnNetworkRefresh_Click);
			this.btnNetworkRefresh.MouseEnter += new System.EventHandler(this.btnNetworkRefresh_MouseEnter);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.menuStrip1.AutoSize = false;
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(544, 25);
			this.menuStrip1.TabIndex = 10;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.AutoSize = false;
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveOpenedWindowLocationsToolStripMenuItem,
            this.loadSavedLocationsToolStripMenuItem,
            this.loadAtStartup,
            this.loadSizeAtStartup,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// saveOpenedWindowLocationsToolStripMenuItem
			// 
			this.saveOpenedWindowLocationsToolStripMenuItem.Name = "saveOpenedWindowLocationsToolStripMenuItem";
			this.saveOpenedWindowLocationsToolStripMenuItem.ShowShortcutKeys = false;
			this.saveOpenedWindowLocationsToolStripMenuItem.Size = new System.Drawing.Size(312, 22);
			this.saveOpenedWindowLocationsToolStripMenuItem.Text = "Save opened window locations and sizes";
			this.saveOpenedWindowLocationsToolStripMenuItem.Click += new System.EventHandler(this.saveOpenedWindowLocationsToolStripMenuItem_Click);
			// 
			// loadSavedLocationsToolStripMenuItem
			// 
			this.loadSavedLocationsToolStripMenuItem.Name = "loadSavedLocationsToolStripMenuItem";
			this.loadSavedLocationsToolStripMenuItem.Size = new System.Drawing.Size(312, 22);
			this.loadSavedLocationsToolStripMenuItem.Text = "Load saved locations and sizes manually";
			this.loadSavedLocationsToolStripMenuItem.Click += new System.EventHandler(this.loadSavedLocationsToolStripMenuItem_Click);
			// 
			// loadAtStartup
			// 
			this.loadAtStartup.Checked = true;
			this.loadAtStartup.CheckOnClick = true;
			this.loadAtStartup.CheckState = System.Windows.Forms.CheckState.Checked;
			this.loadAtStartup.Name = "loadAtStartup";
			this.loadAtStartup.Size = new System.Drawing.Size(312, 22);
			this.loadAtStartup.Text = "Load saved locations at startup";
			this.loadAtStartup.Click += new System.EventHandler(this.loadAtStartup_Click);
			// 
			// loadSizeAtStartup
			// 
			this.loadSizeAtStartup.CheckOnClick = true;
			this.loadSizeAtStartup.Name = "loadSizeAtStartup";
			this.loadSizeAtStartup.Size = new System.Drawing.Size(312, 22);
			this.loadSizeAtStartup.Text = "Load saved sizes at startup";
			this.loadSizeAtStartup.Click += new System.EventHandler(this.loadSizeAtStartup_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(309, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(312, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// optionToolStripMenuItem
			// 
			this.optionToolStripMenuItem.AutoSize = false;
			this.optionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.doNotShowGPUAtStartToolStripMenuItem,
            this.showVirtualNetworkToolStrip,
            this.fakeToolStripMenuItem});
			this.optionToolStripMenuItem.Name = "optionToolStripMenuItem";
			this.optionToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
			this.optionToolStripMenuItem.Text = "Option";
			// 
			// doNotShowGPUAtStartToolStripMenuItem
			// 
			this.doNotShowGPUAtStartToolStripMenuItem.CheckOnClick = true;
			this.doNotShowGPUAtStartToolStripMenuItem.Name = "doNotShowGPUAtStartToolStripMenuItem";
			this.doNotShowGPUAtStartToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
			this.doNotShowGPUAtStartToolStripMenuItem.Text = "Do not show GPU at start";
			this.doNotShowGPUAtStartToolStripMenuItem.Click += new System.EventHandler(this.doNotShowGPUAtStartToolStripMenuItem_Click);
			// 
			// showVirtualNetworkToolStrip
			// 
			this.showVirtualNetworkToolStrip.CheckOnClick = true;
			this.showVirtualNetworkToolStrip.Name = "showVirtualNetworkToolStrip";
			this.showVirtualNetworkToolStrip.Size = new System.Drawing.Size(253, 22);
			this.showVirtualNetworkToolStrip.Text = "Show virtual network adapters";
			this.showVirtualNetworkToolStrip.Click += new System.EventHandler(this.showVirtualNetworkToolStrip_Click);
			// 
			// fakeToolStripMenuItem
			// 
			this.fakeToolStripMenuItem.Name = "fakeToolStripMenuItem";
			this.fakeToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
			this.fakeToolStripMenuItem.Text = "Fake CPU cores...";
			this.fakeToolStripMenuItem.Click += new System.EventHandler(this.fakeToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.AutoSize = false;
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(47, 21);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// btnFocus
			// 
			this.btnFocus.Location = new System.Drawing.Point(358, 324);
			this.btnFocus.Margin = new System.Windows.Forms.Padding(2);
			this.btnFocus.Name = "btnFocus";
			this.btnFocus.Size = new System.Drawing.Size(88, 23);
			this.btnFocus.TabIndex = 11;
			this.btnFocus.Text = "Focus all";
			this.btnFocus.UseVisualStyleBackColor = true;
			this.btnFocus.Click += new System.EventHandler(this.btnFocus_Click);
			// 
			// btnFocusCPU
			// 
			this.btnFocusCPU.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFocusCPU.Location = new System.Drawing.Point(94, 91);
			this.btnFocusCPU.Name = "btnFocusCPU";
			this.btnFocusCPU.Size = new System.Drawing.Size(20, 20);
			this.btnFocusCPU.TabIndex = 12;
			this.btnFocusCPU.UseVisualStyleBackColor = true;
			this.btnFocusCPU.Click += new System.EventHandler(this.btnFocusCPU_Click);
			// 
			// btnFocusRAM
			// 
			this.btnFocusRAM.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFocusRAM.Location = new System.Drawing.Point(94, 120);
			this.btnFocusRAM.Name = "btnFocusRAM";
			this.btnFocusRAM.Size = new System.Drawing.Size(20, 20);
			this.btnFocusRAM.TabIndex = 13;
			this.btnFocusRAM.UseVisualStyleBackColor = true;
			this.btnFocusRAM.Click += new System.EventHandler(this.btnFocusRAM_Click);
			// 
			// btnFocusDisk
			// 
			this.btnFocusDisk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFocusDisk.Location = new System.Drawing.Point(94, 150);
			this.btnFocusDisk.Name = "btnFocusDisk";
			this.btnFocusDisk.Size = new System.Drawing.Size(20, 20);
			this.btnFocusDisk.TabIndex = 14;
			this.btnFocusDisk.UseVisualStyleBackColor = true;
			this.btnFocusDisk.Click += new System.EventHandler(this.btnFocusDisk_Click);
			// 
			// btnFocusNetwork
			// 
			this.btnFocusNetwork.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFocusNetwork.Location = new System.Drawing.Point(94, 180);
			this.btnFocusNetwork.Name = "btnFocusNetwork";
			this.btnFocusNetwork.Size = new System.Drawing.Size(20, 20);
			this.btnFocusNetwork.TabIndex = 15;
			this.btnFocusNetwork.UseVisualStyleBackColor = true;
			this.btnFocusNetwork.Click += new System.EventHandler(this.btnFocusNetwork_Click);
			// 
			// btnFocusGPU
			// 
			this.btnFocusGPU.Enabled = false;
			this.btnFocusGPU.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnFocusGPU.Location = new System.Drawing.Point(94, 211);
			this.btnFocusGPU.Name = "btnFocusGPU";
			this.btnFocusGPU.Size = new System.Drawing.Size(20, 20);
			this.btnFocusGPU.TabIndex = 16;
			this.btnFocusGPU.UseVisualStyleBackColor = true;
			this.btnFocusGPU.Click += new System.EventHandler(this.btnFocusGPU_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(544, 359);
			this.Controls.Add(this.btnFocusGPU);
			this.Controls.Add(this.btnFocusNetwork);
			this.Controls.Add(this.btnFocusDisk);
			this.Controls.Add(this.btnFocusRAM);
			this.Controls.Add(this.btnFocusCPU);
			this.Controls.Add(this.btnFocus);
			this.Controls.Add(this.btnNetworkRefresh);
			this.Controls.Add(this.btnDiskRefresh);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbUpdateInterval);
			this.Controls.Add(this.buttonExit);
			this.Controls.Add(this.showGPU);
			this.Controls.Add(this.showNetwork);
			this.Controls.Add(this.showDisk);
			this.Controls.Add(this.showRAM);
			this.Controls.Add(this.showCPU);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "GetSystemStatus";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.DpiChanged += new System.Windows.Forms.DpiChangedEventHandler(this.Form1_DpiChanged);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
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
		private System.Windows.Forms.ComboBox cbUpdateInterval;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnDiskRefresh;
        private System.Windows.Forms.Button btnNetworkRefresh;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ToolStripMenuItem saveOpenedWindowLocationsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadSavedLocationsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem doNotShowGPUAtStartToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadAtStartup;
		private System.Windows.Forms.ToolStripMenuItem loadSizeAtStartup;
		private System.Windows.Forms.Button btnFocus;
		private System.Windows.Forms.ToolStripMenuItem fakeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showVirtualNetworkToolStrip;
		private System.Windows.Forms.Button btnFocusCPU;
		private System.Windows.Forms.Button btnFocusRAM;
		private System.Windows.Forms.Button btnFocusDisk;
		private System.Windows.Forms.Button btnFocusNetwork;
		private System.Windows.Forms.Button btnFocusGPU;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
	}
}

