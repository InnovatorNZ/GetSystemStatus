namespace GetSystemStatusGUI {
	public static class Global {
		public static int interval_ms = 1000;
		public static int history_length = 60;
		public static int refresh_gpupc_interval = 5;

		public static bool enableAffinity = true;
		public static int doNotUseFirstCores = 16;
	}
}