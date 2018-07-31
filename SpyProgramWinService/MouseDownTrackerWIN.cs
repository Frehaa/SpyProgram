using System;
using System.Collections.Generic;
using SpyProgram.Windows.WINAPI;

namespace SpyProgram.Windows
{
    public class MouseDownTrackerWIN : IMouseDownTracker
    {
        public event Action<int> MouseDown;

        private LowLevelMouseHook mouseHook = new LowLevelMouseHook();
        private readonly List<MouseFlag> validFlags = new List<MouseFlag>() { MouseFlag.WM_LBUTTONDOWN, MouseFlag.WM_MBUTTONDOWN, MouseFlag.WM_NCXBUTTONDOWN, MouseFlag.WM_XBUTTONDOWN };

        public void Stop()
        {
            mouseHook.UnHook();
        }

        public void Track()
        {
            mouseHook.HookActivated += MouseHook_HookActivated;
            mouseHook.Hook();
        }

        private void MouseHook_HookActivated(Point point, MouseFlag flag)
        {
            //if (validFlags.Contains(flag))
            MouseDown?.Invoke((int)flag);
        }
    }
}
