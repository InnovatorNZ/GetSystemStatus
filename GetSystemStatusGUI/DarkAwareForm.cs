using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSystemStatusGUI {
    public class DarkAwareForm : Form {
        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttribute, int cbAttribute);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            SetImmersiveDarkMode(Global.IsDarkMode);
        }

        public void SetImmersiveDarkMode(bool enabled) {
            if (Environment.OSVersion.Version.Major >= 10) {
                int attribute = Environment.OSVersion.Version.Build >= 18985
                    ? DWMWA_USE_IMMERSIVE_DARK_MODE
                    : DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;

                // 启用深色标题栏
                int value = enabled ? 1 : 0;
                DwmSetWindowAttribute(this.Handle, attribute, ref value, sizeof(int));
            }
        }
    }
}