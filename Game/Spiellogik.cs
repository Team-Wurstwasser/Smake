using Smake.Enums;
using Smake.Game.Gegenstaende;
using Smake.Helper;
using Smake.SFX;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Game
{
    public class Spiellogik : RenderSpielfeld
    {
        public static GameOverType Gameovertype { get; set; }

        public static List<Futter> Essen { get; private set; } = [];

        public static List<Gegenstand> Mauer { get; private set; } = [];

        // Allen Variablen den Startwert geben
        void Start()
        {
            SpeicherSystem.Speichern_Laden(StorageAction.Save);

            Gameovertype = GameOverType.None;

            // Zeit einstellen
            if (Spielvalues.Difficulty == Difficultys.Slow) Spielvalues.Zeit = ConfigSystem.Game.Difficulty.Slow;
            else if (Spielvalues.Difficulty == Difficultys.Medium) Spielvalues.Zeit = ConfigSystem.Game.Difficulty.Medium;
            else Spielvalues.Zeit = ConfigSystem.Game.Difficulty.Fast;

            // Initialisiere das Spielfeld mit Rahmen
            InitialisiereSpielfeld();

            Spiel.Player[0].Start();

            if (Spielvalues.Multiplayer)
            {
                Spiel.Player[1].Start();
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

                    Essen.Add(new Futter(Skinvalues.FoodSkin, ConfigSystem.Skins.Farben[farbenIndex]));
                }

            }
        }

        // Spielablauf
        public void Spielloop()
        {
            Sounds.Melodie(MusikSelector.Selector());
            Essen = [];

            Mauer = [];

            Start();
            Render();

            Thread.Sleep(5);

            Steuerung Input = new();

            // Game Loop 
            do
            {

                Update();   // Spielerposition aktualisieren

                Render();   // Spielfeld neu zeichnen

                Thread.Sleep(Spielvalues.Zeit); // Spieltempo regulieren

                Spiel.Player[0].Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                if (Spielvalues.Multiplayer)
                {
                    Spiel.Player[1].Aenderung = true;
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
                var (spielerTot, Maxpunkte) = Spiel.Player[0].Update(Spiel.Player[1]);
                spieler1Tot |= spielerTot;
                spieler2Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            // Update Player 2
            if (Spielvalues.Multiplayer)
            {
                var (spielerTot, Maxpunkte) = Spiel.Player[1].Update(Spiel.Player[0]);
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
                    Console.WriteLine(LanguageSystem.Get("gameover.draw"));
                    if (Spielvalues.Multiplayer)
                    {
                        ShowPoints(Spiel.Player[0].Name, Spiel.Player[0].Punkte);
                        ShowPoints(Spiel.Player[1].Name, Spiel.Player[1].Punkte);
                    }
                    break;

                case GameOverType.Player1:
                    if (Spielvalues.Multiplayer)
                    {
                        Console.WriteLine(LanguageSystem.Get("gameover.playerWins").Replace("{player}", Spiel.Player[1].Name));
                        Console.WriteLine(LanguageSystem.Get("gameover.points").Replace("{points}", Spiel.Player[1].Punkte.ToString()));
                        ShowPoints(Spiel.Player[0].Name, Spiel.Player[0].Punkte);
                    }
                    else
                    {
                        Console.WriteLine(LanguageSystem.Get("gameover.lose"));
                        Console.WriteLine(LanguageSystem.Get("gameover.losePoints").Replace("{points}", Spiel.Player[0].Punkte.ToString()));
                    }
                    break;

                case GameOverType.Player2:
                        if (Spielvalues.Multiplayer)
                        {
                            Console.WriteLine(LanguageSystem.Get("gameover.playerWins").Replace("{player}", Spiel.Player[0].Name));
                            Console.WriteLine(LanguageSystem.Get("gameover.points").Replace("{points}", Spiel.Player[0].Punkte.ToString()));
                            ShowPoints(Spiel.Player[1].Name, Spiel.Player[1].Punkte);
                        }
                        else
                        {
                            Console.WriteLine(LanguageSystem.Get("gameover.win"));
                            Console.WriteLine(LanguageSystem.Get("gameover.winPoints").Replace("{points}", Spiel.Player[0].Punkte.ToString()));
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
            Console.WriteLine(LanguageSystem.Get("gameover.backToMenu"));
            Console.WriteLine(LanguageSystem.Get("gameover.restart"));

            WaitForInput();
        }

        // Hilfsmethode für die Punkteanzeige
        static void ShowPoints(string? name, int points)
        {
            Console.WriteLine(LanguageSystem.Get("gameover.playerPoints").Replace("{player}", name).Replace("{points}", points.ToString()));
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

        // Eingaben für Spielernamen
        public static string?[] Eingaben()
        {
            string?[] namen = new string[2];

            Sounds.Melodie(ConfigSystem.Sounds.Musik.Game.Input);

            for (int i = 0; i < namen.Length; i++)
            {
                Console.Clear();
                Console.Write(LanguageSystem.Get($"input.player{i + 1}"));
                namen[i] = Console.ReadLine();
            }

            Console.Clear();
            return namen;
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (Spielvalues.Gamemode != Gamemodes.Babymode && Spielvalues.Gamemode != Gamemodes.BabymodeUnendlich)
            {
                if (Spielstatus.Highscore < Spiel.Player[0].Punkte)
                { Spielstatus.Highscore = Spiel.Player[0].Punkte; }
                else if (Spielstatus.Highscore < Spiel.Player[1].Punkte)
                { Spielstatus.Highscore = Spiel.Player[1].Punkte; }

                Spielstatus.SpieleGesamt++;

                switch (Spielvalues.Difficulty)
                {
                    case Difficultys.Slow:
                        Spielstatus.Gesamtcoins = Spiel.Player[0].Punkte + Spiel.Player[1].Punkte + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = Spiel.Player[0].Punkte + Spiel.Player[1].Punkte + Spielstatus.Coins;
                        Spielstatus.Xp = Spiel.Player[0].Punkte + Spiel.Player[1].Punkte + Spielstatus.Xp;
                        break;

                    case Difficultys.Medium:
                        Spielstatus.Gesamtcoins = 2 * (Spiel.Player[0].Punkte + Spiel.Player[1].Punkte) + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = 2 * (Spiel.Player[0].Punkte + Spiel.Player[1].Punkte) + Spielstatus.Coins;
                        Spielstatus.Xp = 2 * (Spiel.Player[0].Punkte + Spiel.Player[1].Punkte) + Spielstatus.Xp;
                        break;

                    case Difficultys.Fast:
                        Spielstatus.Gesamtcoins = 3 * (Spiel.Player[0].Punkte + Spiel.Player[1].Punkte) + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = 3 * (Spiel.Player[0].Punkte + Spiel.Player[1].Punkte) + Spielstatus.Coins;
                        Spielstatus.Xp = 3 * (Spiel.Player[0].Punkte + Spiel.Player[1].Punkte) + Spielstatus.Xp;
                        break;
                }
            }
            else
            {
                Spielstatus.Gesamtcoins = (ConfigSystem.Game.MaxPunkte) / 2 + Spielstatus.Gesamtcoins;
                Spielstatus.Coins = (ConfigSystem.Game.MaxPunkte) / 2 + Spielstatus.Coins;
            }

        }
    }
}
