using System;
using System.Collections.Generic;
using System.Text;

namespace Waddu.Core.BusinessObjects
{
    public class AddonUpdateStats
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set { _version = value; }
        }

        private DateTime _lastUpdated;
        public DateTime LastUpdated
        {
            get { return _lastUpdated; }
            set { _lastUpdated = value; }
        }

        public AddonUpdateStats()
        {
        }
    }
}
