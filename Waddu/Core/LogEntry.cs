using System;
using Waddu.Types;

namespace Waddu.Classes
{
    public class LogEntry
    {
        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
        }

        private LogType _type;
        public LogType Type
        {
            get { return _type; }
        }

        public LogEntry(string message, LogType type)
        {
            _date = DateTime.Now;
            _message = message;
            _type = type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (obj.GetType() != typeof(LogEntry)) { return false; }
            return (this as LogEntry).Date.Equals((obj as LogEntry).Date);
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
