using Smake.io.Spiel;
using Smake.io.Speicher;
using Smake.io.Render;

namespace Smake.io.Values
{
    public class Menüsvalues
    {
        //Freigeschalteneskins/farben
        public static bool[] freigeschaltetTail = new bool[GameData.TailSkins.Length];
        public static bool[] freigeschaltetFood = new bool[GameData.FoodSkins.Length];
        public static bool[] freigeschaltetRand = new bool[GameData.RandSkins.Length];
        public static bool[] freigeschaltetFarben = new bool[GameData.Farben.Length];

        // Statistik
        public static int spieleGesamt;
        public static int highscore;
        public static int gesamtcoins;

    }
}