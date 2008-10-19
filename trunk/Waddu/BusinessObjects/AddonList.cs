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
                            }
                        }

                        // Add the Addon to the List
                        addonList.Add(addon);
                    }
                }
            }

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
