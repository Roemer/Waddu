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
            throw new NotImplementedException();
        }

        public override string GetInfoLink(string tag)
        {
            throw new NotImplementedException();
        }

        public override string GetDownloadLink(string tag)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
