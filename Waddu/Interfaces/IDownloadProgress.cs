
namespace Waddu.Interfaces
{
    public interface IDownloadProgress
    {
        void DownloadStatusChanged(long currentBytes, long totalBytes);
    }
}
