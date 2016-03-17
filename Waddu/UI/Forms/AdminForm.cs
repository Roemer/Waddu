using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Waddu.Core;
using Waddu.Core.AddonSites;
using Waddu.Core.BusinessObjects;
using Waddu.Types;

namespace Waddu.UI.Forms
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();

            cbSite.DataSource = Enum.GetValues(typeof(AddonSiteId));
        }

        private void CreateMappings(string tag, AddonSiteId siteId)
        {
            txtOut.Text = string.Empty;

            var tagList = tag.Split(',');
            foreach (var itTag in tagList)
            {
                var currTag = itTag.Trim();
                try
                {
                    var site = AddonSiteBase.GetSite(siteId);
                    var map = new Mapping(siteId, currTag) { Addon = new Addon("tag") };
                    var downLink = site.GetFilePath(map);
                    var archiveFilePath = WebHelper.DownloadFileToTemp(downLink);
                    var cont = ArchiveHelper.GetArchiveContent(archiveFilePath);
                    var folderList = new List<string>();
                    foreach (var s in cont)
                    {
                        var index = s.IndexOf("\\");
                        if (index > 0)
                        {
                            Helpers.AddIfNeeded(folderList, s.Substring(0, index));
                        }
                    }
                    var main = string.Empty;
                    for (var i = 0; i < folderList.Count; i++)
                    {
                        var s = folderList[i];
                        if (i == 0)
                        {
                            txtOut.Text += string.Format(@"GetAddon(""{0}"").Mappings.Add(new Mapping(""{1}"", AddonSiteId.{2}));{3}", s, currTag, siteId, Environment.NewLine);
                            main = s;
                        }
                        else
                        {
                            txtOut.Text += string.Format(@"GetAddon(""{0}"").SubAddons.Add(GetAddon(""{1}""));{2}", main, s, Environment.NewLine);
                        }
                    }
                }
                catch
                {
                    txtOut.Text += string.Format(@"Tag ""{0}"" failed{1}", currTag, Environment.NewLine);
                }
                txtOut.Text += Environment.NewLine;
                Application.DoEvents();
                //System.Threading.Thread.Sleep(1000);
            }
        }

        private void btnCreateMapping_Click(object sender, EventArgs e)
        {
            var url = txtUrl.Text;
            var m = Regex.Match(url, "http://wow.curse.com/downloads/wow-addons/details/(.*).aspx");
            if (m.Success)
            {
            }
        }

        private void btnCreateFromTag_Click(object sender, EventArgs e)
        {
            var tag = txtUrl.Text;
            CreateMappings(tag, GetSite());
        }

        private string GetUrl()
        {
            var site = GetSite();
            if (site == AddonSiteId.curse)
            {
                return "http://wow.curse.com/downloads/wow-addons/details/(.*).aspx";
            }
            return string.Empty;
        }

        private AddonSiteId GetSite()
        {
            return (AddonSiteId)cbSite.SelectedItem;
        }
    }
}
