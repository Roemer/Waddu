using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Waddu.Types;
using System.Collections.Generic;
using Waddu.BusinessObjects;

namespace Waddu.Classes
{
    public class Config
    {
        // Instance
        private static Config _instance;
        public static Config Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Config();
                }
                return _instance;
            }
        }

        // Members
        private XmlDocument _xmlDoc;
        private string _configFilePath;

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

        private bool _savePassword = false;
        public bool SavePassword
        {
            get { return _savePassword; }
            set { _savePassword = value; }
        }

        private string _mappingFile = "http://www.flauschig.ch/transfer/waddu_mappings.xml";
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

        private bool _preferNoLib = false;
        public bool PreferNoLib
        {
            get { return _preferNoLib; }
            set { _preferNoLib = value; }
        }

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
            // Build config Path
            string configFileName = "waddu_config.xml";
            _configFilePath = Path.Combine(Application.StartupPath, configFileName);

            // Create File if it doesn't exist
            if (!File.Exists(_configFilePath))
            {
                XmlTextWriter w = new XmlTextWriter(_configFilePath, null);
                w.WriteStartDocument();
                w.WriteStartElement("Waddu_Config");
                w.WriteAttributeString("Version", this.GetType().Assembly.GetName().Version.ToString());
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
            if (GetSettingValue("PathTo7z", out value))
            {
                PathTo7z = value;
            }
            // Get Addon Sites
            if (GetSettingValue("AddonSites", out value))
            {
                string[] addonSiteList = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string addonSite in addonSiteList)
                {
                    _addonSites.Add((AddonSiteId)Enum.Parse(typeof(AddonSiteId), addonSite));
                }
            }
            // Fill missing AddonSites
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.curse);
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.curseforge);
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.direct);
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.wowace);
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.wowinterface);
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.wowspecial);
            Helpers.AddIfNeeded<AddonSiteId>(_addonSites, AddonSiteId.wowui);
            // Get Ignored Addons
            if (GetSettingValue("IgnoredAddons", out value))
            {
                string[] addonList = value.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                _ignoredAddons.AddRange(addonList);
            }
            // Get Preferred Mappings
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (GetSettingDict("PreferredMappings", dict))
            {
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    _preferredMappings.Add(kvp.Key, (AddonSiteId)Enum.Parse(typeof(AddonSiteId), kvp.Value));
                }
            }
        }

        private XmlElement GetSettingElement(string settingName)
        {
            XmlElement settingElement = _xmlDoc.DocumentElement.SelectSingleNode(string.Format(@"settings/setting[@name=""{0}""]", settingName)) as XmlElement;
            return settingElement;
        }

        private bool GetSettingValue(string settingName, out string value)
        {
            XmlElement settingElement = GetSettingElement(settingName);
            if (settingElement != null)
            {
                value = settingElement.ChildNodes[0].InnerText;
                return true;
            }
            value = string.Empty;
            return false;
        }

        private bool GetSettingDict(string settingName, Dictionary<string, string> dict)
        {
            XmlElement settingElement = GetSettingElement(settingName);
            dict = new Dictionary<string, string>();
            if (settingElement != null)
            {
                foreach (XmlNode child in settingElement.ChildNodes)
                {
                    string key = child.Attributes["key"].Value;
                    string value = child.Attributes["value"].Value;
                    dict.Add(key, value);
                }
                return true;
            }
            return false;
        }

        public void SaveSettings()
        {
            // Update Version
            _xmlDoc.DocumentElement.Attributes["Version"].Value = this.GetType().Assembly.GetName().Version.ToString();

            // Save Settings
            SaveSetting("WowFolderPath", WowFolderPath);
            SaveSetting("DeleteBeforeUpdate", DeleteBeforeUpdate.ToString());
            SaveSetting("MoveToTrash", MoveToTrash.ToString());
            SaveSetting("NumberOfThreads", NumberOfThreads.ToString());
            SaveSetting("CurseLogin", CurseLogin);
            if (SavePassword)
            {
                SaveSetting("CursePassword", CursePassword);
            }
            else
            {
                SaveSetting("CursePassword", "");
            }
            SaveSetting("SavePassword", SavePassword.ToString());
            SaveSetting("MappingFile", MappingFile);
            SaveSetting("LogLevel", LogLevel.ToString());
            SaveSetting("PreferNoLib", PreferNoLib.ToString());
            SaveSetting("PathTo7z", PathTo7z);
            SaveSetting("AddonSites", Helpers.Join<AddonSiteId>("|", AddonSites));
            SaveSetting("IgnoredAddons", Helpers.Join<string>("|", IgnoredAddons));
            SaveSettingDict("PreferredMappings", PreferredMappings);
            _xmlDoc.Save(_configFilePath);
        }

        private XmlElement CreateSetting(string settingName)
        {
            XmlElement settingElement = _xmlDoc.CreateElement("setting");
            // Name Attribute
            XmlAttribute nameAttribute = _xmlDoc.CreateAttribute("name");
            nameAttribute.Value = settingName;
            settingElement.Attributes.Append(nameAttribute);
            return settingElement;
        }

        private XmlElement CreateSetting(string settingName, string value)
        {
            XmlElement settingElement = CreateSetting(settingName);
            // Value Element
            XmlElement valueElement = _xmlDoc.CreateElement("value");
            valueElement.InnerText = value;
            settingElement.AppendChild(valueElement);
            return settingElement;
        }

        private void SaveSettingDict<TKey, TValue>(string settingName, IDictionary<TKey, TValue> dict)
        {
            XmlElement settingListElement = FindElement(_xmlDoc.DocumentElement, "settings", true);
            // Setting Element
            XmlElement settingElement = settingListElement.SelectSingleNode(string.Format(@"setting[@name=""{0}""]", settingName)) as XmlElement;
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

            foreach (KeyValuePair<TKey, TValue> kvp in dict)
            {
                XmlElement entryElement = _xmlDoc.CreateElement("entry");
                XmlAttribute keyAttribute = _xmlDoc.CreateAttribute("key");
                keyAttribute.Value = kvp.Key.ToString();
                entryElement.Attributes.Append(keyAttribute);
                XmlAttribute valueAttribute = _xmlDoc.CreateAttribute("value");
                valueAttribute.Value = kvp.Value.ToString();
                entryElement.Attributes.Append(valueAttribute);
                settingElement.AppendChild(entryElement);
            }
        }

        private void SaveSetting(string settingName, string value)
        {
            XmlElement settingListElement = FindElement(_xmlDoc.DocumentElement, "settings", true);

            // Setting Element
            XmlElement settingElement = settingListElement.SelectSingleNode(string.Format(@"setting[@name=""{0}""]", settingName)) as XmlElement;
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
                settingElement.ChildNodes[0].InnerText = value.ToString();
            }
        }

        private XmlElement FindElement(XmlElement parentElement, string elementName, bool createIfNeeded)
        {
            XmlElement subElement = parentElement.SelectSingleNode(elementName) as XmlElement;
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
        public bool IsIgnored(Addon addon)
        {
            return _ignoredAddons.Contains(addon.Name);
        }
        public bool AddIgnored(Addon addon)
        {
            if (IsIgnored(addon))
            {
                return false;
            }
            _ignoredAddons.Add(addon.Name);
            return true;
        }
        public bool RemoveIgnored(Addon addon)
        {
            if (IsIgnored(addon))
            {
                _ignoredAddons.Remove(addon.Name);
                return true;
            }
            return false;
        }

        public void SetPreferredMapping(Mapping mapping)
        {
            Helpers.AddOrUpdate<string, AddonSiteId>(_preferredMappings, mapping.Addon.Name, mapping.AddonSiteId);
        }
        public bool GetPreferredMapping(Addon addon, out AddonSiteId preferredAddonSite)
        {
            if (_preferredMappings.ContainsKey(addon.Name))
            {
                preferredAddonSite = _preferredMappings[addon.Name];
                return true;
            }
            preferredAddonSite = AddonSiteId.curse;
            return false;
        }
        #endregion
    }
}
