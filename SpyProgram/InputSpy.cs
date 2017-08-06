using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace SpyProgram
{
    public class InputSpy
    {
        public delegate void FocusChanged(string newWindowTitle, string oldWindowTitle, TimeSpan focusSpan);
        public event FocusChanged WindowFocusChanged;
        
        private Stopwatch watch;
        private string windowTitle;

        public void Start()
        {
            watch = Stopwatch.StartNew();

        }

        private void TimerCallback(object state)
        {

        }

        private void ActiveWindowChanged(string newActiveWindowTitle)
        {
        }
    }    
}
