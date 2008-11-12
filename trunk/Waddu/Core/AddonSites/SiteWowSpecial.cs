using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.BusinessObjects;
using Waddu.Classes;

namespace Waddu.Classes.AddonSites
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

        private object CallHandler(Mapping mapping, Type type)
        {
            // MarsPartyBuff
            if (mapping.AddonTag == "MarsPartyBuff")
            {
                return HandleMars(mapping, type);
            }
            // CT
            else if (mapping.AddonTag.StartsWith("CT_"))
            {
                return HandleCT(mapping, type);
            }
            // xchar
            else if (mapping.AddonTag.StartsWith("xchar"))
            {
                return Handlexchar(mapping, type);
            }
            // MobMap
            else if (mapping.AddonTag == "MobMap")
            {
                return HandleMobMap(mapping, type);
            }
            throw new NotImplementedException();
        }

        #region AddonSiteBase Overrides

        public override string GetVersion(Mapping mapping)
        {
            return (string)CallHandler(mapping, Type.Version);
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            return (DateTime)CallHandler(mapping, Type.Date);
        }

        public override string GetChangeLog(Mapping mapping)
        {
            return string.Empty;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return (string)CallHandler(mapping, Type.Info);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            return (string)CallHandler(mapping, Type.Download);
        }

        #endregion

        private object HandleMars(Mapping mapping, Type type)
        {
            string infoUrl = "http://groups.google.com/group/marsmod/files";

            if (type == Type.Info)
            {
                return infoUrl;
            }

            List<string> infoPage = WebHelper.GetHtml(infoUrl);
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

        private object HandleCT(Mapping mapping, Type type)
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
            List<string> infoPage = WebHelper.GetHtml(infoUrl);
            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                Match m = Regex.Match(line, string.Format(@"<h2><a href=""(.*)"" title=""Click to download"">{0}</a>", mapping.AddonTag));
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

        private object Handlexchar(Mapping mapping, Type type)
        {
            if (type == Type.Info)
            {
                return "http://www.xchar.de";
            }
            if (type == Type.Version)
            {
                return "?";
            }

            // Main Addon
            if (mapping.AddonTag == "xchar")
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
                    return string.Format("http://faces.xchar.de/zip/eu/{0}.zip", mapping.AddonTag);
                }
                if (type == Type.Date)
                {
                    List<string> infoPage = WebHelper.GetHtml("http://www.xchar.de/index.php?content=downloads&tab=faces_manual");
                    for (int i = 0; i < infoPage.Count; i++)
                    {
                        string line = infoPage[i];
                        Match m = Regex.Match(line, @".*Last update: (.*)</div>.*");
                        if (m.Success)
                        {
                            string dateStr = m.Groups[1].Captures[0].Value;
                            string[] dateList = dateStr.Split(new char[] { '.' });
                            return new DateTime(Convert.ToInt32(dateList[2]), Convert.ToInt32(dateList[1]), Convert.ToInt32(dateList[0]));
                        }
                    }
                }
            }
            return new object();
        }

        private object HandleMobMap(Mapping mapping, Type type)
        {
            if (type == Type.Info)
            {
                return "http://www.mobmap.de";
            }
            if (type == Type.Date)
            {
                return DateTime.MinValue;
            }

            List<string> infoPage = WebHelper.GetHtml("http://www.mobmap.de/addon.php");
            if (type == Type.Download)
            {    
                string regex1 = @".*<td>- <a href=""(.*)""><font size=""3"" face=""Verdana, Arial, Helvetica, sans-serif""><u>Download with the newest German database</u></font></a></td>";
                string regex2 = @"<p><font face=""Verdana, Arial, Helvetica, sans-serif"">If that is not the case, please click  <a href=""(.*)""><font size=""3"">here</font></a> to start the download manually!</font></p>";
                
                Match m = WebHelper.GetMatch(infoPage, regex1);
                string downUrl = "http://www.mobmap.de/" + m.Groups[1].Captures[0].Value;
                List<string> downPage = WebHelper.GetHtml(downUrl);
                m = WebHelper.GetMatch(downPage, regex2);
                return m.Groups[1].Captures[0].Value;
            }
            else if (type == Type.Version)
            {
                Match m = WebHelper.GetMatch(infoPage, @"<p><font face=""Verdana, Arial, Helvetica, sans-serif"">newest version : (.*)</font></p>");
                if (m != null)
                {
                    return m.Groups[1].Captures[0].Value;
                }
            }
            return new object();
        }
    }
}
