using SpyProgramCore.WINAPI;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SpyProgramCore
{
    /// <summary>
    /// Spy for window focus. 
    /// Every time the focus of the window changes to another window the class evokes the WindowFocusChanged event.
    /// There is a 500ms internal sleep timer so if focus changes faster than this there is a chance 
    /// </summary>
    public class WindowFocusSpy
    {
        private const int sleepTime = 500;
        public event Action<string, string, TimeSpan> WindowFocusChanged;
        
        private Stopwatch watch;
        private string windowTitle;
        private Task spyTask = null;
        private CancellationTokenSource cts = new CancellationTokenSource();
        
        public void Start()
        {
            if (spyTask != null)
                throw new InvalidOperationException("Spy already started.");

            watch = Stopwatch.StartNew();
            windowTitle = WindowsAPIHelper.GetActiveWindowTitle();

            spyTask = Task.Run(() => CheckForNewActiveWindow(), cts.Token);            
        }

        public void Stop()
        {
            InvokeIfWindowChanged();
            cts.Cancel();
            spyTask.Wait();
            spyTask = null;
        }

        private void CheckForNewActiveWindow()
        {
            while (!cts.IsCancellationRequested)
            {   
                InvokeIfWindowChanged();
                Thread.Sleep(sleepTime);
            }
        }

        private void InvokeIfWindowChanged()
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
