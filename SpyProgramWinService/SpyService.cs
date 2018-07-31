using System.ServiceProcess;
using System;
using SpyProgram.Logging;
using System.IO;
using System.Runtime.InteropServices;

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
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            windowSpy.Start();
            apmSpy.Start();

            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
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

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
