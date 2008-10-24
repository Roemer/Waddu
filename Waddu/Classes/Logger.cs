using System.Collections.Generic;
using Waddu.Types;
using System.ComponentModel;

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

        public BindingList<LogEntry> GetEntries(LogType type)
        {
            BindingList<LogEntry> retList = new BindingList<LogEntry>();
            lock (_entryList)
            {
                foreach (LogEntry entry in _entryList)
                {
                    if ((int)entry.Type >= (int)type)
                    {
                        retList.Add(entry);
                    }
                }
            }
            return retList;
        }

        public void AddLog(LogType logType, string message, params object[] strParams)
        {
            LogEntry newEntry = new LogEntry(string.Format(message, strParams), logType);
            _entryList.Add(newEntry);
            if (LogEntry != null)
            {
                LogEntry(newEntry);
            }
        }

        public void Clear()
        {
            _entryList.Clear();
        }
    }
}
