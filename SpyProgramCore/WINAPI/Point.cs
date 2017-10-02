using System.Runtime.InteropServices;

namespace SpyProgramCore.WINAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public int x;
        public int y;
    }
}
