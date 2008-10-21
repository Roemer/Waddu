using System;

namespace Waddu.AddonSites
{
    public class SiteDirect : AddonSiteBase
    {
        #region AddonSiteBase Overrides
        public override string GetVersion(string tag)
        {
            return "None";
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
