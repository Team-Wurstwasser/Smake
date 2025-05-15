namespace Snake.io

{

    using System;

    using System.Threading;

    using System.Collections.Generic;
    using System.Drawing;

    class Program

    {

        // Spielstatus: true = Spiel läuft, false = Spiel beendet

        static bool spiel = true;

        static int gameover;

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

        static int[] playerX;

        static int[] playerY;

        static int[] playerX2;

        static int[] playerY2;

        // Position des Futters

        static int futterX;

        static int futterY;

        // Spielmodi

        static bool multiplayer;

        static string difficulty;

        static string gamemode;

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
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            difficulty = "Mittel";
            gamemode = "Normal";
            multiplayer = false;

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            Eingaben();
            do
            {
                Neustart();
                ShowMainMenue();

            } while (!exit);

        }

        static void Neustart()
        {
            spiel = true;

            gameover = 0;

            // Taillängen zurücksetzen
            tail = 3;
            tail2 = 3;

            // Punkte zurücksetzen

            punkte = 0;
            punkte2 = 0;

            maxpunkte = 20;

            // Maximale Länge einstellen

            playerX = new int[maxpunkte + tail + 2];
            playerY = new int[maxpunkte + tail + 2];
            playerX2 = new int[maxpunkte + tail2 + 2];
            playerY2 = new int[maxpunkte + tail2 + 2];

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

            // Aussehen einstellen

            head = '∨';

            head2 = '∨';

            skin = '+';

            skin2 = 'x';

            food = '*';

            foodfarbe = ConsoleColor.Green;

            // Zeit einstellen

            zeit = 50;

            // Alle Eingabewerte zurücksetzen
            inputX = 0;
            inputX2 = 0;
            inputY = 0;
            inputY2 = 0;

        }
        static void Spiel()
        {
            Thread inputThread = new(ReadInput);
            inputThread.Start();

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
        }
        static void ShowMainMenue()
        {
            Console.Clear();
            DrawTitle();
            bool menu = true;
            int MenueOptions = 1;

            do
            {
                Console.SetCursorPosition(0, 11);
                ShowMenuOptions(MenueOptions);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        MenueOptions--;
                        if (MenueOptions < 1) MenueOptions = 5;
                        break;

                    case ConsoleKey.DownArrow:
                        MenueOptions++;
                        if (MenueOptions > 5) MenueOptions = 1;
                        break;

                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        menu = false;
                        break;
                }

            } while (menu);

            switch (MenueOptions)
            {
                case 1:

                    Console.Clear();
                    Spiel();
                    break;
                case 2:
                    Einstellungen();
                    break;
                case 3:
                    Shop();
                    break;
                case 4:
                    Skin_Farben();
                    break;
                case 5:
                    Anleitung();
                    break;
                case 6:
                    exit = true;
                    break;
            }
        }


        static void DrawTitle()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
  ██████  ███▄ ▄███▓ ▄▄▄       ██ ▄█▀▓█████ 
▒██    ▒ ▓██▒▀█▀ ██▒▒████▄     ██▄█▒ ▓█   ▀ 
░ ▓██▄   ▓██    ▓██░▒██  ▀█▄  ▓███▄░ ▒███   
  ▒   ██▒▒██    ▒██ ░██▄▄▄▄██ ▓██ █▄ ▒▓█  ▄ 
▒██████▒▒▒██▒   ░██▒ ▓█   ▓██▒▒██▒ █▄░▒████▒
▒ ▒▓▒ ▒ ░░ ▒░   ░  ░ ▒▒   ▓▒█░▒ ▒▒ ▓▒░░ ▒░ ░
░ ░▒  ░ ░░  ░      ░  ▒   ▒▒ ░░ ░▒ ▒░ ░ ░  ░
░  ░  ░  ░      ░     ░   ▒   ░ ░░ ░    ░   
      ░         ░         ░  ░░  ░      ░  ░
