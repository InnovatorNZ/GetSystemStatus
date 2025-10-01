namespace GetSystemStatusGUI {
	public static class Global {
		public static int interval_ms = 1000;
		public static int history_length = 60;
		public static int refresh_gpupc_interval = 5;

		public static bool IsDarkMode = SystemThemeHelper.IsDarkModeEnabled();
		public const bool renderAllSubtitleLightGray = true;

		public const bool enableAffinity = true;
		public const int doNotUseFirstCores = 16;

		// 自适应采集间隔的配置参数
		public const bool enableAdaptiveInterval = true;
		public const int MIN_INTERVAL_MS = 750;             // 最小采集间隔（毫秒）
		public const int INTERVAL_INCREMENT_MS = 250;       // 间隔增加量（毫秒）
		public const float CHANGE_THRESHOLD = 5.0f;         // 变化阈值（百分比）
		public const float CHANGE_THRESHOLD_NETWORK = 2.5f; // 变化阈值（网络，百分比）
		public const float CHANGE_THRESHOLD_GPU = 12.5f;    // 变化阈值（GPU，百分比）
		public const float IDLE_THRESHOLD_DISK = 10.0f;     // 闲置阈值（磁盘，百分比）
		public const float IDLE_THRESHOLD_NETWORK = 5.0f;   // 闲置阈值（网络，百分比）
		public const float IDLE_THRESHOLD_GPU = 40.0f;      // 闲置阈值（GPU，百分比）
	}
}