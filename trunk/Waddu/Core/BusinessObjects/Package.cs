using System.ComponentModel;

namespace Waddu.Core.BusinessObjects
{
    public class Package
    {
        public string Name { get; set; }

        public BindingList<Addon> Addons { get; set; }

        public BindingList<Mapping> Mappings { get; set; }

        public Package(string name)
        {
            Name = name;
            Addons = new BindingList<Addon>();
            Mappings = new BindingList<Mapping>();

            Addons.ListChanged += _addonList_ListChanged;
        }

        private void _addonList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Addon newAddon = Addons[e.NewIndex];
                newAddon.Packages.Add(this);
            }
        }
    }
}
