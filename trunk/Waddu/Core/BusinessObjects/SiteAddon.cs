using System;

namespace Waddu.Core.BusinessObjects
{
    public class SiteAddon
    {
        private string _versionString = string.Empty;
        public string VersionString
        {
            get { return _versionString; }
            set { _versionString = value; }
        }

        private DateTime _versionDate = DateTime.MinValue;
        public DateTime VersionDate
        {
            get { return _versionDate; }
            set { _versionDate = value; }
        }

        private string _fileUrl = string.Empty;
        public string FileUrl
        {
            get { return _fileUrl; }
            set { _fileUrl = value; }
        }

        public bool IsValid
        {
            get
            {
                return _versionString.Length > 0;
            }
        }

        public SiteAddon()
        {
        }
    }
}
