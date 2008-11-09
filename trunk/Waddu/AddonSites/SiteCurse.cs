﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.BusinessObjects;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteCurse : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curse.com/downloads/wow-addons/details/{tag}.aspx";
        private string _downUrl = "http://wow.curse.com{0}";
        private string _versionPrePattern = @"<th>Current Version:</th>";
        private string _versionPattern = @"<a href=""(.*.aspx)"">(.*)</a>";
        private string _updatedPattern = @"<td><script>document.write\(Curse.Utils.getDateSince\((.*)000\)\);</script>(.*)</td>";
        private string _downloadPattern = @"<a class=""button button-pop"" href=""(.*)""><span>Manual Install</span></a>";
        private Dictionary<string, SiteAddon> _addonCache = new Dictionary<string, SiteAddon>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = new SiteAddon();

            bool versionFound = false;
            bool dateFound = false;

            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            List<string> infoPage = WebHelper.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                // Version Check / Download Url
                if (!versionFound)
                {
                    Match m = Regex.Match(line, _versionPrePattern);
                    if (m.Success)
                    {
                        string realLine = infoPage[i + 2];
                        m = Regex.Match(realLine, _versionPattern);
                        if (m.Success)
                        {
                            string version = m.Groups[2].Captures[0].Value;
                            version = FormatVersion(mapping, version);
                            addon.VersionString = version;
                            string file = string.Format(_downUrl, m.Groups[1].Captures[0].Value);
                            addon.FileUrl = file;
                            versionFound = true;
                        }
                    }
                }

                // Last Update Check
                if (!dateFound)
                {
                    Match m = Regex.Match(line, _updatedPattern);
                    if (m.Success)
                    {
                        string date = m.Groups[1].Captures[0].Value;
                        DateTime dt = UnixTimeStamp.GetDateTime(Convert.ToDouble(date));
                        addon.VersionDate = dt;
                        dateFound = true;
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
            if (!_addonCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            string fileUrl = _addonCache[mapping.AddonTag].FileUrl;

            string downloadUrl = string.Empty;
            List<string> filePage = WebHelper.GetHtml(fileUrl);
            for (int i = 0; i < filePage.Count; i++)
            {
                string line = filePage[i];
                Match m = Regex.Match(line, _downloadPattern);
                if (m.Success)
                {
                    downloadUrl = string.Format(_downUrl, m.Groups[1].Captures[0].Value);
                    downloadUrl = downloadUrl.Replace("&amp;", "&");
                    break;
                }
            }
            return downloadUrl;
        }

        #endregion
    }
}
