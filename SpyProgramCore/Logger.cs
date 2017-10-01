using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SpyProgramCore.Logging
{
    public enum EventType
    {
        INFO, ERROR
    }

    /*
     * The logger is used to write simple logging messages to a class of type Textwriter with an event type, timestamp and message
     * 
     * 
     */
    public static class Logger
    {
        private static ICollection<TextWriter> outputStreams = new List<TextWriter>();
        private static ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private static object sync = new object();

        private static bool IsProcessing { get; set; }
        public static string NewLine { get; set; } = "";
        
        public static void AddOutputStream(TextWriter stream)
        {
            outputStreams.Add(stream);
        }

        public static void Write(EventType type, string message)
        {
            string logMessage = CreateLogMessage(type, message);
            messageQueue.Enqueue(logMessage);
            StartProccessingQueue();
        }

        private static void StartProccessingQueue()
        {
            lock (sync)
            {
                if (IsProcessing) return;
                IsProcessing = true;
            }

            Thread thread = new Thread(ProccessQueue);
            thread.Start();
        }

        private static void ProccessQueue()
        {
            if (!messageQueue.IsEmpty)
            {
                string message = CombineLogMessages();
                WriteMessage(message);
            }

            IsProcessing = false;
        }

        private static void WriteMessage(string message)
        {
            foreach (var stream in outputStreams)
            {
                try
                {
                    stream.Write(message);
                    stream.Flush();
                }
                catch (IOException e)
                {
                    Debug.Write("IOException happened: " + e.Message);
                }
            }
        }

        private static string CombineLogMessages()
        {
            StringBuilder messageBuilder = new StringBuilder();
            while (!messageQueue.IsEmpty)
            {
                if (messageQueue.TryDequeue(out string message))
                {
                    messageBuilder.Append(message + NewLine);
                }
            }
            return messageBuilder.ToString();
        }

        private static string CreateLogMessage(EventType type, string message)
        {
            return string.Format("[{0}] [{1}] - {2}", DateTime.Now, TypeToString(type), message);
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
