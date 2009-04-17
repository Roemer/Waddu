using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core.AddonSites
{
    public class SiteWowUi : AddonSiteBase
    {
        private string _infoUrl = "http://wowui.worldofwar.net/?p=mod&m={tag}";
        private string _downUrl = "http://wowui.worldofwar.net/?p=download&m={tag}";
        private string _versionPrePattern = @"<title>";
        private string _versionPattern = @"(.*?) - .*";
        private string _datePattern = @".*<br />Updated <b>(.*?)</b>.*";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);

            bool versionFound = false;
            bool dateFound = false;
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            List<string> infoPage = WebHelper.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                if (!versionFound)
                {
                    Match m = Regex.Match(line, _versionPrePattern);
                    if (m.Success)
                    {
                        string realLine = infoPage[i + 1];
                        m = Regex.Match(realLine, _versionPattern);
                        if (m.Success)
                        {
                            string version = m.Groups[1].Captures[0].Value;
                            addon.VersionString = version;
                            versionFound = true;
                        }
                    }
                }

                if (!dateFound)
                {
                    Match m = Regex.Match(line, _datePattern);
                    if (m.Success)
                    {
                        string dateStr = m.Groups[1].Captures[0].Value;
                        string[] dateList = dateStr.Split(new char[] { '/' });
                        DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
                        addon.VersionDate = dt;
                        dateFound = true;
                    }
                }
            }
        }

        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionDate;
        }

        public override string GetChangeLog(Mapping mapping)
        {
            return string.Empty;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            return _downUrl.Replace("{tag}", mapping.AddonTag);
        }
        #endregion
    }
}
