using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpyProgram
{
    /*
     * The logger is used to write simple logging messages to a file with an event type, timestamp and message
     * 
     * 
     */
    public static class Logger
    {
        private static Stopwatch watch = Stopwatch.StartNew();
        private static DateTime datetime = DateTime.Now;
        private static ICollection<TextWriter> outputStreams = new List<TextWriter>();
        private static ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private static string newLine = "";

        private static bool IsProcessing { get; set; }
        public static string NewLine
        {
            get
            {
                return newLine;
            }
            set
            {
                newLine = value;
            }
        }

        public enum EventType
        {
            INFO, ERROR
        }
        
        public static void AddOutputStream(TextWriter stream)
        {
            outputStreams.Add(stream);
        }

        public static void Write(EventType type, string message)
        {
            IncrementTime();
            messageQueue.Enqueue(CreateCompleteMessage(type, message));
            StartProccessingQueue();
        }
        
        private static void IncrementTime()
        {
            long milliseconds = watch.ElapsedMilliseconds;
            watch.Restart();

            datetime = datetime.AddMilliseconds(milliseconds);
        }

        private static void StartProccessingQueue()
        {
            if (IsProcessing)
            {
                return;
            }
            IsProcessing = true;
            Thread thread = new Thread(ProccessQueue);
            thread.Start();
        }

        private static void ProccessQueue()
        {
            if (messageQueue.IsEmpty)
                return;

            StringBuilder messageBuilder = new StringBuilder();

            while (!messageQueue.IsEmpty)
            {
                if (messageQueue.TryDequeue(out string message))
                {    
                    messageBuilder.Append(message + NewLine);
                }                
            }

            foreach (var stream in outputStreams)
            {
                try
                {
                    stream.Write(messageBuilder.ToString());
                    stream.Flush();
                }
                catch (IOException e)
                {
                    Debug.Write("IOException happened: " + e.Message);
                }
            }

            IsProcessing = false;
        }

        private static string CreateCompleteMessage(EventType type, string message)
        {
            return string.Format("[{0}] [{1}] - {2}", datetime, TypeToString(type), message);
        }

        private static void Flush()
        {
            foreach (var stream in outputStreams)
            {
                stream.Flush();
            }
        }


        private static string TypeToString(EventType type)
        {
            switch (type)
            {
                case EventType.INFO:
                    return "INFO";
                case EventType.ERROR:
                    return "ERROR";
                default:
                    throw new ArgumentException("No string associated with type");
                    
            }
        }        
    }

}
