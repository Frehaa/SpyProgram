using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SpyProgram.Windows.WINAPI
{
    public static class WindowsAPIHelper
    {
        public static string GetActiveWindowTitle()
        {
            IntPtr handle = User32.GetForegroundWindow();
            int textLength = User32.GetWindowTextLength(handle) + 1;
            StringBuilder buffer = new StringBuilder(textLength);

            User32.GetWindowText(handle, buffer, textLength);
            return buffer.ToString();
        }

        public static IntPtr GetCurrentProccessModuleHandle()
        {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule module = process.MainModule)
                return Kernel32.GetModuleHandle(module.ModuleName);
        }

        public static string GetActiveWindowProcessFileName()
        {
            IntPtr hWnd = User32.GetForegroundWindow();
            User32.GetWindowThreadProcessId(hWnd, out uint pid);
            var activeProccess = Process.GetProcessById((int)pid);
            return activeProccess.ProcessName;
        }

        public static (int, string) GetLastError()
        {
            int code = Marshal.GetLastWin32Error();
            return (code, new Win32Exception(code).Message);
        }

    }
}
