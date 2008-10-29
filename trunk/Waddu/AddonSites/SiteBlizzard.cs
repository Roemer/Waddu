using System;
using Waddu.BusinessObjects;

namespace Waddu.AddonSites
{
    public class SiteBlizzard : AddonSiteBase
    {
        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            return "None";
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            return DateTime.Now;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return "http://www.worldofwarcraft.com";
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            return "http://www.worldofwarcraft.com";
        }
        #endregion
    }
}
