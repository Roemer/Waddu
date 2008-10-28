using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Waddu.Types;

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

        private string _pathTo7z = @"C:\Program Files\7-Zip\7z.exe";
        public string PathTo7z
        {
            get { return _pathTo7z; }
            set { _pathTo7z = value; }
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
            if (GetSetting("WowFolderPath", out value))
            {
                WowFolderPath = value;
            }
            if (GetSetting("DeleteBeforeUpdate", out value))
            {
                DeleteBeforeUpdate = Convert.ToBoolean(value);
            }
            if (GetSetting("MoveToTrash", out value))
            {
                MoveToTrash = Convert.ToBoolean(value);
            }
            if (GetSetting("NumberOfThreads", out value))
            {
                NumberOfThreads = Convert.ToInt32(value);
            }
            if (GetSetting("CurseLogin", out value))
            {
                CurseLogin = value;
            }
            if (GetSetting("CursePassword", out value))
            {
                CursePassword = value;
            }
            if (GetSetting("SavePassword", out value))
            {
                SavePassword = Convert.ToBoolean(value);
            }
            if (GetSetting("MappingFile", out value))
            {
                MappingFile = value;
            }
            if (GetSetting("LogLevel", out value))
            {
                LogLevel = (LogType)Enum.Parse(typeof(LogType), value);
            }
            if (GetSetting("PreferNoLib", out value))
            {
                PreferNoLib = Convert.ToBoolean(value);
            }
            if (GetSetting("PathTo7z", out value))
            {
                PathTo7z = value;
            }    
        }

        private bool GetSetting(string settingName, out string value)
        {
            XmlElement settingElement = _xmlDoc.DocumentElement.SelectSingleNode(string.Format(@"settings/setting[@name=""{0}""]", settingName)) as XmlElement;
            if (settingElement != null)
            {
                value = settingElement.ChildNodes[0].InnerText;
                return true;
            }
            value = string.Empty;
            return false;
        }

        public void SaveSettings()
        {
            SaveSetting("WowFolderPath", WowFolderPath);
            SaveSetting("DeleteBeforeUpdate", DeleteBeforeUpdate);
            SaveSetting("MoveToTrash", MoveToTrash);
            SaveSetting("NumberOfThreads", NumberOfThreads);
            SaveSetting("CurseLogin", CurseLogin);
            if (SavePassword)
            {
                SaveSetting("CursePassword", CursePassword);
            }
            else
            {
                SaveSetting("CursePassword", "");
            }
            SaveSetting("SavePassword", SavePassword);
            SaveSetting("MappingFile", MappingFile);
            SaveSetting("LogLevel", LogLevel);
            SaveSetting("PreferNoLib", PreferNoLib);
            SaveSetting("PathTo7z", PathTo7z);
            _xmlDoc.Save(_configFilePath);
        }

        private void SaveSetting(string settingName, object value)
        {
            XmlElement settingListElement = FindElement(_xmlDoc.DocumentElement, "settings", true);

            // Setting Element
            XmlElement settingElement = settingListElement.SelectSingleNode(string.Format(@"setting[@name=""{0}""]", settingName)) as XmlElement;
            if (settingElement == null)
            {
                settingElement = _xmlDoc.CreateElement("setting");
                // Name Attribute
                XmlAttribute nameAttribute = _xmlDoc.CreateAttribute("name");
                nameAttribute.Value = settingName;
                settingElement.Attributes.Append(nameAttribute);

                // Value Element
                XmlElement valueElement = _xmlDoc.CreateElement("value");
                valueElement.InnerText = value.ToString();
                settingElement.AppendChild(valueElement);

                // Append Setting to SettingList
                settingListElement.AppendChild(settingElement);
            }
            else
            {
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
    }
}
