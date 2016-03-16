using System;

namespace Waddu.Core.BusinessObjects
{
    public class SiteAddon
    {
        private string _versionString;
        public string VersionString
        {
            get { return _versionString; }
            set { _versionString = value; SetUpdated(); }
        }

        private DateTime _versionDate;
        public DateTime VersionDate
        {
            get { return _versionDate; }
            set { _versionDate = value; SetUpdated(); }
        }

        private string _fileUrl;
        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl = value; SetUpdated(); }
        }

        public bool IsCollectRequired
        {
            get
            {
                if (IsExpired)
                {
                    Clear();
                }
                return !IsCollected;
            }
        }

        private DateTime _lastCheckingTime;
        private bool IsExpired
        {
            get
            {
                // Keep Entries for some Time
                return (DateTime.Now - _lastCheckingTime) >= new TimeSpan(0, 1, 0);
            }
        }

        public bool IsCollected { get; private set; }

        public SiteAddon()
        {
            Clear();
        }

        public void Clear()
        {
            _versionString = string.Empty;
            _versionDate = DateTime.MinValue;
            _fileUrl = string.Empty;

            _lastCheckingTime = DateTime.MinValue;
            IsCollected = false;
        }

        private void SetUpdated()
        {
            _lastCheckingTime = DateTime.Now;
            IsCollected = true;
        }
    }
}
