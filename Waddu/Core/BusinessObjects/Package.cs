using System.ComponentModel;

namespace Waddu.BusinessObjects
{
    public class Package
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private BindingList<Addon> _addonList;
        public BindingList<Addon> Addons
        {
            get { return _addonList; }
            set { _addonList = value; }
        }

        private BindingList<Mapping> _mappingList;
        public BindingList<Mapping> Mappings
        {
            get { return _mappingList; }
            set { _mappingList = value; }
        }

        public Package(string name)
        {
            _name = name;
            _addonList = new BindingList<Addon>();
            _mappingList = new BindingList<Mapping>();

            _addonList.ListChanged += new ListChangedEventHandler(_addonList_ListChanged);
        }

        private void _addonList_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                Addon newAddon = _addonList[e.NewIndex];
                newAddon.Packages.Add(this);
            }
        }
    }
}
