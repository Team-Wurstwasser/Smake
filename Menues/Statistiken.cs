using Smake.io.Render;
using Smake.io.Speicher;
using Smake.io.Values;

namespace Smake.io.Menues
{
    public class Statistiken : RendernMenue
    {
        public Statistiken()
        {
            // Zuweisung an dein Musiksystem
            Musik.Currentmusik = GameData.MusikDaten.Menue.Statistiken;
            Musik.Melodie();

            RenderStatistiken();
            Console.ReadKey();
            _ = new Menue();
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
