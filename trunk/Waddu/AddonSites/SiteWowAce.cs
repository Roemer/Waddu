using System;
using System.Collections.Generic;
using Waddu.Classes;
using Waddu.Types;
using System.Text.RegularExpressions;

namespace Waddu.AddonSites
{
    public class SiteWowAce : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowace.com/projects/{tag}/files/";
        private Dictionary<string, string> _versionCache = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _dateCache = new Dictionary<string, DateTime>();

        #region AddonSiteBase Overrides

        private void ParseInfoSite(string tag)
        {
            string url = _infoUrl.Replace("{tag}", tag);

            bool nameFound = false;

            Regex regexDate = new Regex("<span class=\"date\" title=\".*\">(.*)</span>");
            bool dateFound = false;

            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.wowace);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                // Version Check
                //<td class="first"><a href="/projects/gathermate/files/153-v1-03/">v1.03</a></td>
                if (line.Contains("<td class=\"first\">") && !nameFound)
                {
                    int start = line.IndexOf("/\">") + 3;
                    int end = line.IndexOf("</a>", start);
                    string versionString = line.Substring(start, (end - start));
                    Helpers.AddOrUpdate<string, string>(_versionCache, tag, versionString);
                    nameFound = true;
                    /*if (versionString.Contains("-r"))
                    {
                        start = versionString.IndexOf("-r") + 1;
                        versionString = versionString.Substring(start);
                    }*/
                }

                // Last Update Check
                if (!dateFound)
                {
                    Match m = regexDate.Match(line);
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
            string downloadUrl = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.wowace);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                //<td class="first"><a href="/projects/gathermate/files/153-v1-03/">v1.03</a></td>
                if (line.Contains("<td class=\"first\">"))
                {
                    // Get File Url
                    string fileUrl = "http://www.wowace.com";
                    int start = line.IndexOf("/");
                    int end = line.IndexOf("/\">");
                    fileUrl += line.Substring(start, (end - start));

                    // Get DownloadURL
                    List<string> filePage = Helpers.GetHtml(fileUrl, AddonSiteId.wowace);
                    for (int ii = 0; ii < filePage.Count; ii++)
                    {
                        string line2 = filePage[ii];
                        //<a href="http://static.wowace.com/uploads/18/201/533/GatherMate-v1.03.zip"><span>Download</span></a>
                        if (line2.Contains("<span>Download</span>"))
                        {
                            int start2 = line2.IndexOf("http");
                            int end2 = line2.IndexOf(".zip") + 4;
                            downloadUrl = line2.Substring(start2, (end2 - start2));
                            break;
                        }
                    }
                    break;
                }
            }
            return downloadUrl;
        }

        #endregion
    }
}
