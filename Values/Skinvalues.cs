namespace Smake.Values
{
    public static class Skinvalues
    {
        public static char RandSkin { get; set; }
        public static ConsoleColor RandFarbe { get; set; }
        public static char FoodSkin { get; set; }
        public static ConsoleColor FoodFarbe { get; set; }
        public static bool FoodfarbeRandom { get; set; }
        public const char MauerSkin = '█';
        public const ConsoleColor MauerFarbe = ConsoleColor.Red;
        public const char SchluesselSkin = 'Q';
        public const ConsoleColor SchluesselFarbe = ConsoleColor.Yellow;
        public const char BombenSkin = 'o';
        public const ConsoleColor BombenFarbe = ConsoleColor.Red;
    }
}