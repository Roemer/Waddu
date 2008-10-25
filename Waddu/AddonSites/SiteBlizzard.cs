using System;

namespace Waddu.AddonSites
{
    public class SiteBlizzard : AddonSiteBase
    {
        #region AddonSiteBase Overrides
        public override string GetVersion(string tag)
        {
            return "None";
        }

        public override DateTime GetLastUpdated(string tag)
        {
            return DateTime.Now;
        }

        public override string GetInfoLink(string tag)
        {
            return "http://www.worldofwarcraft.com";
        }

        public override string GetDownloadLink(string tag)
        {
            return "http://www.worldofwarcraft.com";
        }
        #endregion
    }
}
