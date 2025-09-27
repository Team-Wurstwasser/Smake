using Smake.io.Values;
using Smake.io.Menus;
using Smake.io.Render;
using Smake.io.Speicher;

namespace Smake.io.Spiel
{
    public class Spiellogik : RendernSpielfeld
    {
        public static bool spiel;
        public static int gameover;
        public static bool unentschieden;

        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] grid = new char[Spielvalues.hoehe, Spielvalues.weite];

        public static Spieler player = new(GameData.Startpositionen.Spieler1.X, GameData.Startpositionen.Spieler1.Y);

        public static Spieler player2 = new(GameData.Startpositionen.Spieler2.X, GameData.Startpositionen.Spieler2.Y);

        public static List<Futter> Essen;

        public Spiellogik()
        {
          Spiel();
        }

        // Allen Variablen den Startwert geben
        static void Neustart()
        {
            int currentMusik = 0;

            if (Spielvalues.gamemode == "Normal")
            {
                if (Spielvalues.difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Normal.Langsam;
                else if (Spielvalues.difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Normal.Mittel;
                else if (Spielvalues.difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Normal.Schnell;
            }
            else if (Spielvalues.gamemode == "Unendlich")
            {
                if (Spielvalues.difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Unendlich.Langsam;
                else if (Spielvalues.difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Unendlich.Mittel;
                else if (Spielvalues.difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Unendlich.Schnell;
            }
            else if (Spielvalues.gamemode == "Babymode")
            {
                if (Spielvalues.difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Babymode.Langsam;
                else if (Spielvalues.difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Babymode.Mittel;
                else if (Spielvalues.difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Babymode.Schnell;
            }

            // Zuweisung an dein Musiksystem
            Musik.currentmusik = currentMusik;
            Musik.Melodie();
            SpeicherSystem.Speichern_Laden("Speichern");

            spiel = true;

            gameover = 0;

            unentschieden = false;

            // Zeit einstellen
            if (Spielvalues.difficulty == "Langsam") Spielvalues.zeit = GameData.SpielSchwierigkeit.Langsam;
            else if (Spielvalues.difficulty == "Mittel") Spielvalues.zeit = GameData.SpielSchwierigkeit.Mittel;
            else Spielvalues.zeit = GameData.SpielSchwierigkeit.Schnell;

            // Initialisiere das Spielfeld mit Rahmen
            InitialisiereSpiel();

            player.Neustart();

            if (Spielvalues.multiplayer)
            {
                player2.Neustart();
            }

            for (int i = 0; i < Spielvalues.maxfutter; i++)
            {
                if(!Skinvalues.foodfarbeRandom)
                {
                    Essen.Add(new Futter(Skinvalues.food, Skinvalues.foodfarbe));
                }
                else
                {
                    List<int> freigeschalteteFarbenIndex = [0]; // Weiß ist immer dabei

                    for (int t = 1; t < Menüsvalues.freigeschaltetFarben.Length; t++)
                    {
                        if (Menüsvalues.freigeschaltetFarben[t])
                            freigeschalteteFarbenIndex.Add(t);
                    }

                    // Zufällige Auswahl aus den freigeschalteten Farben
                    Random random = new();
                    int zufallIndex = random.Next(freigeschalteteFarbenIndex.Count);
                    int farbenIndex = freigeschalteteFarbenIndex[zufallIndex];

                    Essen.Add(new Futter(Skinvalues.food, GameData.Farben[farbenIndex]));
                }

            }
        }

        // Spielablauf
        static void Spiel()
        {
            Essen = [];

            Neustart();
            Render();

            Thread.Sleep(5);

            Thread inputThread = new(Steuerung.ReadInput);

            inputThread.Start();

            // Game Loop 
            while (spiel)
            {

                Update();   // Spielerposition aktualisieren

                Render();   // Spielfeld neu zeichnen

                Thread.Sleep(Spielvalues.zeit); // Spieltempo regulieren

                player.Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                if (Spielvalues.multiplayer)
                {
                    player2.Aenderung = true;
                }

            }

            Coins();

            inputThread.Join(); // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        static void Update()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            (var gegnerTot1, var spielerTot1) = player.Update();
            spieler1Tot |= gegnerTot1;
            spieler2Tot |= spielerTot1;

            if (Spielvalues.multiplayer)
            {
                (var gegnerTot2, var spielerTot2) = player2.Update();
                spieler1Tot |= gegnerTot2;
                spieler2Tot |= spielerTot2;
            }

            GameoverCheck(spieler1Tot, spieler2Tot);
            if (!spiel) return;
        }

        // Prüft, ob das Spiel vorbei ist
        static void GameoverCheck(bool spieler1Tot, bool spieler2Tot)
        {

            if (spieler1Tot && spieler2Tot)
            {
                unentschieden = true;
                gameover = 0;
                spiel = false;
            }
            else if (spieler1Tot)
            {
                gameover = 1;
                spiel = false;
            }
            else if (spieler2Tot)
            {
                gameover = 2;
                spiel = false;
            }

        }

        // Zeigt den Game-Over-Screen an
        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("              GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");

            if (Spielvalues.multiplayer)
            {
                if (unentschieden)
                {
                    Console.WriteLine();
                    Console.WriteLine("Unentschieden!");
                    Console.WriteLine($"{player.Name} hat {player.Punkte} Punkte erreicht.");
                    Console.WriteLine($"{player2.Name} hat {player2.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{player2.Name} gewinnt!");
                    Console.WriteLine($"Punkte: {player2.Punkte}");
                    Console.WriteLine($"{player.Name} hat {player.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{player.Name} gewinnt!");
                    Console.WriteLine($"Punkte: {player.Punkte}");
                    Console.WriteLine($"{player2.Name} hat {player2.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
            }
            else
            {
                if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Leider verloren – versuch's noch einmal!");
                    Console.WriteLine($"Du hast {player.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("Glückwunsch! Du hast gewonnen!");
                    Console.WriteLine($"Deine Punktzahl: {player.Punkte}");
                    Console.WriteLine();
                }
            }

            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("Drücke [Esc],   um zum Menü zurückzukehren\n" +
                              "oder   [Enter], um ein neues Spiel zu starten ");
            bool check = false;
            do
            {
                while (Console.KeyAvailable) Console.ReadKey(true);
                var key2 = Console.ReadKey(true).Key;
                switch (key2)
                {
                    case ConsoleKey.Enter:
                        check = true;
                        _ = new Spiellogik();
                        break;

                    case ConsoleKey.Escape:
                        check = true;
                        _ = new Menu();
                        break;

                }
            }
            while (!check);
            while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (Spielvalues.gamemode != "Babymode")
            {
                if (Menüsvalues.highscore < player.Punkte)
                { Menüsvalues.highscore = player.Punkte; }
                else if (Menüsvalues.highscore < player2.Punkte)
                { Menüsvalues.highscore = player2.Punkte; }

                Menüsvalues.spieleGesamt++;

                switch (Spielvalues.difficulty)
                {
                    case "Langsam":
                        Menüsvalues.gesamtcoins = player.Punkte + player2.Punkte + Menüsvalues.gesamtcoins;
                        Spielstatus.coins = player.Punkte + player2.Punkte + Spielstatus.coins;
                        Spielstatus.xp = player.Punkte + player2.Punkte + Spielstatus.xp;
                        break;

                    case "Mittel":
                        Menüsvalues.gesamtcoins = 2 * (player.Punkte + player2.Punkte) + Menüsvalues.gesamtcoins;
                        Spielstatus.coins = 2 * (player.Punkte + player2.Punkte) + Spielstatus.coins;
                        Spielstatus.xp = 2 * (player.Punkte + player2.Punkte) + Spielstatus.xp;
                        break;

                    case "Schnell":
                        Menüsvalues.gesamtcoins = 3 * (player.Punkte + player2.Punkte) + Menüsvalues.gesamtcoins;
                        Spielstatus.coins = 3 * (player.Punkte + player2.Punkte) + Spielstatus.coins;
                        Spielstatus.xp = 3 * (player.Punkte + player2.Punkte) + Spielstatus.xp;
                        break;
                }
            }
            else
            {
                Menüsvalues.gesamtcoins = (GameData.MaxPunkte) / 2 + Menüsvalues.gesamtcoins;
                Spielstatus.coins = (GameData.MaxPunkte) / 2 + Spielstatus.coins;
            }

        }

        // Initialisiert das Spielfeld: Rahmen, leere Fläche
        static void InitialisiereSpiel()
        {
            Console.Clear();

            for (int reihe = 0; reihe < grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < grid.GetLength(1); symbol++)

                {

                    // Rand des Spielfelds mit RandSkin markieren

                    if (reihe == 0 || reihe == grid.GetLength(0) - 1 || symbol == 0 || symbol == grid.GetLength(1) - 1)

                    {

                        grid[reihe, symbol] = Skinvalues.rand;

                    }

                    else

                    {

                        grid[reihe, symbol] = ' ';

                    }

                }

            }

        }
    }
}
