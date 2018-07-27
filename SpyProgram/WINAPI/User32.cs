using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SpyProgram.WINAPI
{
    public static class User32
    {
        public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);

        public enum HookType
        {
            WH_MIN = (-1),
            WH_MSGFILTER = (-1),        // that monitors messages generated as a result of an input event in a dialog box, message box, menu, or scroll bar. For more information, see the MessageProc hook procedure.
            WH_JOURNALRECORD = 0,       // that records input messages posted to the system message queue. This hook is useful for recording macros. For more information, see the JournalRecordProc hook procedure.
            WH_JOURNALPLAYBACK = 1,     // that posts messages previously recorded by a WH_JOURNALRECORD hook procedure. For more information, see the JournalPlaybackProc hook procedure.
            WH_KEYBOARD = 2,            // that monitors keystroke messages. For more information, see the KeyboardProc hook procedure.
            WH_GETMESSAGE = 3,          // that monitors messages posted to a message queue. For more information, see the GetMsgProc hook procedure.
            WH_CALLWNDPROC = 4,         // that monitors messages before the system sends them to the destination window procedure.For more information, see the CallWndProc hook procedure.
            WH_CBT = 5,                 // that receives notifications useful to a CBT application.For more information, see the CBTProc hook procedure.
            WH_SYSMSGFILTER = 6,        // that monitors messages generated as a result of an input event in a dialog box, message box, menu, or scroll bar. The hook procedure monitors these messages for all applications in the same desktop as the calling thread. For more information, see the SysMsgProc hook procedure.
            WH_MOUSE = 7,               // that monitors mouse messages. For more information, see the MouseProc hook procedure.
            WH_HARDWARE = 8,            // ???
            WH_DEBUG = 9,               // useful for debugging other hook procedures. For more information, see the DebugProc hook procedure.
            WH_SHELL = 10,              // that receives notifications useful to shell applications. For more information, see the ShellProc hook procedure.
            WH_FOREGROUNDIDLE = 11,     // that will be called when the application's foreground thread is about to become idle. This hook is useful for performing low priority tasks during idle time. For more information, see the ForegroundIdleProc hook procedure.
            WH_CALLWNDPROCRET = 12,     // that monitors messages after they have been processed by the destination window procedure.For more information, see the CallWndRetProc hook procedure.
            WH_KEYBOARD_LL = 13,        // that monitors low-level keyboard input events. For more information, see the LowLevelKeyboardProc hook procedure.
            WH_MOUSE_LL = 14            // that monitors low-level mouse input events. For more information, see the LowLevelMouseProc hook procedure.
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc hookProc, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int CallNextHookEx(IntPtr hhk, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetLastError();
    }
}
