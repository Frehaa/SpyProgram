using System.ServiceProcess;
using System;
using SpyProgram.Logging;
using System.IO;

namespace SpyProgram.Windows
{
    public partial class SpyService : ServiceBase
    {
        private readonly Logger logger = new Logger();
        private readonly WindowFocusSpy windowSpy = new WindowFocusSpy(new WindowInformerWIN());
        private readonly ApmSpy apmSpy = new ApmSpy(new KeyDownTrackerWIN(), new MouseDownTrackerWIN());
        private StreamWriter streamWriter;
        
        public SpyService(string logfilePath)
        {
            InitializeComponent();
            windowSpy.WindowFocusChanged += Spy_WindowFocusChanged;
            apmSpy.ActionCount += ApmSpy_ActionCount;

            var fileStream = new FileStream(logfilePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            streamWriter = new StreamWriter(fileStream);
            logger.AddOutputStream(streamWriter);
        }

        protected override void OnStart(string[] args)
        {   
            windowSpy.Start();
            apmSpy.Start();
        }

        protected override void OnContinue()
        {
            windowSpy.Start();
            apmSpy.Start();
        }

        protected override void OnPause()
        {
            windowSpy.Stop();
            apmSpy.Stop();
        }

        protected override void OnStop()
        {
            windowSpy.Stop();
            apmSpy.Stop();
        }

        private void ApmSpy_ActionCount(int actions, TimeSpan time)
        {
            logger.Write(EventType.INFO, $"Actions made the last: {time.Seconds} seconds: {actions}");
        }

        private void Spy_WindowFocusChanged(string newWindowTitle, string oldWindowTitle, TimeSpan windowsFocusTime)
        {
            logger.Write(EventType.INFO, $"Focus lost: {oldWindowTitle} - focus duration: {windowsFocusTime}");
            logger.Write(EventType.INFO, $"Focus gained: {newWindowTitle}");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                logger.Dispose();
                streamWriter.Dispose();

            }
            base.Dispose(disposing);
        }
    }
}
