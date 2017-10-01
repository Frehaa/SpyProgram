using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Diagnostics;
using System.Windows.Forms;
using SpyProgram.Logging;

namespace SpyProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileStream = File.Open("logfile.log", FileMode.Append, FileAccess.Write, FileShare.Read);
            var streamWritter = new StreamWriter(fileStream);

            Logger.NewLine = streamWritter.NewLine;

            Logger.AddOutputStream(streamWritter);
            Logger.AddOutputStream(Console.Out);

            Logger.Write(EventType.INFO, "Application start");

            WindowFocusSpy spy = new WindowFocusSpy();
            spy.WindowFocusChanged += Spy_WindowFocusChanged;
            spy.Start();

            Application.EnterThreadModal += Application_EnterThreadModal;
            Application.LeaveThreadModal += Application_LeaveThreadModal;
            Application.ThreadException += Application_ThreadException;
            Application.ThreadExit += Application_ThreadExit;
            Application.Idle += Application_Idle;
            Application.ApplicationExit += Application_ApplicationExit;

            AppDomain.CurrentDomain.ProcessExit += Application_ApplicationExit;

            Application.Run();
        }

        private static void Spy_WindowFocusChanged(string newWindowTitle, string oldWindowTitle, TimeSpan focusSpan)
        {
            Logger.Write(EventType.INFO, "Focus lost: " + oldWindowTitle + " focus duration: " + focusSpan);
            Logger.Write(EventType.INFO, "Focus gained: " + newWindowTitle);
        }

        private static void Application_ThreadExit(object sender, EventArgs e)
        {
            Logger.Write(EventType.INFO, "Application exiting thread: " + e.ToString());
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Write(EventType.INFO, "Application thread exception: " + e.Exception.Message);
        }

        private static void Application_LeaveThreadModal(object sender, EventArgs e)
        {
            Logger.Write(EventType.INFO, "Application leave thread modal: " + e.ToString());
        }

        private static void Application_Idle(object sender, EventArgs e)
        {
            Logger.Write(EventType.INFO, "Application idle: " + e.ToString());
        }

        private static void Application_EnterThreadModal(object sender, EventArgs e)
        {
            Logger.Write(EventType.INFO, "Application enter thread modal:" + e.ToString());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Logger.Write(EventType.INFO, "Application quit: " + e.ToString());
        }
    }
}
