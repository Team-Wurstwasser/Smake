namespace Snake.io

{

    using System;
    using System.Threading;
    using System.Collections.Generic;
    using System.Drawing;
    using Microsoft.VisualBasic.FileIO;
    using System.Runtime.InteropServices;

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

        static string? difficulty;

        static string? gamemode;

        // Namen der Spieler

        static string? name;

        static string? name2;

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

        static char rand;

        static ConsoleColor randfarbe;

        // Punkte der Spieler

        static int punkte;

        static int punkte2;

        static int maxpunkte;

        // Spielgeschwindigkeit

        static int zeit;

        static int randzahl = 0;

        static int foodzahl = 0;

        static int skinzahl = 0;

        static int skin2zahl = 1;

        static int headfarbezahl = 14;

        static int headfarbe2zahl = 14;

        static int farbezahl = 14;

        static int farbe2zahl = 14;

        static int foodfarbezahl = 14;

        static int randfarbezahl = 14;

        static readonly ConsoleColor[] farben = [
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
                            ConsoleColor.White,
                                ];
    
        static readonly char[] tailskins = ['+', 'x', '~', '=', '-', 'o', '•'];

        static readonly char[] foodskins = ['*', '@', '$', '♥', '%', '¤', '&'];

        static readonly char[] randskins = ['█', '#', '▓', '░', '■'];

        static int coins;

        static bool[] freigeschaltetTail = new bool[tailskins.Length];
        static bool[] freigeschaltetFood = new bool[foodskins.Length];
        static bool[] freigeschaltetRand = new bool[randskins.Length];
        static bool[] freigeschaltetFarben = new bool[farben.Length];

        static void Main()

        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            coins = 10000; // Startguthaben
            difficulty = "Mittel";
            gamemode = "Normal";
            multiplayer = false;
            rand = '█';
            randfarbe = ConsoleColor.White;
            food = '*';
            foodfarbe = ConsoleColor.White;
            skin = '+';
            skin2 = 'x';
            farbe = ConsoleColor.White;
            headfarbe = ConsoleColor.White; ;
            farbe2 = ConsoleColor.White; ;
            headfarbe2 = ConsoleColor.White;

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;
            Eingaben();
            do
            {
             
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

            head = skin;

            head2 = skin2;

            // Zeit einstellen

           
            if (difficulty == "Langsam") zeit = 150;
            else if (difficulty == "Mittel") zeit = 100;
            else zeit = 50;

            // Alle Eingabewerte zurücksetzen
            inputX = 0;
            inputX2 = 0;
            inputY = 0;
            inputY2 = 0;

        }
        static void Spiel()
        {
            Neustart();
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
                        if (MenueOptions < 1) MenueOptions = 6;
                        break;

                    case ConsoleKey.DownArrow:
                        MenueOptions++;
                        if (MenueOptions > 6) MenueOptions = 1;
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
                string option = i switch
                {
                    0 => "Spiel starten",
                    1 => "Einstellungen",
                    2 => "Shop",
                    3 => "Skins/Farben",
                    4 => "Anleitung",
                    5 => "Beenden",
                };
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {option,-25}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
        }



        static void Eingaben()
        {
            Console.Clear();
            Console.Write("Spieler 1, gib deinen Namen ein: ");
            name = Console.ReadLine();
            Console.Clear();
        
            Console.Clear();
            Console.Write("Spieler 2, gib deinen Namen ein: ");
            name2 = Console.ReadLine();
            Console.Clear();
        }

        static void Einstellungen()
        {
            Console.Clear();
            bool menu = true;
            int einstellungsAuswahl = 1;

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
                        break;
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
                Console.WriteLine($"{zeiger} {option,-30}");
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
            if (gamemode == "Normal") gamemode = "Unendlich";
            else if (gamemode == "Unendlich") gamemode = "Babymode";
            else gamemode = "Normal";
        }
        static void Shop()
        {
            Console.Clear();
            bool menu = true;
            int auswahl = 1;

            // Zähle alle Shop-Optionen zusammen
            int gesamtOptionenSkins = tailskins.Length + foodskins.Length + randskins.Length + 1;
            int gesamtOptionenFarben = farben.Length + 1;

            bool Shopskins = false;
            do
            {
                if (Shopskins)
                {
                    if (auswahl < 1) auswahl = gesamtOptionenSkins;
                    if (auswahl > gesamtOptionenSkins) auswahl = 1;

                    ShopSkinsOptions(auswahl);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            auswahl--;
                            break;

                        case ConsoleKey.DownArrow:
                            auswahl++;
                            break;
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.LeftArrow:
                            Console.Clear();
                            Shopskins = false;
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Spacebar:
                            Console.Clear();
                            if (auswahl == gesamtOptionenSkins)
                            {
                                menu = false; // Zurück zum Hauptmenü
                                break;
                            }

                            // Kauflogik für Skins
                            if (auswahl - 1 < tailskins.Length)
                            {
                                if (!freigeschaltetTail[auswahl - 1] && coins >= 10)
                                {
                                    freigeschaltetTail[auswahl - 1] = true;
                                    coins -= 10;
                                }
                            }
                            else if (auswahl - 1 < tailskins.Length + foodskins.Length)
                            {
                                int i = auswahl - 1 - tailskins.Length;
                                if (!freigeschaltetFood[i] && coins >= 10)
                                {
                                    freigeschaltetFood[i] = true;
                                    coins -= 10;
                                }
                            }
                            else if (auswahl - 1 < tailskins.Length + foodskins.Length + randskins.Length)
                            {
                                int i = auswahl - 1 - tailskins.Length - foodskins.Length;
                                if (!freigeschaltetRand[i] && coins >= 10)
                                {
                                    freigeschaltetRand[i] = true;
                                    coins -= 10;
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (auswahl < 1) auswahl = gesamtOptionenFarben;
                    if (auswahl > gesamtOptionenFarben) auswahl = 1;

                    ShopFarbenOptions(auswahl);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            auswahl--;
                            break;

                        case ConsoleKey.DownArrow:
                            auswahl++;
                            break;
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.LeftArrow:
                            Console.Clear();
                            Shopskins = true;
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Spacebar:
                            Console.Clear();
                            if (auswahl == gesamtOptionenFarben)
                            {
                                menu = false;
                                break;
                            }

                            
                            if (!freigeschaltetFarben[auswahl - 1] && coins >= 10)
                            {
                                freigeschaltetFarben[auswahl - 1] = true;
                                coins -= 10;
                            }
                            break;

                    }
                }
                
                
            } while (menu);
        }

        static void ShopFarbenOptions(int selected)
        {

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("           Shop           ");
            Console.WriteLine($"Coins: {coins}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
           
            int option = 0;

            Console.WriteLine("\nFarben:");
            for (int i = 0; i < farben.Length; i++, option++)
            {
                string shoptext = freigeschaltetFarben[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.ForegroundColor = farben[i];
                Console.WriteLine($"{zeiger} {farben[i],-12} {shoptext}");
                Console.ResetColor();
            }

            string zeiger2 = (option + 1 == selected) ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        static void ShopSkinsOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("           Shop           ");
            Console.WriteLine($"Coins: {coins}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
            int option = 0;

            Console.WriteLine("\nTail Skins:");
            for (int i = 0; i < tailskins.Length; i++, option++)
            {
                string shoptext = freigeschaltetTail[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {tailskins[i]} {shoptext}");
            }

            Console.WriteLine("\nFood Skins:");
            for (int i = 0; i < foodskins.Length; i++, option++)
            {
                string shoptext = freigeschaltetFood[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {foodskins[i]} {shoptext}");
            }

            Console.WriteLine("\nRand Skins:");
            for (int i = 0; i < randskins.Length; i++, option++)
            {
                string shoptext = freigeschaltetRand[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {randskins[i]} {shoptext}");
            }

            string zeiger2 = (option + 1 == selected) ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        static void Skin_Farben()
        {
            Console.Clear();
            bool menu = true;
            int Skin_FarbenAuswahl = 1;

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
                        if (Skin_FarbenAuswahl < 1) Skin_FarbenAuswahl = 11;
                        break;

                    case ConsoleKey.DownArrow:
                        Skin_FarbenAuswahl++;
                        if (Skin_FarbenAuswahl > 11) Skin_FarbenAuswahl = 1;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        switch (Skin_FarbenAuswahl)
                        {
                            case 1:
                                do
                                {
                                    skinzahl = (skinzahl + 1) % tailskins.Length;
                                    skin = tailskins[skinzahl];
                                } while (skin == skin2);
                                break;

                            case 2:
                                do
                                {
                                    skin2zahl = (skin2zahl + 1) % tailskins.Length;
                                    skin2 = tailskins[skin2zahl];
                                } while (skin2 == skin);
                                break;
                            case 3:
                                foodzahl = (foodzahl + 1) % foodskins.Length;
                                food = foodskins[foodzahl];
                                break;
                            case 4:
                                randzahl = (randzahl + 1) % randskins.Length;
                                rand = randskins[randzahl];
                                break;
                            case 5:
                                headfarbezahl = (headfarbezahl + 1) % farben.Length;
                                headfarbe = farben[headfarbezahl];
                                break;
                            case 6:
                                headfarbe2zahl = (headfarbe2zahl + 1) % farben.Length;
                                headfarbe2 = farben[headfarbe2zahl];
                                break;
                            case 7:
                                farbezahl = (farbezahl + 1) % farben.Length;
                                farbe = farben[farbezahl];
                                break;
                            case 8:
                                farbe2zahl = (farbe2zahl + 1) % farben.Length;
                                farbe2 = farben[farbe2zahl];
                                break;
                            case 9:
                                foodfarbezahl = (foodfarbezahl + 1) % farben.Length;
                                foodfarbe = farben[foodfarbezahl];
                                break;
                            case 10:
                                randfarbezahl = (randfarbezahl + 1) % farben.Length;
                                randfarbe = farben[randfarbezahl];
                                break;
                            case 11:
                                menu = false; // Zurück zum Hauptmenü
                                break;
                        }
                    break;
                }
                          
            } 
            while (menu);
        }

        static void Skin_FarbenOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.ResetColor();
            Console.WriteLine("Skins/Farben");
            Console.WriteLine("══════════════════════════════");

            bool color = false;

            for (int i = 0; i < 11; i++)
            {
                ConsoleColor farbemenue = ConsoleColor.White;
                string option = "";

                switch (i)
                {
                    case 0:
                        option = $"Player 1 Tailskin ändern    [Aktuell: {skin}";
                        color = false;
                        break;
                    case 1:
                        option = $"Player 2 Tailskin ändern    [Aktuell: {skin2}";
                        color = false;
                        break;
                    case 2:
                        option = $"Foodskin ändern             [Aktuell: {food}";
                        color = false;
                        break;
                    case 3:
                        option = $"Randskin ändern             [Aktuell: {rand}";
                        color = false;
                        break;
                    case 4:
                        option = $"Player 1 Farbe ändern       [Aktuell: ";
                        farbemenue = headfarbe;
                        color = true;
                        break;
                    case 5:
                        option = $"Player 2 Farbe ändern       [Aktuell: ";
                        farbemenue = headfarbe2;
                        color = true;
                        break;
                    case 6:
                        option = $"Player 1 Tailfarbe ändern   [Aktuell: ";
                        farbemenue = farbe;
                        color = true;
                        break;
                    case 7:
                        option = $"Player 2 Tailfarbe ändern   [Aktuell: ";
                        farbemenue = farbe2;
                        color = true;
                        break;
                   
                    case 8:
                        option = $"Foodfarbe ändern            [Aktuell: ";
                        farbemenue = foodfarbe;
                        color = true;
                        break;
                    
                    case 9:
                        option = $"Randfarbe ändern            [Aktuell: ";
                        farbemenue = randfarbe;
                        color = true;
                        break;
                    case 10:
                        color = false;
                        option = "Zurück zum Hauptmenü";
                        break;
                }
                Console.ResetColor();
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.Write(zeiger + " " + option);
                if (color)
                {
                    Console.ForegroundColor = farbemenue;
                    Console.Write(farbemenue);
                    Console.ResetColor();
                }
                Console.WriteLine("]");
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
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("             GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");

            if (multiplayer)
            {
                if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{name2} gewinnt!");
                    Console.WriteLine($"Punkte: {punkte2}");
                    Console.WriteLine($"{name} hat {punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{name} gewinnt!");
                    Console.WriteLine($"Punkte: {punkte}");
                    Console.WriteLine($"{name2} hat {punkte2} Punkte erreicht.");
                    Console.WriteLine();
                }
            }
            else
            {
                if (gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Leider verloren – versuch's noch einmal!");
                    Console.WriteLine($"Du hast {punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("Glückwunsch! Du hast gewonnen!");
                    Console.WriteLine($"Deine Punktzahl: {punkte}");
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

            if (grid[newPlayerY, newPlayerX] == ' ' || grid[newPlayerY, newPlayerX] == food)

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
                if (grid[newPlayerY2, newPlayerX2] == ' ' || grid[newPlayerY2, newPlayerX2] == food)

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

            if (grid[newPlayerY, newPlayerX] != ' ' && grid[newPlayerY, newPlayerX] != head && grid[newPlayerY, newPlayerX] != food || punkte2 == maxpunkte)
            {

                spiel = false;

                gameover = 1;

            }

            if (multiplayer)
            {
                if (grid[newPlayerY2, newPlayerX2] != ' ' && grid[newPlayerY2, newPlayerX2] != head2 && grid[newPlayerY2, newPlayerX2] != food || punkte == maxpunkte)
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



            grid[futterY, futterX] = food; // Setze Futter an die berechnete Position
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
                                head = 'V';
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
                                head2 = '^';
                            }

                            break;

                        case ConsoleKey.S:

                            if (inputY2 != -1 && aenderung2 && multiplayer)
                            {
                                inputY2 = 1;
                                inputX2 = 0;
                                aenderung2 = false;
                                head2 = 'V';
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
                    else if (zeichen == rand)
                        neueFarbe = randfarbe;

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

                        grid[reihe, symbol] = rand;

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

