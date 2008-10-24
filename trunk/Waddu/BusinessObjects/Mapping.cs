using System.ComponentModel;
using Waddu.AddonSites;
using Waddu.Classes;
using Waddu.Types;
using System;

namespace Waddu.BusinessObjects
{
    public class Mapping : INotifyPropertyChanged
    {
        #region Members
        private Addon _addon;
        public Addon Addon
        {
            get { return _addon; }
            set { _addon = value; }
        }

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
        public Mapping()
        {
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
            AddonSiteBase addonSite = AddonSiteBase.GetSite(AddonSiteId);
            RemoteVersion = addonSite.GetVersion(AddonTag);
            LastUpdated = addonSite.GetLastUpdated(AddonTag);

            // Hack to fire Property Changed
            Addon.BestMapping = Addon.BestMapping;
        }

        public override string ToString()
        {
            if (RemoteVersion == string.Empty)
            {

            }
            return string.Format("{0}/{1}/{2}", 
                AddonSiteId,
                RemoteVersion == string.Empty ? "?" : RemoteVersion,
                LastUpdated == DateTime.MinValue ? "?" : LastUpdated.ToShortDateString()
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
