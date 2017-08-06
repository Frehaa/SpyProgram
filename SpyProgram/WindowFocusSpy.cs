using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            windowTitle = GetActiveWindowTitle();            
            timer = new Timer(
                callback: TimerCallback, 
                state: this, 
                dueTime: 500, 
                period: 500
            );
        }

        private void TimerCallback(object state)
        {
            string activeWindowTitle = GetActiveWindowTitle();
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

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
    }
}
