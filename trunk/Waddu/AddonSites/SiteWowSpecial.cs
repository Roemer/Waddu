using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowSpecial : AddonSiteBase
    {
        private enum Type
        {
            Version,
            Date,
            Info,
            Download
        }

        private object CallHandler(string tag, Type type)
        {
            // MarsPartyBuff
            if (tag == "MarsPartyBuff")
            {
                return HandleMars(tag, type);
            }
            // CT
            else if (tag.StartsWith("CT_"))
            {
                return HandleCT(tag, type);
            }
            // xchar
            else if (tag.StartsWith("xchar"))
            {
                return Handlexchar(tag, type);
            }
            // MobMap
            else if (tag == "MobMap")
            {
                return HandleMobMap(tag, type);
            }
            throw new NotImplementedException();
        }

        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            return (string)CallHandler(tag, Type.Version);
        }

        public override DateTime GetLastUpdated(string tag)
        {
            return (DateTime)CallHandler(tag, Type.Date);
        }

        public override string GetInfoLink(string tag)
        {
            return (string)CallHandler(tag, Type.Info);
        }

        public override string GetDownloadLink(string tag)
        {
            return (string)CallHandler(tag, Type.Download);
        }

        #endregion

        private object HandleMars(string tag, Type type)
        {
            string infoUrl = "http://groups.google.com/group/marsmod/files";

            if (type == Type.Info)
            {
                return infoUrl;
            }

            List<string> infoPage = Helpers.GetHtml(infoUrl);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];
                //<a target="_blank" rel="nofollow" href="http://marsmod.googlegroups.com/web/MarsPartyBuff20080913.zip?gda=m5AfYEsAAACtzIgBbXirP3jp_ydva2I82ttm1sKUCOa0UxcaRPSkdt4R24vKdJjOI5LNi9WHjftGsDZEKUyz4xa7aJwfjKxgBkXa90K8pT5MNmkW1w_4BQ">MarsPartyBuff20080913.zip</a>
                if (line.Contains("MarsPartyBuff"))
                {
                    if (type == Type.Download)
                    {
                        int start = line.IndexOf("href=\"") + 6;
                        int end = line.IndexOf("\"", start);
                        return line.Substring(start, (end - start));
                    }
                    if (type == Type.Version)
                    {
                        int start = line.IndexOf("\">MarsPartyBuff") + 15;
                        int end = line.IndexOf(".zip</a>", start);
                        return line.Substring(start, (end - start));
                    }
                    if (type == Type.Date)
                    {
                        int start = line.IndexOf("\">MarsPartyBuff") + 15;
                        int end = line.IndexOf(".zip</a>", start);
                        string dateStr = line.Substring(start, (end - start));
                        DateTime dt = new DateTime(Convert.ToInt32(dateStr.Substring(0, 4)), Convert.ToInt32(dateStr.Substring(4, 2)), Convert.ToInt32(dateStr.Substring(6, 2)));
                        return dt;
                    }
                }
            }
            return new object();
        }

        private object HandleCT(string tag, Type type)
        {
            if (type == Type.Info)
            {
                return "http://www.ctmod.net";
            }
            if (type == Type.Date)
            {
                return DateTime.MinValue;
            }

            string infoUrl = "http://www.ctmod.net/downloads/";
            List<string> infoPage = Helpers.GetHtml(infoUrl);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                Match m = Regex.Match(line, string.Format(@"<h2><a href=""(.*)"" title=""Click to download"">{0}</a>", tag));
                if (m.Success)
                {
                    if (type == Type.Download)
                    {
                        return "http://www.ctmod.net" + m.Groups[1].Captures[0].Value;
                    }
                    if (type == Type.Version)
                    {
                        string realLine = infoPage[i + 1];
                        m = Regex.Match(realLine, @"\((.*)\)");
                        if (m.Success)
                        {
                            return m.Groups[1].Captures[0].Value;
                        }
                    }
                }
            }
            return new object();
        }

        private object Handlexchar(string tag, Type type)
        {
            if (type == Type.Info)
            {
                return "http://www.xchar.de";
            }
            if (type == Type.Version)
            {
                return "Unknown";
            }

            // Main Addon
            if (tag == "xchar")
            {
                if (type == Type.Download)
                {
                    return "http://faces.xchar.de/zip/addons/xchar.zip";
                }
                if (type == Type.Date)
                {
                    return DateTime.MinValue;
                }
            }
            // Image Addons
            else
            {
                if (type == Type.Download)
                {
                    return string.Format("http://faces.xchar.de/zip/eu/{0}.zip", tag);
                }
                if (type == Type.Date)
                {
                    List<string> infoPage = Helpers.GetHtml("http://www.xchar.de/index.php?content=downloads&tab=faces_manual");
                    for (int i = 0; i < infoPage.Count; i++)
                    {
                        string line = infoPage[i];
                        Match m = Regex.Match(line, @".*Last update: (.*)</div>.*");
                        if (m.Success)
                        {
                            string dateStr = m.Groups[1].Captures[0].Value;
                            string[] dateList = dateStr.Split('.');
                            return new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
                        }
                    }
                }
            }
            return new object();
        }

        private object HandleMobMap(string tag, Type type)
        {
            if (type == Type.Info)
            {
                return "http://www.mobmap.de";
            }
            if (type == Type.Date)
            {
                return DateTime.MinValue;
            }

            List<string> infoPage = Helpers.GetHtml("http://www.mobmap.de/addon.php");
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                if (type == Type.Download)
                {
                    //<td>- <a href="dodownload.php?dl=files/MobMap212.german.zip"><font size="3" face="Verdana, Arial, Helvetica, sans-serif"><u>Download mit aktueller deutscher Datenbank</u></font></a></td>
                    if (line.Contains("Download with the newest German database"))
                    {
                        int start = line.IndexOf("href=\"") + 6;
                        int end = line.IndexOf("\"", start);
                        return "http://www.mobmap.de" + line.Substring(start, (end - start));
                    }
                }
                if (type == Type.Version)
                {
                    Match m = Regex.Match(line, @"<p><font face=""Verdana, Arial, Helvetica, sans-serif"">newest version : (.*)</font></p>");
                    if (m.Success)
                    {
                        return m.Groups[1].Captures[0].Value;
                    }
                }
            }
            return new object();
        }
    }
}
