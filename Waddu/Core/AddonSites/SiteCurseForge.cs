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

            // Get the Row Object
            HtmlNodeCollection rowNodes = doc.DocumentNode.SelectNodes("//table[@class='listing']/tbody/tr");
            // Use the first Row by default
            HtmlNode fileRow = rowNodes[0];
            HtmlNode noLibfileRow = null;
            if (Config.Instance.PreferNoLib)
            {
                // Search for a NoLib Version
                for (int i = 1; i < rowNodes.Count; i++)
                {
                    HtmlNode rowNode = rowNodes[i];
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
            HtmlNode fileNode = htmlRow.SelectSingleNode("td[@class='col-file']/a");
            string fileUrl = fileNode.Attributes["href"].Value;
            string version = fileNode.InnerText;
            // Get the Date
            HtmlNode dateNode = htmlRow.SelectSingleNode("td[@class='col-date']/span");
            string dateValue = dateNode.Attributes["data-epoch"].Value;
            DateTime date = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateValue));
            // Assign the Values
            addon.VersionString = version;
            addon.FileUrl = string.Format(_fileUrl, fileUrl);
            addon.VersionDate = date;
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

            // Get the Html
            string html = string.Join("", WebHelper.GetHtml(fileUrl, mapping.AddonSiteId).ToArray());
            // Get the Document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string changeLog = doc.DocumentNode.SelectSingleNode("//div[@class='content-box-inner']").InnerHtml;
            return changeLog;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            string fileUrl = GetSiteAddon(mapping).FileUrl;
            // Get the Html
            string html = string.Join("", WebHelper.GetHtml(fileUrl, mapping.AddonSiteId).ToArray());
            // Get the Document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string downloadUrl = doc.DocumentNode.SelectSingleNode("//li[@class='user-action user-action-download']/span/a").GetAttributeValue("href", string.Empty);

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
