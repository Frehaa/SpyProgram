
namespace SpyProgram.WINAPI
{
    public enum MouseFlag : uint
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSEHWHEEL = 0x020E,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_XBUTTONDOWN = 0x020B,
        WM_XBUTTONUP = 0x020C,
        WM_XBUTTONDBLCLK = 0x020D,
        WM_NCXBUTTONDOWN = 0x00AB,
        WM_NCXBUTTONUP = 0x00AC,
        WM_NCXBUTTONDBLCLK = 0x00AD,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
    }
    
    public enum KeyFlag : uint
    {
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105
    }
}
