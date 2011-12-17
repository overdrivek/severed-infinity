using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SIEngine.Other;

namespace SIEngine.Logging
{
    /// <summary>
    /// Class for managing logs and errors.
    /// </summary>
    public static class LogManager
    {
        private static StreamWriter InfoWriter { get; set; }
        private static StreamWriter ErrorWriter { get; set; }

        static LogManager()
        {
            InfoWriter = new StreamWriter(GameConstants.InfoLogDirectory, false, Encoding.ASCII);
            ErrorWriter = new StreamWriter(GameConstants.ErrorLogDirectory, false, Encoding.ASCII);

            string time = DateTime.Now.ToString();
            InfoWriter.WriteLine("Severed Infinity info log");
            InfoWriter.WriteLine("started: " + time);
            InfoWriter.Flush();

            ErrorWriter.WriteLine("Severed Infinity error log");
            ErrorWriter.WriteLine("started: " + time);
            ErrorWriter.Flush();
        }

        /// <summary>
        /// Write an info message.
        /// </summary>
        /// <param name="info">The message to write</param>
        public static void WriteInfo(string info)
        {
            string time = DateTime.Now.ToString();
            InfoWriter.WriteLine(time + " : " + info);
            InfoWriter.Flush();

        }

        /// <summary>
        /// Write an error message to log.
        /// </summary>
        /// <param name="error">The error message to write.</param>
        public static void WriteError(string error)
        {
            string time = DateTime.Now.ToString();
            ErrorWriter.WriteLine(time + " : ERROR " + error);
            ErrorWriter.Flush();
            InfoWriter.WriteLine(time + " : ERROR " + error);
            InfoWriter.Flush();
        }

        public static void CloseLog()
        {
            InfoWriter.Close();
            InfoWriter.Dispose();
            ErrorWriter.Close();
            ErrorWriter.Dispose();
        }
    }
}
