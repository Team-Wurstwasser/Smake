using Smake.Gegenstaende;
using Smake.Helper;
using Smake.Render;
using Smake.SFX;
using Smake.Speicher;
using Smake.Spieler;
using Smake.Values;

namespace Smake.Spiel
{
    public class Spiellogik : RendernSpielfeld
    {
        public static bool Spiel { get; set; }
        int gameover;
        bool unentschieden;

        public static Player Player { get; set; } = new(GameData.Startpositionen.Spieler1.X, GameData.Startpositionen.Spieler1.Y);

        public static Player Player2 { get; set; } = new(GameData.Startpositionen.Spieler2.X, GameData.Startpositionen.Spieler2.Y);

        public static List<Futter> Essen { get; private set; } = [];

        public static List<Mauer> Mauer { get; private set; } = [];

        public Spiellogik()
        {
            Sounds.Melodie(MusikSelector.Selector() ?? 1);
            Spielloop();
        }

        // Allen Variablen den Startwert geben
        void Neustart()
        {
            SpeicherSystem.Speichern_Laden("Speichern");

            Spiel = true;

            gameover = 0;

            unentschieden = false;

            // Zeit einstellen
            if (Spielvalues.DifficultyInt == 1) Spielvalues.Zeit = GameData.SpielSchwierigkeit.Langsam;
            else if (Spielvalues.DifficultyInt == 2) Spielvalues.Zeit = GameData.SpielSchwierigkeit.Mittel;
            else Spielvalues.Zeit = GameData.SpielSchwierigkeit.Schnell;

            // Initialisiere das Spielfeld mit Rahmen
            InitialisiereSpielfeld();

            Player.Neustart();

            if (Spielvalues.Multiplayer)
            {
                Player2.Neustart();
            }

            for (int i = 0; i < Spielvalues.Maxfutter; i++)
            {
                if (!Skinvalues.FoodfarbeRandom)
                {
                    Essen.Add(new Futter(Skinvalues.FoodSkin, Skinvalues.FoodFarbe));
                }
                else
                {
                    List<int> freigeschalteteFarbenIndex = [0]; // Weiß ist immer dabei

                    for (int t = 1; t < Menüsvalues.FreigeschaltetFarben.Length; t++)
                    {
                        if (Menüsvalues.FreigeschaltetFarben[t])
                            freigeschalteteFarbenIndex.Add(t);
                    }

                    // Zufällige Auswahl aus den freigeschalteten Farben
                    Random random = new();
                    int zufallIndex = random.Next(freigeschalteteFarbenIndex.Count);
                    int farbenIndex = freigeschalteteFarbenIndex[zufallIndex];

                    Essen.Add(new Futter(Skinvalues.FoodSkin, GameData.Farben[farbenIndex]));
                }

            }
        }

