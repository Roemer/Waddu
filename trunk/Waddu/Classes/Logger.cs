using System;

namespace Waddu.Classes
{
    public delegate void LogEntryEventHandler(string message);
    public class Logger
    {
        public static event LogEntryEventHandler LogEntry;

        private Logger()
        {
        }

        public static void AddLog(string message, params object[] strParams)
        {
            if (LogEntry != null)
            {
                LogEntry(string.Format(message, strParams));
            }
        }
    }
}
