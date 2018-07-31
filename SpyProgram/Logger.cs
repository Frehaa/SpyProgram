using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace SpyProgram.Logging
{
    public enum EventType
    {
        INFO, ERROR
    }

    /// <summary>
    /// The logger is used to write simple logging messages to a class of type Textwriter with an event type, timestamp and message
    /// </summary>
    public class Logger : IDisposable
    {
        private ICollection<TextWriter> outputStreams = new List<TextWriter>();
        private ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
        private Thread proccessingThread;
        private readonly object sync = new object();

        private bool IsProcessing { get; set; }
        public string NewLine { get; set; } = Environment.NewLine;
        
        public void AddOutputStream(TextWriter stream)
        {
            outputStreams.Add(stream);
        }

        public void Write(EventType type, string message)
        {
            string logMessage = CreateLogMessage(type, message);
            messageQueue.Enqueue(logMessage);
            StartProccessingQueue();
        }

        private void StartProccessingQueue()
        {
            lock (sync)
            {
                if (IsProcessing) return;
                IsProcessing = true;
            }

            proccessingThread = new Thread(ProccessQueue);
            proccessingThread.Start();
        }

        private void ProccessQueue()
        {
            if (!messageQueue.IsEmpty)
            {
                string message = CombineLogMessages();
                WriteMessage(message);
            }

            IsProcessing = false;
        }

        private void WriteMessage(string message)
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

        private string CombineLogMessages()
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

        private string CreateLogMessage(EventType type, string message)
        {
            return $"[{DateTime.Now}] [{type}] - {message}";
        }
        
        private static string TypeToString(EventType type)
        {
            return type.ToString();
        }

        public void Dispose()
        {
            outputStreams.Clear();
        }
    }

}
