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
        private string _logFilePath;

        private Logger()
        {
            string logFileName = "log.txt";
            _logFilePath = Path.Combine(Application.StartupPath, logFileName);
            _entryList = new List<LogEntry>();
        }

        public BindingList<LogEntry> GetEntries(LogType type)
        {
            BindingList<LogEntry> retList = new BindingList<LogEntry>();
            lock (this)
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
            lock (this)
            {
                _entryList.Add(newEntry);
                File.AppendAllText(_logFilePath, newEntry.ToString() + Environment.NewLine);
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
