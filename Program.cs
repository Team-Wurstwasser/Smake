namespace Snake

{

    using System;

    using System.Threading;

    using System.Collections.Generic;

    class Program

    {

        // Spielstatus: true = Spiel läuft, false = Spiel beendet

        static bool spiel = true;

        static int gameover;

        static bool check;

        static bool exit = false;


        // Spielfeldgröße (Breite x Höhe)

        static int weite = 41;

        static int hoehe = 20;

        // Das Spielfeld als zweidimensionales Zeichen-Array

        static char[,] grid = new char[hoehe, weite];

        // Eingabe-Richtung (durch Pfeiltasten)

        static int inputX;

        static int inputY;

        static int inputX2;

        static int inputY2;

        static bool aenderung;

        static bool aenderung2;

        // Position des Spielers (Startkoordinaten)

        static int[] playerX = new int[25];

        static int[] playerY = new int[25];

        static int[] playerX2 = new int[25];

        static int[] playerY2 = new int[25];

        // Position des Futters

        static int futterX;

        static int futterY;

        // Länge des Spielers

        static int tail;

        static int tail2;

        // Punkte der Spieler

        static int punkte;

        static int punkte2;

        static int maxpunkte;

        // Spielgeschwindigkeit

        static int zeit;

        static void Main()

        {

            do
            {
                neustart();

                // Mauszeiger im Konsolenfenster ausblenden

                Console.CursorVisible = false;

                // Starte separaten Thread für Tastatureingaben

                Thread inputThread = new Thread(ReadInput);
                inputThread.Start();


                // Starte Begrüßungsbildschirm

                ShowStartScreen();

                // Initialisiere das Spielfeld mit Rahmen und Spielerposition

                InitialisiereSpiel();

                SetzeFutter(); // Futter setzen

                Render();

                Thread.Sleep(1000);


                // Game Loop 

                while (spiel)
                {

                    Update();   // Spielerposition aktualisieren

                    Render();   // Spielfeld neu zeichnen

                    Thread.Sleep(zeit); // Spieltempo regulieren

                    aenderung = true;

                    aenderung2 = true;

                    // Reguliert wie oft wird der Loop durchgeführt wird

                    // Spiele geschwindigkeit

                }


                inputThread.Join();   // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird
                                
                ShowGameOverScreen();// Spielende-Bildschirm

                // Leere Eingabepuffer vollständig
                while (Console.KeyAvailable) Console.ReadKey(true);


                do
                {
                    while (Console.KeyAvailable) Console.ReadKey(true);
                    var key2 = Console.ReadKey(true).Key;
                    switch (key2)
                    {
                        case ConsoleKey.Enter:
                            check = true;
                            break;

                        case ConsoleKey.Escape:
                            exit = true;
                            check = true;
                            break;

                    }
                }
                while (!check);
                
            } while (!exit);



        }

        static void neustart()
        {
            spiel = true;
            check = false;

            gameover = 0;

            // Arrays zurücksetzen
            Array.Clear(playerX, 0, playerX.Length);
            Array.Clear(playerY, 0, playerY.Length);
            Array.Clear(playerX2, 0, playerX2.Length);
            Array.Clear(playerY2, 0, playerY2.Length);

            // Spieler-Positionen auf Startwerte setzen
            playerX[0] = 36;
            playerY[0] = 4;
            playerX2[0] = 4;
            playerY2[0] = 4;

            aenderung = true;
            aenderung2 = true;

            // Taillängen zurücksetzen
            tail = 3;
            tail2 = 3;

            // Punkte zurücksetzen

            punkte = 0;
            punkte2 = 0;

            maxpunkte = 10;

            // Zeit einstellen

            zeit = 100;

            // Alle Eingabewerte zurücksetzen
            inputX = 0;
            inputX2 = 0;
            inputY = 0;
            inputY2 = 0;

        }

        // Zeigt den Startbildschirm mit Anweisungen

        static void ShowStartScreen()

        {

            Console.Clear();

            Console.WriteLine("======================");

            Console.WriteLine("       Snake.io       ");

            Console.WriteLine("======================");

            Console.WriteLine("P1: Pfeiltasten: Links/Rechts/Hoch/Runter");

            Console.WriteLine("P2: WASD: Links/Rechts/Hoch/Runter");

            Console.WriteLine("Taste Enter zum Starten...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { } 

            Console.Clear();

        }


        static void ShowGameOverScreen()

        {

            Console.Clear();

            Console.WriteLine("======================");

            if (gameover == 1)
            {
                Console.WriteLine("    Player 2 Wins!    ");   // Zeigt, dass Spieler 2 gewinnt

                Console.WriteLine($"    With {punkte2} Points!   ");
            }
            else if (gameover == 2)
            {
                Console.WriteLine("    Player 1 Wins!    ");   // Zeigt, dass Spieler 1 gewinnt

                Console.WriteLine($"    With {punkte} Points!   ");
            }
            else
            {
                Console.WriteLine("       GAME OVER      ");   // Zeigt den "Game Over"-Bildschirm
            }

            Console.WriteLine("======================");

            Console.WriteLine("Drücke ESC zum Beenden oder Enter für eine neue Runde...");

        }

        // Aktualisiert die Position des Spielers anhand der Eingabe

        static void Update()

        {

            // Neue Zielkoordinaten berechnen

            int newPlayerX = playerX[0] + 2 * inputX;

            int newPlayerY = playerY[0] + inputY;

            int newPlayerX2 = playerX2[0] + 2 * inputX2;

            int newPlayerY2 = playerY2[0] + inputY2;

            // Tailkoordinaten berechnen

            for (int i = playerX.Length - 1; i > 0; i--)
            {
                playerX[i] = playerX[i - 1];
            }

            for (int i = playerY.Length - 1; i > 0; i--)
            {
                playerY[i] = playerY[i - 1];
            }

            for (int i = playerX2.Length - 1; i > 0; i--)
            {
                playerX2[i] = playerX2[i - 1];
            }

            for (int i = playerY2.Length - 1; i > 0; i--)
            {
                playerY2[i] = playerY2[i - 1];
            }

            // Wenn das Zielfeld leer ist (kein Hindernis), bewege den Spieler

            if (grid[newPlayerY, newPlayerX] == ' ' || grid[newPlayerY, newPlayerX] == '*')

            {

                grid[newPlayerY, newPlayerX] = '█';  // Spieler auf neues Feld setzen

                for (int i = 0; i <= tail; i++)       // Tail des Spielers Zeichnen
                {
                    grid[playerY[i], playerX[i]] = '+';
                }

                grid[playerY[tail + 1], playerX[tail + 1]] = ' ';        // Altes Feld leeren

                playerX[0] = newPlayerX;

                playerY[0] = newPlayerY;

            }

            if (grid[newPlayerY2, newPlayerX2] == ' ' || grid[newPlayerY2, newPlayerX2] == '*')

            {

                grid[newPlayerY2, newPlayerX2] = 'O';  // Spieler auf neues Feld setzen

                for (int i = 0; i <= tail2; i++)       // Tail des Spielers Zeichnen
                {
                    grid[playerY2[i], playerX2[i]] = '+';
                }

                grid[playerY2[tail2 + 1], playerX2[tail2 + 1]] = ' ';        // Altes Feld leeren

                playerX2[0] = newPlayerX2;

                playerY2[0] = newPlayerY2;

            }

            if (grid[newPlayerY, newPlayerX] != ' ' && grid[newPlayerY, newPlayerX] != '█' && grid[newPlayerY, newPlayerX] != '*' || punkte2 == maxpunkte)
            {

                spiel = false;

                gameover = 1;

            }

            if (grid[newPlayerY2, newPlayerX2] != ' ' && grid[newPlayerY2, newPlayerX2] != 'O' && grid[newPlayerY2, newPlayerX2] != '*' || punkte == maxpunkte)
            {

                spiel = false;

                gameover = 2;

            }

            // Spieler 1 frisst Futter
            if (playerX[0] == futterX && playerY[0] == futterY)
            {
                tail++;
                punkte++;
                SetzeFutter();
            }

            // Spieler 2 frisst Futter
            if (playerX2[0] == futterX && playerY2[0] == futterY)
            {
                tail2++;
                punkte2++;
                SetzeFutter();
            }

            // Eingabe zurücksetzen (nur eine Bewegung pro Tick)

        }
        static void SetzeFutter()
        {
            Random rand = new Random();

            // Futter nur auf X-Positionen spawnen lassen, die durch 2 teilbar sind
            do
            {
                futterX = rand.Next(1, weite - 2);
                if (futterX % 2 != 0) futterX++; // Auf gerade X-Position korrigieren

                futterY = rand.Next(1, hoehe - 2);
            }
            while (grid[futterY, futterX] != ' '); // Stelle muss wirklich leer sein



            grid[futterY, futterX] = '*'; // Setze Futter an die berechnete Position
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

                            if (inputY != 1 && aenderung)
                            {
                                inputY = -1;
                                inputX = 0;
                                aenderung = false;
                            }
                            
                            break;

                        case ConsoleKey.DownArrow:

                            if (inputY != -1 && aenderung)
                            {
                                inputY = 1;
                                inputX = 0;
                                aenderung = false;
                            }
                            
                            break;

                        case ConsoleKey.RightArrow:

                            if (inputX != -1 && aenderung)
                            {
                                inputY = 0;
                                inputX = 1;
                                aenderung = false;
                            }
                            
                            break;

                        case ConsoleKey.LeftArrow:

                            if (inputX != 1 && aenderung)
                            {
                                inputY = 0;
                                inputX = -1;
                                aenderung = false;
                            }
                            
                            break;

                        case ConsoleKey.Escape:

                            spiel = false;

                            break;

                        case ConsoleKey.W:

                            if (inputY2 != 1 && aenderung2)
                            {
                                inputY2 = -1;
                                inputX2 = 0;
                                aenderung2 = false;
                            }
                            
                            break;

                        case ConsoleKey.S:

                            if (inputY2 != -1 && aenderung2)
                            {
                                inputY2 = 1;
                                inputX2 = 0;
                                aenderung2 = false;
                            }
                            
                            break;

                        case ConsoleKey.D:

                            if (inputX2 != -1 && aenderung2)
                            {
                                inputY2 = 0;
                                inputX2 = 1;
                                aenderung2 = false;
                            }
                            
                            break;

                        case ConsoleKey.A:

                            if (inputX2 != 1 && aenderung2)
                            {
                                inputY2 = 0;
                                inputX2 = -1;
                                aenderung2 = false;
                            }
                            
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

            grid[playerY[0], playerX[0]] = '█';

            grid[playerY2[0], playerX2[0]] = 'O';

        }

    }

}
