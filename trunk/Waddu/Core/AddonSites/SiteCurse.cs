using System;
using System.Windows.Forms;
using HtmlAgilityPack;
using Waddu.Core.BusinessObjects;
using Waddu.Types;
using Waddu.UI.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Waddu.Core.AddonSites
{
    public class SiteCurse : AddonSiteBase
    {
        private string _infoUrl = "http://www.curse.com/addons/wow/{tag}";
        private string _downUrl = "http://www.curse.com/addons/wow/{tag}/download";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            // Build the Url
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            // Get the Html
            string html = string.Join("", WebHelper.GetHtml(url, mapping.AddonSiteId).ToArray());
            if (string.IsNullOrEmpty(html))
            {
                return;
            }
            // Get the Document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Get the Version
            HtmlNode versionNode = doc.DocumentNode.SelectSingleNode("//li[@class='newest-file']");
            string version = versionNode.InnerText.Replace("Newest File: ", string.Empty);
            // Get the Date
            HtmlNode dateNode = doc.DocumentNode.SelectSingleNode("//li[@class='updated']/abbr");
            string dateValue = dateNode.Attributes["data-epoch"].Value;
            DateTime date = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateValue));

            // Assign the Values
            addon.VersionString = version;
            addon.FileUrl = _downUrl.Replace("{tag}", mapping.AddonTag);
            addon.VersionDate = date;
        }

        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionDate;
        }

        public override string GetChangeLog(Mapping mapping)
        {
            string fileUrl = _infoUrl.Replace("{tag}", mapping.AddonTag);

            // Get the Html
            string html = string.Join("", WebHelper.GetHtml(fileUrl).ToArray());
            // Get the Document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            string changeLog = doc.DocumentNode.SelectSingleNode("//div[@id='tab-changes']").InnerHtml;
            return changeLog;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            string fileUrl = addon.FileUrl;

            string downloadUrl = string.Empty;
            WebBrowserForm form = new WebBrowserForm(fileUrl, AddonSiteId.curse, mapping.Addon.Name);
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
