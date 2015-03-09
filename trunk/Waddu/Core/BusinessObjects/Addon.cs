using System;
using System.ComponentModel;
using System.IO;
using Waddu.Types;
using Waddu.Win32;

namespace Waddu.Core.BusinessObjects
{
    public class Addon : ObservableObject
    {
        #region Members

        public string Name { get; private set; }

        [DisplayName("Your Version")]
        public string LocalVersion
        {
            get { return GetLocalVersion(); }
        }

        public DateTime LastUpdated
        {
            get
            {
                var stats = UpdateStatusList.Get(Name);
                return stats != null ? UpdateStatusList.Get(Name).LastUpdated : DateTime.MinValue;
            }
        }

        private AddonType _addonType;
        public bool IsMain
        {
            get { return (_addonType & AddonType.Main) == AddonType.Main; }
            set
            {
                if (value) { _addonType |= AddonType.Main; }
                else { _addonType &= ~AddonType.Main; }
            }
        }
        public bool IsSubAddon
        {
            get { return (_addonType & AddonType.Sub) == AddonType.Sub; }
            set
            {
                if (value) { _addonType |= AddonType.Sub; }
                else { _addonType &= ~AddonType.Sub; }
            }
        }
        public bool IsDepreciated
        {
            get { return (_addonType & AddonType.Depreciated) == AddonType.Depreciated; }
            set
            {
                if (value) { _addonType |= AddonType.Depreciated; }
                else { _addonType &= ~AddonType.Depreciated; }
            }
        }

        [Browsable(false)]
        public bool IsInstalled
        {
            get
            {
                var addonFolderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
                addonFolderPath = Path.Combine(addonFolderPath, Name);
                return Directory.Exists(addonFolderPath);
            }
        }

        [Browsable(false)]
        public bool IsIgnored
        {
            get
            {
                return Config.Instance.IsIgnored(Name);
            }
            set
            {
                if (value)
                {
                    Config.Instance.AddIgnored(Name);
                }
                else
                {
                    Config.Instance.RemoveIgnored(Name);
                }
                Config.Instance.SaveSettings();
            }
        }

        [Browsable(false)]
        public bool IsUnhandled { get; set; }

        public Mapping PreferredMapping
        {
            get { return GetProperty<Mapping>(); }
            set { SetProperty(value); }
        }

        public BindingList<Addon> SubAddons { get; set; }

        public BindingList<Addon> SuperAddons { get; set; }

        public BindingList<Mapping> Mappings { get; set; }

        public BindingList<Package> Packages { get; set; }

        #endregion

        #region Constructors
        private Addon()
        {
            IsUnhandled = false;
            SubAddons = new BindingList<Addon>();
            SuperAddons = new BindingList<Addon>();
            Mappings = new BindingList<Mapping>();
            Packages = new BindingList<Package>();

            Mappings.ListChanged += _mappingList_ListChanged;
            SubAddons.ListChanged += _subAddonList_ListChanged;
        }

        public Addon(string name)
            : this()
        {
            Name = name;
            OnPropertyChanged(() => Name);
        }
        #endregion

        #region Functions
        private void _subAddonList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                var newSubAddon = SubAddons[e.NewIndex];
                newSubAddon.SuperAddons.Add(this);
            }
        }

        // Assign a new Preferred Mapping
        private void _mappingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                var newMapping = Mappings[e.NewIndex];
                newMapping.Addon = this;
            }
        }

        public void LocalVersionUpdated()
        {
            // Notify Properties Changed
            OnPropertyChanged(() => LastUpdated);
            OnPropertyChanged(() => LocalVersion);
            // Delete the Preferred Mapping
            PreferredMapping = null;
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
            if (IsSubAddon && !IsMain)
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
            foreach (var file in Directory.GetFiles(addonFolderPath, "Changelog*"))
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
            string version;
            if (TocHelper.GetVersion(this, out version))
            {
                return version;
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
            return "?";
        }

        /// <summary>
        /// Deletes an Addon
        /// </summary>
        public DeleteType Delete()
        {
            return DeleteByName(Name);
        }
        public static DeleteType DeleteByName(string addonName)
        {
            string addonPath = Path.Combine(Addon.GetFolderPath(), addonName);
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
            var folderPath = Path.Combine(Config.Instance.WowFolderPath, @"Interface\Addons");
            return folderPath;
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
            return Name.Equals((obj as Addon).Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
