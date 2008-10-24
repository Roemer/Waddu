using System;
using System.Collections.Generic;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowSpecial : AddonSiteBase
    {
        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            string versionString = string.Empty;

            // MarsPartyBuff
            if (tag == "MarsPartyBuff")
            {
                return HandleMars(tag, false);
            }
            // CT
            else if (tag.StartsWith("CT_"))
            {
                return HandleCT(tag, false);
            }
            // xchar
            else if (tag.StartsWith("xchar"))
            {
                return Handlexchar(tag, false);
            }
            // MobMap
            else if (tag == "MobMap")
            {
                return HandleMobMap(tag, false);
            }
            throw new NotImplementedException();
        }

        public override string GetInfoLink(string tag)
        {
            // TODO
            throw new NotImplementedException();
        }

        public override DateTime GetLastUpdated(string tag)
        {
            throw new NotImplementedException();
        }

        public override string GetDownloadLink(string tag)
        {
            // Mars
            if (tag == "MarsPartyBuff")
            {
                return HandleMars(tag, true);
            }
            // CT
            else if (tag.StartsWith("CT_"))
            {
                return HandleCT(tag, true);
            }
            // xchar
            else if (tag.StartsWith("xchar"))
            {
                return Handlexchar(tag, true);
            }
            // MobMap
            else if (tag == "MobMap")
            {
                return HandleMobMap(tag, true);
            }
            throw new NotImplementedException();
        }

        #endregion

        private string HandleMars(string tag, bool isDownload)
        {
            string infoUrl = "http://groups.google.com/group/marsmod/files";
            List<string> infoPage = Helpers.GetHtml(infoUrl);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];
                //<a target="_blank" rel="nofollow" href="http://marsmod.googlegroups.com/web/MarsPartyBuff20080913.zip?gda=m5AfYEsAAACtzIgBbXirP3jp_ydva2I82ttm1sKUCOa0UxcaRPSkdt4R24vKdJjOI5LNi9WHjftGsDZEKUyz4xa7aJwfjKxgBkXa90K8pT5MNmkW1w_4BQ">MarsPartyBuff20080913.zip</a>
                if (line.Contains("MarsPartyBuff"))
                {
                    if (isDownload)
                    {
                        int start = line.IndexOf("href=\"") + 6;
                        int end = line.IndexOf("\"", start);
                        return line.Substring(start, (end - start));
                    }
                    else
                    {
                        int start = line.IndexOf("\">MarsPartyBuff") + 15;
                        int end = line.IndexOf(".zip</a>", start);
                        return line.Substring(start, (end - start));
                    }
                }
            }
            return "Unknown";
        }

        private string HandleCT(string tag, bool isDownload)
        {
            string infoUrl = "http://www.ctmod.net/downloads/";
            List<string> infoPage = Helpers.GetHtml(infoUrl);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                //<h2><a href="/download/16/" title="Click to download">CT_UnitFrames</a>
                //(v2.4)
                if (line.Contains("title=\"Click to download\">" + tag + "</a>"))
                {
                    if (isDownload)
                    {
                        int start = line.IndexOf("href=\"") + 6;
                        int end = line.IndexOf("\"", start);
                        return "http://www.ctmod.net" + line.Substring(start, (end - start));
                    }
                    else
                    {
                        string realLine = infoPage[i + 1];
                        int start = realLine.IndexOf("(") + 1;
                        int end = realLine.IndexOf(")", start);
                        return realLine.Substring(start, (end - start));
                    }
                }
            }
            return "Unknown";
        }

        private string Handlexchar(string tag, bool isDownload)
        {
            // Main Addon
            if (tag == "xchar")
            {
                if (isDownload)
                {
                    return "http://faces.xchar.de/zip/addons/xchar.zip";
                }
                return "Nobody knows";
            }
            // Image Addons
            else
            {
                if (isDownload)
                {
                    return string.Format("http://faces.xchar.de/zip/eu/{0}.zip", tag);
                }
                //Last update: 12.10.2008</div>
                List<string> infoPage = Helpers.GetHtml("http://www.xchar.de/index.php?content=downloads&tab=faces_manual");
                for (int i = 0; i < infoPage.Count; i++)
                {
                    string line = infoPage[i];
                    if (line.Contains("Last update: "))
                    {
                        int start = line.IndexOf("Last update: ") + 13;
                        int end = line.IndexOf("</div>", start);
                        return line.Substring(start, (end - start));
                    }
                }
                return "Unknown";
            }
        }

        private string HandleMobMap(string tag, bool isDownload)
        {
            List<string> infoPage = Helpers.GetHtml("http://www.mobmap.de/addon.php");
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                if (isDownload)
                {
                    //<td>- <a href="dodownload.php?dl=files/MobMap212.german.zip"><font size="3" face="Verdana, Arial, Helvetica, sans-serif"><u>Download mit aktueller deutscher Datenbank</u></font></a></td>
                    if (line.Contains("Download with the newest German database"))
                    {
                        int start = line.IndexOf("href=\"") + 6;
                        int end = line.IndexOf("\"", start);
                        return "http://www.mobmap.de" + line.Substring(start, (end - start));
                    }
                }
                else
                {
                    //<p><font face="Verdana, Arial, Helvetica, sans-serif">Aktuelle Version: 2.12</font></p>
                    if (line.Contains("newest version :"))
                    {
                        int start = line.IndexOf("newest version :") + 16;
                        int end = line.IndexOf("<", start);
                        return line.Substring(start, (end - start));
                    }
                }
            }
            return "Unknown";
        }
    }
}
