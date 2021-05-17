using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GetSystemStatusWeb {
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
        public static string FormatDuplexString(string firstDesc, float firstByte, string secondDesc, float secondByte, int baseSystem = 1024) {
            string[] scale_unit;
            switch (baseSystem) {
                case 1024:
                    scale_unit = new string[] { "B/s", "KB/s", "MB/s", "GB/s", "TB/s" };
                    break;
                case 1000:
                    scale_unit = new string[] { "bps", "Kbps", "Mbps", "Gbps" };
                    break;
                default:
                    throw new NotSupportedException();
            }
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
    }
}