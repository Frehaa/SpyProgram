using System;
using System.Runtime.InteropServices;

namespace SpyProgram.Windows.WINAPI
{
    public static class Kernel32
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
