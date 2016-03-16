using System;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core.AddonSites
{
    public class SiteDirect : AddonSiteBase
    {
        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            var strList = mapping.AddonTag.Split(new char[] { '|' });
            return strList[0];
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            var strList = mapping.AddonTag.Split(new char[] { '|' });
            var dateStr = strList[1];
            var dateList = dateStr.Split(new char[] { '.' });
            var dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
            return dt;
        }

        public override string GetChangeLog(Mapping mapping)
        {
            return string.Empty;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            var strList = mapping.AddonTag.Split(new char[] { '|' });
            return strList[2];
        }

        public override string GetFilePath(Mapping mapping)
        {
            var strList = mapping.AddonTag.Split(new char[] { '|' });
            return strList[3];
        }
        #endregion
    }
}
