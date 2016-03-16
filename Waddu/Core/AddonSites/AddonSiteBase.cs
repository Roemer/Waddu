using System;
using System.Collections.Generic;
using Waddu.Core.BusinessObjects;
using Waddu.Types;

namespace Waddu.Core.AddonSites
{
    public abstract class AddonSiteBase
    {
        private static readonly Dictionary<AddonSiteId, AddonSiteBase> SiteDict = new Dictionary<AddonSiteId, AddonSiteBase>();

        #region Overridable Functions
        public abstract string GetVersion(Mapping mapping);

        public abstract DateTime GetLastUpdated(Mapping mapping);

        public abstract string GetChangeLog(Mapping mapping);

        public abstract string GetInfoLink(Mapping mapping);

        public abstract string GetFilePath(Mapping mapping);
        #endregion

        #region Helper Functions
        public string FormatVersion(Mapping mapping, string versionString)
        {
            var retString = versionString;
            if (mapping.AddonSiteId == AddonSiteId.wowace || mapping.AddonSiteId == AddonSiteId.curseforge || mapping.AddonSiteId == AddonSiteId.curse)
            {
                // Remove "AddonName-" if existent
                if (retString.StartsWith(mapping.Addon.Name + "-"))
                {
                    retString = retString.Substring(mapping.Addon.Name.Length + 1);
                }
            }
            return retString;
        }

        public static AddonSiteBase GetSite(int addonSiteId)
        {
            return GetSite((AddonSiteId)addonSiteId);
        }

        public static AddonSiteBase GetSite(AddonSiteId addonSiteId)
        {
            AddonSiteBase site = null;
            // Lock the Dictionary
            lock (SiteDict)
            {
                // Get the Site if it's already created
                if (SiteDict.ContainsKey(addonSiteId))
                {
                    site = SiteDict[addonSiteId];
                }
                // If not, create it
                if (site == null)
                {
                    if (addonSiteId == AddonSiteId.wowace)
                    {
                        site = new SiteWowAce();
                    }
                    else if (addonSiteId == AddonSiteId.wowinterface)
                    {
                        site = new SiteWowInterface();
                    }
                    else if (addonSiteId == AddonSiteId.wowui)
                    {
                        site = new SiteWowUi();
                    }
                    else if (addonSiteId == AddonSiteId.curse)
                    {
                        site = new SiteCurse();
                    }
                    else if (addonSiteId == AddonSiteId.curseforge)
                    {
                        site = new SiteCurseForge();
                    }
                    else if (addonSiteId == AddonSiteId.blizzard)
                    {
                        site = new SiteBlizzard();
                    }
                    else if (addonSiteId == AddonSiteId.wowspecial)
                    {
                        site = new SiteWowSpecial();
                    }
                    else if (addonSiteId == AddonSiteId.direct)
                    {
                        site = new SiteDirect();
                    }
                    // Add the Site to the Dict
                    SiteDict.Add(addonSiteId, site);
                }
            }
            // Return the site
            return site;
        }
        #endregion
    }
}
