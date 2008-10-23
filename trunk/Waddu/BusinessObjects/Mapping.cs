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
        private string _addonName;
        public string AddonName
        {
            get { return _addonName; }
            set { _addonName = value; NotifyPropertyChanged("AddonName"); }
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
        }
        public Mapping(string addonName)
            : this()
        {
            AddonName = addonName;
        }
        public Mapping(string addonName, string addonTag)
            : this(addonName)
        {
            AddonTag = addonTag;
        }
        public Mapping(string addonName, string addonTag, AddonSiteId addonSiteId)
            : this(addonName, addonTag)
        {
            AddonSiteId = addonSiteId;
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
            //LastUpdated = addonSite.GetLastUpdated(AddonTag);
        }

        public override string ToString()
        {
            return AddonSiteId.ToString();
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
