using System.ComponentModel;
using Waddu.AddonSites;
using Waddu.Classes;
using Waddu.Types;

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

        public string RemoteVersion
        {
            get { return GetRemoteVersion(); }
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
        private string GetRemoteVersion()
        {
            AddonSiteBase addonSite = AddonSiteBase.GetSite(AddonSiteId);
            return addonSite.GetVersion(AddonTag);
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
