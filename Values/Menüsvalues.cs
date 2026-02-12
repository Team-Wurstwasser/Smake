using Smake.Speicher;

namespace Smake.Values
{
    public static class Menüsvalues
    {
        //Freigeschalteneskins/farben
        public static bool[] FreigeschaltetTail { get; set; } = new bool[ConfigSystem.Skins.Tail.Length];
        public static bool[] FreigeschaltetFood { get; set; } = new bool[ConfigSystem.Skins.Food.Length];
        public static bool[] FreigeschaltetRand { get; set; } = new bool[ConfigSystem.Skins.Rand.Length];
        public static bool[] FreigeschaltetFarben { get; set; } = new bool[ConfigSystem.Skins.Farben.Length];
    }
}