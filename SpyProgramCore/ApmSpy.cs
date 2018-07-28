using SpyProgramCore.WINAPI;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SpyProgramCore
{
    /// <summary>
    /// Spy for action counting.
    /// Once a minute the spy invokes an event with the numbers of actions done in the last minute.
    /// </summary>
    public class ApmSpy
    {
        public event Action<int> ActionCount;
        private readonly object sync = new object();

        private Stopwatch watch;
        private Task spyTask = null;
        private Task keyTrackerTask;
        private Task mouseTrackerTask;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private readonly IKeydownTracker keyTracker;
        private readonly IMouseDownTracker mouseTracker;
        private int actionCount = 0;

        public ApmSpy(IKeydownTracker keyTracker, IMouseDownTracker mouseTracker)
        {
            this.keyTracker = keyTracker;
            this.mouseTracker = mouseTracker;

        }

        public void Start()
        {
            if (spyTask != null)
                throw new InvalidOperationException("Spy already started.");

            watch = Stopwatch.StartNew();

            spyTask = Task.Run(() => TrackActions(), cts.Token);
        }

        private void TrackActions()
        {
            keyTracker.KeyDown += ((i) => CountAction());
            mouseTracker.MouseDown += ((i) => CountAction());

            keyTrackerTask = Task.Run(() => keyTracker.Track(), cts.Token);
            mouseTrackerTask = Task.Run(() => mouseTracker.Track(), cts.Token);

            while (!cts.IsCancellationRequested)
            {
                if (watch.Elapsed.TotalMinutes >= 1.0d)
                {
                    lock (sync)
                    {
                        ActionCount?.Invoke(actionCount);
                        actionCount = 0;
                    }
                    
                }
                Task.Delay(500).Wait();
            }
                

            keyTracker.Stop();
            mouseTracker.Stop();
        }

        private void CountAction()
        {
            lock (sync) {
                actionCount++;
            }
        }

        public void Stop()
        {
            cts.Cancel();
            spyTask.Wait();
            spyTask = null;
        }
    }
}
