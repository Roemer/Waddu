using System;

namespace Waddu.Classes
{
    public interface IDownloadProgress
    {
        void DownloadStatusChanged(long currentBytes, long totalBytes);
    }
}
