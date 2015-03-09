﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Text.RegularExpressions;

namespace Waddu.Core.BusinessObjects
{
    public class UpdateStatusList
    {
        private static readonly List<AddonUpdateStats> AddonNameList = new List<AddonUpdateStats>();

        private static readonly object LockObject = new object();

        private static string GetFilePath()
        {
            return Path.Combine(Application.StartupPath, "UpdateStatus.xml");
        }

        public static void Load()
        {
            string filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                var doc = new XmlDocument();
                doc.Load(filePath);
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    var obj = new AddonUpdateStats();
                    obj.Name = node.Attributes["Name"].Value;
                    obj.Version = node.Attributes["Version"].Value;
                    Match m = Regex.Match(node.Attributes["Date"].Value, @"(.*)\.(.*)\.(.*) (.*):(.*):(.*)");
                    int year = Convert.ToInt32(m.Groups[1].Captures[0].Value);
                    int month = Convert.ToInt32(m.Groups[2].Captures[0].Value);
                    int day = Convert.ToInt32(m.Groups[3].Captures[0].Value);
                    int hour = Convert.ToInt32(m.Groups[4].Captures[0].Value);
                    int min = Convert.ToInt32(m.Groups[5].Captures[0].Value);
                    int sec = Convert.ToInt32(m.Groups[6].Captures[0].Value);
                    obj.LastUpdated = new DateTime(year, month, day, hour, min, sec);
                    AddonNameList.Add(obj);
                }
            }
        }

        public static AddonUpdateStats Get(string addonName)
        {
            var statusObj = AddonNameList.Find(delegate(AddonUpdateStats obj) { return (obj.Name.ToUpper().Equals(addonName.ToUpper())); });
            if (statusObj != null)
            {
                return statusObj;
            }
            return null;
        }

        public static void Set(string addonName, string version)
        {
            var statusObj = AddonNameList.Find(delegate(AddonUpdateStats obj) { return (obj.Name.ToUpper().Equals(addonName.ToUpper())); });
            if (statusObj == null)
            {
                statusObj = new AddonUpdateStats();
                statusObj.Name = addonName;
                AddonNameList.Add(statusObj);
            }
            statusObj.Version = version;
            statusObj.LastUpdated = DateTime.Now;
        }

        public static void Save()
        {
            // Lock the File Access for Thread-Safety
            lock (LockObject)
            {
                string xmlFile = GetFilePath();
                if (File.Exists(xmlFile))
                {
                    File.Delete(xmlFile);
                }

                XmlTextWriter w = new XmlTextWriter(xmlFile, null);
                w.Formatting = Formatting.Indented;
                w.WriteStartDocument();
                w.WriteStartElement("WadduUpdateStatus");

                foreach (AddonUpdateStats obj in AddonNameList)
                {
                    w.WriteStartElement("Addon");
                    w.WriteStartAttribute("Name");
                    w.WriteString(obj.Name);
                    w.WriteEndAttribute();
                    w.WriteStartAttribute("Date");
                    w.WriteString(obj.LastUpdated.ToString("yyyy.MM.dd HH:mm:ss"));
                    w.WriteEndAttribute();
                    w.WriteStartAttribute("Version");
                    w.WriteString(obj.Version);
                    w.WriteEndAttribute();
                    w.WriteEndElement();
                }

                w.WriteEndElement();
                w.WriteEndDocument();
                w.Close();
            }
        }
    }
}
