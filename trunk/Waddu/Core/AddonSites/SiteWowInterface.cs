﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core.AddonSites
{
    public class SiteWowInterface : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowinterface.com/downloads/info{tag}.html";
        private string _downUrl = "http://www.wowinterface.com/downloads/download{tag}";
        private string _versionPrePattern = @"<td class=""alt2""><div class=""infoboxfont1"">Version:</div></td>";
        private string _versionPattern = @"<td class=""alt2""><div class=""smallfont"">(.*)</div></td>";
        private string _datePrePattern = @"<td class=""alt1""><div class=""infoboxfont1"">Date:</div></td>";
        private string _datePattern = @"<td class=""alt1""><div class=""smallfont"">([^ ]*).*</div></td>";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private string _xmlUrl = "http://www.wowinterface.com/patcher.php?id={tag}";
        private string _xmlVersionPattern = @"<UIVersion>(.*)</UIVersion>";
        private string _xmlFilePattern = @"<UIFileURL>(.*)</UIFileURL>";
        private string _xmlDownloadPath = string.Empty;

        private void ParseXmlSite(Mapping mapping, SiteAddon addon)
        {
            bool versionFound = false;
            bool downloadFound = false;
            string xmlurl = _xmlUrl.Replace("{tag}", mapping.AddonTag);
            List<string> xmlPage = WebHelper.GetHtml(xmlurl);

            for (int i = 0; i < xmlPage.Count; i++)
            {
                string line = xmlPage[i];
                if (!versionFound)
                {
                    Match m = Regex.Match(line, _xmlVersionPattern);
                    if (m.Success)
                    {
                        string version = m.Groups[1].Captures[0].Value;
                        if (addon != null)
                        {
                            addon.VersionString = version;
                        }
                        versionFound = true;
                    }
                }
                if (!downloadFound)
                {
                    Match m = Regex.Match(line, _xmlFilePattern);
                    if (m.Success)
                    {
                        _xmlDownloadPath = m.Groups[1].Captures[0].Value;
                        downloadFound = true;
                    }
                }
            }
        }

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            ParseXmlSite(mapping, addon);

            bool dateFound = false;
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            List<string> infoPage = WebHelper.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                // Replaced by XML
                //if (!versionFound)
                //{
                //    Match m = Regex.Match(line, _versionPrePattern);
                //    if (m.Success)
                //    {
                //        string realLine = infoPage[i + 1];
                //        m = Regex.Match(realLine, _versionPattern);
                //        if (m.Success)
                //        {
                //            string version = m.Groups[1].Captures[0].Value;
                //            addon.VersionString = version;
                //            versionFound = true;
                //        }
                //    }
                //}

                if (!dateFound)
                {
                    Match m = Regex.Match(line, _datePrePattern);
                    if (m.Success)
                    {
                        string realLine = infoPage[i + 1];
                        m = Regex.Match(realLine, _datePattern);
                        if (m.Success)
                        {
                            string dateStr = m.Groups[1].Captures[0].Value;
                            string[] dateList = dateStr.Split(new char[] { '-' });
                            DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
                            addon.VersionDate = dt;
                            dateFound = true;
                        }
                    }
                }
            }
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
            return string.Empty;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            // This is the Captcha URL
            //string captchaURL = _downUrl.Replace("{tag}", mapping.AddonTag);

            ParseXmlSite(mapping, null);
            return _xmlDownloadPath;
        }
        #endregion
    }
}
