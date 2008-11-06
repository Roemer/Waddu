﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.BusinessObjects;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowUi : AddonSiteBase
    {
        private string _infoUrl = "http://wowui.worldofwar.net/?p=mod&m={tag}";
        private string _downUrl = "http://wowui.worldofwar.net/?p=download&m={tag}";
        private string _versionPrePattern = @"<title>";
        private string _versionPattern = @"(.*?) - .*";
        private string _datePattern = @".*<br />Updated <b>(.*?)</b>.*";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(Mapping mapping)
        {
            bool versionFound = false;
            bool dateFound = false;
            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            List<string> infoPage = WebHelper.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                if (!versionFound)
                {
                    Match m = Regex.Match(line, _versionPrePattern);
                    if (m.Success)
                    {
                        string realLine = infoPage[i + 1];
                        m = Regex.Match(realLine, _versionPattern);
                        if (m.Success)
                        {
                            string version = m.Groups[1].Captures[0].Value;
                            Helpers.AddOrUpdate<string, string>(_versionCache, mapping.AddonTag, version);
                            versionFound = true;
                        }
                    }
                }

                if (!dateFound)
                {
                    Match m = Regex.Match(line, _datePattern);
                    if (m.Success)
                    {
                        string dateStr = m.Groups[1].Captures[0].Value;
                        string[] dateList = dateStr.Split('/');
                        DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
                        Helpers.AddOrUpdate<string, DateTime>(_dateCache, mapping.AddonTag, dt);
                        dateFound = true;
                    }
                }
            }
        }

        public override string GetVersion(Mapping mapping)
        {
            if (!_versionCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            return _versionCache[mapping.AddonTag];
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            if (!_dateCache.ContainsKey(mapping.AddonTag))
            {
                ParseInfoSite(mapping);
            }
            return _dateCache[mapping.AddonTag];
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            return _downUrl.Replace("{tag}", mapping.AddonTag);
        }

        #endregion
    }
}
