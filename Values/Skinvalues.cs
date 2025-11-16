namespace Smake.Values
{
    public class Skinvalues
    {
        internal static char RandSkin;

        internal static ConsoleColor RandFarbe;

        internal static char FoodSkin;

        internal static ConsoleColor FoodFarbe;

        internal static bool FoodfarbeRandom;

        private static char mauerSkin = '█';
        private static ConsoleColor mauerFarbe = ConsoleColor.Red;
        private static char schluesselSkin = 'Q';
        private static ConsoleColor schluesselFarbe = ConsoleColor.Yellow;
        private static char bombenSkin = 'o';
        private static ConsoleColor bombenFarbe = ConsoleColor.Red;

        public static char MauerSkin { get => mauerSkin; set => mauerSkin = value; }
        public static ConsoleColor MauerFarbe { get => mauerFarbe; set => mauerFarbe = value; }
        public static char SchluesselSkin { get => schluesselSkin; set => schluesselSkin = value; }
        public static ConsoleColor SchluesselFarbe { get => schluesselFarbe; set => schluesselFarbe = value; }
        public static char BombenSkin { get => bombenSkin; set => bombenSkin = value; }
        public static ConsoleColor BombenFarbe { get => bombenFarbe; set => bombenFarbe = value; }

        static Skinvalues()
        {
            RandSkin = ' ';
            RandFarbe = ConsoleColor.DarkGray;
            FoodSkin = ' ';
            FoodFarbe = ConsoleColor.White;
            FoodfarbeRandom = false;
        }
    }
}