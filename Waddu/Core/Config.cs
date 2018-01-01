using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Waddu.Core.BusinessObjects;
using Waddu.Types;

namespace Waddu.Core
{
    public class Config
    {
        // Instance
        public static Config Instance = new Config();

        // Members
        private readonly XmlDocument _xmlDoc;
        private readonly string _configFilePath;

        # region Settings
        private string _wowFolderPath = @"C:\Program Files\World of Warcraft";
        public string WowFolderPath
        {
            get { return _wowFolderPath; }
            set { _wowFolderPath = value; }
        }

        private bool _deleteBeforeUpdate = true;
        public bool DeleteBeforeUpdate
        {
            get { return _deleteBeforeUpdate; }
            set { _deleteBeforeUpdate = value; }
        }

        private bool _moveToTrash = true;
        public bool MoveToTrash
        {
            get { return _moveToTrash; }
            set { _moveToTrash = value; }
        }

        private int _numberOfThreads = 3;
        public int NumberOfThreads
        {
            get { return _numberOfThreads; }
            set { _numberOfThreads = value; }
        }

        private string _curseLogin = "";
        public string CurseLogin
        {
            get { return _curseLogin; }
            set { _curseLogin = value; }
        }

        private string _cursePassword = "";
        public string CursePassword
        {
            get { return _cursePassword; }
            set { _cursePassword = value; }
        }

        public bool SavePassword { get; set; }

        public bool UseCustomMapping { get; set; }

        private string _mappingFile = @".\mappings.xml";
        public string MappingFile
        {
            get { return _mappingFile; }
            set { _mappingFile = value; }
        }

        private LogType _logLevel = LogType.Information;
        public LogType LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }

        public bool PreferNoLib { get; set; }

        public bool UseOlderNoLib { get; set; }

        private string _pathTo7z = @"C:\Program Files\7-Zip";
        public string PathTo7z
        {
            get { return _pathTo7z; }
            set { _pathTo7z = value; }
        }

        private List<AddonSiteId> _addonSites = new List<AddonSiteId>();
        public List<AddonSiteId> AddonSites
        {
            get { return _addonSites; }
            set { _addonSites = value; }
        }

        private List<string> _ignoredAddons = new List<string>();
        public List<string> IgnoredAddons
        {
            get { return _ignoredAddons; }
            set { _ignoredAddons = value; }
        }

        private Dictionary<string, AddonSiteId> _preferredMappings = new Dictionary<string, AddonSiteId>();
        public Dictionary<string, AddonSiteId> PreferredMappings
        {
            get { return _preferredMappings; }
            set { _preferredMappings = value; }
        }
        #endregion

        // Constructor
        private Config()
        {
            UseOlderNoLib = false;
            PreferNoLib = false;
            UseCustomMapping = false;
            SavePassword = false;
            // Build config Path
            var configFileName = "waddu_config.xml";
            _configFilePath = Path.Combine(Application.StartupPath, configFileName);

            // Create File if it doesn't exist
            if (!File.Exists(_configFilePath))
            {
                var w = new XmlTextWriter(_configFilePath, null);
                w.WriteStartDocument();
                w.WriteStartElement("Waddu_Config");
                w.WriteAttributeString("Version", GetType().Assembly.GetName().Version.ToString());
                w.WriteEndElement();
                w.WriteEndDocument();
                w.Close();
            }

            _xmlDoc = new XmlDocument();
            _xmlDoc.Load(_configFilePath);

            LoadSettings();
        }

