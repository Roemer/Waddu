using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteCurse : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curse.com/downloads/wow-addons/details/{tag}.aspx";
        private string _downUrl = "http://wow.curse.com{0}";
        private string _versionPrePattern = @"<th>Current Version:</th>";
        private string _versionPattern = @"<a href=""(.*.aspx)"">(.*)</a>";
        private string _updatedPattern = @"<td><script>document.write\(Curse.Utils.getDateSince\((.*)000\)\);</script>(.*)</td>";
        private string _downloadPattern = @"<a class=""button button-pop"" href=""(.*)""><span>Manual Install</span></a>";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();
        private Dictionary<string, string> _fileLinkCache = new Dictionary<string, string>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(string tag)
        {
            bool versionFound = false;
            bool dateFound = false;

            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                // Version Check / Download Url
                if (!versionFound)
                {
                    Match m = Regex.Match(line, _versionPrePattern);
                    if (m.Success)
                    {
                        string realLine = infoPage[i + 2];
                        m = Regex.Match(realLine, _versionPattern);
                        if (m.Success)
                        {
                            string version = m.Groups[2].Captures[0].Value;
                            Helpers.AddOrUpdate<string, string>(_versionCache, tag, version);
                            string file = string.Format(_downUrl, m.Groups[1].Captures[0].Value);
                            Helpers.AddOrUpdate<string, string>(_fileLinkCache, tag, file);
                            versionFound = true;
                        }
                    }
                }

                // Last Update Check
                if (!dateFound)
                {
                    Match m = Regex.Match(line, _updatedPattern);
                    if (m.Success)
                    {
                        string date = m.Groups[1].Captures[0].Value;
                        DateTime dt = UnixTimeStamp.GetDateTime(Convert.ToDouble(date));
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
            if (_versionCache.ContainsKey(tag))
            {
                return _versionCache[tag];
            }
            return string.Empty;
        }

        public override DateTime GetLastUpdated(string tag)
        {
            if (!_dateCache.ContainsKey(tag))
            {
                ParseInfoSite(tag);
            }
            if (_dateCache.ContainsKey(tag))
            {
                return _dateCache[tag];
            }
            return DateTime.MinValue;
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
            List<string> filePage = Helpers.GetHtml(fileUrl);
            for (int i = 0; i < filePage.Count; i++)
            {
                string line = filePage[i];
                Match m = Regex.Match(line, _downloadPattern);
                if (m.Success)
                {
                    downloadUrl = string.Format(_downUrl, m.Groups[1].Captures[0].Value);
                    downloadUrl = downloadUrl.Replace("&amp;", "&");
                    break;
                }
            }
            return downloadUrl;
        }

        #endregion
    }
}
