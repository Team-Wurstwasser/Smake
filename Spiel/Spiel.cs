using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Numerics;
using Smake.io.Render;

namespace Smake.io.Spiel
{
    public class Spiellogik
    {
        // Spielstatus: true = Spiel läuft, false = Spiel beendet
        public static bool spiel = true;
        static int gameover;
        static bool unentschieden;
        public static bool exit = false;

        // Spielfeldgröße (Breite x Höhe)
        public readonly static int weite = 41;
        public readonly static int hoehe = 20;

        // Das Spielfeld als zweidimensionales Zeichen-Array
        public static char[,] grid = new char[hoehe, weite];

        // Position des Futters
        static int futterX;
        static int futterY;

        // Spielmodi
        public static bool multiplayer;
        public static string difficulty;
        public static string gamemode;

        //Weitere Skins
        public static char food;
        public static ConsoleColor foodfarbe;
        public static char rand;
        public static ConsoleColor randfarbe;

        // Maximale Punkte
        public readonly static int maxpunkte = 20;

        // Spielgeschwindigkeit
        static int zeit;


        public static Spieler player = new();

        public static Spieler player2 = new();

        // Allen Variablen den Startwert geben
        static void Neustart()
        {
            Musik.currentmusik = 1;
            SpeicherSystem.Speichern_Laden("Speichern");

            spiel = true;

            gameover = 0;

            unentschieden = false;

            player.Neustart();
            player2.Neustart();

            // Spieler-Positionen auf Startwerte setzen
            player.PlayerX[0] = 36;
            player.PlayerY[0] = 4;
            player2.PlayerX[0] = 4;
            player2.PlayerY[0] = 4;


            // Zeit einstellen
            if (difficulty == "Langsam") zeit = 150;
            else if (difficulty == "Mittel") zeit = 100;
            else zeit = 50;

        }

