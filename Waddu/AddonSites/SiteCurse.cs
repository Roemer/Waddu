using System.Collections.Generic;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteCurse : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curse.com/downloads/wow-addons/details/{tag}.aspx";
        private string _downUrl = "http://wow.curse.com{0}";

        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            string versionString = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                //<th>Current Version:</th>
                if (line.Contains("<th>Current Version:</th>"))
                {
                    //<a href="/downloads/wow-addons/details/quest-helper/download/213635.aspx">0.57</a>
                    string realLine = infoPage[i + 2];
                    int start = realLine.IndexOf("aspx\">") + 6;
                    int end = realLine.IndexOf("</a>", start);
                    versionString = realLine.Substring(start, (end - start));
                    break;
                }
            }
            return versionString;
        }

        public override string GetDownloadLink(string tag)
        {
            string downloadUrl = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                //<th>Current Version:</th>
                if (line.Contains("<th>Current Version:</th>"))
                {
                    //<a href="/downloads/wow-addons/details/quest-helper/download/213635.aspx">0.57</a>
                    string realLine = infoPage[i + 2];
                    int start = realLine.IndexOf("href=\"") + 6;
                    int end = realLine.IndexOf("\">", start);
                    downloadUrl = realLine.Substring(start, (end - start));
                    downloadUrl = string.Format(_downUrl, downloadUrl);

                    // Get real Download Link
                    List<string> filePage = Helpers.GetHtml(downloadUrl);
                    foreach (string line2 in filePage)
                    {
                        //<a class="button button-pop" href="/downloads/download.aspx?pi=7436&amp;fi=246034"><span>Manual Install</span></a>
                        if (line2.Contains("<span>Manual Install</span>"))
                        {
                            int start2 = line2.IndexOf("href=\"") + 6;
                            int end2 = line2.IndexOf("\">", start2);
                            downloadUrl = string.Format(_downUrl, line2.Substring(start2, (end2 - start2)));
                            downloadUrl = downloadUrl.Replace("&amp;", "&");
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
