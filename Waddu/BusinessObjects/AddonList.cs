using System;
using System.Collections.Generic;
using System.Xml;
using Waddu.Types;
using System.IO;
using System.Windows.Forms;
using Waddu.Classes;

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

        // Private Constructor
        private AddonList()
        {
            // Initialize List
            Addons = new List<Addon>();

            // Get all Local Addons
            string addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            List<Addon> localAddons = new List<Addon>();
            foreach (string addonPath in Directory.GetDirectories(addonFolderPath))
            {
                Addon addon = new Addon(Path.GetFileName(addonPath));
                localAddons.Add(addon);
            }

            // Load XML File
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(Config.Instance.MappingFile);
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
                        // Create Addon
                        string addonName = addonNode.Attributes["Name"].Value;
                        Addon addon = new Addon(addonName);
                        // Remove the Addon from the Local Addons List
                        localAddons.Remove(addon);

                        // Loop thru the Mappings
                        XmlNode mappingListElement = addonNode.SelectSingleNode("Mappings");
                        if (mappingListElement != null)
                        {
                            foreach (XmlNode mappingElement in mappingListElement.ChildNodes)
                            {
                                Mapping mapping = new Mapping();
                                mapping.AddonName = addonName;
                                mapping.AddonSiteId = (AddonSiteId)Enum.Parse(typeof(AddonSiteId), mappingElement.Attributes["Site"].Value);
                                mapping.AddonTag = mappingElement.Attributes["Tag"].Value;
                                addon.Mappings.Add(mapping);
                            }
                        }

                        // Loop thru the SubAddons
                        XmlNode subAddonListElement = addonNode.SelectSingleNode("SubAddons");
                        if (subAddonListElement != null)
                        {
                            foreach (XmlNode subAddonElement in subAddonListElement.ChildNodes)
                            {
                                Addon subAddon = new Addon(subAddonElement.Attributes["Name"].Value);
                                subAddon.IsSubAddon = true;
                                addon.SubAddons.Add(subAddon);
                                // Remove the SubAddon from the Local Addons List
                                localAddons.Remove(subAddon);
                            }
                        }

                        // Add the Addon to the List
                        addonList.Add(addon);
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
