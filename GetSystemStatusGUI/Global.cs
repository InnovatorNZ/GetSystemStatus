namespace GetSystemStatusGUI {
	public static class Global {
		public static int interval_ms = 1000;
		public static int history_length = 60;
		public static int refresh_gpupc_interval = 5;

		// 主题设置：深色模式
		public static bool IsDarkMode = true;

		public static bool enableAffinity = true;
		public static int doNotUseFirstCores = 16;
	}
}