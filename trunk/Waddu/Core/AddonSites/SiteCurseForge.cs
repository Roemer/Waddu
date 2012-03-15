using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using HtmlAgilityPack;
using Waddu.Core.BusinessObjects;
using Waddu.Types;
using Waddu.UI.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Waddu.Core.AddonSites
{
    public class SiteCurseForge : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curseforge.com/addons/{tag}/files/";
        private string _fileUrl = "http://wow.curseforge.com{0}";
        private string _versionPattern = "<td class=\"col-file\"><a href=\"(.*)\">(.*)</a></td>";
        private string _datePattern = "<td class=\"col-date\"><span class=\"standard-date\" title=\".*\" data-epoch=\"(.*)\" data-shortdate=\".*\">.*</span></td>";
        private string _downloadPattern = "<ul class=\"user-actions user-actions-by-header\"><li class=\"user-action user-action-download\"><span><a href=\"(.*)\">Download</a>";
        private SiteAddonCache _addonCache = new SiteAddonCache();
        private SiteAddonCache _noLibCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            // Clear the Caches
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();
            SiteAddon noLibAddon = _noLibCache.Get(mapping.AddonTag);
            noLibAddon.Clear();

            // Build the Url
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            // Get the Html
            string html = string.Join("", WebHelper.GetHtml(url, mapping.AddonSiteId).ToArray());
            // Get the Document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Get the Version and Link
            // <a href="/addons/afflicted3/files/20-afflicted3-2-2-1/">Afflicted3 2.2.1</a>
            HtmlNode fileNode = doc.DocumentNode.SelectSingleNode("//table[@class='listing']/tbody/tr/td[@class='col-file']");
            string fUrl = fileNode.ChildNodes[0].Attributes["href"].Value;
            string version = fileNode.ChildNodes[0].InnerText;

            // Get the Date
            //<td class="col-date"><span class="standard-date" title="Mar 15, 2012 at 00:16 UTC" data-epoch="1331770599" data-shortdate="true">Mar 15, 2012</span></td>
            HtmlNode dateNode = doc.DocumentNode.SelectSingleNode("//table[@class='listing']/tbody/tr/td[@class='col-date']");
            string dateValue = dateNode.ChildNodes[0].Attributes["data-epoch"].Value;
            DateTime date = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateValue));

            //Config.Instance.PreferNoLib


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
                if (line.Contains(@"<h3>Change log</h3>"))
                {
                    changeLogAdd = true;
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

        public override string GetFilePath(Mapping mapping)
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
                    // This is the URL with the Captcha
                    downloadUrl = m.Groups[1].Captures[0].Value;
                    break;
                }
            }

            WebBrowserForm form = new WebBrowserForm(fileUrl, AddonSiteId.curseforge, mapping.Addon.Name);
            if (form.ShowDialog() == DialogResult.OK)
            {
                downloadUrl = form.UseFile ? form.FileUrl : form.DownloadUrl;
            }
            else
            {
                downloadUrl = string.Empty;
            }

            return downloadUrl;
        }
        #endregion
    }
}
