using System;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public abstract class AddonSiteBase
    {
        #region Overridable Functions
        public abstract string GetVersion(string tag);

        public abstract string GetDownloadLink(string tag);
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
            return site;
        }
        #endregion
    }
}
