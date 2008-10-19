using System;
using System.Collections.Generic;
using System.Text;

namespace Waddu.Classes
{
    public interface IDownloadProgress
    {
        string StatusText { get; set; }
    }
}
