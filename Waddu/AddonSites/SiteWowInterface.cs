using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.BusinessObjects;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowInterface : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowinterface.com/downloads/info{tag}.html";
        private string _downUrl = "http://www.wowinterface.com/downloads/download{tag}";
        private string _versionPrePattern = @"<td class=""alt2""><div class=""infoboxfont1"">Version:</div></td>";
        private string _versionPattern = @"<td class=""alt2""><div class=""smallfont"">(.*)</div></td>";
        private string _datePrePattern = @"<td class=""alt1""><div class=""infoboxfont1"">Date:</div></td>";
        private string _datePattern = @"<td class=""alt1""><div class=""smallfont"">([^ ]*).*</div></td>";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(Mapping mapping)
        {
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            bool versionFound = false;
            bool dateFound = false;
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
                            Helpers.AddOrUpdate<string, string>(_versionCache, mapping.AddonTag, version);
                            versionFound = true;
                        }
                    }
                }

                if (!dateFound)
                {
                    Match m = Regex.Match(line, _datePrePattern);
                    if (m.Success)
                    {
                        string realLine = infoPage[i + 1];
                        m = Regex.Match(realLine, _datePattern);
                        if (m.Success)
                        {
                            string dateStr = m.Groups[1].Captures[0].Value;
                            string[] dateList = dateStr.Split('-');
                            DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
                            Helpers.AddOrUpdate<string, DateTime>(_dateCache, mapping.AddonTag, dt);
                            dateFound = true;
                        }
                    }
                }
            }
        }

        public override string GetVersion(Mapping mapping)
        {
            if (!_versionCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            return _versionCache[mapping.AddonTag];
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            if (!_dateCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            return _dateCache[mapping.AddonTag];
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            return _downUrl.Replace("{tag}", mapping.AddonTag);
        }

        #endregion
    }
}
