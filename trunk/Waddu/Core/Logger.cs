using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Waddu.Types;

namespace Waddu.Core
{
    public delegate void LogEntryEventHandler(LogEntry entry);

    public class Logger
    {
        public static Logger Instance = new Logger();

        public event LogEntryEventHandler LogEntry;

        private readonly List<LogEntry> _entryList;
        private readonly string _logFilePath;

        private Logger()
        {
            var logFileName = "log.txt";
            _logFilePath = Path.Combine(Application.StartupPath, logFileName);
            _entryList = new List<LogEntry>();
        }

        public BindingList<LogEntry> GetEntries(LogType type)
        {
            var retList = new BindingList<LogEntry>();
            lock (this)
            {
                foreach (var entry in _entryList)
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
            var newEntry = new LogEntry(string.Format(message, strParams), logType);
            lock (this)
            {
                _entryList.Add(newEntry);
                File.AppendAllText(_logFilePath, newEntry + Environment.NewLine);
            }
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
