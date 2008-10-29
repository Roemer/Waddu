using System;
using Waddu.BusinessObjects;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public abstract class AddonSiteBase
    {
        #region Overridable Functions
        public abstract string GetVersion(Mapping mapping);

        public abstract DateTime GetLastUpdated(Mapping mapping);

        public abstract string GetInfoLink(Mapping mapping);

        public abstract string GetDownloadLink(Mapping mapping);
        #endregion

        #region Helper Functions
        public static AddonSiteBase GetSite(int addonSiteId)
        {
            return GetSite((AddonSiteId)addonSiteId);
        }

        public static AddonSiteBase GetSite(AddonSiteId addonSiteId)
        {
            AddonSiteBase site = null;
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
            return site;
        }
        #endregion
    }
}
