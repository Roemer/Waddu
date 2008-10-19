using System.Collections.Generic;
using Waddu.Classes;
using Waddu.Types;

namespace Waddu.AddonSites
{
    public class SiteCurseForge : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curseforge.com/projects/{tag}/files/";

        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            string versionString = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.curseforge);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];
                //<td class="first"><a href="/projects/quest-helper/files/72-0-57/">0.57</a></td>
                if (line.Contains("<td class=\"first\">"))
                {
                    int start = line.IndexOf("/\">") + 3;
                    int end = line.IndexOf("</a>", start);
                    versionString = line.Substring(start, (end - start));
                    if (versionString.Contains("-r"))
                    {
                        start = versionString.IndexOf("-r") + 1;
                        versionString = versionString.Substring(start);
                    }
                    break;
                }
            }
            return versionString;
        }

        public override string GetDownloadLink(string tag)
        {
            string downloadUrl = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url, AddonSiteId.curseforge);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];
                //<td class="first"><a href="/projects/quest-helper/files/72-0-57/">0.57</a></td>
                if (line.Contains("<td class=\"first\">"))
                {
                    // Get File Url
                    string fileUrl = "http://wow.curseforge.com";
                    int start = line.IndexOf("/");
                    int end = line.IndexOf("/\">");
                    fileUrl += line.Substring(start, (end - start));

                    // Get DownloadURL
                    List<string> filePage = Helpers.GetHtml(fileUrl, AddonSiteId.curseforge);
                    for (int ii = 0; ii < filePage.Count; ii++)
                    {
                        string line2 = filePage[ii];

                        //<th>Filename:</th>
                        //<td><a href="http://static.curseforge.net/uploads/18/263/436/QuestHelper-0.59.zip">QuestHelper-0.59.zip</a></td>
                        if (line2.Contains("<th>Filename:</th>"))
                        {
                            string realLine = filePage[ii + 1];
                            int start2 = realLine.IndexOf("http");
                            int end2 = realLine.IndexOf(".zip") + 4;
                            downloadUrl = realLine.Substring(start2, (end2 - start2));
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
