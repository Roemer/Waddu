using System;

namespace Waddu.Classes.Interfaces
{
    public interface IDownloadProgress
    {
        void DownloadStatusChanged(long currentBytes, long totalBytes);
    }
}
