﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Arheisel.Log
{
    public class Log
    {
        public const string TYPE_DEBUG = "DEBUG";
        public const string TYPE_INFO = "INFO";
        public const string TYPE_WARNING = "WARNING";
        public const string TYPE_ERROR = "ERROR";
        public const string TYPE_EXCEPTION = "EXCEPTION";

        private static Thread thread;
        private static ConcurrentQueue<KeyValuePair<string, string>> queue;


        public static void Start()
        {
            queue = new ConcurrentQueue<KeyValuePair<string, string>>();

            thread = new Thread(new ThreadStart(WriteThread)) { IsBackground = true };
            thread.Start();

            Write(TYPE_INFO, "========================= PROGRAM START =========================");
        }

        public static void Stop()
        {
            thread.Abort();
        }

        public static void Write(string type, string message)
        {
#if !DEBUG
            if (type == TYPE_DEBUG) return;
#endif
            if (type != TYPE_DEBUG)
            {
                Console.WriteLine(type + ": " + message);
            }
            queue.Enqueue(new KeyValuePair<string, string>(type, message));
        }

        public static void Write(Exception e)
        {
            Write(TYPE_EXCEPTION, e.Message + Environment.NewLine + e.StackTrace);
        }

        public static void Debug(string message)
        {
            Write(TYPE_DEBUG, message);
        }

        public static void Info(string message)
        {
            Write(TYPE_INFO, message);
        }

        public static void Warning(string message)
        {
            Write(TYPE_WARNING, message);
        }

        public static void Error(string message)
        {
            Write(TYPE_ERROR, message);
        }

        public static void Exception(Exception e)
        {
            Write(e);
        }

        private static void WriteThread()
        {
            while (true)
            {
                if (queue.TryDequeue(out KeyValuePair<string, string> log))
                {
                    WriteFile(log.Key, log.Value);
                }
                Thread.Sleep(50);
            }
        }

        private static void WriteFile(string type, string message)
        {

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = Path.Combine(path, DateTime.Now.ToString("yyyy_MM_dd") + "_ServiceLog.txt");
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + type + ": " + message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + type + ": " + message);
                }
            }
        }
    }
}
