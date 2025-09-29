using Smake.io.Speicher;

namespace Smake.io.Values
{
    public class Spielvalues
    {
        public static int Maxfutter { get; set; }

        // Spielfeldgröße (Breite x Höhe)
        public readonly static int weite = GameData.Weite;
        public readonly static int hoehe = GameData.Hoehe;

        // Spielmodi
        public static bool Multiplayer { get; set; }
        public static string? Difficulty { get; set; }
        public static string? Gamemode { get; set; }

        // Spielgeschwindigkeit
        public static int Zeit { get; set; }
    }
}
