using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.BusinessObjects;
using Waddu.Classes;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public class SiteCurseForge : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curseforge.com/projects/{tag}/files/";
        private string _fileUrl = "http://wow.curseforge.com{0}";
        private string _versionPattern = @"<td class=""first""><a href=""(.*)"">(.*)</a></td>";
        private string _datePattern = @"<span class=""date"" title="".*"">(.*)</span>";
        private string _downloadPrePattern = @"<th>Filename:</th>";
        private string _downloadPattern = @"<td><a href=""(.*)"">.*</a></td>";
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

            List<string> infoPage = WebHelper.GetHtml(url, AddonSiteId.curseforge);
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
                        versionString = FormatVersion(mapping, versionString);
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
                        if (!versionString.ToLower().Contains("nolib"))
                        {
                            continue;
                        }
                        versionString = FormatVersion(mapping, versionString);
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
                        string[] dateList = dateStr.Split(new char[] { '/' });
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
                        string[] dateList = dateStr.Split(new char[] { '/' });
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
            if (!Config.Instance.PreferNoLib || !noLibAddon.IsValid)
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
                    string versionNoLib = noLibAddon.VersionString.Replace("-nolib", "");
                    if (addon.VersionString.Length >= versionNoLib.Length)
                    {
                        string versionWithLib = addon.VersionString.Substring(0, versionNoLib.Length);
                        // Versions are the same
                        if (versionWithLib == versionNoLib)
                        {
                            return noLibAddon;
                        }
                    }
                    return addon;
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
            List<string> filePage = WebHelper.GetHtml(fileUrl, AddonSiteId.curseforge);
            for (int i = 0; i < filePage.Count; i++)
            {
                string line = filePage[i];
                Match m = Regex.Match(line, _downloadPrePattern);
                if (m.Success)
                {
                    string realLine = filePage[i + 1];
                    m = Regex.Match(realLine, _downloadPattern);
                    if (m.Success)
                    {
                        downloadUrl = m.Groups[1].Captures[0].Value;
                        break;
                    }
                }
            }
            return downloadUrl;
        }

        #endregion
    }
}
