using System.Collections.Generic;
using Waddu.Classes;

namespace Waddu.AddonSites
{
    public class SiteWowUi : AddonSiteBase
    {
        private string _infoUrl = "http://wowui.worldofwar.net/?p=mod&m={tag}";
        private string _downUrl = "http://wowui.worldofwar.net/?p=download&m={tag}";

        #region AddonSiteBase Overrides

        public override string GetVersion(string tag)
        {
            string versionString = string.Empty;
            string url = _infoUrl.Replace("{tag}", tag);
            List<string> infoPage = Helpers.GetHtml(url);

            for (int i = 0; i < infoPage.Count; i++)
            {
                string line = infoPage[i];

                //<title>
                //Coconuts 2.5 - Inventory/Item - World of Warcraft Mods, Addons, and More!</title>
                if (line.Contains("<title>"))
                {
                    string realLine = infoPage[i + 1];
                    int start = 0;
                    int end = realLine.IndexOf(" - ", start);
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
