using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GetSystemStatusGUI {
    public static class Utility {
        public static void FactorDecompose(int original, out int bigger, out int smaller) {
            double sqrt = Math.Sqrt(original);
            int a = (int)Math.Ceiling(sqrt), b = (int)Math.Floor(sqrt);
            while (a * b != original) {
                if (a * b > original) b--;
                else a++;
            }
            bigger = a;
            smaller = b;
        }
        public static string FormatSpeedString(string firstDesc, float firstByte, string secondDesc, float secondByte, bool bps = false) {
            string[] scale_unit;
            const int baseSystem = 1000;
            if (!bps)
                scale_unit = new string[] { "B/s", "KB/s", "MB/s", "GB/s", "TB/s" };
            else
                scale_unit = new string[] { "bps", "Kbps", "Mbps", "Gbps" };
            string ret = string.Empty;
            ret += firstDesc + " ";
            int firstScale = (int)Math.Max(Math.Floor(Math.Log(firstByte, baseSystem)), 0);
            int secondScale = (int)Math.Max(Math.Floor(Math.Log(secondByte, baseSystem)), 0);
            firstByte /= (float)Math.Pow(baseSystem, firstScale);
            secondByte /= (float)Math.Pow(baseSystem, secondScale);
            firstByte = (float)Math.Round(firstByte, 1);
            secondByte = (float)Math.Round(secondByte, 1);
            ret += firstByte.ToString() + " " + scale_unit[firstScale];
            ret += "\n" + secondDesc + " ";
            ret += secondByte.ToString() + " " + scale_unit[secondScale];
            return ret;
        }
        public static string FormatSizeString(string desc, long bytes) {
            string[] scale_unit = { "Bytes", "KB", "MB", "GB", "TB" };
            const int baseSystem = 1024;
            int scale = (int)Math.Max(Math.Floor(Math.Log(bytes, baseSystem)), 0);
            double finalValue = Math.Round((double)bytes / Math.Pow(baseSystem, scale), 1);
            string strscale = scale_unit[scale];
            string ret = desc + ": " + finalValue.ToString() + " " + strscale + "\n";
            return ret;
        }
        public static Font ScaleFont(Font proto, float scale) {
            Font ret = new Font(proto.Name, proto.Size * scale, proto.Style, proto.Unit);
            return ret;
        }
        public static List<int> FactorDisposeRecurse(int ori) {
            FactorDecompose(ori, out int bigger, out int smaller);
            if (bigger / smaller >= 10) {
                if (ori % 2 == 1) {
                    int sub_ori1 = (ori - 1) / 2;
                    int sub_ori2 = (ori - 1) / 2 + 1;
                    List<int> sub1 = FactorDisposeRecurse(sub_ori1);
                    List<int> sub2 = FactorDisposeRecurse(sub_ori2);
                    return sub1.Concat(sub2).ToList<int>();
                }
            }
            return new List<int> { bigger, smaller };
        }
        public static List<int> FactorDisposeRecurse2(int ori) {
            const int bound = 6;
            FactorDecompose(ori, out int bigger, out int smaller);
            List<int> result = null;
            if (!LegalBound(bigger, smaller, bound)) {
                int min_D = 1073741824;
                for (int d = 0; d < ori / 4; d++) {
                    int b = (ori - ori % 2) / 2;
                    int sub_ori1 = b - d;
                    int sub_ori2 = b + d + (ori % 2);
                    FactorDecompose(sub_ori1, out int bigger1, out int smaller1);
                    FactorDecompose(sub_ori2, out int bigger2, out int smaller2);
                    if (LegalBound(bigger1, smaller1, bound) && LegalBound(bigger2, smaller2, bound)) {
                        int c_D = (bigger1 - smaller1) + (bigger2 - smaller2) + d;
                        if (c_D < min_D) {
                            min_D = c_D;
                            result = new List<int> { bigger1, smaller1, bigger2, smaller2 };
                        }
                    }
                }
                if (result == null) {
                    int rec_ori1 = (ori - 1) / 2;
                    int rec_ori2 = (ori - 1) / 2 + 1;
                    List<int> sub1 = FactorDisposeRecurse2(rec_ori1);
                    List<int> sub2 = FactorDisposeRecurse2(rec_ori2);
                    result = sub1.Concat(sub2).ToList<int>();
                }
            } else {
                result = new List<int> { bigger, smaller };
            }
            return result;
        }
        private static bool LegalBound(int bigger, int smaller, int bound) {
            double ratio = (double)bigger / (double)smaller;
            return ratio <= (double)bound;
        }
    }
}