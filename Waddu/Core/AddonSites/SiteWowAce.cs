using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Waddu.Core.BusinessObjects;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core.AddonSites
{
    public class SiteWowAce : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowace.com/projects/{tag}/files/";
        private string _fileUrl = "http://www.wowace.com{0}";
        private string _versionPattern = "<td class=\"first\"><a href=\"(.*)\">(.*)</a></td>";
        private string _datePattern = "<span class=\"date\" title=\".*\" data-epoch=\"(.*)\">.*</span>";
        private string _downloadPattern = "<a href=\"(.*)\">Download</a>";
        private SiteAddonCache _addonCache = new SiteAddonCache();
        private SiteAddonCache _noLibCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();
            SiteAddon noLibAddon = _noLibCache.Get(mapping.AddonTag);
            noLibAddon.Clear();

            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            bool versionFound = false;
            bool dateFound = false;
            bool nolibVersionFound = false;
            bool nolibDateFound = false;

            List<string> infoPage = WebHelper.GetHtml(url, mapping.AddonSiteId);
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
                        noLibAddon.VersionString = versionString;
                        noLibAddon.FileUrl = string.Format(_fileUrl, fileUrl);
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
                        DateTime dt = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateStr));
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
                        DateTime dt = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateStr));
                        noLibAddon.VersionDate = dt;
                        nolibDateFound = true;
                    }
                }
            }
        }

        private SiteAddon GetSiteAddon(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            SiteAddon noLibAddon = _noLibCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }

            // Lib Addons or no NoLib Addon Found
            if (!Config.Instance.PreferNoLib || !noLibAddon.IsCollected)
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

        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            return GetSiteAddon(mapping).VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            return GetSiteAddon(mapping).VersionDate;
        }

        public override string GetChangeLog(Mapping mapping)
        {
            string fileUrl = GetSiteAddon(mapping).FileUrl;
            List<string> filePage = WebHelper.GetHtml(fileUrl, mapping.AddonSiteId);
            string changeLog = string.Empty;
            bool changeLogAdd = false;
            for (int i = 0; i < filePage.Count; i++)
            {
                string line = filePage[i];
                if (line.Contains(@"<dt>Change Log</dt>"))
                {
                    changeLogAdd = true;
                    i += 4;
                    continue;
                }
                if (line.Contains(@"</div>") && changeLogAdd)
                {
                    break;
                }
                if (changeLogAdd)
                {
                    changeLog += line;
                }
            }

            return changeLog;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            string fileUrl = GetSiteAddon(mapping).FileUrl;

            string downloadUrl = string.Empty;
            List<string> filePage = WebHelper.GetHtml(fileUrl, mapping.AddonSiteId);
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

            // This is the URL with the Captcha
            downloadUrl = "http://www.wowace.com" + downloadUrl;

            WebBrowserForm form = new WebBrowserForm(fileUrl, AddonSiteId.wowace);
            if (form.ShowDialog() == DialogResult.OK)
            {
                downloadUrl = form.DownloadUrl;
            }

            return downloadUrl;
        }
        #endregion
    }
}
