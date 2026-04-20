using Newtonsoft.Json;
using NLog;
using System;
using System.Diagnostics;
using System.Text;

namespace Elipgo.SmartClient.Common
{
    public enum LogPriority { Verbose = 0, Information = 1, Warning = 2, Important = 3, Fatal = 4, Sentry = 5 }
    public class Logger
    {
        public static LogPriority LoggerVerbosity = LogPriority.Information;
        public delegate void OnEvent(object who, string what, LogPriority priority);
        public static event OnEvent Event;

        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        public static void Log(string message, LogPriority logPriority = LogPriority.Verbose)
        {
            NLog(new object(), message, logPriority);
        }

        public static void Log(object who, string what, string path, string token, LogPriority logPriority = LogPriority.Verbose)
        {
            string response = JsonConvert.SerializeObject(who).Replace("{", "").Replace("}", "");
            StringBuilder message = new StringBuilder();
            message.Append("\n------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            message.Append("\nMethod --> " + what);

            if (!string.IsNullOrEmpty(path))
            {
                message.Append("\nPath --> " + path);
            }

            if (!string.IsNullOrEmpty(token))
            {
                message.Append("\nToken --> " + token);
            }

            message.Append("\nResponse --> " + response);
            message.Append("\n------------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Log(message.ToString(), logPriority);
        }

        public static void Log(Exception Error, LogPriority logPriority = LogPriority.Verbose)
        {
            try
            {
                StringBuilder ErrorMessage = new StringBuilder();
                ErrorMessage.Append("Message --> " + Error.Message);
                ErrorMessage.Append("\nStackTrace --> " + Error.StackTrace);
                ErrorMessage.Append("\nSource --> " + Error.Source);
                ErrorMessage.Append("\nInnerException --> " + Error.InnerException);
                ErrorMessage.Append("\nTargetSite --> " + Error.TargetSite);
                ErrorMessage.Append("\nData --> " + Error.Data);
                if (LogPriority.Sentry == logPriority)
                {
                    Sentry.SentrySdk.CaptureException(Error);
                }

                Log(ErrorMessage.ToString(), logPriority);
            }
            catch
            {
            }
        }

        public static void Log(Exception e)
        {
            try
            {
                // Since we can't prevent the app from terminating, log this to the event log.
                if (!EventLog.SourceExists("SmartClientException"))
                {
                    EventLog.CreateEventSource("SmartClientException", "Elipgo.SmartClient");
                }
                EventLog LogError = new EventLog();
                Exception Error = e;
                StringBuilder ErrorMessage = new StringBuilder();
                ErrorMessage.Append("Message --> " + Error.Message);
                ErrorMessage.Append("\nStackTrace --> " + Error.StackTrace);
                ErrorMessage.Append("\nSource --> " + Error.Source);
                ErrorMessage.Append("\nInnerException --> " + Error.InnerException);
                ErrorMessage.Append("\nTargetSite --> " + Error.TargetSite);
                ErrorMessage.Append("\nData --> " + Error.Data);
                LogError.Source = "SmartClientException";
                LogError.WriteEntry(ErrorMessage.ToString(), EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                Log(ex, LogPriority.Fatal);
            }
        }

        private async static void NLog(object who, string what, LogPriority logPriority)
        {
            // Escapar llaves para que NLog no las interprete como placeholders de structured logging
            var escapedMessage = what?.Replace("{", "{{").Replace("}", "}}");
            string stackTrace = string.Empty;
            if (who is Exception ex)
            {
                stackTrace = ex.StackTrace;
            }


            switch (logPriority)
            {
                case LogPriority.Verbose:
                    logger.Trace($"{escapedMessage} stackTrace: {stackTrace}");
                    break;

                case LogPriority.Information:
                    logger.Info($"{escapedMessage} stackTrace: {stackTrace}");
                    break;

                case LogPriority.Warning:
                    logger.Error($"{escapedMessage} stackTrace: {stackTrace}");
                    break;

                case LogPriority.Important:
                    logger.Warn($"{escapedMessage} stackTrace: {stackTrace}");
                    break;
                case LogPriority.Sentry:
                case LogPriority.Fatal:
                    logger.Fatal($"{escapedMessage} stackTrace: {stackTrace}");
                    break;
            }
        }
    }
}
