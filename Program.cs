namespace SpielEinfuehrungLoesung

{

    using System;

    using System.Threading;

    using System.Collections.Generic;

    class Program

    {

        // Spielstatus: true = Spiel läuft, false = Spiel beendet

        static bool spiel = true;

        // Spielfeldgröße (Breite x Höhe)

        static int weite = 51;

        static int hoehe = 20;

        // Das Spielfeld als zweidimensionales Zeichen-Array

        static char[,] grid = new char[hoehe, weite];

        // Eingabe-Richtung (durch Pfeiltasten)

        static int inputX;

        static int inputY;

        // Position des Spielers (Startkoordinaten)

        static int playerX = 4;

        static int playerY = 4;

        static void Main()

        {

            // Mauszeiger im Konsolenfenster ausblenden

            Console.CursorVisible = false;

            // Starte separaten Thread für Tastatureingaben

            Thread inputThread = new Thread(ReadInput);

            inputThread.Start();

            // Starte Begrüßungsbildschirm

            ShowStartScreen();

            // Initialisiere das Spielfeld mit Rahmen und Spielerposition

            InitialisiereSpiel();

            Render();

            Thread.Sleep(1000);


            // Game Loop 

            while (spiel)

            {

                Update();   // Spielerposition aktualisieren

                Render();   // Spielfeld neu zeichnen

                Thread.Sleep(50); // Spieltempo regulieren (250 ms)

                // Reguliert wie oft wird der Loop durchgeführt wird

                // Spiele geschwindigkeit

            }

            // Spielende-Bildschirm

            ShowGameOverScreen();

            // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            inputThread.Join();

        }

        // Zeigt den Startbildschirm mit Anweisungen

        static void ShowStartScreen()

        {

            Console.Clear();

            Console.WriteLine("======================");

            Console.WriteLine("    KONSOLEN SPIEL    ");

            Console.WriteLine("======================");

            Console.WriteLine("Pfeiltasten: Links/Rechts/Hoch/Runter");

            Console.WriteLine("Taste Enter zum Starten...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }

            Console.Clear();

        }

        // Zeigt den "Game Over"-Bildschirm

        static void ShowGameOverScreen()

        {

            Console.Clear();

            Console.WriteLine("======================");

            Console.WriteLine("     GAME OVER 🎮     ");

            Console.WriteLine("======================");

            Console.WriteLine("Drücke eine Taste zum Beenden...");

            Console.ReadKey(true);


        }

        // Aktualisiert die Position des Spielers anhand der Eingabe

        static void Update()

        {

            // Neue Zielkoordinaten berechnen

            int newPlayerX = playerX + 2 * inputX;

            int newPlayerY = playerY + inputY;

            // Wenn das Zielfeld leer ist (kein Hindernis), bewege den Spieler

            if (grid[newPlayerY, newPlayerX] == ' ')

            {

                grid[newPlayerY, newPlayerX] = '█';  // Spieler auf neues Feld setzen

                grid[playerY, playerX] = '+';        // Altes Feld leeren

                playerX = newPlayerX;

                playerY = newPlayerY;

            }

            if (grid[newPlayerY, newPlayerX] != ' ' && grid[newPlayerY, newPlayerX] != '█')
            {

                spiel = false;

            }



            // Eingabe zurücksetzen (nur eine Bewegung pro Tick)



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

                            inputY = -1;
                            inputX = 0;

                            break;

                        case ConsoleKey.DownArrow:

                            inputY = 1;
                            inputX = 0;

                            break;

                        case ConsoleKey.RightArrow:

                            inputY = 0;
                            inputX = 1;

                            break;

                        case ConsoleKey.LeftArrow:

                            inputY = 0;
                            inputX = -1;

                            break;

                        case ConsoleKey.Escape:

                            spiel = false;

                            break;

                    }

                }

            }

        }

        // Zeichnet das gesamte Spielfeld auf der Konsole

        static void Render()

        {

            // Cursor zurücksetzen "übermahlt" letztes Frame

            Console.SetCursorPosition(0, 0);

            for (int reihe = 0; reihe < grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < grid.GetLength(1); symbol++)

                {

                    Console.Write(grid[reihe, symbol]);

                }

                Console.WriteLine(); // Neue Zeile nach jeder Reihe

            }

        }

        // Initialisiert das Spielfeld: Rahmen, leere Fläche, Spieler

        static void InitialisiereSpiel()

        {

            Console.SetCursorPosition(0, 0);

            for (int reihe = 0; reihe < grid.GetLength(0); reihe++)

            {

                for (int symbol = 0; symbol < grid.GetLength(1); symbol++)

                {

                    // Rand des Spielfelds mit '#' markieren

                    if (reihe == 0 || reihe == grid.GetLength(0) - 1 || symbol == 0 || symbol == grid.GetLength(1) - 1)

                    {

                        grid[reihe, symbol] = '#';

                    }

                    else

                    {

                        grid[reihe, symbol] = ' ';

                    }

                }

            }

            // Spielerzeichen auf Startposition setzen

            grid[playerY, playerX] = '█';

        }

    }

}
