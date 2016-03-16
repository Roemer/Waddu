namespace Waddu.Types
{
    public static class GameType
    {
        public enum Enum
        {
            WorldOfWarcraft,
            WarhammerOnline
        }

        public static string ConvertToString(Enum enumValue)
        {
            switch (enumValue)
            {
                case Enum.WorldOfWarcraft:
                    return "World of Warcraft";
                case Enum.WarhammerOnline:
                    return "Warhammer Online";
            }
            return "Unknown";
        }
    }
}
