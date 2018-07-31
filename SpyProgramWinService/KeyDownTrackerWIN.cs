using System;
using SpyProgram.Windows.WINAPI;

namespace SpyProgram.Windows
{
    public class KeyDownTrackerWIN : IKeyDownTracker
    {
        public event Action<int> KeyDown;

        private LowLevelKeyboardHook keyboardHook = new LowLevelKeyboardHook();

        public void Stop()
        {
            keyboardHook.UnHook();
        }

        public void Track()
        {
            keyboardHook.HookActivated += KeyboardHook_HookActivated;
            keyboardHook.Hook();
        }

        private void KeyboardHook_HookActivated(uint key, LowLevelKeyboardHook.KeyInjectionType injectionType, bool isExtendedKey, bool isAltPressed, LowLevelKeyboardHook.KeyStatus keyStatus)
        {
            KeyDown?.Invoke(1);
        }
    }
}
