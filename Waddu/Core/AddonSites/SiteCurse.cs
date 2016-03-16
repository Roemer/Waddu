using System;
using System.Windows.Forms;
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
            var addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            // Build the Url
            var url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            // Get the Html
            var html = string.Join("", WebHelper.GetHtml(url, mapping.AddonSiteId).ToArray());
            if (string.IsNullOrEmpty(html))
            {
                return;
            }
            // Get the Document
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Get the Version
            var versionNode = doc.DocumentNode.SelectSingleNode("//li[@class='newest-file']");
            var version = versionNode.InnerText.Replace("Newest File: ", string.Empty);
            // Get the Date
            var dateNode = doc.DocumentNode.SelectSingleNode("//li[@class='updated']/abbr");
            var dateValue = dateNode.Attributes["data-epoch"].Value;
            var date = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateValue));

            // Assign the Values
            addon.VersionString = version;
            addon.FileUrl = _downUrl.Replace("{tag}", mapping.AddonTag);
            addon.VersionDate = date;
        }

        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionDate;
        }

        public override string GetChangeLog(Mapping mapping)
        {
            var fileUrl = _infoUrl.Replace("{tag}", mapping.AddonTag);

            // Get the Html
            var html = string.Join("", WebHelper.GetHtml(fileUrl).ToArray());
            // Get the Document
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var changeLog = doc.DocumentNode.SelectSingleNode("//div[@id='tab-changes']").InnerHtml;
            return changeLog;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            var fileUrl = addon.FileUrl;

            var downloadUrl = string.Empty;
            var form = new WebBrowserForm(fileUrl, AddonSiteId.curse, mapping.Addon.Name);
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
