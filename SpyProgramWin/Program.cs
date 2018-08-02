using Microsoft.Win32;
using SpyProgram.Logging;
using SpyProgram.Windows.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SpyProgram.Windows
{
    static class Program
    {
        private static Logger logger;
        private static bool closed = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            string logPath = args.Count() > 0? args[0] : Resources.DefaultLogPath;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += (s, e) => Close();
            SystemEvents.SessionEnding += (s, e) => Close();

            using (logger = new Logger())
            using (var fileStream = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read))
            using (ProcessIcon icon = new ProcessIcon(logPath))
            {
                logger.AddOutputStream(new StreamWriter(fileStream));

                ApmSpy apmSpy = new ApmSpy(60_000, new KeyDownTrackerWIN(), new MouseDownTrackerWIN());
                apmSpy.ActionCount += ApmSpy_ActionCount;

                WindowFocusSpy focusSpy = new WindowFocusSpy(new WindowInformerWIN());
                focusSpy.WindowFocusChanged += FocusSpy_WindowFocusChanged;

                apmSpy.Start();
                focusSpy.Start();

                logger.Write(EventType.INFO, "Starting up");

                icon.Display();
                Application.Run();
            }

        }

        private static void Close()
        {
            if (closed) return;            

            logger.Write(EventType.INFO, "Shutting down");
            logger.Dispose();
            closed = true;
        }

        private static void FocusSpy_WindowFocusChanged(string newWindowTitle, string newWindowProcessName, string oldWindowTitle, string oldWindowProcessName, TimeSpan windowsFocusTime)
        {
            logger.Write(EventType.INFO, $"<FocusLost>: {oldWindowTitle} <ProcessName>: {oldWindowProcessName} <FocusTime>: {windowsFocusTime}");
            logger.Write(EventType.INFO, $"<FocusGain>: {newWindowTitle} <ProcessName>: {newWindowProcessName}");
        }

        private static void ApmSpy_ActionCount(int actions)
        {
            logger.Write(EventType.INFO, $"<ActionCount>: {actions}");
        }
    }
}
