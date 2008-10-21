using System.Collections.Generic;
using Waddu.Classes;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public class SiteWowAce : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowace.com/projects/{tag}/files/";

        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            string versionString = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.wowace);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];
                //<td class="first"><a href="/projects/gathermate/files/153-v1-03/">v1.03</a></td>
                if (line.Contains("<td class=\"first\">"))
                {
                    int start = line.IndexOf("/\">") + 3;
                    int end = line.IndexOf("</a>", start);
                    versionString = line.Substring(start, (end - start));
                    /*if (versionString.Contains("-r"))
                    {
                        start = versionString.IndexOf("-r") + 1;
                        versionString = versionString.Substring(start);
                    }*/
                    break;
                }
            }
            return versionString;
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