        // Spielablauf
        public static void Spiel()
        {
            Neustart();
            Thread inputThread = new(ReadInput);

            inputThread.Start();

            // Initialisiere das Spielfeld mit Rahmen und Spielerposition

            Program.InitialisiereSpiel();

            SetzeFutter(); // Futter setzen

            RendernSpielfeld.Render();

            Thread.Sleep(1000);

            // Game Loop 

            while (spiel)
            {

                Update();   // Spielerposition aktualisieren

                RendernSpielfeld.Render();   // Spielfeld neu zeichnen

                Thread.Sleep(zeit); // Spieltempo regulieren

                player.Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                player2.Aenderung = true;

            }

            Program.Coins();

            inputThread.Join();   // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

            while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        static void Update()
        {
            player.Update();
            if (multiplayer)
            {
                player2.Update();
            }
            Gameover();
            if (!spiel) return;
            EsseFutter();
            grid[futterY, futterX] = food; // Setze Futter an die berechnete Position
        }

        // Prüft, ob das Spiel vorbei ist
        static void Gameover()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            if (gamemode == "Unendlich")
            {
                if (player.KollisionPlayer || player.KollisionRand)
                {
                    spieler1Tot = true;
                }

                if (multiplayer)
                {
                    if (player2.KollisionPlayer || player2.KollisionRand)
                    {
                        spieler2Tot = true;
                    }
                }
            }
            else if (gamemode == "Normal")
            {
                if (player.KollisionPlayer || player.KollisionRand || player2.Punkte == maxpunkte)
                {
                    spieler1Tot = true;
                }

                if (multiplayer)
                {
                    if (player2.KollisionPlayer || player2.KollisionRand || player.Punkte == maxpunkte)
                    {
                        spieler2Tot = true;
                    }
                }
                else
                {
                    if (player.Punkte == maxpunkte)
                    {
                        spieler2Tot = true;
                    }
                }
            }
            else if (gamemode == "Babymode")
            {
                if (player2.Punkte == maxpunkte)
                {
                    spieler1Tot = true;
                    Program.coins += 10;
                    Program.xp += 10;
                }

                if (player.Punkte == maxpunkte)
                {
                    spieler2Tot = true;
                    Program.coins += 10;
                    Program.xp += 10;
                }
            }

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

        // Setzt das Futter an eine Zufällige Position
        static void SetzeFutter()
        {
            Random rand = new();

            // Futter nur auf X-Positionen spawnen lassen, die durch 2 teilbar sind
            do
            {
                futterX = rand.Next(1, weite - 2);
                if (futterX % 2 != 0) futterX++; // Auf gerade X-Position korrigieren

                futterY = rand.Next(1, hoehe - 2);
            }
            while (grid[futterY, futterX] != ' '); // Stelle muss wirklich leer sein

        }

        // Die Spieler Essen das Futter
        static void EsseFutter()
        {
            // Spieler 1 frisst Futter
            if (player.PlayerX[0] == futterX && player.PlayerY[0] == futterY)
            {
                player.Tail++;
                player.Punkte++;
                if (Musik.soundplay)
                {
                    Console.Beep(700, 100);
                }
                SetzeFutter();
            }

            // Spieler 2 frisst Futter
            if (player2.PlayerX[0] == futterX && player2.PlayerY[0] == futterY && multiplayer)
            {
                player2.Tail++;
                player2.Punkte++;
                if (Musik.soundplay)
                {
                    Console.Beep(700, 100);
                }
                SetzeFutter();
            }
        }

        // Zeigt den Game-Over-Screen an
        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("             GAME OVER              ");
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

        // Läuft in einem eigenen Thread(Parallel): verarbeitet Tasteneingaben und Speichert diese
        static void ReadInput()

        {

            while (spiel)

            {

                if (Console.KeyAvailable)

                {


                    var key = Console.ReadKey(true).Key;

                    switch (key)

                    {

                        case ConsoleKey.UpArrow:

                            if (player.InputY != 1 && player.Aenderung)
                            {
                                player.InputY = -1;
                                player.InputX = 0;
                                player.Aenderung = false;
                                player.Head = '^';
                            }

                            break;

                        case ConsoleKey.DownArrow:

                            if (player.InputY != -1 && player.Aenderung)
                            {
                                player.InputY = 1;
                                player.InputX = 0;
                                player.Aenderung = false;
                                player.Head = 'v';
                            }

                            break;

                        case ConsoleKey.RightArrow:

                            if (player.InputX != -1 && player.Aenderung)
                            {
                                player.InputY = 0;
                                player.InputX = 1;
                                player.Aenderung = false;
                                player.Head = '>';
                            }

                            break;

                        case ConsoleKey.LeftArrow:

                            if (player.InputX != 1 && player.Aenderung)
                            {
                                player.InputY = 0;
                                player.InputX = -1;
                                player.Aenderung = false;
                                player.Head = '<';
                            }

                            break;

                        case ConsoleKey.Escape:

                            spiel = false;
                            gameover = 3;

                            break;

                        case ConsoleKey.W:

                            if (player2.InputY != 1 && player2.Aenderung && multiplayer)
                            {
                                player2.InputY = -1;
                                player2.InputX = 0;
                                player2.Aenderung = false;
                                player2.Head = '^';
                            }
                            else if (player.InputY != 1 && player.Aenderung && !multiplayer)
                            {
                                player.InputY = -1;
                                player.InputX = 0;
                                player.Aenderung = false;
                                player.Head = '^';
                            }

                            break;

                        case ConsoleKey.S:

                            if (player2.InputY != -1 && player2.Aenderung && multiplayer)
                            {
                                player2.InputY = 1;
                                player2.InputX = 0;
                                player2.Aenderung = false;
                                player2.Head = 'v';
                            }
                            else if (player.InputY != -1 && player.Aenderung && !multiplayer)
                            {
                                player.InputY = 1;
                                player.InputX = 0;
                                player.Aenderung = false;
                                player.Head = 'v';
                            }

                            break;

                        case ConsoleKey.D:

                            if (player2.InputX != -1 && player2.Aenderung && multiplayer)
                            {
                                player2.InputY = 0;
                                player2.InputX = 1;
                                player2.Aenderung = false;
                                player2.Head = '>';
                            }
                            else if (player.InputX != -1 && player.Aenderung && !multiplayer)
                            {
                                player.InputY = 0;
                                player.InputX = 1;
                                player.Aenderung = false;
                                player.Head = '>';
                            }

                            break;

                        case ConsoleKey.A:

                            if (player2.InputX != 1 && player2.Aenderung && multiplayer)
                            {
                                player2.InputY = 0;
                                player2.InputX = -1;
                                player2.Aenderung = false;
                                player2.Head = '<';
                            }
                            else if (player.InputX != 1 && player.Aenderung && !multiplayer)
                            {
                                player.InputY = 0;
                                player.InputX = -1;
                                player.Aenderung = false;
                                player.Head = '<';
                            }

                            break;

                    }

                }

            }

        }
    }
}
