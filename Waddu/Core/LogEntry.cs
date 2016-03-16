using System;
using Waddu.Types;

namespace Waddu.Core
{
    public class LogEntry
    {
        public DateTime Date { get; private set; }

        public string Message { get; private set; }

        public LogType Type { get; private set; }

        public LogEntry(string message, LogType type)
        {
            Date = DateTime.Now;
            Message = message;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (obj.GetType() != typeof(LogEntry)) { return false; }
            return Date.Equals((obj as LogEntry).Date);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} - {2}", Date, Type, Message);
        }
    }
}
