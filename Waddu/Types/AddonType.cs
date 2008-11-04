using System;
using System.Collections.Generic;
using System.Text;

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
