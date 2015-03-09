using System;
using System.Globalization;
using System.Xml;
using HtmlAgilityPack;
using Waddu.Core.BusinessObjects;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Waddu.Core.AddonSites
{
    public class SiteWowInterface : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowinterface.com/downloads/info{tag}.html";
        private string _downUrl = "http://www.wowinterface.com/downloads/download{tag}";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private string _xmlUrl = "http://www.wowinterface.com/patcher.php?id={tag}";
        private string _xmlVersionPattern = @"<UIVersion>(.*)</UIVersion>";
        private string _xmlFilePattern = @"<UIFileURL>(.*)</UIFileURL>";
        private string _xmlDownloadPath = String.Empty;
        private string _xmlVersionString = String.Empty;

        private void ParseXmlSite(Mapping mapping)
        {
            // Build the Url
            var xmlurl = _xmlUrl.Replace("{tag}", mapping.AddonTag);
            // Get the Html
            var xml = string.Join("", WebHelper.GetHtml(xmlurl, mapping.AddonSiteId).ToArray());
            // Get the Document
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            _xmlVersionString = doc.DocumentElement.SelectSingleNode("//UIVersion").InnerText;
            _xmlDownloadPath = doc.DocumentElement.SelectSingleNode("//UIFileURL").InnerText;
        }

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            // Parse the XML
            ParseXmlSite(mapping);
            addon.VersionString = _xmlVersionString;

            // Build the Url
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            // Get the Html
            string html = string.Join("", WebHelper.GetHtml(url, mapping.AddonSiteId).ToArray());
            // Get the Document
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode dateNode = doc.DocumentNode.SelectSingleNode("//div[@id='safe']");
            string dateString = dateNode.InnerText.Replace("Updated: ", string.Empty);
            //DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
            DateTime dt = DateTime.Parse(dateString, CultureInfo.CreateSpecificCulture("en-US"));
            addon.VersionDate = dt;
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
            return string.Empty;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetFilePath(Mapping mapping)
        {
            ParseXmlSite(mapping);
            return _xmlDownloadPath;
        }
        #endregion
    }
}
