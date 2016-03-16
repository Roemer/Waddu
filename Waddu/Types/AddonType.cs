using System;

namespace Waddu.Types
{
    [Flags]
    public enum AddonType
    {
        Main = 1,
        Sub = 2,
        Depreciated = 4
    }
}
