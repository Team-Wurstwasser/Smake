using Smake.io.Spiel;

namespace Smake.io.Menus
{
    public class Statistiken : Screen
    {
        public Statistiken()
        {
            Console.Clear();
            RenderStatistiken();
            Console.ReadKey();
            Menu menu = new Menu();
        }
        void RenderStatistiken()
        {
            Console.Clear();
            Console.WriteLine("Statistiken\n ");
            Console.WriteLine("══════════════════════════════\n ");

            int punktefürLevel = Spiellogik.xp % 100;
            int balkenLänge = 20;
            int gefüllt = (punktefürLevel * balkenLänge) / 100;
            string bar = new string('█', gefüllt).PadRight(balkenLänge, '-');

            Console.WriteLine($"Level:                    {Spiellogik.level}");
            Console.WriteLine($"Fortschritt:              [{bar}] {punktefürLevel}/100\n");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Gesamte Spiele:           {Menüsvalues.spieleGesamt}");
            Console.WriteLine($"Höchste Punktzahl:        {Menüsvalues.highscore}");
            Console.WriteLine($"Durchschnittliche XP:     {(Menüsvalues.spieleGesamt > 0 ? Spiellogik.xp / Menüsvalues.spieleGesamt : 0)}");
            Console.WriteLine($"Gesamte Coins:            {Menüsvalues.gesamtcoins}");
            Console.WriteLine($"Aktuelle Coins:           {Spiellogik.coins}");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
        }
    }
}
