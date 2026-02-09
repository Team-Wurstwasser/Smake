using Smake.Enums;
using Smake.Speicher;

namespace Smake.Values
{
    public static class Spielvalues
    {
        public static int Maxfutter { get; set; }

        // Spielfeldgröße (Breite x Höhe)
        public readonly static int weite = GameData.Weite;
        public readonly static int hoehe = GameData.Hoehe;

        // Spielmodi
        public static bool Multiplayer { get; set; }
        public static Difficultys Difficulty { get; set; }
        public static Gamemodes Gamemode { get; set; }

        // Spielgeschwindigkeit
        public static int Zeit { get; set; }
    }
}
