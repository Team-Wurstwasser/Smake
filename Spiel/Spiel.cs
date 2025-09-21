using Smake.io.Render;
using Smake.io.Speicher;

namespace Smake.io.Spiel
{
    public class Spiellogik
    {
        // Spielstatus: true = Spiel läuft, false = Spiel beendet
        public static bool spiel;
        public static int gameover;
        static bool unentschieden;
        public static bool exit;
        public static int maxfutter;

        // Spielfeldgröße (Breite x Höhe)
        public readonly static int weite = GameData.Weite;
        public readonly static int hoehe = GameData.Hoehe;

        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] grid = new char[hoehe, weite];

        // Spielmodi
        public static bool multiplayer;
        public static string difficulty;
        public static string gamemode;

        //Weitere Skins
        public static char rand;
        public static ConsoleColor randfarbe;
        public static char food;
        public static ConsoleColor foodfarbe;
        public static bool foodfarbeRandom;

        // Level und Experience
        public static int coins;
        public static int xp;
        public static int level;

        // Spielgeschwindigkeit
        static int zeit;

        public static Spieler player = new(GameData.Startpositionen.Spieler1.X, GameData.Startpositionen.Spieler1.Y);

        public static Spieler player2 = new(GameData.Startpositionen.Spieler2.X, GameData.Startpositionen.Spieler2.Y);

        public static List<Futter> Essen;

        // Allen Variablen den Startwert geben
        static void Neustart()
        {
            int currentMusik = 0;

            if (gamemode == "Normal")
            {
                if (difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Normal.Langsam;
                else if (difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Normal.Mittel;
                else if (difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Normal.Schnell;
            }
            else if (gamemode == "Unendlich")
            {
                if (difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Unendlich.Langsam;
                else if (difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Unendlich.Mittel;
                else if (difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Unendlich.Schnell;
            }
            else if (gamemode == "Babymode")
            {
                if (difficulty == "Langsam") currentMusik = GameData.MusikDaten.Game.Babymode.Langsam;
                else if (difficulty == "Mittel") currentMusik = GameData.MusikDaten.Game.Babymode.Mittel;
                else if (difficulty == "Schnell") currentMusik = GameData.MusikDaten.Game.Babymode.Schnell;
            }

            // Zuweisung an dein Musiksystem
            Musik.currentmusik = currentMusik;

            SpeicherSystem.Speichern_Laden("Speichern");

            spiel = true;

            gameover = 0;

            unentschieden = false;

            // Zeit einstellen
            if (difficulty == "Langsam") zeit = GameData.SpielSchwierigkeit.Langsam;
            else if (difficulty == "Mittel") zeit = GameData.SpielSchwierigkeit.Mittel;
            else zeit = GameData.SpielSchwierigkeit.Schnell;

            // Initialisiere das Spielfeld mit Rahmen
            InitialisiereSpiel();

            player.Neustart();

            if (multiplayer)
            {
                player2.Neustart();
            }

            for (int i = 0; i < maxfutter; i++)
            {
                if(!foodfarbeRandom)
                {
                    Essen.Add(new Futter(food, foodfarbe));
                }
                else
                {
                    Random Random = new();
                    int randomZahl = Random.Next(GameData.Farben.Length);
                    Essen.Add(new Futter(food, GameData.Farben[randomZahl]));
                }

            }
        }

        // Spielablauf
        public static void Spiel()
        {
            Essen = [];
            Neustart();
            Thread inputThread = new(Steuerung.ReadInput);

            inputThread.Start();

            RendernSpielfeld.Render();

            Thread.Sleep(1000);

            // Game Loop 

            while (spiel)
            {

                Update();   // Spielerposition aktualisieren

                RendernSpielfeld.Render();   // Spielfeld neu zeichnen

                Thread.Sleep(zeit); // Spieltempo regulieren

                player.Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                if (multiplayer)
                {
                    player2.Aenderung = true;
                }

            }

            Coins();

            inputThread.Join();   // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

            while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        static void Update()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            (var gegnerTot1, var spielerTot1) = player.Update();
            spieler1Tot |= gegnerTot1;
            spieler2Tot |= spielerTot1;

            if (multiplayer)
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

            if (multiplayer)
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
                        Console.Clear();
                        Spiel();
                        continue;

                    case ConsoleKey.Escape:
                        check = true;
                        break;

                }
            }
            while (!check);
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (gamemode != "Babymode")
            {
                if (Menüs.highscore < player.Punkte)
                { Menüs.highscore = player.Punkte; }
                else if (Menüs.highscore < player2.Punkte)
                { Menüs.highscore = player2.Punkte; }

                Menüs.spieleGesamt++;

                switch (difficulty)
                {
                    case "Langsam":
                        Menüs.gesamtcoins = player.Punkte + player2.Punkte + Menüs.gesamtcoins;
                        coins = player.Punkte + player2.Punkte + coins;
                        xp = player.Punkte + player2.Punkte + xp;
                        break;

                    case "Mittel":
                        Menüs.gesamtcoins = 2 * (player.Punkte + player2.Punkte) + Menüs.gesamtcoins;
                        coins = 2 * (player.Punkte + player2.Punkte) + coins;
                        xp = 2 * (player.Punkte + player2.Punkte) + xp;
                        break;

                    case "Schnell":
                        Menüs.gesamtcoins = 3 * (player.Punkte + player2.Punkte) + Menüs.gesamtcoins;
                        coins = 3 * (player.Punkte + player2.Punkte) + coins;
                        xp = 3 * (player.Punkte + player2.Punkte) + xp;
                        break;
                }
            }
            else
            {
                Menüs.gesamtcoins = (GameData.MaxPunkte) / 2 + Menüs.gesamtcoins;
                coins = (GameData.MaxPunkte) / 2 + coins;
            }

        }

        // Initialisiert das Spielfeld: Rahmen, leere Fläche
        static void InitialisiereSpiel()
        {

            Console.SetCursorPosition(0, 0);

            for (int reihe = 0; reihe < grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < grid.GetLength(1); symbol++)

                {

                    // Rand des Spielfelds mit RandSkin markieren

                    if (reihe == 0 || reihe == grid.GetLength(0) - 1 || symbol == 0 || symbol == grid.GetLength(1) - 1)

                    {

                        grid[reihe, symbol] = rand;

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
