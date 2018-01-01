namespace Waddu.Core.AddonSites
{
    public class SiteCurseForge : CurseSiteBase
    {
        protected override string InfoUrlFormat => "https://wow.curseforge.com/projects/{tag}";

        protected override string DownloadUrlFormat => "https://wow.curseforge.com/projects/{tag}/files/latest";
    }
}
