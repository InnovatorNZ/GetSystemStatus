using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetSystemStatusGUI {
    public class SystemThemeHelper {
        public static bool IsDarkModeEnabled() {
            try {
                var value = Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    "AppsUseLightTheme", 1);
                return value is int i && i == 0;
            } catch {
                return false;
            }
        }
    }
}