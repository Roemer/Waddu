using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Waddu.Classes;
using Waddu.Forms;
using Waddu.Types;

namespace Waddu.BusinessObjects
{
    public class AddonList
    {
        private static AddonList _instance;
        public static AddonList Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AddonList();
                }
                return _instance;
            }
            set { _instance = value; }
        }

        public List<Addon> Addons;

        private string GetNewestVersion()
        {
            string version;
            bool success = WebHelper.GetString("http://waddu.flauschig.ch/mapping/latest.txt", out version);
            if (!success)
            {
                success = WebHelper.GetString("http://www.red-demon.com/waddu/mapping/latest.txt", out version);
            }
            if (success)
            {
                return version;
            }
            return string.Empty;
        }

        private string GetLocalVersion(string xmlFile)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFile);
            }
            catch
            {
                return string.Empty;
            }

            return doc.DocumentElement.Attributes["Version"].Value;
        }

        public Addon GetAddon(string addonName)
        {
            return Addons.Find(delegate(Addon obj) { return (obj.Name.ToUpper().Equals(addonName.ToUpper())); });
        }

        // Private Constructor
        private AddonList()
        {
            // Initialize List
            Addons = new List<Addon>();

            // Get all Local Addons
            string addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            List<Addon> localAddons = new List<Addon>();
            if (Directory.Exists(addonFolderPath))
            {
                foreach (string addonPath in Directory.GetDirectories(addonFolderPath))
                {
                    Addon addon = new Addon(Path.GetFileName(addonPath));
                    localAddons.Add(addon);
                }
            }

            // Load XML File
            string localXml = Config.Instance.MappingFile;
            if (!Config.Instance.UseCustomMapping)
            {
                // Use Default Mapping
                localXml = Path.Combine(Application.StartupPath, "mappings.xml");
                bool getNewest = true;
                if (File.Exists(localXml))
                {
                    string localVersion = GetLocalVersion(localXml);
                    string newestVersion = GetNewestVersion();
                    if (newestVersion != string.Empty && localVersion == newestVersion)
                    {
                        getNewest = false;
                    }
                }

                // Download if needed
                if (getNewest)
                {
                    Logger.Instance.AddLog(LogType.Debug, "Download new Mapping from");
                    using (MappingDownloadForm dlg = new MappingDownloadForm(localXml))
                    {
                        dlg.ShowDialog();
                    }
                }
            }

            XmlDocument doc = new XmlDocument();
            Logger.Instance.AddLog(LogType.Debug, "Load Mapping from {0}", localXml);
            try
            {
                doc.Load(localXml);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Mapping File not found: {0}", ex.Message));
                return;
            }

            // Loop thru the Games
            List<Addon> addonList = new List<Addon>();
            foreach (XmlNode gameNode in doc.DocumentElement.ChildNodes)
            {
                if (gameNode.Attributes["Name"].Value == GameType.ConvertToString(GameType.Enum.WorldOfWarcraft))
                {
                    // Loop thru all Addons
                    foreach (XmlNode addonNode in gameNode.ChildNodes)
                    {
                        // Addon
                        if (addonNode.Name == "Addon")
                        {
                            // Get the Addon Name
                            string addonName = addonNode.Attributes["Name"].Value;
                            // Search the List if there is already an Addon with this Name
                            Addon addon = addonList.Find(delegate(Addon ad) { return ad.Name.Equals(addonName); });
                            // If not, create it
                            if (addon == null)
                            {
                                addon = new Addon(addonName);
                            }
                            // Remove the Addon from the Local Addons List
                            localAddons.Remove(addon);

                            // Set the Addon as Main
                            addon.IsMain = true;

                            // Loop thru the Mappings
                            XmlNode mappingListElement = addonNode.SelectSingleNode("Mappings");
                            if (mappingListElement != null)
                            {
                                foreach (XmlNode mappingElement in mappingListElement.ChildNodes)
                                {
                                    string tag = mappingElement.Attributes["Tag"].Value;
                                    AddonSiteId addonSiteId = (AddonSiteId)Enum.Parse(typeof(AddonSiteId), mappingElement.Attributes["Site"].Value);
                                    Mapping mapping = new Mapping(tag, addonSiteId);
                                    mapping.Addon = addon;
                                    addon.Mappings.Add(mapping);
                                }
                            }

                            // Loop thru the SubAddons
                            XmlNode subAddonListElement = addonNode.SelectSingleNode("SubAddons");
                            if (subAddonListElement != null)
                            {
                                foreach (XmlNode subAddonElement in subAddonListElement.ChildNodes)
                                {
                                    string subAddonName = subAddonElement.Attributes["Name"].Value;
                                    Addon subAddon = addonList.Find(delegate(Addon ad) { return ad.Name.Equals(subAddonName); });
                                    if (subAddon == null)
                                    {
                                        subAddon = new Addon(subAddonName);
                                    }
                                    subAddon.IsSubAddon = true;
                                    addon.SubAddons.Add(subAddon);
                                    // Remove the SubAddon from the Local Addons List
                                    localAddons.Remove(subAddon);

                                    // Add the SubAddon to the List
                                    Helpers.AddIfNeeded<Addon>(addonList, subAddon);
                                }
                            }

                            // Add the Addon to the List
                            Helpers.AddIfNeeded<Addon>(addonList, addon);
                        }
                        // Package
                        else if (addonNode.Name == "Package")
                        {
                            // TODO: Handle Packages
                        }
                    }
                }
            }

            // We now have removed all Addons with Mapping and their SubAddons from the Local Addon List
            // Add the remaining Local Addons (with no Mapping) to the AddonList
            foreach (Addon addon in localAddons)
            {
                addon.IsUnhandled = true;
                addonList.Add(addon);
            }

            // Sort the List
            addonList.Sort(
                delegate(Addon x, Addon y)
                {
                    return string.Compare(x.Name, y.Name);
                }
            );

            Addons.AddRange(addonList);
        }
    }
}
