using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Waddu.Core.BusinessObjects;
using Waddu.UI.Forms;
using Waddu.Types;
using System.Windows.Forms;

namespace Waddu.Core.AddonSites
{
    public class SiteCurse : AddonSiteBase
    {
        private string _infoUrl = "http://wow.curse.com/downloads/wow-addons/details/{tag}.aspx";
        private string _downUrl = "http://wow.curse.com{0}";
        private string _versionPrePattern = @"<th>Current Version:</th>";
        private string _versionPattern = @"<a href=""(.*.aspx)"">(.*)</a>";
        private string _updatedPattern = @"<td><script>document.write\(Curse.Utils.getDateSince\((\d*)\d{3}\)\);</script>(.*)</td>";
        private string _downloadPattern = @"<a class=""button button-pop"" href=""(.*)""><span>Manual Install</span></a>";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            bool versionFound = false;
            bool dateFound = false;

            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            List<string> infoPage = WebHelper.GetHtml(url);

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
                            version = FormatVersion(mapping, version);
                            addon.VersionString = version;
                            string file = string.Format(_downUrl, m.Groups[1].Captures[0].Value);
                            addon.FileUrl = file;
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
                        addon.VersionDate = dt;
                        dateFound = true;
                    }
                }
            }
        }

        #region AddonSiteBase Overrides
        public override string GetVersion(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionString;
        }

        public override DateTime GetLastUpdated(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            return addon.VersionDate;
        }

        public override string GetChangeLog(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }

            string fileUrl = addon.FileUrl;
            List<string> filePage = WebHelper.GetHtml(fileUrl);
            string changeLog = string.Empty;
            bool changeLogAdd = false;
            for (int i = 0; i < filePage.Count; i++)
            {
                string line = filePage[i];
                if (line.Contains(@"<div id=""tab_changes"" class=""body"" style=""display:none"">"))
                {
                    changeLogAdd = true;
                    i += 2;
                    continue;
                }
                if (line.Contains(@"<li class=""title"">ChangeLog</li>"))
                {
                    changeLogAdd = true;
                    i += 3;
                    continue;
                }
                if (line.Contains(@"</div>") && changeLogAdd)
                {
                    break;
                }
                if (changeLogAdd)
                {
                    changeLog += line;
                }
            }

            return changeLog;
        }

        public override string GetInfoLink(Mapping mapping)
        {
            return _infoUrl.Replace("{tag}", mapping.AddonTag);
        }

        public override string GetDownloadLink(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            if (addon.IsCollectRequired)
            {
                ParseInfoSite(mapping);
            }
            string fileUrl = addon.FileUrl;

            string downloadUrl = string.Empty;
            WebBrowserForm form = new WebBrowserForm(fileUrl, AddonSiteId.curse, mapping.Addon.Name);
            if (form.ShowDialog() == DialogResult.OK)
            {
                downloadUrl = form.DownloadUrl;
            }

            // Legacy
            //List<string> filePage = WebHelper.GetHtml(fileUrl);
            //for (int i = 0; i < filePage.Count; i++)
            //{
            //    string line = filePage[i];
            //    Match m = Regex.Match(line, _downloadPattern);
            //    if (m.Success)
            //    {
            //        downloadUrl = string.Format(_downUrl, m.Groups[1].Captures[0].Value);
            //        downloadUrl = downloadUrl.Replace("&amp;", "&");
            //        break;
            //    }
            //}

            return downloadUrl;
        }
        #endregion
    }
}
