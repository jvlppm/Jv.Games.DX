using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinAPI
{
    static class NativeMethods
    {
        public static class Kernel32
        {
            [DllImport("Kernel32.dll")]
            public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

            [DllImport("Kernel32.dll")]
            public static extern bool QueryPerformanceFrequency(out long lpFrequency);
        }
    }
}
