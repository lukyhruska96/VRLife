using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace VrLifeServer.Core
{
    class HwMonitor
    {
        static HwMonitor()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                SetOSWindows();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                SetOSLinux();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                SetOSMac();
            }

        }

        private static ulong totalMemory = 0;

        public static Func<ulong> GetTotalMemory = null;

        private static ulong GetTotalMemoryWin()
        {
            if (totalMemory != 0)
            {
                return totalMemory;
            }
            string output = RunCmd("wmic memorychip get Capacity");
            string[] lines = output.Split('\n');
            totalMemory = (ulong) lines.Skip(1).Sum(str => double.TryParse(str, out double tmp) ? tmp : 0);
            return totalMemory;
        }

        private static ulong GetTotalMemoryLinux()
        {
            if (totalMemory != 0)
            {
                return totalMemory;
            }
            string output = RunCmd("head -n 1 /proc/meminfo | tr -s \" \" | cut -d \" \" -f 2") + "000";
            totalMemory = ulong.TryParse(output, out ulong tmp) ? tmp : 0;
            return totalMemory;
        }

        private static ulong GetTotalMemoryMac()
        {
            if(totalMemory != 0)
            {
                return totalMemory;
            }
            string output = RunCmd("sysctl hw.memsize | cut -d \" \" -f 2");
            totalMemory = ulong.TryParse(output, out ulong tmp) ? tmp: 0;
            return totalMemory;
        }

        public static uint GetCoreCount()
        {
            return (uint)Environment.ProcessorCount;
        }

        private static string cmdFileName;
        private static string cmdArgumentFirst;

        private static string RunCmd(string cmdString)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = cmdFileName;
            cmd.StartInfo.Arguments = cmdArgumentFirst + cmdString;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            string output = cmd.StandardOutput.ReadToEnd();
            cmd.WaitForExit();
            return output;
        }

        private static void SetOSWindows()
        {
            GetTotalMemory = GetTotalMemoryWin;
            cmdFileName = "cmd.exe";
            cmdArgumentFirst = "/C ";
        }

        private static void SetOSLinux()
        {
            GetTotalMemory = GetTotalMemoryLinux;
            cmdFileName = "/bin/bash";
            cmdArgumentFirst = "";
        }

        private static void SetOSMac()
        {
            GetTotalMemory = GetTotalMemoryMac;
            cmdFileName = "/bin/bash";
            cmdArgumentFirst = "";
        }
    }
}
