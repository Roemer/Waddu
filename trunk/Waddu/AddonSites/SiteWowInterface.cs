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
        private Dictionary<string, SiteAddon> _addonCache = new Dictionary<string, SiteAddon>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = new SiteAddon();

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
                            addon.VersionString = version;
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
                            string[] dateList = dateStr.Split(new char[] { '-' });
                            DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
                            addon.VersionDate = dt;
                            dateFound = true;
                        }
                    }
                }
            }

            Helpers.AddOrUpdate<string, SiteAddon>(_addonCache, mapping.AddonTag, addon);
        }

        public override string GetVersion(Mapping mapping)
        {
            if (!_addonCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            return _addonCache[mapping.AddonTag].VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            if (!_addonCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            return _addonCache[mapping.AddonTag].VersionDate;
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
