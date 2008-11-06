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
        private Dictionary<string, SiteAddon> _addonCache = new Dictionary<string, SiteAddon>();
        private Dictionary<string, SiteAddon> _noLibCache = new Dictionary<string, SiteAddon>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = new SiteAddon();
            SiteAddon noLib = new SiteAddon();

            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            bool versionFound = false;
            bool dateFound = false;
            bool nolibVersionFound = false;
            bool nolibDateFound = false;

            List<string> infoPage = WebHelper.GetHtml(url, AddonSiteId.wowace);
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
                        // Remove "AddonName-" if existent
                        if (versionString.StartsWith(mapping.Addon.Name))
                        {
                            versionString = versionString.Substring(mapping.Addon.Name.Length + 1);
                        }
                        string fileUrl = m.Groups[1].Captures[0].Value;
                        addon.VersionString = versionString;
                        addon.FileUrl = string.Format(_fileUrl, fileUrl);
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
                        noLib.VersionString = versionString;
                        noLib.FileUrl = string.Format(_fileUrl, fileUrl);
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
                        addon.VersionDate = dt;
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
                        noLib.VersionDate = dt;
                        nolibDateFound = true;
                    }
                }
            }

            // Add the Addons to the Cache
            Helpers.AddOrUpdate<string, SiteAddon>(_addonCache, mapping.AddonTag, addon);
            Helpers.AddOrUpdate<string, SiteAddon>(_noLibCache, mapping.AddonTag, noLib);
        }

        private SiteAddon GetSiteAddon(Mapping mapping)
        {
            if (!_addonCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            SiteAddon addon = _addonCache[mapping.AddonTag];
            SiteAddon noLibAddon = _noLibCache[mapping.AddonTag];

            // Lib Addons or no NoLib Addon Found
            if (!Config.Instance.PreferNoLib || noLibAddon.VersionString == string.Empty)
            {
                return addon;
            }
            else
            {
                // Force NoLib
                if (Config.Instance.UseOlderNoLib)
                {
                    return noLibAddon;
                }
                else
                {
                    string version2 = noLibAddon.VersionString.Replace("-nolib", "");
                    string version1 = addon.VersionString.Substring(0, version2.Length);
                    // Versions are the same
                    if (version1 == version2)
                    {
                        return noLibAddon;
                    }
                    else
                    {
                        return addon;
                    }
                }
            }
        }

        public override string GetVersion(Mapping mapping)
        {
            return GetSiteAddon(mapping).VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            return GetSiteAddon(mapping).VersionDate;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            string fileUrl = GetSiteAddon(mapping).FileUrl;

            string downloadUrl = string.Empty;
            List<string> filePage = WebHelper.GetHtml(fileUrl, AddonSiteId.wowace);
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
