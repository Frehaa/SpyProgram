using System;
using System.Diagnostics;
using System.Timers;

namespace SpyProgram
{
    /// <summary>
    /// Spy for action counting.
    /// Once a minute the spy invokes an event with the numbers of actions done in the last minute.
    /// </summary>
    public class ApmSpy
    {
        public event Action<int> ActionCount;
        private readonly object sync = new object();

        private Timer timer;
        private readonly IKeyDownTracker keyTracker;
        private readonly IMouseDownTracker mouseTracker;
        private int actionCount;

        public ApmSpy(double event_interval, IKeyDownTracker keyTracker, IMouseDownTracker mouseTracker)
        {
            this.keyTracker = keyTracker;
            this.mouseTracker = mouseTracker;
            timer = new Timer();
            timer.Interval = event_interval;
            timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            if (timer.Enabled)
                throw new InvalidOperationException($"{nameof(ApmSpy)} is already started.");

            actionCount = 0;
            keyTracker.KeyDown += CountAction;
            mouseTracker.MouseDown += CountAction;
            timer.Start();
            keyTracker.Track();
            mouseTracker.Track();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (sync)
            {
                ActionCount?.Invoke(actionCount);
                actionCount = 0;
            }
        }

        private void CountAction(int i)
        {
            lock (sync) {
                actionCount++;
            }
        }

        public void Stop()
        {
            if (!timer.Enabled)
                return;

            timer.Stop();
            keyTracker.KeyDown -= CountAction;
            mouseTracker.MouseDown -= CountAction;
            actionCount = 0;

        }
    }
}
