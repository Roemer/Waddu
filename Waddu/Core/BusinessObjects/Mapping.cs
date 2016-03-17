using System;
using Waddu.Core.AddonSites;
using Waddu.Types;

namespace Waddu.Core.BusinessObjects
{
    public class Mapping : ObservableObject
    {
        public Addon Addon { get; set; }
        public Package Package { get; set; }

        public AddonSiteId AddonSiteId
        {
            get { return GetProperty<AddonSiteId>(); }
            set { SetProperty(value); }
        }

        public string AddonTag
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string RemoteVersion
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public DateTime LastUpdated
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public Mapping(AddonSiteId addonSiteId, string addonTag)
        {
            AddonSiteId = addonSiteId;
            AddonTag = addonTag;

            RemoteVersion = String.Empty;
            LastUpdated = DateTime.MinValue;
        }

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
    }
}
