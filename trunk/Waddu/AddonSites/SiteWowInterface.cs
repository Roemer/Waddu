using System.Collections.Generic;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowInterface : AddonSiteBase
    {
        private string _infoUrl = "http://www.wowinterface.com/downloads/info{tag}.html";
        private string _downUrl = "http://www.wowinterface.com/downloads/download{tag}";

        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            string versionString = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                //<td class="alt2"><div class="infoboxfont1">Version:</div></td>
                //<td class="alt2"><div class="smallfont">0.57</div></td>
                if (line.Contains("Version:"))
                {
                    string realLine = infoPage[i + 1];
                    int start = realLine.IndexOf("smallfont\">") + 11;
                    int end = realLine.IndexOf("</div>", start);
                    versionString = realLine.Substring(start, (end - start));
                    break;
                }
            }
            return versionString;
        }

        public override string GetDownloadLink(string tag)
        {
            return _downUrl.Replace("{tag}", tag);
        }

        #endregion
    }
}
