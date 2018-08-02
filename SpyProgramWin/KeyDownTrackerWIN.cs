using System;
using System.Diagnostics;
using SpyProgram;
using SpyProgram.Windows.WINAPI;

namespace SpyProgram.Windows
{
    public class KeyDownTrackerWIN : IKeyDownTracker
    {
        public event Action<int> KeyDown;

        private LowLevelKeyboardHook keyboardHook = new LowLevelKeyboardHook();
        private bool tracking = false;

        public void Stop()
        {
            if (!tracking) return;

            keyboardHook.HookActivated -= KeyboardHook_HookActivated;
            keyboardHook.UnHook();
        }

        public void Track()
        {
            if (tracking)
                throw new InvalidOperationException("Already tracking. Can't call track on a started tracker");

            keyboardHook.HookActivated += KeyboardHook_HookActivated;
            keyboardHook.Hook();
        }

        private void KeyboardHook_HookActivated(uint key, LowLevelKeyboardHook.KeyInjectionType injectionType, bool isExtendedKey, bool isAltPressed, LowLevelKeyboardHook.KeyStatus keyStatus)
        {            
            if (LowLevelKeyboardHook.KeyStatus.Released == keyStatus)
            {
                KeyDown?.Invoke((int)key);
            }
                
        }
    }
}
