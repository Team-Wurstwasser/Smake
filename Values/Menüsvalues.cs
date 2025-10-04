using Smake.Speicher;

namespace Smake.Values
{
    public class Menüsvalues
    {
        //Freigeschalteneskins/farben
        public static bool[] FreigeschaltetTail { get; set; } = new bool[GameData.TailSkins.Length];
        public static bool[] FreigeschaltetFood { get; set; } = new bool[GameData.FoodSkins.Length];
        public static bool[] FreigeschaltetRand { get; set; } = new bool[GameData.RandSkins.Length];
        public static bool[] FreigeschaltetFarben { get; set; } = new bool[GameData.Farben.Length];
    }
}