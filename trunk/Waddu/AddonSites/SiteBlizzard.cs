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

        public override string GetDownloadLink(string tag)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
