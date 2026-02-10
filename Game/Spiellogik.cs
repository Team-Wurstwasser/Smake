using Smake.Enums;
using Smake.Game.Spieler;
using Smake.Gegenstaende;
using Smake.Helper;
using Smake.Render;
using Smake.SFX;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Game
{
    public class Spiellogik : RendernSpielfeld
    {
        public static GameOverType Gameovertype { get; set; }

        public static Player Player { get; set; } = new(GameData.Startpositionen.Spieler1.X, GameData.Startpositionen.Spieler1.Y, GameData.TailStartLaenge);

        public static Player Player2 { get; set; } = new(GameData.Startpositionen.Spieler2.X, GameData.Startpositionen.Spieler2.Y, GameData.TailStartLaenge);

        public static List<Futter> Essen { get; private set; } = [];

        public static List<Gegenstand> Mauer { get; private set; } = [];

        public Spiellogik()
        {
            Sounds.Melodie(MusikSelector.Selector() ?? 1);
            Spielloop();
        }

        // Allen Variablen den Startwert geben
        void Neustart()
        {
            SpeicherSystem.Speichern_Laden(StorageAction.Save);

            Gameovertype = GameOverType.None;

            // Zeit einstellen
            if (Spielvalues.Difficulty == Difficultys.Slow) Spielvalues.Zeit = GameData.SpielSchwierigkeit.Langsam;
            else if (Spielvalues.Difficulty == Difficultys.Medium) Spielvalues.Zeit = GameData.SpielSchwierigkeit.Mittel;
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
                    int zufallIndex = RandomHelper.Next(freigeschalteteFarbenIndex.Count);
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
            do
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
            while (Gameovertype == GameOverType.None);

            Coins();

            Input.StopInputStream(); // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        static void Update()
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

            GameoverCheck(spieler1Tot, spieler2Tot);
        }

        // Prüft, ob das Spiel vorbei ist
        static void GameoverCheck(bool spieler1Tot, bool spieler2Tot)
        {
            if (spieler1Tot && spieler2Tot)
            {
                Gameovertype = GameOverType.Draw;
            }
            else if (spieler1Tot)
            {
                Gameovertype = GameOverType.Player2;
            }
            else if (spieler2Tot)
            {
                Gameovertype = GameOverType.Player1;
            }
        }

        // Zeigt den Game-Over-Screen an
        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("            GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine();

            switch (Gameovertype)
            {
                case GameOverType.Draw:
                    Console.WriteLine(LanguageManager.Get("gameover.draw"));
                    if (Spielvalues.Multiplayer)
                    {
                        ShowPoints(Player.Name, Player.Punkte);
                        ShowPoints(Player2.Name, Player2.Punkte);
                    }
                    break;

                case GameOverType.Player1:
                    if (Spielvalues.Multiplayer)
                    {
                        Console.WriteLine(LanguageManager.Get("gameover.playerWins").Replace("{player}", Player2.Name));
                        Console.WriteLine(LanguageManager.Get("gameover.points").Replace("{points}", Player2.Punkte.ToString()));
                        ShowPoints(Player.Name, Player.Punkte);
                    }
                    else
                    {
                        Console.WriteLine(LanguageManager.Get("gameover.lose"));
                        Console.WriteLine(LanguageManager.Get("gameover.losePoints").Replace("{points}", Player.Punkte.ToString()));
                    }
                    break;

                case GameOverType.Player2:
                        if (Spielvalues.Multiplayer)
                        {
                            Console.WriteLine(LanguageManager.Get("gameover.playerWins").Replace("{player}", Player.Name));
                            Console.WriteLine(LanguageManager.Get("gameover.points").Replace("{points}", Player.Punkte.ToString()));
                            ShowPoints(Player2.Name, Player2.Punkte);
                        }
                        else
                        {
                            Console.WriteLine(LanguageManager.Get("gameover.win"));
                            Console.WriteLine(LanguageManager.Get("gameover.winPoints").Replace("{points}", Player.Punkte.ToString()));
                        }
                    break;

                case GameOverType.Exit:

                    break;
            }

            if(Gameovertype != GameOverType.Exit)
            {
                Console.WriteLine();
                Console.WriteLine("═════════════════════════════════════");
            }
            Console.WriteLine(LanguageManager.Get("gameover.backToMenu"));
            Console.WriteLine(LanguageManager.Get("gameover.restart"));

            WaitForInput();
        }

        // Hilfsmethode für die Punkteanzeige
        static void ShowPoints(string? name, int points)
        {
            Console.WriteLine(LanguageManager.Get("gameover.playerPoints").Replace("{player}", name).Replace("{points}", points.ToString()));
        }

        static void WaitForInput()
        {
            bool check = false;
            do
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
                var key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Enter)
                {
                    check = true;
                    Program.CurrentView = ViewType.Game;
                }
                else if (key == ConsoleKey.Escape)
                {
                    check = true;
                    Program.CurrentView = ViewType.MainMenu;
                }
            } while (!check);

            while (Console.KeyAvailable) Console.ReadKey(true);
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (Spielvalues.Gamemode != Gamemodes.Babymode && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich)
            {
                if (Spielstatus.Highscore < Player.Punkte)
                { Spielstatus.Highscore = Player.Punkte; }
                else if (Spielstatus.Highscore < Player2.Punkte)
                { Spielstatus.Highscore = Player2.Punkte; }

                Spielstatus.SpieleGesamt++;

                switch (Spielvalues.Difficulty)
                {
                    case Difficultys.Slow:
                        Spielstatus.Gesamtcoins = Player.Punkte + Player2.Punkte + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = Player.Punkte + Player2.Punkte + Spielstatus.Coins;
                        Spielstatus.Xp = Player.Punkte + Player2.Punkte + Spielstatus.Xp;
                        break;

                    case Difficultys.Medium:
                        Spielstatus.Gesamtcoins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Coins;
                        Spielstatus.Xp = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Xp;
                        break;

                    case Difficultys.Fast:
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
