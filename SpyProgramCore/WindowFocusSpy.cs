using SpyProgramCore.WINAPI;
using System;
using System.Diagnostics;
using System.Threading;

namespace SpyProgramCore
{
    public class WindowFocusSpy
    {
        private const int sleepTime = 500;
        public event Action<string, string, TimeSpan> WindowFocusChanged;
        
        private Stopwatch watch;
        private string windowTitle;
        
        public void Start()
        {
            watch = Stopwatch.StartNew();
            windowTitle = WindowsAPIHelper.GetActiveWindowTitle();

            Thread thread = new Thread(CheckForNewActiveWindow);
            thread.Start();
        }

        private void CheckForNewActiveWindow()
        {
            while (true)
            {
                string activeWindowTitle = WindowsAPIHelper.GetActiveWindowTitle();
                if (windowTitle != activeWindowTitle)
                {
                    ActiveWindowChanged(activeWindowTitle);
                }

                Thread.Sleep(sleepTime);
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
