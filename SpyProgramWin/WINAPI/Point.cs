using System.Runtime.InteropServices;

namespace SpyProgram.Windows.WINAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }
}
