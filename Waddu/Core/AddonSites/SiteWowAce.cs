namespace Waddu.Core.AddonSites
{
    public class SiteWowAce : CurseSiteBase
    {
        protected override string InfoUrlFormat => "https://www.wowace.com/projects/{tag}";

        protected override string DownloadUrlFormat => "https://www.wowace.com/projects/{tag}/files/latest";
    }
}