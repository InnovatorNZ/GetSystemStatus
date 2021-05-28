using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PerformanceCounterCategory pfc = new PerformanceCounterCategory("PhysicalDisk");
            PerformanceCounter pf;
            string[] instanceNames = pfc.GetInstanceNames();
            PerformanceCounter gpc = new PerformanceCounter("GPU Engine", "Utilization Percentage", "pid_*_luid_0x00000000_0x0001685E_phys_0_eng_0_engtype_3D");
            Console.WriteLine(gpc.NextValue());
            for (int _ = 0; _ < 10; _++)
            {
                foreach (var name in instanceNames)
                {
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