using Smake.io.Values;
using Smake.io.Render;
using Smake.io.Spiel;

namespace Smake.io.Menus
{
    public class Statistiken : Screen
    {
        public Statistiken()
        {
            RenderStatistiken();
            Console.ReadKey();
            _ = new Menu();
            Thread.Sleep(5);
        }
        static void RenderStatistiken()
        {
            Console.Clear();
            Console.WriteLine("Statistiken\n ");
            Console.WriteLine("══════════════════════════════\n ");

            int punktefürLevel = Spielstatus.xp % 100;
            int balkenLänge = 20;
            int gefüllt = (punktefürLevel * balkenLänge) / 100;
            string bar = new string('█', gefüllt).PadRight(balkenLänge, '-');

            Console.WriteLine($"Level:                    {Spielstatus.level}");
            Console.WriteLine($"Fortschritt:              [{bar}] {punktefürLevel}/100\n");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Gesamte Spiele:           {Menüsvalues.spieleGesamt}");
            Console.WriteLine($"Höchste Punktzahl:        {Menüsvalues.highscore}");
            Console.WriteLine($"Durchschnittliche XP:     {(Menüsvalues.spieleGesamt > 0 ? Spielstatus.xp / Menüsvalues.spieleGesamt : 0)}");
            Console.WriteLine($"Gesamte Coins:            {Menüsvalues.gesamtcoins}");
            Console.WriteLine($"Aktuelle Coins:           {Spielstatus.coins}");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
        }
    }
}