        private void LoadSettings()
        {
            string value;
            if (GetSettingValue("WowFolderPath", out value))
            {
                WowFolderPath = value;
            }
            if (GetSettingValue("DeleteBeforeUpdate", out value))
            {
                DeleteBeforeUpdate = Convert.ToBoolean(value);
            }
            if (GetSettingValue("MoveToTrash", out value))
            {
                MoveToTrash = Convert.ToBoolean(value);
            }
            if (GetSettingValue("NumberOfThreads", out value))
            {
                NumberOfThreads = Convert.ToInt32(value);
            }
            if (GetSettingValue("CurseLogin", out value))
            {
                CurseLogin = value;
            }
            if (GetSettingValue("CursePassword", out value))
            {
                CursePassword = value;
            }
            if (GetSettingValue("SavePassword", out value))
            {
                SavePassword = Convert.ToBoolean(value);
            }
            if (GetSettingValue("UseCustomMapping", out value))
            {
                UseCustomMapping = Convert.ToBoolean(value);
            }
            if (GetSettingValue("MappingFile", out value))
            {
                MappingFile = value;
            }
            if (GetSettingValue("LogLevel", out value))
            {
                LogLevel = (LogType)Enum.Parse(typeof(LogType), value);
            }
            if (GetSettingValue("PreferNoLib", out value))
            {
                PreferNoLib = Convert.ToBoolean(value);
            }
            if (GetSettingValue("UseOlderNoLib", out value))
            {
                UseOlderNoLib = Convert.ToBoolean(value);
            }
            if (GetSettingValue("PathTo7z", out value))
            {
                PathTo7z = value;
            }
            // Get Addon Sites
            if (GetSettingValue("AddonSites", out value))
            {
                var addonSiteList = value.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var addonSite in addonSiteList)
                {
                    AddonSiteId result;
                    if (Enum.TryParse(addonSite, out result))
                    {
                        _addonSites.Add(result);
                    }
                    else
                    {
                        Console.WriteLine("Unknown Addon Site: {0}", addonSite);
                    }
                }
            }
            // Fill missing AddonSites
            Helpers.AddIfNeeded(_addonSites, AddonSiteId.curseforge);
            Helpers.AddIfNeeded(_addonSites, AddonSiteId.direct);
            Helpers.AddIfNeeded(_addonSites, AddonSiteId.wowace);
            Helpers.AddIfNeeded(_addonSites, AddonSiteId.wowinterface);
            Helpers.AddIfNeeded(_addonSites, AddonSiteId.wowspecial);
            // Get Ignored Addons
            if (GetSettingValue("IgnoredAddons", out value))
            {
                var addonList = value.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                _ignoredAddons.AddRange(addonList);
            }
            // Get Preferred Mappings
            Dictionary<string, string> dict;
            if (GetSettingDict("PreferredMappings", out dict))
            {
                foreach (var kvp in dict)
                {
                    AddonSiteId result;
                    if (Enum.TryParse(kvp.Value, out result))
                    {
                        _preferredMappings.Add(kvp.Key, result);
                    }
                }
            }
        }

        private XmlElement GetSettingElement(string settingName)
        {
            var settingElement = _xmlDoc.DocumentElement.SelectSingleNode(string.Format(@"settings/setting[@name=""{0}""]", settingName)) as XmlElement;
            return settingElement;
        }

        private bool GetSettingValue(string settingName, out string value)
        {
            var settingElement = GetSettingElement(settingName);
            if (settingElement != null)
            {
                value = settingElement.ChildNodes[0].InnerText;
                return true;
            }
            value = string.Empty;
            return false;
        }

        private bool GetSettingDict(string settingName, out Dictionary<string, string> dict)
        {
            var settingElement = GetSettingElement(settingName);
            dict = new Dictionary<string, string>();
            if (settingElement != null)
            {
                foreach (XmlNode child in settingElement.ChildNodes)
                {
                    var key = child.Attributes["key"].Value;
                    var value = child.Attributes["value"].Value;
                    dict.Add(key, value);
                }
                return true;
            }
            return false;
        }

        public void SaveSettings()
        {
            // Update Version
            _xmlDoc.DocumentElement.Attributes["Version"].Value = GetType().Assembly.GetName().Version.ToString();

            // Save Settings
            SaveSetting("WowFolderPath", WowFolderPath);
            SaveSetting("DeleteBeforeUpdate", DeleteBeforeUpdate.ToString());
            SaveSetting("MoveToTrash", MoveToTrash.ToString());
            SaveSetting("NumberOfThreads", NumberOfThreads.ToString());
            SaveSetting("CurseLogin", CurseLogin);
            SaveSetting("CursePassword", SavePassword ? CursePassword : "");
            SaveSetting("SavePassword", SavePassword.ToString());
            SaveSetting("UseCustomMapping", UseCustomMapping.ToString());
            SaveSetting("MappingFile", MappingFile);
            SaveSetting("LogLevel", LogLevel.ToString());
            SaveSetting("PreferNoLib", PreferNoLib.ToString());
            SaveSetting("UseOlderNoLib", UseOlderNoLib.ToString());
            SaveSetting("PathTo7z", PathTo7z);
            SaveSetting("AddonSites", Helpers.Join("|", AddonSites));
            SaveSetting("IgnoredAddons", Helpers.Join("|", IgnoredAddons));
            SaveSettingDict("PreferredMappings", PreferredMappings);
            _xmlDoc.Save(_configFilePath);
        }

        private XmlElement CreateSetting(string settingName)
        {
            var settingElement = _xmlDoc.CreateElement("setting");
            // Name Attribute
            var nameAttribute = _xmlDoc.CreateAttribute("name");
            nameAttribute.Value = settingName;
            settingElement.Attributes.Append(nameAttribute);
            return settingElement;
        }

        private XmlElement CreateSetting(string settingName, string value)
        {
            var settingElement = CreateSetting(settingName);
            // Value Element
            var valueElement = _xmlDoc.CreateElement("value");
            valueElement.InnerText = value;
            settingElement.AppendChild(valueElement);
            return settingElement;
        }

        private void SaveSettingDict<TKey, TValue>(string settingName, IDictionary<TKey, TValue> dict)
        {
            var settingListElement = FindElement(_xmlDoc.DocumentElement, "settings", true);
            // Setting Element
            var settingElement = settingListElement.SelectSingleNode(string.Format(@"setting[@name=""{0}""]", settingName)) as XmlElement;
            if (settingElement == null)
            {
                // Create Setting
                settingElement = CreateSetting(settingName);
                // Append Setting to SettingList
                settingListElement.AppendChild(settingElement);
            }
            else
            {
                // Update only
                while (settingElement.ChildNodes.Count > 0)
                {
                    settingElement.RemoveChild(settingElement.ChildNodes[0]);
                }
            }

            foreach (var kvp in dict)
            {
                var entryElement = _xmlDoc.CreateElement("entry");
                var keyAttribute = _xmlDoc.CreateAttribute("key");
                keyAttribute.Value = kvp.Key.ToString();
                entryElement.Attributes.Append(keyAttribute);
                var valueAttribute = _xmlDoc.CreateAttribute("value");
                valueAttribute.Value = kvp.Value.ToString();
                entryElement.Attributes.Append(valueAttribute);
                settingElement.AppendChild(entryElement);
            }
        }

        private void SaveSetting(string settingName, string value)
        {
            var settingListElement = FindElement(_xmlDoc.DocumentElement, "settings", true);

            // Setting Element
            var settingElement = settingListElement.SelectSingleNode(string.Format(@"setting[@name=""{0}""]", settingName)) as XmlElement;
            if (settingElement == null)
            {
                // Create Setting
                settingElement = CreateSetting(settingName, value);
                // Append Setting to SettingList
                settingListElement.AppendChild(settingElement);
            }
            else
            {
                // Update only
                settingElement.ChildNodes[0].InnerText = value;
            }
        }

        private XmlElement FindElement(XmlElement parentElement, string elementName, bool createIfNeeded)
        {
            var subElement = parentElement.SelectSingleNode(elementName) as XmlElement;
            if (subElement == null && createIfNeeded)
            {
                // Create if it doesn't exist yet
                subElement = _xmlDoc.CreateElement(elementName);
                // Append the Sub Element
                parentElement.AppendChild(subElement);
            }
            return subElement;
        }

        #region Public Helpers
        public bool IsIgnored(string addonName)
        {
            return _ignoredAddons.Contains(addonName);
        }
        public bool AddIgnored(string addonName)
        {
            if (IsIgnored(addonName))
            {
                return false;
            }
            _ignoredAddons.Add(addonName);
            return true;
        }
        public bool RemoveIgnored(string addonName)
        {
            if (IsIgnored(addonName))
            {
                _ignoredAddons.Remove(addonName);
                return true;
            }
            return false;
        }

        public void SetPreferredMapping(Mapping mapping)
        {
            SetPreferredMapping(mapping.Addon.Name, mapping.AddonSiteId);
        }
        public void SetPreferredMapping(string addonName, AddonSiteId addonSiteId)
        {
            Helpers.AddOrUpdate(_preferredMappings, addonName, addonSiteId);
        }
        public bool GetPreferredMapping(Addon addon, out AddonSiteId preferredAddonSite)
        {
            if (_preferredMappings.ContainsKey(addon.Name))
            {
                preferredAddonSite = _preferredMappings[addon.Name];
                return true;
            }
            preferredAddonSite = AddonSiteId.curseforge;
            return false;
        }
        #endregion
    }
}