        // Spielablauf
        void Spielloop()
        {
            Essen = [];

            Mauer = [];

            Neustart();
            Render();

            Thread.Sleep(5);

            Steuerung Input = new();

            // Game Loop 
            while (Spiel)
            {

                Update();   // Spielerposition aktualisieren

                Render();   // Spielfeld neu zeichnen

                Thread.Sleep(Spielvalues.Zeit); // Spieltempo regulieren

                Player.Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                if (Spielvalues.Multiplayer)
                {
                    Player2.Aenderung = true;
                }

            }

            Coins();

            Input.StopInputStream(); // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        void Update()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            {
                // Update Player 1
                var (spielerTot, Maxpunkte) = Player.Update(Player2);
                spieler1Tot |= spielerTot;
                spieler2Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            // Update Player 2
            if (Spielvalues.Multiplayer)
            {
                var (spielerTot, Maxpunkte) = Player2.Update(Player);
                spieler2Tot |= spielerTot;
                spieler1Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            if (Spielvalues.GamemodeInt == 5)
            {
                foreach (var Mauer in Mauer)
                {
                    Mauer.ZeichneMauer();
                }
            }

            GameoverCheck(spieler1Tot, spieler2Tot);
        }

        // Prüft, ob das Spiel vorbei ist
        void GameoverCheck(bool spieler1Tot, bool spieler2Tot)
        {

            if (spieler1Tot && spieler2Tot)
            {
                unentschieden = true;
                Spiel = false;
            }
            else if (spieler1Tot)
            {
                gameover = 1;
                Spiel = false;
            }
            else if (spieler2Tot)
            {
                gameover = 2;
                Spiel = false;
            }

        }

        // Zeigt den Game-Over-Screen an
        void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("              GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");

            if (Spielvalues.Multiplayer)
            {
                if (unentschieden)
                {
                    Console.WriteLine();
                    Console.WriteLine(LanguageManager.Get("gameover.draw"));
                    Console.WriteLine(LanguageManager.Get("gameover.playerPoints")
                        .Replace("{player}", Player.Name)
                        .Replace("{points}", Player.Punkte.ToString()));
                    Console.WriteLine(LanguageManager.Get("gameover.playerPoints")
                        .Replace("{player}", Player2.Name)
                        .Replace("{points}", Player2.Punkte.ToString()));
                    Console.WriteLine();
                }
                else if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine(LanguageManager.Get("gameover.playerWins")
                        .Replace("{player}", Player2.Name));
                    Console.WriteLine($"Punkte: {Player2.Punkte}");
                    Console.WriteLine(LanguageManager.Get("gameover.playerPoints")
                        .Replace("{player}", Player.Name)
                        .Replace("{points}", Player.Punkte.ToString()));
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine(LanguageManager.Get("gameover.playerWins")
                        .Replace("{player}", Player.Name));
                    Console.WriteLine($"Punkte: {Player.Punkte}");
                    Console.WriteLine(LanguageManager.Get("gameover.playerPoints")
                        .Replace("{player}", Player2.Name)
                        .Replace("{points}", Player2.Punkte.ToString()));
                    Console.WriteLine();
                }
            }
            else
            {
                if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine(LanguageManager.Get("gameover.lose"));
                    Console.WriteLine(LanguageManager.Get("gameover.losePoints")
                        .Replace("{points}", Player.Punkte.ToString()));
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine(LanguageManager.Get("gameover.win"));
                    Console.WriteLine(LanguageManager.Get("gameover.winPoints")
                        .Replace("{points}", Player.Punkte.ToString()));
                    Console.WriteLine();
                }
            }

            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine(LanguageManager.Get("gameover.backToMenu"));
            Console.WriteLine(LanguageManager.Get("gameover.restart"));

            bool check = false;
            do
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
                var key2 = Console.ReadKey(true).Key;
                switch (key2)
                {
                    case ConsoleKey.Enter:
                        check = true;
                        Program.CurrentView = 1;
                        break;

                    case ConsoleKey.Escape:
                        check = true;
                        Program.CurrentView = 7;
                        break;
                }
            }
            while (!check);
            while (Console.KeyAvailable) Console.ReadKey(true); // Eingabepuffer leeren
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (Spielvalues.GamemodeInt != 3 && Spielvalues.GamemodeInt != 4)
            {
                if (Spielstatus.Highscore < Player.Punkte)
                { Spielstatus.Highscore = Player.Punkte; }
                else if (Spielstatus.Highscore < Player2.Punkte)
                { Spielstatus.Highscore = Player2.Punkte; }

                Spielstatus.SpieleGesamt++;

                switch (Spielvalues.DifficultyInt)
                {
                    case 1:
                        Spielstatus.Gesamtcoins = Player.Punkte + Player2.Punkte + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = Player.Punkte + Player2.Punkte + Spielstatus.Coins;
                        Spielstatus.Xp = Player.Punkte + Player2.Punkte + Spielstatus.Xp;
                        break;

                    case 2:
                        Spielstatus.Gesamtcoins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Coins;
                        Spielstatus.Xp = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Xp;
                        break;

                    case 3:
                        Spielstatus.Gesamtcoins = 3 * (Player.Punkte + Player2.Punkte) + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = 3 * (Player.Punkte + Player2.Punkte) + Spielstatus.Coins;
                        Spielstatus.Xp = 3 * (Player.Punkte + Player2.Punkte) + Spielstatus.Xp;
                        break;
                }
            }
            else
            {
                Spielstatus.Gesamtcoins = (GameData.MaxPunkte) / 2 + Spielstatus.Gesamtcoins;
                Spielstatus.Coins = (GameData.MaxPunkte) / 2 + Spielstatus.Coins;
            }

        }
    }
}
