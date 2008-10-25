using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Classes;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public class SiteWowAce : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowace.com/projects/{tag}/files/";
        private string _fileUrl = "http://www.wowace.com{0}";
        private string _versionPattern = @"<td class=""first""><a href=""(.*)"">(.*)</a></td>";
        private string _datePattern = @"<span class=""date"" title="".*"">(.*)</span>";
        private string _downloadPattern = @"<a href=""(.*)""><span>Download</span></a>";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();
        private Dictionary<string, string> _fileLinkCache = new Dictionary<string, string>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(string tag)
        {
            string url = _infoUrl.Replace("{tag}", tag);
            bool versionFound = false;
            bool dateFound = false;

            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.wowace);
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
                        Helpers.AddOrUpdate<string, string>(_versionCache, tag, versionString);
                        string fileUrl = m.Groups[1].Captures[0].Value;
                        Helpers.AddOrUpdate<string, string>(_fileLinkCache, tag, string.Format(_fileUrl, fileUrl));
                        versionFound = true;
                    }
                }

                // Last Update Check
                if (!dateFound)
                {
                    Match m = Regex.Match(line, _datePattern);
                    if (m.Success)
                    {
                        string dateStr = m.Groups[1].Captures[0].Value;
                        string[] dateList = dateStr.Split('/');
                        DateTime dt = new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[0]), Convert.ToInt32(dateList[1]));
                        Helpers.AddOrUpdate<string, DateTime>(_dateCache, tag, dt);
                        dateFound = true;
                    }
                }
            }
        }

        public override string GetVersion(string tag)
        {
            if (!_versionCache.ContainsKey(tag))
            {
                ParseInfoSite(tag);
            }
            return _versionCache[tag];
        }

        public override DateTime GetLastUpdated(string tag)
        {
            if (!_dateCache.ContainsKey(tag))
            {
                ParseInfoSite(tag);
            }
            return _dateCache[tag];
        }

        public override string GetInfoLink(string tag)
        {
            return _infoUrl.Replace("{tag}", tag);
        }

        public override string GetDownloadLink(string tag)
        {
            if (!_fileLinkCache.ContainsKey(tag))
            {
                ParseInfoSite(tag);
            }
            string fileUrl = _fileLinkCache[tag];

            string downloadUrl = string.Empty;
            List<string> filePage = Helpers.GetHtml(fileUrl, AddonSiteId.wowace);
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
            return downloadUrl;
        }

        #endregion
    }
}
