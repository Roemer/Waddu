using System;

namespace Waddu.Types
{
    public abstract class FolderBrowseType
    {
        public enum Enum
        {
            WoW,
            Folder7z
        }

        public static string GetDescription(FolderBrowseType.Enum type)
        {
            if (type == FolderBrowseType.Enum.WoW)
            {
                return "Select your World of Warcraft Folder";
            }
            else if (type == FolderBrowseType.Enum.Folder7z)
            {
                return "Select your 7z Folder";
            }
            return string.Empty;
        }
    }
}
