using System;

namespace Waddu.AddonSites
{
    public class SiteDirect : AddonSiteBase
    {
        #region AddonSiteBase Overrides
        public override string GetVersion(string tag)
        {
            string[] strList = tag.Split('|');
            return strList[0];
        }

        public override DateTime GetLastUpdated(string tag)
        {
            string[] strList = tag.Split('|');
            string dateStr = strList[1];
            string[] dateList = dateStr.Split('.');
            DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
            return dt;
        }

        public override string GetInfoLink(string tag)
        {
            string[] strList = tag.Split('|');
            return strList[2];
        }

        public override string GetDownloadLink(string tag)
        {
            string[] strList = tag.Split('|');
            return strList[3];
        }
        #endregion
    }
}
