using System;
using System.Text.RegularExpressions;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core.AddonSites
{
    public class SiteWowUi : AddonSiteBase
    {
        private string _infoUrl = "http://wowui.incgamers.com/?p=mod&m={tag}";
        private string _downUrl = "http://wowui.incgamers.com/?p=download&m={tag}";
        private string _versionPrePattern = @"<title>";
        private string _versionPattern = @"(.*?) - .*";
        private string _datePattern = @".*<br />Updated <b>(.*?)</b>.*";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);

            var versionFound = false;
            var dateFound = false;
            var url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            var infoPage = WebHelper.GetHtml(url);

            for (var i = 0; i < infoPage.Count; i++)
            {
                var line = infoPage[i];

                if (!versionFound)
                {
                    var m = Regex.Match(line, _versionPrePattern);
                    if (m.Success)
                    {
                        var realLine = infoPage[i + 1];
                        m = Regex.Match(realLine, _versionPattern);
                        if (m.Success)
                        {
                            var version = m.Groups[1].Captures[0].Value;
                            addon.VersionString = version;
                            versionFound = true;
                        }
                    }
                }

                if (!dateFound)
                {
                    var m = Regex.Match(line, _datePattern);
                    if (m.Success)
                    {
                        var dateStr = m.Groups[1].Captures[0].Value;
                        var dateList = dateStr.Split(new char[] { '/' });
                        var dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
                        addon.VersionDate = dt;
                        dateFound = true;
                    }
                }
            }
        }

        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
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
