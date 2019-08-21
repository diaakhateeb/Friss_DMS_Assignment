using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;

namespace CustomLogger
{
    /// <summary>
    /// Logging different system events.
    /// </summary>
    public class CustomLogger : ILogger
    {
        private readonly string _path;
        /// <summary>
        /// Creates CustomLogger instance.
        /// </summary>
        /// <param name="fileName">File path where events are getting registered.</param>
        public CustomLogger(string fileName)
        {
            if (!Directory.Exists(@"C\Logs")) Directory.CreateDirectory(@"C:\Logs");

            _path = @"C:\Logs\" + fileName;
            //_path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
            //            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))))) + @"\Logs\" + fileName;
        }
        /// <summary>
        /// Logs event.
        /// </summary>
        /// <typeparam name="TState">TState value.</typeparam>
        /// <param name="logLevel">Log level.</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="state">State.</param>
        /// <param name="exception">Exception object.</param>
        /// <param name="formatter">Log data Format.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            WriteTextToFile(message);
        }
        /// <summary>
        /// Logs event.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="exception">Exception object.</param>
        public void Log(LogLevel logLevel, Exception exception)
        {
            var message = $"{logLevel.ToString()}: {exception.Message} : {exception.Source}";
            WriteTextToFile(message);
        }
        /// <summary>
        /// Logs event.
        /// </summary>
        /// <param name="logLevel">Log level.</param>
        /// <param name="msg">Message to log.</param>
        /// <param name="source">source.</param>
        /// <param name="logger">logger Username.</param>
        /// <param name="httpStatusCode">HttpStatusCode value.</param>
        public void Log(LogLevel logLevel, string msg, string source, string logger, HttpStatusCode httpStatusCode)
        {
            var message = $"{logLevel.ToString()}: {msg} : {source} : {logger} : {httpStatusCode}";
            WriteTextToFile(message);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        private void WriteTextToFile(string message)
        {
            using (var streamWriter = new StreamWriter(_path, true))
            {
                streamWriter.WriteLine(message + "......." + DateTime.Now);
                streamWriter.Close();
            }
        }
    }
}
