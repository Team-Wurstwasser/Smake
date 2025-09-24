using Smake.io.Speicher;

namespace Smake.io.Values
{
    public class Spielvalues
    {
        public static int maxfutter;

        // Spielfeldgröße (Breite x Höhe)
        public readonly static int weite = GameData.Weite;
        public readonly static int hoehe = GameData.Hoehe;

        // Spielmodi
        public static bool multiplayer;
        public static string difficulty;
        public static string gamemode;

        // Spielgeschwindigkeit
        public static int zeit;
    }
}
