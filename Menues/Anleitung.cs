using Smake.Render;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Menues
{
    public class Anleitung : RendernMenue
    {
        public Anleitung()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Main;
            Musik.Melodie();

            RenderAnleitung();
            Console.ReadKey();
            _ = new Menue();
        }
        static void RenderAnleitung()
        {
            Console.Clear();
            Console.WriteLine("ANLEITUNG");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Ziel: Iss so viele {Skinvalues.FoodSkin} wie möglich!");
            Console.WriteLine("\nSteuerung:\n");
            Console.WriteLine("Spieler 1:\n  ↑ - Hoch\n  ← - Links\n  ↓ - Runter\n  → - Rechts\n");
            Console.WriteLine("Spieler 2:\n  W - Hoch\n  A - Links\n  S - Runter\n  D - Rechts\n");
            Console.WriteLine("Vermeide Kollisionen mit dir selbst oder dem Rand!");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
        }
    }
}
