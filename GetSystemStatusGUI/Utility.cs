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
    }
}