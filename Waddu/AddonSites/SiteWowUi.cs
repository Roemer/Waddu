using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowUi : AddonSiteBase
    {
        private string _infoUrl = "http://wowui.worldofwar.net/?p=mod&m={tag}";
        private string _downUrl = "http://wowui.worldofwar.net/?p=download&m={tag}";
        private string _versionPrePattern = @"<title>";
        private string _versionPattern = @"(.*?) - .*";
        private string _datePattern = @".*<br />Updated <b>(.*?)</b>.*";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(string tag)
        {
            bool versionFound = false;
            bool dateFound = false;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url);

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
                            Helpers.AddOrUpdate<string, string>(_versionCache, tag, version);
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
                        string[] dateList = dateStr.Split('/');
                        DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
                        Helpers.AddOrUpdate<string, DateTime>(_dateCache, tag, dt);
                        dateFound = true;
                    }
                }
            }
        }

        public override string GetVersion(string tag)
        {
            if (!_versionCache.ContainsKey(tag))
            {
                ParseInfoSite(tag);
            }
            return _versionCache[tag];
        }

        public override DateTime GetLastUpdated(string tag)
        {
            if (!_dateCache.ContainsKey(tag))
            {
                ParseInfoSite(tag);
            }
            return _dateCache[tag];
        }

        public override string GetInfoLink(string tag)
        {
            return _infoUrl.Replace("{tag}", tag);
        }

        public override string GetDownloadLink(string tag)
        {
            return _downUrl.Replace("{tag}", tag);
        }

        #endregion
    }
}
