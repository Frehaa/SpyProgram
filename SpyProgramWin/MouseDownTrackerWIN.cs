using System;
using System.Collections.Generic;
using System.Diagnostics;
using SpyProgram;
using SpyProgram.Windows.WINAPI;

namespace SpyProgram.Windows
{
    public class MouseDownTrackerWIN : IMouseDownTracker
    {
        public event Action<int> MouseDown;

        private LowLevelMouseHook mouseHook = new LowLevelMouseHook();
        private readonly List<MouseFlag> validFlags = new List<MouseFlag>()
            { MouseFlag.WM_LBUTTONDOWN, MouseFlag.WM_MBUTTONDOWN, MouseFlag.WM_NCXBUTTONDOWN, MouseFlag.WM_XBUTTONDOWN, MouseFlag.WM_RBUTTONDOWN };

        private bool tracking = false;

        public void Stop()
        {
            if (!tracking) return;

            mouseHook.HookActivated -= MouseHook_HookActivated;
            mouseHook.UnHook();
            tracking = false;
        }

        public void Track()
        {
            if (tracking)
                throw new InvalidOperationException("Already tracking. Can't call track on a started tracker");

            mouseHook.HookActivated += MouseHook_HookActivated;
            mouseHook.Hook();
            tracking = true;
        }
        
        private void MouseHook_HookActivated(Point point, MouseFlag flag)
        {
            if (validFlags.Contains(flag)) MouseDown?.Invoke((int)flag);
        }
    }
}
