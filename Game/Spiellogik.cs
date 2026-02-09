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
    public class Spiellogik(Spiel game) : RendernSpielfeld(game)
    {
        int gameover;
        bool unentschieden;

        public List<Futter> Essen { get; private set; } = [];

        public List<Gegenstand> Mauer { get; private set; } = [];

        void Start()
        {
            SpeicherSystem.Speichern_Laden(StorageAction.Save);

            // Zeit einstellen
            if (Spielvalues.Difficulty == Difficultys.slow) Spielvalues.Zeit = GameData.SpielSchwierigkeit.Langsam;
            else if (Spielvalues.Difficulty == Difficultys.medium) Spielvalues.Zeit = GameData.SpielSchwierigkeit.Mittel;
            else Spielvalues.Zeit = GameData.SpielSchwierigkeit.Schnell;

            // Initialisiere das Spielfeld mit Rahmen
            InitialisiereSpielfeld();

            game.Player[0].Start();

            if (Spielvalues.Multiplayer)
            {
                game.Player[1].Start();
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

        // Aktualisiert die Position des Spielers anhand der Eingabe
        void Update()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            {
                // Update Player 1
                var (spielerTot, Maxpunkte) = game.Player[0].Update(game.Player[1]);
                spieler1Tot |= spielerTot;
                spieler2Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            // Update Player 2
            if (Spielvalues.Multiplayer)
            {
                var (spielerTot, Maxpunkte) = game.Player[1].Update(game.Player[0]);
                spieler2Tot |= spielerTot;
                spieler1Tot |= Maxpunkte;  // Falls MaxPunkte
            }

            GameoverCheck(spieler1Tot, spieler2Tot);
        }

        // Prüft, ob das Spiel vorbei ist
        void GameoverCheck(bool spieler1Tot, bool spieler2Tot)
        {

            if (spieler1Tot && spieler2Tot)
            {
                unentschieden = true;
                game.Game = false;
            }
            else if (spieler1Tot)
            {
                gameover = 1;
                game.Game = false;
            }
            else if (spieler2Tot)
            {
                gameover = 2;
                game.Game = false;
            }

        }

        // Zeigt den Game-Over-Screen an
        void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("              GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");

            if (gameover !=0)
            {
                if (Spielvalues.Multiplayer)
                {
                    if (unentschieden)
                    {
                        Console.WriteLine();
                        Console.WriteLine(LanguageManager.Get("gameover.draw"));
                        Console.WriteLine(LanguageManager.Get("gameover.playerPoints").Replace("{player}", Spiel.Name[0]).Replace("{points}", game.Player[0].Punkte.ToString()));
                        Console.WriteLine(LanguageManager.Get("gameover.playerPoints").Replace("{player}", Spiel.Name[1]).Replace("{points}", game.Player[1].Punkte.ToString()));
                        Console.WriteLine();
                    }
                    else if (gameover == 1)
                    {
                        Console.WriteLine();
                        Console.WriteLine(LanguageManager.Get("gameover.playerWins").Replace("{player}", Player2.Name));
                        Console.WriteLine($"Punkte: {Player2.Punkte}");
                        Console.WriteLine(LanguageManager.Get("gameover.playerPoints").Replace("{player}", game.Player[0]).Replace("{points}", game.Player[0].ToString()));
                        Console.WriteLine();
                    }
                    else if (gameover == 2)
                    {
                        Console.WriteLine();
                        Console.WriteLine(LanguageManager.Get("gameover.playerWins").Replace("{player}", game.Player[0]));
                        Console.WriteLine($"Punkte: {game.Player[0]}");
                        Console.WriteLine(LanguageManager.Get("gameover.playerPoints").Replace("{player}", Player2.Name).Replace("{points}", Player2.Punkte.ToString()));
                        Console.WriteLine();
                    }
                }
                else
                {
                    if (gameover == 1)
                    {
                        Console.WriteLine();
                        Console.WriteLine(LanguageManager.Get("gameover.lose"));
                        Console.WriteLine(LanguageManager.Get("gameover.losePoints").Replace("{points}", Player.Punkte.ToString()));
                        Console.WriteLine();
                    }
                    else if (gameover == 2)
                    {
                        Console.WriteLine();
                        Console.WriteLine(LanguageManager.Get("gameover.win"));
                        Console.WriteLine(LanguageManager.Get("gameover.winPoints").Replace("{points}", Player.Punkte.ToString()));
                        Console.WriteLine();
                    }
                }
                Console.WriteLine("═════════════════════════════════════");
            }
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
                        Program.CurrentView = ViewType.Game;
                        break;

                    case ConsoleKey.Escape:
                        check = true;
                        Program.CurrentView = ViewType.MainMenu;
                        break;
                }
            }
            while (!check);
            while (Console.KeyAvailable) Console.ReadKey(true); // Eingabepuffer leeren
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
                    case Difficultys.slow:
                        Spielstatus.Gesamtcoins = Player.Punkte + Player2.Punkte + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = Player.Punkte + Player2.Punkte + Spielstatus.Coins;
                        Spielstatus.Xp = Player.Punkte + Player2.Punkte + Spielstatus.Xp;
                        break;

                    case Difficultys.medium:
                        Spielstatus.Gesamtcoins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Gesamtcoins;
                        Spielstatus.Coins = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Coins;
                        Spielstatus.Xp = 2 * (Player.Punkte + Player2.Punkte) + Spielstatus.Xp;
                        break;

                    case Difficultys.fast:
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

        // Eingaben für Spielernamen
        static void Eingaben()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.Eingabe ?? 0);

            Console.Clear();

            Console.Write(LanguageManager.Get("input.player1"));
            Spiellogik.Player.Name = Console.ReadLine();

            Console.Clear();

            Console.Write(LanguageManager.Get("input.player2"));
            Spiellogik.Player2.Name = Console.ReadLine();

            Console.Clear();
        }
    }
}
