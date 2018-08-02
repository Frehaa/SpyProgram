using System;
using System.Diagnostics;
using System.Timers;

namespace SpyProgram
{
    /// <summary>
    /// Spy for window focus. 
    /// Every time the focus of the window changes to another window the class evokes the WindowFocusChanged event.
    /// There is a 500ms internal sleep timer so if focus changes faster than this there is a chance 
    /// </summary>
    public class WindowFocusSpy
    {
        private const double interval = 500;
        /// <summary>
        ///  New window title, old window title, focus time span
        /// </summary>
        public event Action<string, string, string, string, TimeSpan> WindowFocusChanged;

        private readonly Timer timer;
        private Stopwatch watch;
        private string windowTitle;
        private string fileName;
        private readonly IWindowInformer informer;

        public WindowFocusSpy(IWindowInformer informer)
        {
            this.informer = informer;
            timer = new Timer();
            timer.Elapsed += (s,e) => InvokeIfWindowChanged();
            timer.Interval = interval;
        }
        
        public void Start()
        {
            if (timer.Enabled)
                throw new InvalidOperationException($"{nameof(WindowFocusSpy)} already started.");

            watch = Stopwatch.StartNew();
            windowTitle = informer.GetActiveWindowTitle();
            fileName = informer.GetActiveWindowFileName();
            timer.Start();
        }

        public void Stop()
        {
            if (!timer.Enabled)
                return;

            InvokeIfWindowChanged();
            timer.Stop();
        }
        
        private void InvokeIfWindowChanged()
        {
            string activeWindowTitle = informer.GetActiveWindowTitle();
            string activeFileName = informer.GetActiveWindowFileName();

            if (windowTitle != activeWindowTitle || fileName != activeFileName)
                ActiveWindowChanged(activeWindowTitle, activeFileName);
        }

        private void ActiveWindowChanged(string newActiveWindowTitle, string newActiveWindowFileName)
        {
            WindowFocusChanged?.Invoke(newActiveWindowTitle, newActiveWindowFileName, windowTitle, fileName, GetWindowFocusTime());
            windowTitle = newActiveWindowTitle;
            fileName = newActiveWindowFileName;
        }

        private TimeSpan GetWindowFocusTime()
        {
            TimeSpan focusTime = watch.Elapsed;
            watch.Restart();
            return focusTime;
        }        
    }
}
