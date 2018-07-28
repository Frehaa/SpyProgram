//using System;
//using SpyProgramCore.Logging;
//using System.IO;
//using System.Threading.Tasks;
//using System.Threading;

namespace SpyProgramCore
{
    class Program
    {
        //private static Logger logger;

        static void Main(string[] args)
        {

        //    using (var fileStream = new FileStream("logfile.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
        //    using (var streamWriter = new StreamWriter(fileStream))
        //    using (var cts = new CancellationTokenSource())
        //    using (logger = new Logger())
        //    {
        //        logger.AddOutputStream(Console.Out);
        //        logger.AddOutputStream(streamWriter);

        //        var t = Task.Run(() => Producer(cts.Token));

        //        Console.ReadKey();
        //        cts.Cancel();

        //        t.Wait();
        //    }

        //    Console.WriteLine("Wait done");
        //}

        //private static void Producer(CancellationToken token)
        //{
        //    try
        //    {
        //        ProducerWork(token);
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Console.WriteLine("Producer Cancelled");
        //    }
        //}

        //private static void ProducerWork(CancellationToken token)
        //{
        //    var spy = new WindowFocusSpy();
        //    spy.WindowFocusChanged += Spy_WindowFocusChanged;
        //    spy.Start();

        //    while (!token.IsCancellationRequested)
        //        Task.Delay(500).Wait();

        //    spy.Stop();
        //    token.ThrowIfCancellationRequested();
        //}

        //private static void Spy_WindowFocusChanged(string newWindowTitle, string oldWindowTitle, TimeSpan windowsFocusTime)
        //{
        //    logger.Write(EventType.INFO, "Focus lost: " + oldWindowTitle + " - focus duration: " + windowsFocusTime);
        //    logger.Write(EventType.INFO, "Focus gained: " + newWindowTitle);
        }
    }
}
