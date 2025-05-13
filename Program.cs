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

        // Spielmodi

        static bool multiplayer;

        static int dificulty;

        static int mode;

        // Namen der Spieler

        static string name;

        static string name2;

        // Länge des Spielers

        static int tail;

        static int tail2;

        // Aussehen des Spielers

        static char head;

        static char head2;

        static char skin;

        static char skin2;

        static char food;

        static ConsoleColor foodfarbe;

        static ConsoleColor farbe;

        static ConsoleColor headfarbe;

        static ConsoleColor farbe2;

        static ConsoleColor headfarbe2;

        // Punkte der Spieler

        static int punkte;

        static int punkte2;

        static int maxpunkte;

        // Spielgeschwindigkeit

        static int zeit;

        static void Main()

        {
            
            Eingaben();

            do
            {
                neustart();

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
                                
                while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
                                
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

            dificulty = 0;

            mode = 0;

            // Taillängen zurücksetzen
            tail = 3;
            tail2 = 3;

            // Aussehen einstellen

            head = 'v';

            head2 = 'v';

            skin = '+';

            skin2 = 'x';

            food = '*';

            foodfarbe = ConsoleColor.Green;

            farbe = ConsoleColor.Red;

            headfarbe = ConsoleColor.DarkRed;

            farbe2 = ConsoleColor.Blue;

            headfarbe2 = ConsoleColor.DarkBlue;

            // Punkte zurücksetzen

            punkte = 0;
            punkte2 = 0;

            maxpunkte = 10;

            // Zeit einstellen

            zeit = 50;

            // Alle Eingabewerte zurücksetzen
            inputX = 0;
            inputX2 = 0;
            inputY = 0;
            inputY2 = 0;

            // Mauszeiger im Konsolenfenster ausblenden

            Console.CursorVisible = false;
            
        }

        static void Eingaben()
        {
            Console.Clear();

            Console.WriteLine("======================");

            Console.WriteLine("       Snake.io       ");

            Console.WriteLine("======================");

            Console.Write("Spieler 1, gib deinen Namen ein: ");

            name = Console.ReadLine();

            bool i = true;

            do
            {               

                Console.Clear();

                Console.WriteLine("======================");

                Console.WriteLine("       Snake.io       ");

                Console.WriteLine("======================");

                Console.Write("Multiplayer? (y/n): ");

                switch (Console.ReadLine())
                {
                    case "y":
                        multiplayer = true;
                        i = false;
                        break;

                    case "n":
                        multiplayer = false;
                        i = false;
                        break;

                    default:
                        Console.WriteLine("Falsche Eingabe!!!");
                        Thread.Sleep(500);
                        break;
                }
                
            }
            while (i);

            if (multiplayer)
            {
                Console.Clear();

                Console.WriteLine("======================");

                Console.WriteLine("       Snake.io       ");

                Console.WriteLine("======================");

                Console.Write("Spieler 2, gib deinen Namen ein: ");

                name2 = Console.ReadLine();
            }
        }

        // Zeigt den Startbildschirm mit Anweisungen

        static void ShowStartScreen()

        {

            Console.Clear();

            Console.WriteLine("======================");

            Console.WriteLine("       Snake.io       ");

            Console.WriteLine("======================");

            Console.WriteLine($"{name}: Pfeiltasten: Links/Rechts/Hoch/Runter");

            if (multiplayer)
            {
                Console.WriteLine($"{name2}: WASD: Links/Rechts/Hoch/Runter");
            }

            Console.WriteLine("Taste Enter zum Starten...");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { } 

            Console.Clear();

        }

        static void ShowGameOverScreen()

        {

            Console.Clear();

            Console.WriteLine("========================");

            if (multiplayer)
            {
                if (gameover == 1)
                {
                    Console.WriteLine($"     {name2} Wins!     ");   // Zeigt, dass Spieler 2 gewinnt

                    Console.WriteLine($"     With {punkte2} Points!    ");

                    Console.WriteLine($" {name} has {punkte} Points  ");
                }
                else if (gameover == 2)
                {
                    Console.WriteLine($"     {name} Wins!     ");   // Zeigt, dass Spieler 1 gewinnt

                    Console.WriteLine($"     With {punkte} Points!    ");

                    Console.WriteLine($"  {name2}  has s {punkte2} Points  ");
                }
                else
                {
                    Console.WriteLine("       GAME OVER      ");   // Zeigt den "Game Over"-Bildschirm
                }
            }
            else
            {
                if (gameover == 1)
                {
                    Console.WriteLine("       GAME OVER      ");   // Zeigt den "Game Over"-Bildschirm

                    Console.WriteLine($"  {name} has {punkte} Points  ");
                }
                else if (gameover == 2)
                {
                    Console.WriteLine($"      {name} Wins!     ");   // Zeigt, dass Spieler 1 gewinnt

                    Console.WriteLine($"     With {punkte} Points!    ");
                }
                else
                {
                    Console.WriteLine("       GAME OVER      ");   // Zeigt den "Game Over"-Bildschirm
                }
            }

            Console.WriteLine("========================");

            Console.WriteLine("Drücke ESC zum Beenden oder Enter für eine neue Runde...");

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

            if (multiplayer)
            {
                for (int i = playerX2.Length - 1; i > 0; i--)
                {
                    playerX2[i] = playerX2[i - 1];
                }

                for (int i = playerY2.Length - 1; i > 0; i--)
                {
                    playerY2[i] = playerY2[i - 1];
                }
            }

            // Wenn das Zielfeld leer ist (kein Hindernis), bewege den Spieler

            if (grid[newPlayerY, newPlayerX] == ' ' || grid[newPlayerY, newPlayerX] == '*')

            {

                grid[newPlayerY, newPlayerX] = head;  // Spieler auf neues Feld setzen

                for (int i = 0; i <= tail; i++)       // Tail des Spielers Zeichnen
                {
                    grid[playerY[i], playerX[i]] = skin;
                }

                grid[playerY[tail + 1], playerX[tail + 1]] = ' ';        // Altes Feld leeren

                playerX[0] = newPlayerX;

                playerY[0] = newPlayerY;

            }

            if (multiplayer)
            {
                if (grid[newPlayerY2, newPlayerX2] == ' ' || grid[newPlayerY2, newPlayerX2] == '*')

                {

                    grid[newPlayerY2, newPlayerX2] = head2;  // Spieler auf neues Feld setzen

                    for (int i = 0; i <= tail2; i++)       // Tail des Spielers Zeichnen
                    {
                        grid[playerY2[i], playerX2[i]] = skin2;
                    }

                    grid[playerY2[tail2 + 1], playerX2[tail2 + 1]] = ' ';     // Altes Feld leeren

                    playerX2[0] = newPlayerX2;

                    playerY2[0] = newPlayerY2;

                }
            }

            if (grid[newPlayerY, newPlayerX] != ' ' && grid[newPlayerY, newPlayerX] != head && grid[newPlayerY, newPlayerX] != '*' || punkte2 == maxpunkte)
            {

                spiel = false;

                gameover = 1;

            }

            if (multiplayer)
            {
                if (grid[newPlayerY2, newPlayerX2] != ' ' && grid[newPlayerY2, newPlayerX2] != head2 && grid[newPlayerY2, newPlayerX2] != '*' || punkte == maxpunkte)
                {

                    spiel = false;

                    gameover = 2;

                }
            }
            else
            {
                if (punkte == maxpunkte)
                {

                    spiel = false;

                    gameover = 2;

                }
            }

            // Spieler 1 frisst Futter
            if (playerX[0] == futterX && playerY[0] == futterY)
            {
                tail++;
                punkte++;
                Thread.Sleep(10);
                SetzeFutter();
            }

            // Spieler 2 frisst Futter
            if (playerX2[0] == futterX && playerY2[0] == futterY && multiplayer)
            {
                tail2++;
                punkte2++;
                Thread.Sleep(10);
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
                                head = '^';
                            }
                            
                            break;

                        case ConsoleKey.DownArrow:

                            if (inputY != -1 && aenderung)
                            {
                                inputY = 1;
                                inputX = 0;
                                aenderung = false;
                                head = 'v';
                            }
                            
                            break;

                        case ConsoleKey.RightArrow:

                            if (inputX != -1 && aenderung)
                            {
                                inputY = 0;
                                inputX = 1;
                                aenderung = false;
                                head = '>';
                            }
                            
                            break;

                        case ConsoleKey.LeftArrow:

                            if (inputX != 1 && aenderung)
                            {
                                inputY = 0;
                                inputX = -1;
                                aenderung = false;
                                head = '<';
                            }
                            
                            break;

                        case ConsoleKey.Escape:

                            spiel = false;

                            break;

                        case ConsoleKey.W:

                            if (inputY2 != 1 && aenderung2 && multiplayer)
                            {
                                inputY2 = -1;
                                inputX2 = 0;
                                aenderung2 = false;
                                head2 = '^';
                            }
                            
                            break;

                        case ConsoleKey.S:

                            if (inputY2 != -1 && aenderung2 && multiplayer)
                            {
                                inputY2 = 1;
                                inputX2 = 0;
                                aenderung2 = false;
                                head2 = 'v';
                            }
                            
                            break;

                        case ConsoleKey.D:

                            if (inputX2 != -1 && aenderung2 && multiplayer)
                            {
                                inputY2 = 0;
                                inputX2 = 1;
                                aenderung2 = false;
                                head2 = '>';
                            }
                            
                            break;

                        case ConsoleKey.A:

                            if (inputX2 != 1 && aenderung2 && multiplayer)
                            {
                                inputY2 = 0;
                                inputX2 = -1;
                                aenderung2 = false;
                                head2 = '<';
                            }
                            
                            break;

                    }

                }

            }
            
        }



        // Zeichnet das gesamte Spielfeld auf der Konsole

        static void Render()
        {
            Console.SetCursorPosition(0, 0);
            ConsoleColor aktuelleFarbe = Console.ForegroundColor;

            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(1); x++)
                {
                    char zeichen = grid[y, x];
                    ConsoleColor neueFarbe = ConsoleColor.White;

                    // Farbwahl je nach Position oder Zeichen
                    if (x == playerX[0] && y == playerY[0])
                        neueFarbe = headfarbe;
                    else if (x == playerX2[0] && y == playerY2[0] && multiplayer)
                        neueFarbe = headfarbe2;
                    else if (zeichen == skin)
                        neueFarbe = farbe;
                    else if (zeichen == skin2)
                        neueFarbe = farbe2;
                    else if (zeichen == food)
                        neueFarbe = foodfarbe;

                    // Nur Farbe wechseln, wenn nötig
                    if (neueFarbe != aktuelleFarbe)
                    {
                        Console.ForegroundColor = neueFarbe;
                        aktuelleFarbe = neueFarbe;
                    }

                    Console.Write(zeichen);
                }
                Console.WriteLine();
            }

            Console.ResetColor();
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

            grid[playerY[0], playerX[0]] = head;

            if (multiplayer)
            {
                grid[playerY2[0], playerX2[0]] = head2;
            }
            
        }

    }

}
