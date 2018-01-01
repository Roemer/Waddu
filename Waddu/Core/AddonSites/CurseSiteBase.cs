using System;
using HtmlAgilityPack;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core.AddonSites
{
    public abstract class CurseSiteBase : AddonSiteBase
    {
        private SiteAddonCache _addonCache = new SiteAddonCache();

        protected abstract string InfoUrlFormat { get; }
        protected abstract string DownloadUrlFormat { get; }

        private void ParseInfoSite(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            // Build the Url
            var url = InfoUrlFormat.Replace("{tag}", mapping.AddonTag);
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
            var versionNode = doc.DocumentNode.SelectSingleNode("//ul[@class='cf-recentfiles']/li//div[@class='project-file-name-container']");
            var version = versionNode.InnerText.Trim();
            // Get the Date
            var dateNode = doc.DocumentNode.SelectSingleNode("//li/div[starts-with(text(), 'Last Released File')]/following-sibling::div/abbr");
            var dateValue = dateNode.Attributes["data-epoch"].Value;
            var date = UnixTimeStamp.GetDateTime(Convert.ToDouble(dateValue));

            // Assign the Values
            addon.VersionString = version;
            addon.FileUrl = DownloadUrlFormat.Replace("{tag}", mapping.AddonTag);
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
            var fileUrl = InfoUrlFormat.Replace("{tag}", mapping.AddonTag);

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
            return InfoUrlFormat.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            var addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            var fileUrl = addon.FileUrl;

            return fileUrl;
        }
        #endregion
    }
}
