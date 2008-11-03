using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Waddu.Classes;
using Waddu.Classes.Win32;
using Waddu.Types;

namespace Waddu.BusinessObjects
{
    public class Addon : INotifyPropertyChanged
    {
        #region Members
        private string _name;
        public string Name
        {
            get { return _name; }
        }

        [DisplayName("Your Version")]
        public string LocalVersion
        {
            get { return GetLocalVersion(); }
        }

        private bool _isSubAddon = false;
        [Browsable(false)]
        public bool IsSubAddon
        {
            get { return _isSubAddon; }
            set { _isSubAddon = value; }
        }

        [Browsable(false)]
        public bool IsInstalled
        {
            get
            {
                string addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
                addonFolderPath = Path.Combine(addonFolderPath, Name);
                return Directory.Exists(addonFolderPath);
            }
        }

        private bool _isUnhandled = false;
        [Browsable(false)]
        public bool IsUnhandled
        {
            get { return _isUnhandled; }
            set { _isUnhandled = value; }
        }

        private Mapping _preferredMapping = null;
        public Mapping PreferredMapping
        {
            get { return _preferredMapping; }
            set { _preferredMapping = value; NotifyPropertyChanged("PreferredMapping"); }
        }

        private BindingList<Addon> _subAddonList;
        public BindingList<Addon> SubAddons
        {
            get { return _subAddonList; }
            set { _subAddonList = value; }
        }

        private BindingList<Mapping> _mappingList;
        public BindingList<Mapping> Mappings
        {
            get { return _mappingList; }
            set { _mappingList = value; }
        }
        #endregion

        #region Constructors
        private Addon()
        {
            _subAddonList = new BindingList<Addon>();
            _mappingList = new BindingList<Mapping>();

            _mappingList.ListChanged += new ListChangedEventHandler(_mappingList_ListChanged);
        }

        public Addon(string name)
            : this()
        {
            _name = name;
            NotifyPropertyChanged("Name");
        }
        #endregion

        #region Functions
        private void _mappingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Mapping newMapping = _mappingList[e.NewIndex];

                if (_preferredMapping == null)
                {
                    _preferredMapping = newMapping;
                }
                else
                {
                    // Assign by Preferred
                    AddonSiteId preferred;
                    if (Config.Instance.GetPreferredMapping(this, out preferred))
                    {
                        if (newMapping.AddonSiteId == preferred)
                        {
                            _preferredMapping = newMapping;
                            return;
                        }
                    }

                    // Assign by Priority
                    int indexOld = Config.Instance.AddonSites.IndexOf(_preferredMapping.AddonSiteId);
                    int indexNew = Config.Instance.AddonSites.IndexOf(newMapping.AddonSiteId);
                    if (indexNew >= 0 && indexNew < indexOld)
                    {
                        _preferredMapping = newMapping;
                    }
                }
            }
        }

        /// <summary>
        /// Get the Local Version of an Addon
        /// </summary>
        private string GetLocalVersion()
        {
            // Check for Blizzard Addon
            if (Mappings.Count > 0)
            {
                if (Mappings[0].AddonSiteId == AddonSiteId.blizzard)
                {
                    return "None";
                }
            }

            // Check for SubAddon
            if (IsSubAddon)
            {
                return "SubAddon";
            }

            // Check if it is Installed
            if (!IsInstalled)
            {
                return "Not Installed";
            }

            string addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            addonFolderPath = Path.Combine(addonFolderPath, Name);

            // Try by Changelog
            foreach (string file in Directory.GetFiles(addonFolderPath, "Changelog*"))
            {
                int start = file.IndexOf("-") + 1;
                start = file.IndexOf("-", start) + 1;
                int end = file.LastIndexOf(".txt");
                if (start > 0 && end > 0)
                {
                    return file.Substring(start, end - start);
                }
            }

            // Try by .toc File
            foreach (string file in Directory.GetFiles(addonFolderPath, "*.toc"))
            {
                foreach (string line in File.ReadAllLines(file))
                {
                    if (line.Contains("## Version:"))
                    {
                        return line.Substring(11).Trim() + " (TOC)";
                    }
                }
            }

            // Special
            if (Name == "MarsPartyBuff")
            {
                string versionFile = Path.Combine(addonFolderPath, "VERSION.TXT");
                if (File.Exists(versionFile))
                {
                    return File.ReadAllLines(versionFile)[0];
                }
            }

            // Unknown
            return "Unknown";
        }

        /// <summary>
        /// Deletes an Addon
        /// </summary>
        public DeleteType Delete()
        {
            string addonPath = Path.Combine(Addon.GetFolderPath(), Name);
            if (!Directory.Exists(addonPath))
            {
                return DeleteType.Inexistent;
            }
            if (Config.Instance.MoveToTrash)
            {
                int ret = NativeMethods.MoveToRecycleBin(addonPath);
                if (ret != 0)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("Move to bin failed: {0}", ret));
                    return DeleteType.Failed;
                }
                return DeleteType.MovedToTrash;
            }
            else
            {
                Directory.Delete(addonPath, true);
                return DeleteType.Deleted;
            }
        }

        /// <summary>
        /// Get the Local Folder Path
        /// </summary>
        public static string GetFolderPath()
        {
            string folderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            return folderPath;
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) { return false; }
            if (obj.GetType() != typeof(Addon)) { return false; }
            return (this as Addon).Name.Equals((obj as Addon).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
