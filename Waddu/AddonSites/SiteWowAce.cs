using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.BusinessObjects;
using Waddu.Classes;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public class SiteWowAce : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowace.com/projects/{tag}/files/";
        private string _fileUrl = "http://www.wowace.com{0}";
        private string _versionPattern = @"<td class=""first""><a href=""(.*)"">(.*)</a></td>";
        private string _datePattern = @"<span class=""date"" title="".*"">(.*)</span>";
        private string _downloadPattern = @"<a href=""(.*)""><span>Download</span></a>";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();
        private Dictionary<string, string> _fileLinkCache = new Dictionary<string, string>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(Mapping mapping)
        {
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            bool versionFound = false;
            bool dateFound = false;
            bool nolibVersionFound = false;
            bool nolibDateFound = false;

            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.wowace);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                // Version Check
                if (!versionFound)
                {
                    Match m = Regex.Match(line, _versionPattern);
                    if (m.Success)
                    {
                        string versionString = m.Groups[2].Captures[0].Value;
                        // Remove <AddonName-> if existent
                        if (versionString.StartsWith(mapping.Addon.Name))
                        {
                            versionString = versionString.Substring(mapping.Addon.Name.Length + 1);
                        }
                        string fileUrl = m.Groups[1].Captures[0].Value;
                        Helpers.AddOrUpdate<string, string>(_versionCache, mapping.AddonTag, versionString);
                        Helpers.AddOrUpdate<string, string>(_fileLinkCache, mapping.AddonTag, string.Format(_fileUrl, fileUrl));
                        versionFound = true;
                    }
                }
                // NoLib Check
                else if (Config.Instance.PreferNoLib && !nolibVersionFound)
                {
                    Match m = Regex.Match(line, _versionPattern);
                    if (m.Success)
                    {
                        string versionString = m.Groups[2].Captures[0].Value;
                        // Remove <AddonName-> if existent
                        if (!versionString.ToLower().Contains("nolib"))
                        {
                            continue;
                        }
                        if (versionString.StartsWith(mapping.Addon.Name))
                        {
                            versionString = versionString.Substring(mapping.Addon.Name.Length + 1);
                        }
                        string fileUrl = m.Groups[1].Captures[0].Value;
                        Helpers.AddOrUpdate<string, string>(_versionCache, mapping.AddonTag, versionString);
                        Helpers.AddOrUpdate<string, string>(_fileLinkCache, mapping.AddonTag, string.Format(_fileUrl, fileUrl));
                        nolibVersionFound = true;
                    }
                }

                // Last Update Check
                if (!dateFound)
                {
                    Match m = Regex.Match(line, _datePattern);
                    if (m.Success)
                    {
                        string dateStr = m.Groups[1].Captures[0].Value;
                        string[] dateList = dateStr.Split('/');
                        DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
                        Helpers.AddOrUpdate<string, DateTime>(_dateCache, mapping.AddonTag, dt);
                        dateFound = true;
                    }
                }
                // NoLib Check
                else if (Config.Instance.PreferNoLib && !nolibDateFound)
                {
                    Match m = Regex.Match(line, _datePattern);
                    if (m.Success)
                    {
                        string dateStr = m.Groups[1].Captures[0].Value;
                        string[] dateList = dateStr.Split('/');
                        DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
                        Helpers.AddOrUpdate<string, DateTime>(_dateCache, mapping.AddonTag, dt);
                        nolibDateFound = true;
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
            if (!_fileLinkCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            string fileUrl = _fileLinkCache[mapping.AddonTag];

            string downloadUrl = string.Empty;
            List<string> filePage = Helpers.GetHtml(fileUrl, AddonSiteId.wowace);
            for (int i = 0; i < filePage.Count; i++)
            {
                string line = filePage[i];
                Match m = Regex.Match(line, _downloadPattern);
                if (m.Success)
                {
                    downloadUrl = m.Groups[1].Captures[0].Value;
                    break;
                }
            }
            return downloadUrl;
        }

        #endregion
    }
}
