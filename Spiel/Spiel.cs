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
        public static int futterX;
        public static int futterY;

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

        public static Spieler player = new(36,4);

        public static Spieler player2 = new(4,4);

        // Allen Variablen den Startwert geben
        static void Neustart()
        {
            Musik.currentmusik = 1;
            SpeicherSystem.Speichern_Laden("Speichern");

            spiel = true;

            gameover = 0;

            unentschieden = false;

            // Zeit einstellen
            if (difficulty == "Langsam") zeit = 150;
            else if (difficulty == "Mittel") zeit = 100;
            else zeit = 50;

            // Initialisiere das Spielfeld mit Rahmen
            Program.InitialisiereSpiel();

            SetzeFutter(); // Futter setzen

            player.Neustart();

            if (multiplayer)
            {
                player2.Neustart();
            }
        }

        // Spielablauf
        public static void Spiel()
        {
            Neustart();
            Thread inputThread = new(ReadInput);

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
        }

        // Prüft, ob das Spiel vorbei ist
        static void Gameover()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            var ergebnis1 = player.Gameover();
            spieler1Tot = spieler1Tot || ergebnis1.gegnerTot;
            spieler2Tot = spieler2Tot || ergebnis1.spielerTot;

            if (multiplayer)
            {
                var ergebnis2 = player2.Gameover();
                spieler1Tot = spieler1Tot || ergebnis2.gegnerTot;
                spieler2Tot = spieler2Tot || ergebnis2.spielerTot;
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
        public static void SetzeFutter()
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
            grid[futterY, futterX] = food; // Setze Futter an die berechnete Position

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
