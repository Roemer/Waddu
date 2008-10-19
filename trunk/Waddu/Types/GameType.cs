using System;

namespace Waddu.Types
{
    public sealed class GameType
    {
        public enum Enum
        {
            WorldOfWarcraft,
            WarhammerOnline
        }

        private GameType() { }

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
