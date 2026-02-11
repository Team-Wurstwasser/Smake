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
            Sounds.Melodie(ConfigSystem.Sounds.Musik.Menue.Instructions);

            RenderAnleitung();
            Console.ReadKey();
            Program.CurrentView = ViewType.MainMenu;
        }
        static void RenderAnleitung()
        {
            Console.Clear();
            Console.WriteLine(LanguageSystem.Get("anleitung.title"));
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageSystem.Get("anleitung.goal").Replace("{food}", Skinvalues.FoodSkin.ToString()));
            Console.WriteLine();
            Console.WriteLine(LanguageSystem.Get("anleitung.controls"));
            Console.WriteLine();
            Console.WriteLine(LanguageSystem.Get("anleitung.player1"));
            Console.WriteLine(LanguageSystem.Get("anleitung.player2"));
            Console.WriteLine(LanguageSystem.Get("anleitung.collision"));
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageSystem.Get("anleitung.back"));
        }
    }
}
