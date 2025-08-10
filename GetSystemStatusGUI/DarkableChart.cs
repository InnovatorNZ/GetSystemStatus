using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GetSystemStatusGUI {
    public class DarkableChart : DarkAwareForm {

        public void ApplyDarkMode() {
            if (!Global.IsDarkMode) return;

            Color backColor = Color.FromArgb(32, 32, 32);
            Color foreColor = Color.WhiteSmoke;

            this.BackColor = backColor;
            this.ForeColor = foreColor;

            ApplyThemeToControls(this.Controls, backColor, foreColor);
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls, Color backColor, Color foreColor) {
            foreach (Control ctrl in controls) {
                ctrl.BackColor = backColor;
                ctrl.ForeColor = foreColor;
                if (ctrl.HasChildren) {
                    ApplyThemeToControls(ctrl.Controls, backColor, foreColor);
                } else if (ctrl is Chart) {
                    Chart chart = (Chart)ctrl;
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
                }
            }
        }
    }
}
