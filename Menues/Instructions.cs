using Smake.Speicher;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Menues
{
    public class Instructions : RenderMenue
    {
        public Instructions()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.Anleitung ?? 0);

            RenderAnleitung();
            Console.ReadKey();
            Program.CurrentView = ViewType.MainMenu;
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
