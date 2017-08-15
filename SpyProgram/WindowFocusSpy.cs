using SpyProgram.WINAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace SpyProgram
{
    public class WindowFocusSpy
    {
        public delegate void FocusChanged(string newWindowTitle, string oldWindowTitle, TimeSpan focusSpan);
        public event FocusChanged WindowFocusChanged;

        private Timer timer;
        private Stopwatch watch;
        private string windowTitle;
        
        public void Start()
        {
            watch = Stopwatch.StartNew();
            windowTitle = WindowsAPIHelper.GetActiveWindowTitle();
            timer = new Timer(
                callback: TimerCallback, 
                state: this, 
                dueTime: 500, 
                period: 500
            );
        }

        private void TimerCallback(object state)
        {
            string activeWindowTitle = WindowsAPIHelper.GetActiveWindowTitle();
            if (windowTitle != activeWindowTitle)
            {
                ActiveWindowChanged(activeWindowTitle);
            }
        }

        private void ActiveWindowChanged(string newActiveWindowTitle)
        {
            WindowFocusChanged?.Invoke(newActiveWindowTitle, windowTitle, GetWindowFocusTime());
            windowTitle = newActiveWindowTitle;
        }

        private TimeSpan GetWindowFocusTime()
        {
            TimeSpan focusTime = watch.Elapsed;
            watch.Restart();
            return focusTime;
        }


    }
}
