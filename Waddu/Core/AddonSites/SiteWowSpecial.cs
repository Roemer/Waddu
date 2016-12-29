using System;
using System.Text.RegularExpressions;
using Waddu.Core.BusinessObjects;

namespace Waddu.Core.AddonSites
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
            if (mapping.AddonTag.StartsWith("CT_"))
            {
                return HandleCT(mapping, type);
            }
            // xchar
            if (mapping.AddonTag.StartsWith("xchar"))
            {
                return Handlexchar(mapping, type);
            }
            // MobMap
            if (mapping.AddonTag == "MobMap")
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

        public override string GetFilePath(Mapping mapping)
        {
            return (string)CallHandler(mapping, Type.Download);
        }

        #endregion

        private object HandleMars(Mapping mapping, Type type)
        {
            var infoUrl = "http://groups.google.com/group/marsmod/files";

            if (type == Type.Info)
            {
                return infoUrl;
            }

            var infoPage = WebHelper.GetHtml(infoUrl);
            for (var i = 0; i < infoPage.Count; i++)
            {
                var line = infoPage[i];
                //<a target="_blank" rel="nofollow" href="http://marsmod.googlegroups.com/web/MarsPartyBuff20080913.zip?gda=m5AfYEsAAACtzIgBbXirP3jp_ydva2I82ttm1sKUCOa0UxcaRPSkdt4R24vKdJjOI5LNi9WHjftGsDZEKUyz4xa7aJwfjKxgBkXa90K8pT5MNmkW1w_4BQ">MarsPartyBuff20080913.zip</a>
                if (line.Contains("MarsPartyBuff"))
                {
                    if (type == Type.Download)
                    {
                        var start = line.IndexOf("href=\"") + 6;
                        var end = line.IndexOf("\"", start);
                        return line.Substring(start, (end - start));
                    }
                    if (type == Type.Version)
                    {
                        var start = line.IndexOf("\">MarsPartyBuff") + 15;
                        var end = line.IndexOf(".zip</a>", start);
                        return line.Substring(start, (end - start));
                    }
                    if (type == Type.Date)
                    {
                        var start = line.IndexOf("\">MarsPartyBuff") + 15;
                        var end = line.IndexOf(".zip</a>", start);
                        var dateStr = line.Substring(start, (end - start));
                        var dt = new DateTime(Convert.ToInt32(dateStr.Substring(0, 4)), Convert.ToInt32(dateStr.Substring(4, 2)), Convert.ToInt32(dateStr.Substring(6, 2)));
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

            var infoUrl = "http://www.ctmod.net/downloads/";
            var infoPage = WebHelper.GetHtml(infoUrl);
            for (var i = 0; i < infoPage.Count; i++)
            {
                var line = infoPage[i];
                var m = Regex.Match(line, @"<h2><a title=""Click to download"" href=""(.*)"">.*</a>\((.*)\)</h2>");
                if (m.Success)
                {
                    if (type == Type.Download)
                    {
                        return m.Groups[1].Captures[0].Value;
                    }
                    if (type == Type.Version)
                    {
                        return m.Groups[2].Captures[0].Value;
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
                    var infoPage = WebHelper.GetHtml("http://www.xchar.de/index.php?content=downloads&tab=faces_manual");
                    for (var i = 0; i < infoPage.Count; i++)
                    {
                        var line = infoPage[i];
                        var m = Regex.Match(line, @".*Last update: (.*)</div>.*");
                        if (m.Success)
                        {
                            var dateStr = m.Groups[1].Captures[0].Value;
                            var dateList = dateStr.Split('.');
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

            var infoPage = WebHelper.GetHtml("http://www.mobmap.de/addon.php");
            if (type == Type.Download)
            {    
                var regex1 = @".*<td>- <a href=""(.*)""><font size=""3"" face=""Verdana, Arial, Helvetica, sans-serif""><u>Download with the newest German database</u></font></a></td>";
                var regex2 = @"<p><font face=""Verdana, Arial, Helvetica, sans-serif"">If that is not the case, please click  <a href=""(.*)""><font size=""3"">here</font></a> to start the download manually!</font></p>";
                
                var m = WebHelper.GetMatch(infoPage, regex1);
                var downUrl = "http://www.mobmap.de/" + m.Groups[1].Captures[0].Value;
                var downPage = WebHelper.GetHtml(downUrl);
                m = WebHelper.GetMatch(downPage, regex2);
                return m.Groups[1].Captures[0].Value;
            }
            if (type == Type.Version)
            {
                var m = WebHelper.GetMatch(infoPage, @"<p><font face=""Verdana, Arial, Helvetica, sans-serif"">newest version : (.*)</font></p>");
                if (m != null)
                {
                    return m.Groups[1].Captures[0].Value;
                }
            }
            return new object();
        }
    }
}
