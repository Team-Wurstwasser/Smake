using Smake.Speicher;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Menues
{
    public class Statistics
    {
        public Statistics()
        {
            Sounds.Melodie(ConfigSystem.Sounds.Musik.Menue.Statistics);

            RenderStatistiken();
            Console.ReadKey();
            Program.CurrentView = ViewType.MainMenu;
        }
        static void RenderStatistiken()
        {
            Console.Clear();
            Console.WriteLine(LanguageSystem.Get("stats.title") + "\n ");
            Console.WriteLine("══════════════════════════════" + "\n ");

            int punktefürLevel = Spielstatus.Xp % 100;
            int balkenLänge = 20;
            int gefüllt = (punktefürLevel * balkenLänge) / 100;
            string bar = new string('█', gefüllt).PadRight(balkenLänge, '-');

            Console.WriteLine(LanguageSystem.Get("stats.level").Replace("{level}", Spielstatus.Level.ToString()));
            Console.WriteLine(LanguageSystem.Get("stats.progress").Replace("{bar}", bar).Replace("{points}", punktefürLevel.ToString()));
            Console.WriteLine();
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageSystem.Get("stats.totalGames").Replace("{games}", Spielstatus.SpieleGesamt.ToString()));
            Console.WriteLine(LanguageSystem.Get("stats.highscore").Replace("{highscore}", Spielstatus.Highscore.ToString()));
            Console.WriteLine(LanguageSystem.Get("stats.avgXp").Replace("{avgxp}", (Spielstatus.SpieleGesamt > 0 ? (Spielstatus.Xp / Spielstatus.SpieleGesamt).ToString() : "0")));
            Console.WriteLine(LanguageSystem.Get("stats.totalCoins").Replace("{totalcoins}", Spielstatus.Gesamtcoins.ToString()));
            Console.WriteLine(LanguageSystem.Get("stats.currentCoins").Replace("{coins}", Spielstatus.Coins.ToString()));
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageSystem.Get("stats.back"));
        }
    }
}
