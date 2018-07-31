using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SpyProgram
{
    /// <summary>
    /// Spy for action counting.
    /// Once a minute the spy invokes an event with the numbers of actions done in the last minute.
    /// </summary>
    public class ApmSpy
    {
        public event Action<int, TimeSpan> ActionCount;
        private readonly object sync = new object();

        private Stopwatch watch;
        private Task spyTask = null;
        private Task keyTrackerTask;
        private Task mouseTrackerTask;
        private CancellationTokenSource cts; 
        private readonly IKeyDownTracker keyTracker;
        private readonly IMouseDownTracker mouseTracker;
        private int actionCount;

        public ApmSpy(IKeyDownTracker keyTracker, IMouseDownTracker mouseTracker)
        {
            this.keyTracker = keyTracker;
            this.mouseTracker = mouseTracker;
        }

        public void Start()
        {
            if (spyTask != null)
                throw new InvalidOperationException("Spy already started.");

            cts = new CancellationTokenSource();
            watch = Stopwatch.StartNew();
            actionCount = 0;
            spyTask = Task.Run(() => TrackActions(), cts.Token);
        }

        private void TrackActions()
        {
            keyTracker.KeyDown += CountAction;
            mouseTracker.MouseDown += CountAction;
            keyTrackerTask = Task.Run(() => keyTracker.Track(), cts.Token);
            mouseTrackerTask = Task.Run(() => mouseTracker.Track(), cts.Token);

            while (!cts.IsCancellationRequested)
            {
                if (watch.Elapsed.Minutes >= 1.0d)
                    SendEvent();

                Task.Delay(500).Wait();
            }
            Task.WhenAll(keyTrackerTask, mouseTrackerTask).Wait();

            keyTracker.KeyDown -= CountAction;
            mouseTracker.MouseDown -= CountAction;
        }

        private void SendEvent()
        {
            lock (sync)
            {
                ActionCount?.Invoke(actionCount, watch.Elapsed);
                actionCount = 0;
                watch.Restart();
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
            if (spyTask == null)
                return;

            cts.Cancel();
            spyTask.Wait();
            actionCount = 0;
            spyTask = null;
            
        }
    }
}
