using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpyProgram.Windows.WINAPI
{
    public static class Psai
    {
        [DllImport("psapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetProcessImageFileName(IntPtr hProcess, StringBuilder lpImageFileName, uint nSize);
    }
}
