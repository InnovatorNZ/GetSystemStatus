using System;

namespace GetSystemStatusGUI {
    public static class Utility {
        public static void FactorDecompose(int original, ref int bigger, ref int smaller) {
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
    }
}