");
            Console.ResetColor();
        }

        static void ShowMenuOptions(int selected)
        {
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       SMAKE MAIN MENU        ║");
            Console.WriteLine("╠══════════════════════════════╣");

            for (int i = 0; i < 6; i++)
            {
                string option = "";

                switch (i)
                {
                    case 0:
                        option = "Spiel starten";
                        break;
                    case 1:
                        option = "Einstellungen";
                        break;
                    case 2:
                        option = "Shop";
                        break;
                    case 3:
                        option = "Skin/Farben";
                        break;
                    case 4:
                        option = "Anleitung";
                        break;
                    case 5:
                        option = "Beenden";
                        break;
                    default:
                        option = "";
                        break;
                }

                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {option.PadRight(25)}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
        }



        static void Eingaben()
        {
            Console.Clear();
            Console.Write("Spieler 1, gib deinen Namen ein: ");
            name = Console.ReadLine();

            farbe = WähleFarbe(name);
            headfarbe = WähleFarbe(name + " (Kopf)");

            
            
            Console.Clear();
            Console.Write("Spieler 2, gib deinen Namen ein: ");
            name2 = Console.ReadLine();

            farbe2 = WähleFarbe(name2);
            headfarbe2 = WähleFarbe(name2 + " (Kopf)");
           
            Console.Clear();
        }

        static ConsoleColor WähleFarbe(string spielerName)
        {
            ConsoleColor[] farben =
                        [
                            ConsoleColor.DarkBlue,
                            ConsoleColor.DarkGreen,
                            ConsoleColor.DarkCyan,
                            ConsoleColor.DarkRed,
                            ConsoleColor.DarkMagenta,
                            ConsoleColor.DarkYellow,
                            ConsoleColor.Gray,
                            ConsoleColor.DarkGray,
                            ConsoleColor.Blue,
                            ConsoleColor.Green,
                            ConsoleColor.Cyan,
                            ConsoleColor.Red,
                            ConsoleColor.Magenta,
                            ConsoleColor.Yellow,
                            ConsoleColor.White
                        ];


            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Wähle eine Farbe für {spielerName}:");

                for (int i = 0; i < farben.Length; i++)
                {
                    Console.ForegroundColor = farben[i];
                    Console.WriteLine($"{i + 1}. {farben[i]}");
                }

                Console.ResetColor();
                Console.Write("Nummer der Farbe: ");
                if (int.TryParse(Console.ReadLine(), out int eingabe) &&
                    eingabe >= 1 && eingabe <= farben.Length)
                {
                    return farben[eingabe - 1];
                }

                Console.WriteLine("Ungültige Eingabe. Beliebige Taste zum Wiederholen...");
                Console.ReadKey();
            }
        }

        static void Einstellungen()
        {
            Console.Clear();
            bool menu = true;
            int einstellungsAuswahl = 1;
            bool change = false;

            do
            {
                EinstellungenOptions(einstellungsAuswahl);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        einstellungsAuswahl--;
                        if (einstellungsAuswahl < 1) einstellungsAuswahl = 4;
                        break;

                    case ConsoleKey.DownArrow:
                        einstellungsAuswahl++;
                        if (einstellungsAuswahl > 4) einstellungsAuswahl = 1;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        change = true;
                        break;
                }

                if (change)
                {
                    switch (einstellungsAuswahl)
                    {
                        case 1:
                            ChangeDifficulty();
                            break;
                        case 2:
                            multiplayer = !multiplayer;
                            break;
                        case 3:
                            ChangeGamemode();
                            break;
                        case 4:
                            menu = false;
                            break;
                    }
                    change = false;
                }

            } while (menu);
        }
        static void EinstellungenOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("EINSTELLUNGEN");
            Console.WriteLine("══════════════════════════════");

            
            for (int i = 0; i < 4; i++)
            {
                string option = "";

                switch (i)
                {
                    case 0:
                        option = $"Schwierigkeit ändern   [Aktuell: {difficulty}]";
                        break;
                    case 1:
                        option = $"Multiplayer            [Aktuell: {(multiplayer ? "An" : "Aus")}]";
                        break;
                    case 2:
                        option = $"Gamemode ändern        [Aktuell: {gamemode}]";
                        break;
                    case 3:
                        option = "Zurück zum Hauptmenü";
                        break;
                }

                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {option.PadRight(30)}");
            }

            Console.WriteLine("══════════════════════════════");
        }

        static void ChangeDifficulty()
        {
            if (difficulty == "Langsam") difficulty = "Mittel";
            else if (difficulty == "Mittel") difficulty = "Schnell";
            else difficulty = "Langsam";
        }

        static void ChangeGamemode()
        {
            if (gamemode == "Normal") gamemode = "Normal2";
            else if (gamemode == "Normal2") gamemode = "Normal3";
            else gamemode = "Normal";
        }
        static void Shop()
        {
            
        }

        static void Skin_Farben()
        {
            Console.Clear();
            bool menu = true;
            int Skin_FarbenAuswahl = 1;
            bool change = false;

            do
            {
                Skin_FarbenOptions(Skin_FarbenAuswahl);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        Skin_FarbenAuswahl--;
                        if (Skin_FarbenAuswahl < 1) Skin_FarbenAuswahl = 9;
                        break;

                    case ConsoleKey.DownArrow:
                        Skin_FarbenAuswahl++;
                        if (Skin_FarbenAuswahl > 9) Skin_FarbenAuswahl = 1;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        change = true;
                        break;
                }

            } while (menu);
        }

        static void Skin_FarbenOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Skins/Farben");
            Console.WriteLine("══════════════════════════════");


            for (int i = 0; i < 9; i++)
            {
                string option = "";

                switch (i)
                {
                    case 0:
                        option = $"Player 1 Tailskin ändern    [Aktuell: {skin}]";
                        break;
                    case 1:
                        option = $"Player 2 Tailskin ändern    [Aktuell: {skin2}]";
                        break;
                    case 2:
                        option = $"Player 1 Farbe ändern       [Aktuell: {headfarbe}]";
                        break;
                    case 3:
                        option = $"Player 2 Farbe ändern       [Aktuell: {headfarbe2}]";
                        break;
                    case 4:
                        option = $"Player 1 Tailfarbe ändern   [Aktuell: {farbe}]";
                        break;
                    case 5:
                        option = $"Player 2 Tailfarbe ändern   [Aktuell: {farbe2}]";
                        break;
                    case 6:
                        option = $"Foodskin ändern             [Aktuell: {food}]";
                        break;
                    case 7:
                        option = $"Foodfarbe ändern            [Aktuell: {foodfarbe}]";
                        break;
                    case 8:
                        option = "Zurück zum Hauptmenü";
                        break;
                }

                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {option.PadRight(30)}");
            }

            Console.WriteLine("══════════════════════════════");
        }
        static void Anleitung()
        { 
            Console.Clear();
            Console.WriteLine("ANLEITUNG");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Ziel: Iss so viele {food} wie möglich!");
            Console.WriteLine();
            Console.WriteLine("Steuerung:");
            Console.WriteLine();
            Console.WriteLine("Spieler 1:");
            Console.WriteLine("  ↑ - Hoch");
            Console.WriteLine("  ← - Links");
            Console.WriteLine("  ↓ - Runter");
            Console.WriteLine("  → - Rechts");
            Console.WriteLine(); 
            Console.WriteLine("Spieler 2:");
            Console.WriteLine("  W - Hoch");
            Console.WriteLine("  A - Links");
            Console.WriteLine("  S - Runter");
            Console.WriteLine("  D - Rechts");
            Console.WriteLine();
            Console.WriteLine("Vermeide Kollisionen mit dir selbst oder dem Rand!");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
            Console.ReadKey();
        }

        static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("══════════════════════════════");

            if (multiplayer)
            {
                ShowMultiplayerResult();
            }
            else
            {
                ShowSingleplayerResult();
            }

            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine Taste, um zum Menü zurückzukehren...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
        }

        static void ShowMultiplayerResult()
        {
            switch (gameover)
            {
                case 1:
                    Console.WriteLine($"     {name2} Wins!     ");
                    Console.WriteLine($"     With {punkte2} Points!    ");
                    Console.WriteLine($"     {name} has {punkte} Points  ");
                    break;

                case 2:
                    Console.WriteLine($"     {name} Wins!     ");
                    Console.WriteLine($"     With {punkte} Points!    ");
                    Console.WriteLine($"     {name2} has {punkte2} Points  ");
                    break;

                case 3:
                    Console.WriteLine("       Game Over!      ");
                    break;

            }
        }

        static void ShowSingleplayerResult()
        {
            switch (gameover)
            {
                case 1:
                    Console.WriteLine("       Game Over!      ");
                    Console.WriteLine($"You scored {punkte} points.");
                    break;

                case 2:
                    Console.WriteLine("     Congratulations! You Win!     ");
                    Console.WriteLine($"You scored {punkte} points.");
                    break;

                case 3:
                    Console.WriteLine("       Game Over!      ");
                    break;
            }
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
            Random rand = new();

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
                                head = '∧';
                            }

                            break;

                        case ConsoleKey.DownArrow:

                            if (inputY != -1 && aenderung)
                            {
                                inputY = 1;
                                inputX = 0;
                                aenderung = false;
                                head = '∨';
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
                            gameover = 3;

                            break;

                        case ConsoleKey.W:

                            if (inputY2 != 1 && aenderung2 && multiplayer)
                            {
                                inputY2 = -1;
                                inputX2 = 0;
                                aenderung2 = false;
                                head2 = '∧';
                            }

                            break;

                        case ConsoleKey.S:

                            if (inputY2 != -1 && aenderung2 && multiplayer)
                            {
                                inputY2 = 1;
                                inputX2 = 0;
                                aenderung2 = false;
                                head2 = '∨';
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
