using System;
using System.Diagnostics;
using System.Text;

namespace SpyProgramCore.WINAPI
{
    public static class WindowsAPIHelper
    {
        public static string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = User32.GetForegroundWindow();

            if (User32.GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public static IntPtr GetCurrentProccessModuleHandle()
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
            {
                return Kernel32.GetModuleHandle(module.ModuleName);
            }
        }
    }
}
