using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

namespace Waddu.Core.BusinessObjects
{
    public class UpdateStatusList
    {
        private class UpdateStatusObject
        {
            public string Name;
            public string Version;
            public DateTime Date;
        }

        private static List<UpdateStatusObject> _addonNameList = new List<UpdateStatusObject>();

        public static void Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(Application.StartupPath, "UpdateStatus.xml"));
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                UpdateStatusObject obj = new UpdateStatusObject();
                obj.Name = node.Name;
                obj.Version = node.Attributes["Version"].Value;
                Match m = Regex.Match(node.Attributes["Date"].Value, @"(.*)\.(.*)\.(.*) (.*):(.*):(.*)");
                int year = Convert.ToInt32(m.Groups[1].Captures[0].Value);
                int month = Convert.ToInt32(m.Groups[2].Captures[0].Value);
                int day = Convert.ToInt32(m.Groups[3].Captures[0].Value);
                int hour = Convert.ToInt32(m.Groups[4].Captures[0].Value);
                int min = Convert.ToInt32(m.Groups[5].Captures[0].Value);
                int sec = Convert.ToInt32(m.Groups[6].Captures[0].Value);
                obj.Date = new DateTime(year, month, day, hour, min, sec);
                _addonNameList.Add(obj);
            }
        }

        public static DateTime Get(string addonName)
        {
            UpdateStatusObject statusObj = _addonNameList.Find(delegate(UpdateStatusObject obj) { return (obj.Name.ToUpper().Equals(addonName.ToUpper())); });
            if (statusObj != null)
            {
                return statusObj.Date;
            }
            return DateTime.MinValue;
        }

        public static void Set(string addonName, string version)
        {
            UpdateStatusObject statusObj = _addonNameList.Find(delegate(UpdateStatusObject obj) { return (obj.Name.ToUpper().Equals(addonName.ToUpper())); });
            if (statusObj == null)
            {
                statusObj = new UpdateStatusObject();
                statusObj.Name = addonName;
                _addonNameList.Add(statusObj);
            }
            statusObj.Version = version;
            statusObj.Date = DateTime.Now;
        }

        public static void Save()
        {
            string xmlFile = Path.Combine(Application.StartupPath, "UpdateStatus.xml");
            if (File.Exists(xmlFile))
            {
                File.Delete(xmlFile);
            }

            XmlTextWriter w = new XmlTextWriter(xmlFile, null);
            w.WriteStartDocument();
            w.WriteStartElement("WadduUpdateStatus");

            foreach (UpdateStatusObject obj in _addonNameList)
            {
                w.WriteStartElement(obj.Name);
                w.WriteStartAttribute("Date");
                w.WriteString(obj.Date.ToString("yyyy.MM.dd HH:mm:ss"));
                w.WriteEndAttribute();
                w.WriteStartAttribute("Version");
                w.WriteString(obj.Version);
                w.WriteEndAttribute();
                w.WriteEndElement();
            }

            w.WriteEndElement();
            w.WriteEndDocument();
            w.Close();

            // Open and Save again (pretty Formatting)
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);
            xmlDoc.Save(xmlFile);
        }
    }
}
