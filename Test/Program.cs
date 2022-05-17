using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test {
    class Program {
        static void Main(string[] args) {
            TestPerformanceCounter();
            for (int i = 1; i <= 2048; i++) {
                var ret = FactorDisposeRecurse2(i);
                Console.Write(i.ToString() + ": ");
                PrintFDRResult(ret);
                Console.WriteLine();
            }
            Console.ReadKey();
        }
        static List<int> FactorDisposeRecurse(int ori) {
            FactorDecompose(ori, out int bigger, out int smaller);
            if (bigger / smaller >= 10) {
                if (ori % 2 == 1) {
                    int sub_ori1 = (ori - 1) / 2;
                    int sub_ori2 = (ori - 1) / 2 + 1;
                    List<int> sub1 = FactorDisposeRecurse(sub_ori1);
                    List<int> sub2 = FactorDisposeRecurse(sub_ori2);
                    return sub1.Concat(sub2).ToList<int>();
                } else {
                    Console.WriteLine("Consider " + ori.ToString() + "!");
                }
            }
            return new List<int> { bigger, smaller };
        }
        static List<int> FactorDisposeRecurse2(int ori) {
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
        static bool LegalBound(int bigger, int smaller, int bound) {
            double ratio = (double)bigger / (double)smaller;
            return ratio <= (double)bound;
        }
        static void PrintFDRResult(List<int> result) {
            for (int i = 0; i < result.Count; i++) {
                Console.Write(result[i].ToString());
                if (i % 2 == 0) Console.Write(" ");
                else Console.Write("; ");
            }
        }
        static void FactorDecompose(int original, out int bigger, out int smaller) {
            double sqrt = Math.Sqrt(original);
            int a = (int)Math.Ceiling(sqrt), b = (int)Math.Floor(sqrt);
            while (a * b != original) {
                if (a * b > original) b--;
                else a++;
            }
            bigger = a;
            smaller = b;
        }
        static void TestPerformanceCounter() {
            PerformanceCounterCategory pfc = new PerformanceCounterCategory("PhysicalDisk");
            PerformanceCounter pf;
            string[] instanceNames = pfc.GetInstanceNames();
            PerformanceCounter gpc = new PerformanceCounter("GPU Engine", "Utilization Percentage", "pid_*_luid_0x00000000_0x0001685E_phys_0_eng_0_engtype_3D");
            Console.WriteLine(gpc.NextValue());
            for (int _ = 0; _ < 10; _++) {
                foreach (var name in instanceNames) {
                    pf = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
                    pf.MachineName = ".";
                    //pf.NextValue();
                    //pf.NextValue();
                    Console.WriteLine(name + " " + pf.NextValue());
                }
                Thread.Sleep(1000);
            }
        }
    }
}