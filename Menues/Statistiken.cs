using Smake.Render;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Menues
{
    public class Statistiken : RendernMenue
    {
        public Statistiken()
        {
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

            int punktefürLevel = Spielstatus.Xp % 100;
            int balkenLänge = 20;
            int gefüllt = (punktefürLevel * balkenLänge) / 100;
            string bar = new string('█', gefüllt).PadRight(balkenLänge, '-');

            Console.WriteLine($"Level:                    {Spielstatus.Level}");
            Console.WriteLine($"Fortschritt:              [{bar}] {punktefürLevel}/100\n");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Gesamte Spiele:           {Spielstatus.SpieleGesamt}");
            Console.WriteLine($"Höchste Punktzahl:        {Spielstatus.Highscore}");
            Console.WriteLine($"Durchschnittliche XP:     {(Spielstatus.SpieleGesamt > 0 ? Spielstatus.Xp / Spielstatus.SpieleGesamt : 0)}");
            Console.WriteLine($"Gesamte Coins:            {Spielstatus.Gesamtcoins}");
            Console.WriteLine($"Aktuelle Coins:           {Spielstatus.Coins}");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
        }
    }
}
