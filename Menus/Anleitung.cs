using Smake.io.Values;
using Smake.io.Render;
using Smake.io.Spiel;

namespace Smake.io.Menus
{
    public class Anleitung : Screen
    {
        public Anleitung()
        {
            Console.Clear();
            RenderAnleitung();
            Console.ReadKey();
            Menu menu = new();
        }
        static void RenderAnleitung()
        {
            Console.Clear();
            Console.WriteLine("ANLEITUNG");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Ziel: Iss so viele {Skinvalues.food} wie möglich!");
            Console.WriteLine("\nSteuerung:\n");
            Console.WriteLine("Spieler 1:\n  ↑ - Hoch\n  ← - Links\n  ↓ - Runter\n  → - Rechts\n");
            Console.WriteLine("Spieler 2:\n  W - Hoch\n  A - Links\n  S - Runter\n  D - Rechts\n");
            Console.WriteLine("Vermeide Kollisionen mit dir selbst oder dem Rand!");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
        }
    }
}
