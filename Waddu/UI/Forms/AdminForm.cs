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

            string[] tagList = tag.Split(new char[] { ',' });
            foreach (string itTag in tagList)
            {
                string currTag = itTag.Trim();
                try
                {
                    AddonSiteBase site = AddonSiteBase.GetSite(siteId);
                    Mapping map = new Mapping(currTag, siteId);
                    map.Addon = new Addon("tag");
                    string downLink = site.GetFilePath(map);
                    string archiveFilePath = WebHelper.DownloadFileToTemp(downLink);
                    List<string> cont = ArchiveHelper.GetArchiveContent(archiveFilePath);
                    List<string> folderList = new List<string>();
                    foreach (string s in cont)
                    {
                        int index = s.IndexOf("\\");
                        if (index > 0)
                        {
                            Helpers.AddIfNeeded<string>(folderList, s.Substring(0, index));
                        }
                    }
                    string main = string.Empty;
                    for (int i = 0; i < folderList.Count; i++)
                    {
                        string s = folderList[i];
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
            string url = txtUrl.Text;
            Match m = Regex.Match(url, "http://wow.curse.com/downloads/wow-addons/details/(.*).aspx");
            if (m.Success)
            {
            }
        }

        private void btnCreateFromTag_Click(object sender, EventArgs e)
        {
            string tag = txtUrl.Text;
            CreateMappings(tag, GetSite());
        }

        private string GetUrl()
        {
            AddonSiteId site = GetSite();
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
