﻿using System;
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
        private const string InfoUrl = "http://www.curseforge.com/projects/{tag}/files/";
        private const string FileUrl = "http://www.curseforge.com{0}";
        private readonly SiteAddonCache _addonCache = new SiteAddonCache();
        private readonly SiteAddonCache _noLibCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            // Clear the Caches
            var addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();
            var noLibAddon = _noLibCache.Get(mapping.AddonTag);
            noLibAddon.Clear();

            // Build the Url
            var url = InfoUrl.Replace("{tag}", mapping.AddonTag);
            // Get the Html
            var html = string.Join("", WebHelper.GetHtml(url, mapping.AddonSiteId).ToArray());
            // Get the Document
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Get the Row Object
            var rowNodes = doc.DocumentNode.SelectNodes("//table//tr[@class='project-file-list-item']");
            // Use the first Row by default
            var fileRow = rowNodes[0];
            HtmlNode noLibfileRow = null;
            if (Config.Instance.PreferNoLib)
            {
                // Search for a NoLib Version
                for (var i = 1; i < rowNodes.Count; i++)
                {
                    var rowNode = rowNodes[i];
                    // Check if the Filename Contains a nolib
                    if (rowNode.SelectSingleNode("td[6]").InnerText.Contains("-nolib"))
                    {
                        // It does, assign it
                        noLibfileRow = rowNode;
                        break;
                    }
                }
            }

            // Parse the Info for the Addon
            ParseRowInfo(fileRow, addon);

            if (Config.Instance.PreferNoLib && noLibfileRow != null)
            {
                // Parse the Info for the NoLib Addon
                ParseRowInfo(noLibfileRow, noLibAddon);
            }
        }

        private void ParseRowInfo(HtmlNode htmlRow, SiteAddon addon)
        {
            // Get the Version and Link
            var fileUrl = htmlRow.SelectSingleNode("//a[@title='Download File']").Attributes["href"].Value;
            var version = htmlRow.SelectSingleNode("//td[contains(@class, 'project-file-name')]/div/div[@class='project-file-name-container']/a").InnerText;
            // Get the Date
            var dateNode = htmlRow.SelectSingleNode("td[@class='project-file-date-uploaded']/abbr");
            var dateValue = dateNode.Attributes["data-epoch"].Value;
            var date = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateValue));
            // Assign the Values
            addon.VersionString = version;
            addon.FileUrl = string.Format(FileUrl, fileUrl);
            addon.VersionDate = date;
        }

        private SiteAddon GetSiteAddon(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            var noLibAddon = _noLibCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }

            // Lib Addons or no NoLib Addon Found
            if (!Config.Instance.PreferNoLib || !noLibAddon.IsCollected)
            {
                return addon;
            }
            // Force NoLib
            if (Config.Instance.UseOlderNoLib)
            {
                return noLibAddon;
            }
            var versionNoLib = noLibAddon.VersionString.Replace("-nolib", "");
            if (addon.VersionString.Length >= versionNoLib.Length)
            {
                var versionWithLib = addon.VersionString.Substring(0, versionNoLib.Length);
                // Versions are the same
                if (versionWithLib == versionNoLib)
                {
                    return noLibAddon;
                }
            }
            return addon;
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
            var fileUrl = GetSiteAddon(mapping).FileUrl;

            // Get the Html
            var html = string.Join("", WebHelper.GetHtml(fileUrl, mapping.AddonSiteId).ToArray());
            // Get the Document
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var changeLog = doc.DocumentNode.SelectSingleNode("//div[@class='content-box-inner']").InnerHtml;
            return changeLog;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return InfoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            var fileUrl = GetSiteAddon(mapping).FileUrl;
            // Get the Html
            var html = string.Join("", WebHelper.GetHtml(fileUrl, mapping.AddonSiteId).ToArray());
            // Get the Document
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var downloadUrl = doc.DocumentNode.SelectSingleNode("//li[@class='user-action user-action-download']/span/a").GetAttributeValue("href", string.Empty);

            var form = new WebBrowserForm(fileUrl, AddonSiteId.curseforge, mapping.Addon.Name);
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
