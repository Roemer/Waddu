using System.Collections.Generic;
using Waddu.Types;

namespace Waddu.Classes
{
    public delegate void LogEntryEventHandler(LogEntry entry);
    public class Logger
    {
        private static Logger _instance;
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        public event LogEntryEventHandler LogEntry;

        private List<LogEntry> _entryList;

        private Logger()
        {
            _entryList = new List<LogEntry>();
        }

        public void AddLog(string message, params object[] strParams)
        {
            AddLog(message, LogType.Information, strParams);
        }
        public void AddLog(string message, LogType logType, params object[] strParams)
        {
            LogEntry newEntry = new LogEntry(string.Format(message, strParams), logType);
            _entryList.Add(newEntry);
            if (LogEntry != null)
            {
                LogEntry(newEntry);
            }
        }
    }
}
