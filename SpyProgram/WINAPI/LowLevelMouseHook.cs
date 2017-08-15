using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SpyProgram.WINAPI.User32;
using static SpyProgram.WINAPI.WindowsAPIHelper;

namespace SpyProgram.WINAPI
{
    public class LowLevelMouseHook
    {
        public enum MouseStatus
        {
            Pressed,
            Released
        }
        
        public delegate void HookStuff(Point point, MouseFlag flag);
        public event HookStuff HookActivated;

        private HookProc procedure;
        private IntPtr hookId = IntPtr.Zero;

        public LowLevelMouseHook()
        {
            procedure = HookProcedureCallback;
        }

        public void Hook()
        {
            IntPtr moduleHandle = GetCurrentProccessModuleHandle();
            hookId = SetWindowsHookEx(HookType.WH_MOUSE_LL, procedure, moduleHandle, 0);
        }

        public void UnHook()
        {
            if (hookId.Equals(IntPtr.Zero))
            {
                throw new InvalidOperationException("Cannot unhook what is not hooked.");
            }

            bool isSuccessfullyUnhooked = UnhookWindowsHookEx(hookId);
            if (!isSuccessfullyUnhooked)
            {
                throw new Exception("Failed to unhook"); // TODO: Make custom exception
            }

            hookId = IntPtr.Zero;
        }

        private int HookProcedureCallback(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code < 0)
            {
                return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
            }

            MouseFlag flag = (MouseFlag)wParam;
            MSLLHOOKSTRUCT mouseStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);

            HookActivated?.Invoke(mouseStruct.point, flag);

            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        private struct MSLLHOOKSTRUCT
        {
            public Point point;
            public uint mouseData;
            public uint flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }
    }
}
