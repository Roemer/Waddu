using System;
using System.ComponentModel;
using Waddu.Core.AddonSites;
using Waddu.Types;

namespace Waddu.Core.BusinessObjects
{
    public class Mapping : INotifyPropertyChanged
    {
        #region Members
        public Addon Addon { get; set; }
        public Package Package { get; set; }

        private string _addonTag;
        public string AddonTag
        {
            get { return _addonTag; }
            set { _addonTag = value; NotifyPropertyChanged("AddonTag"); }
        }

        private AddonSiteId _addonSiteId;
        public AddonSiteId AddonSiteId
        {
            get { return _addonSiteId; }
            set { _addonSiteId = value; NotifyPropertyChanged("AddonSiteId"); }
        }

        private string _remoteVersion;
        public string RemoteVersion
        {
            get { return _remoteVersion; }
            set { _remoteVersion = value; NotifyPropertyChanged("RemoteVersion"); }
        }

        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set { _lastUpdated = value; NotifyPropertyChanged("LastUpdated"); }
        }
        #endregion

        #region Constructors
        public Mapping(string addonTag, AddonSiteId addonSiteId)
        {
            _addonTag = addonTag;
            _addonSiteId = addonSiteId;

            _remoteVersion = string.Empty;
            _lastUpdated = DateTime.MinValue;
        }

        #endregion

        #region Functions
        /// <summary>
        /// Get the Remote Version of an Addon
        /// </summary>
        public void CheckRemote()
        {
            var addonSite = AddonSiteBase.GetSite(AddonSiteId);
            RemoteVersion = addonSite.GetVersion(this);
            LastUpdated = addonSite.GetLastUpdated(this);

            // Hack to fire Property Changed
            Addon.PreferredMapping = Addon.PreferredMapping;
        }

        public string GetChangeLog()
        {
            var site = AddonSiteBase.GetSite(AddonSiteId);
            return site.GetChangeLog(this);
        }

        public string GetInfoLink()
        {
            var site = AddonSiteBase.GetSite(AddonSiteId);
            return site.GetInfoLink(this);
        }

        public string GetFilePath()
        {
            var site = AddonSiteBase.GetSite(AddonSiteId);
            return site.GetFilePath(this);
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})",
                RemoteVersion == string.Empty ? "?" : RemoteVersion,
                LastUpdated == DateTime.MinValue ? "?" : LastUpdated.ToShortDateString(),
                AddonSiteId
            );
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}
