using Smake.Render;
using Smake.Speicher;
using Smake.Values;
using Smake.SFX;

namespace Smake.Menues
{
    public class Statistiken : RendernMenue
    {
        public Statistiken()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.Statistiken ?? 0);

            RenderStatistiken();
            Console.ReadKey();
            Program.CurrentView = 7;
        }
        static void RenderStatistiken()
        {
            Console.Clear();
            Console.WriteLine(LanguageManager.Get("stats.title") + "\n ");
            Console.WriteLine("══════════════════════════════" + "\n ");

            int punktefürLevel = Spielstatus.Xp % 100;
            int balkenLänge = 20;
            int gefüllt = (punktefürLevel * balkenLänge) / 100;
            string bar = new string('█', gefüllt).PadRight(balkenLänge, '-');

            Console.WriteLine(LanguageManager.Get("stats.level").Replace("{level}", Spielstatus.Level.ToString()));
            Console.WriteLine(LanguageManager.Get("stats.progress").Replace("{bar}", bar).Replace("{points}", punktefürLevel.ToString()));
            Console.WriteLine();
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageManager.Get("stats.totalGames").Replace("{games}", Spielstatus.SpieleGesamt.ToString()));
            Console.WriteLine(LanguageManager.Get("stats.highscore").Replace("{highscore}", Spielstatus.Highscore.ToString()));
            Console.WriteLine(LanguageManager.Get("stats.avgXp").Replace("{avgxp}", (Spielstatus.SpieleGesamt > 0 ? (Spielstatus.Xp / Spielstatus.SpieleGesamt).ToString() : "0")));
            Console.WriteLine(LanguageManager.Get("stats.totalCoins").Replace("{totalcoins}", Spielstatus.Gesamtcoins.ToString()));
            Console.WriteLine(LanguageManager.Get("stats.currentCoins").Replace("{coins}", Spielstatus.Coins.ToString()));
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(LanguageManager.Get("stats.back"));
        }
    }
}
