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
        private string _infoUrl = "http://www.curse.com/addons/wow/{tag}";
        private string _downUrl = "http://www.curse.com/addons/wow/{tag}/download";
        private string _versionPattern = @"<li class=""newest-file"">Newest File: (.*)</li>";
        private string _updatedPattern = @"<li class=""updated"">Updated <abbr class=""standard-date"" title="".*"" data-epoch=""(\d*)"">.*</abbr>";
        private SiteAddonCache _addonCache = new SiteAddonCache();

        private void ParseInfoSite(Mapping mapping)
        {
            SiteAddon addon = _addonCache.Get(mapping.AddonTag);
            addon.Clear();

            bool versionFound = false;
            bool dateFound = false;

            string url = _infoUrl.Replace("{tag}", mapping.AddonTag);
            List<string> infoPage = WebHelper.GetHtml(url);

            addon.FileUrl = _downUrl.Replace("{tag}", mapping.AddonTag);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                // Version Check / Download Url
                if (!versionFound)
                {
                    Match m = Regex.Match(line, _versionPattern);
                    if (m.Success)
                    {
                        string version = m.Groups[1].Captures[0].Value;
                        version = FormatVersion(mapping, version);
                        addon.VersionString = version;
                        versionFound = true;
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

        public override string GetFilePath(Mapping mapping)
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
                downloadUrl = form.UseFile ? form.FileUrl : form.DownloadUrl;
            }
            else
            {
                downloadUrl = string.Empty;
            }

            return downloadUrl;
        }
        #endregion
    }
}
