using System.Collections.Generic;

namespace Waddu.Core.BusinessObjects
{
    public class SiteAddonCache
    {
        private readonly Dictionary<string, SiteAddon> _addonCache;

        public SiteAddonCache()
        {
            _addonCache = new Dictionary<string, SiteAddon>();
        }

        public SiteAddon Get(string tag)
        {
            SiteAddon siteAddon = null;
            lock (_addonCache)
            {
                if (_addonCache.ContainsKey(tag))
                {
                    siteAddon = _addonCache[tag];
                }
                else
                {
                    siteAddon = new SiteAddon();
                    _addonCache.Add(tag, siteAddon);
                }
            }
            return siteAddon;
        }
    }
}
