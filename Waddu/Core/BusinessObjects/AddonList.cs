using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Waddu.Types;
using Waddu.UI.Forms;

namespace Waddu.Core.BusinessObjects
{
    public class AddonList
    {
        public static AddonList Instance = new AddonList();

        public List<Addon> Addons;

        private string GetNewestVersion()
        {
            string version;
            var success = WebHelper.GetString("http://waddu.flauschig.ch/mapping/latest.txt", out version);
            return success ? version : string.Empty;
        }

        private string GetLocalVersion(string xmlFile)
        {
            var doc = new XmlDocument();
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
            return Addons.Find(obj => (obj.Name.ToUpper().Equals(addonName.ToUpper())));
        }

        // Private Constructor
        private AddonList()
        {
            // Initialize List
            Addons = new List<Addon>();

            // Get all Local Addons
            var addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            var localAddons = new List<Addon>();
            if (Directory.Exists(addonFolderPath))
            {
                foreach (var addonPath in Directory.GetDirectories(addonFolderPath))
                {
                    var addon = new Addon(Path.GetFileName(addonPath));
                    localAddons.Add(addon);
                }
            }

            // Load XML File
            var localXml = Config.Instance.MappingFile;
            if (!Config.Instance.UseCustomMapping)
            {
                // Use Default Mapping
                localXml = Path.Combine(Application.StartupPath, "mappings.xml");
                var getNewest = true;
                if (File.Exists(localXml))
                {
                    var localVersion = GetLocalVersion(localXml);
                    var newestVersion = GetNewestVersion();
                    if (newestVersion != string.Empty && localVersion == newestVersion)
                    {
                        getNewest = false;
                    }
                }

                // Download if needed
                if (getNewest)
                {
                    Logger.Instance.AddLog(LogType.Debug, "Download new Mapping from");
                    using (var dlg = new MappingDownloadForm(localXml))
                    {
                        dlg.ShowDialog();
                    }
                }
            }

            var doc = new XmlDocument();
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
            var addonList = new List<Addon>();
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
                            var addonName = addonNode.Attributes["Name"].Value;
                            // Search the List if there is already an Addon with this Name
                            var addon = addonList.Find(ad => ad.Name.Equals(addonName));
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
                            var mappingListElement = addonNode.SelectSingleNode("Mappings");
                            if (mappingListElement != null)
                            {
                                foreach (XmlNode mappingElement in mappingListElement.ChildNodes)
                                {
                                    var tag = mappingElement.Attributes["Tag"].Value;
                                    var addonSiteId = (AddonSiteId)Enum.Parse(typeof(AddonSiteId), mappingElement.Attributes["Site"].Value);
                                    var mapping = new Mapping(tag, addonSiteId);
                                    // Skip wowui Mappings since they seem to have shut down
                                    if (mapping.AddonSiteId == AddonSiteId.wowui)
                                    {
                                        continue;
                                    }
                                    mapping.Addon = addon;
                                    addon.Mappings.Add(mapping);
                                }
                            }

                            // Loop thru the SubAddons
                            var subAddonListElement = addonNode.SelectSingleNode("SubAddons");
                            if (subAddonListElement != null)
                            {
                                foreach (XmlNode subAddonElement in subAddonListElement.ChildNodes)
                                {
                                    var subAddonName = subAddonElement.Attributes["Name"].Value;
                                    var subAddon = addonList.Find(ad => ad.Name.Equals(subAddonName));
                                    if (subAddon == null)
                                    {
                                        subAddon = new Addon(subAddonName);
                                    }
                                    subAddon.IsSubAddon = true;
                                    addon.SubAddons.Add(subAddon);
                                    // Remove the SubAddon from the Local Addons List
                                    localAddons.Remove(subAddon);

                                    // Add the SubAddon to the List
                                    Helpers.AddIfNeeded(addonList, subAddon);
                                }
                            }

                            // Add the Addon to the List
                            Helpers.AddIfNeeded(addonList, addon);
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
            foreach (var addon in localAddons)
            {
                addon.IsUnhandled = true;
                addonList.Add(addon);
            }

            // Sort the List
            addonList.Sort((x, y) => String.CompareOrdinal(x.Name, y.Name));

            Addons.AddRange(addonList);
        }
    }
}
