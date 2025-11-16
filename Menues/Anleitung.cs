using Smake.Render;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Menues
{
    public class Anleitung : RendernMenue
    {
        public Anleitung()
        {
            Sounds.Currentmusik = GameData.MusikDaten.Menue?.Anleitung ?? 0;
            Sounds.Melodie();

            RenderAnleitung();
            Console.ReadKey();
            Program.CurrentView = 7;
        }
        static void RenderAnleitung()
        {
            Console.Clear();
            Console.WriteLine(LanguageManager.Get("anleitung.title"));
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageManager.Get("anleitung.goal").Replace("{food}", Skinvalues.FoodSkin.ToString()));
            Console.WriteLine();
            Console.WriteLine(LanguageManager.Get("anleitung.controls"));
            Console.WriteLine();
            Console.WriteLine(LanguageManager.Get("anleitung.player1"));
            Console.WriteLine(LanguageManager.Get("anleitung.player2"));
            Console.WriteLine(LanguageManager.Get("anleitung.collision"));
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageManager.Get("anleitung.back"));
        }
    }
}
