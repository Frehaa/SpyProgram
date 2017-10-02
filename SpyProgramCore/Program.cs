using System;
using SpyProgramCore.Logging;
using System.IO;
using System.Threading.Tasks.Dataflow;
using System.Threading.Tasks;
using System.Threading;

namespace SpyProgramCore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var fileStream = new FileStream("logfile.txt", FileMode.Append, FileAccess.Write, FileShare.Read))
            using (var streamWriter = new StreamWriter(fileStream))
            using (var cts = new CancellationTokenSource())
            {
                Logger.NewLine = Console.Out.NewLine;
                Logger.AddOutputStream(Console.Out);
                Logger.AddOutputStream(streamWriter);

                var bufferBlock = new BufferBlock<string>();

                var t1 = Task.Run(() => Consumer(bufferBlock, cts.Token));
                var t2 = Task.Run(() => Producer(bufferBlock, cts.Token));

                Console.ReadKey();
                cts.Cancel();

                Task.WhenAll(t1, t2).Wait();
            }
            Console.WriteLine("Wait done");
        }

        private static void Producer(ITargetBlock<string> targetBlock, CancellationToken token)
        {
            try
            {
                ProducerWork(targetBlock, token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Producer Cancelled");
            }
        }

        private static void ProducerWork(ITargetBlock<string> targetBlock, CancellationToken token)
        {
            var spy = new WindowFocusSpy();
            spy.WindowFocusChanged += Spy_WindowFocusChanged;
            spy.Start();

            while (true)
            {
               if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                Task.Delay(500).Wait();
            }
        }

        private static void Spy_WindowFocusChanged(string newWindowTitle, string oldWindowTitle, TimeSpan windowsFocusTime)
        {
            Logger.Write(EventType.INFO, "Focus lost: " + oldWindowTitle + " - focus duration: " + windowsFocusTime);
            Logger.Write(EventType.INFO, "Focus gained: " + newWindowTitle);
        }

        private static void Consumer(ISourceBlock<string> sourceBlock, CancellationToken token)
        {
            try
            {
                ConsumerWork(sourceBlock, token);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumer cancelled");
            }
        }

        private static void ConsumerWork(ISourceBlock<string> sourceBlock, CancellationToken token)
        {
            while (true)
            {
                string data = sourceBlock.Receive(token);
                Console.WriteLine(data);
            }
        }
    }
}
