using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GetSystemStatusGUI {
    public class DarkAwareForm : Form {
        public virtual void ApplyDarkMode() {
            if (Global.IsDarkMode) {

                Color backColor = Color.FromArgb(32, 32, 32);
                Color foreColor = Color.WhiteSmoke;

                this.BackColor = backColor;
                this.ForeColor = foreColor;

                ApplyThemeToControls(backColor, foreColor);
            }

            SetWindowTitleDarkMode(Global.IsDarkMode);
        }

        private void ApplyThemeToControls(Color backColor, Color foreColor) {
            ApplyThemeToControls(this.Controls, backColor, foreColor);
        }

        protected virtual void ApplyThemeToControls(Control.ControlCollection controls, Color backColor, Color foreColor) {
            foreach (Control ctrl in controls) {
                ctrl.BackColor = backColor;
                ctrl.ForeColor = foreColor;

                if (ctrl is Chart chart) {
                    foreach (var chartarea in chart.ChartAreas) {
                        chartarea.BackColor = backColor;
                    }
                    foreach (var title in chart.Titles) {
                        if (Global.renderAllSubtitleLightGray || title.ForeColor == SystemColors.GrayText) {
                            title.ForeColor = Color.LightGray;
                        } else {
                            title.ForeColor = Color.White;
                        }
                    }
                } else if (ctrl is ToolStrip strip) {
                    RenderToolStrip(strip, foreColor);
                } else if (ctrl is Button || ctrl is CheckBox || ctrl is ComboBox || ctrl is Label || ctrl is GroupBox || ctrl is ListBox || ctrl is TextBox) {
                    ctrl.BackColor = backColor;
                    ctrl.ForeColor = foreColor;
                } else if (ctrl is Panel || ctrl is TabControl || ctrl is TabPage) {
                    ctrl.BackColor = backColor;
                    ctrl.ForeColor = foreColor;
                }

                if (ctrl.HasChildren) {
                    ApplyThemeToControls(ctrl.Controls, backColor, foreColor);
                }
            }
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int pvAttribute, int cbAttribute);

        public void SetWindowTitleDarkMode(bool enabled) {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;

            if (Environment.OSVersion.Version.Major >= 10) {
                int attribute = Environment.OSVersion.Version.Build >= 18985
                    ? DWMWA_USE_IMMERSIVE_DARK_MODE
                    : DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;

                int value = enabled ? 1 : 0;
                DwmSetWindowAttribute(this.Handle, attribute, ref value, sizeof(int));
            }
        }

        private static void RenderToolStrip(ToolStrip toolStrip, Color foreColor) {
            toolStrip.Renderer = new ToolStripProfessionalRenderer(new DarkMenuColorTable());
            toolStrip.ForeColor = foreColor;

            foreach (ToolStripMenuItem item in toolStrip.Items) {
                SetMenuItemColor(item, foreColor);
            }
        }

        private static void SetMenuItemColor(ToolStripMenuItem item, Color foreColor) {
            item.ForeColor = foreColor;
            foreach (ToolStripItem subItem in item.DropDownItems) {
                subItem.ForeColor = Color.White;
                if (subItem is ToolStripMenuItem subMenu) {
                    SetMenuItemColor(subMenu, foreColor);
                }
            }
        }

        class DarkMenuColorTable : ProfessionalColorTable {
            public override Color MenuStripGradientBegin => Color.FromArgb(45, 45, 48);
            public override Color MenuStripGradientEnd => Color.FromArgb(45, 45, 48);

            public override Color MenuItemSelected => Color.FromArgb(64, 64, 70);
            public override Color MenuItemSelectedGradientBegin => Color.FromArgb(64, 64, 70);
            public override Color MenuItemSelectedGradientEnd => Color.FromArgb(64, 64, 70);

            public override Color MenuItemPressedGradientBegin => Color.FromArgb(80, 80, 84);
            public override Color MenuItemPressedGradientEnd => Color.FromArgb(80, 80, 84);

            public override Color ToolStripDropDownBackground => Color.FromArgb(45, 45, 48);

            public override Color ImageMarginGradientBegin => Color.FromArgb(45, 45, 48);
            public override Color ImageMarginGradientMiddle => Color.FromArgb(45, 45, 48);
            public override Color ImageMarginGradientEnd => Color.FromArgb(45, 45, 48);

            public override Color MenuBorder => Color.FromArgb(64, 64, 70);
            public override Color SeparatorDark => Color.FromArgb(64, 64, 70);
            public override Color SeparatorLight => Color.FromArgb(64, 64, 70);

            public override Color ToolStripBorder => Color.FromArgb(45, 45, 48);
        }
    }
}
