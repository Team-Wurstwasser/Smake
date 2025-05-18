namespace Snake.io

{

    using System;
    using System.Threading;
    using System.Collections.Generic;
    using System.Drawing;
    using Microsoft.VisualBasic.FileIO;
    using System.Runtime.InteropServices;
    using System.IO;


    class Program

    {

        // Spielstatus: true = Spiel läuft, false = Spiel beendet

        static bool spiel = true;

        static int gameover;

        static bool unentschieden;

        static bool exit = false;


        // Spielfeldgröße (Breite x Höhe)

        static readonly int weite = 41;

        static readonly int hoehe = 20;

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

        // Kollisionsvariablen

        static bool kollisionRand;

        static bool kollisionRand2;

        static bool kollisionPlayer;

        static bool kollisionPlayer2;

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

        // Shop variablen

        static int randzahl;

        static int foodzahl;

        static int skinzahl;

        static int skin2zahl;

        static int headfarbezahl;

        static int headfarbe2zahl;

        static int farbezahl;

        static int farbe2zahl;

        static int foodfarbezahl;

        static int randfarbezahl;

        static readonly ConsoleColor[] farben = [
                            ConsoleColor.White,
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
                            ConsoleColor.Yellow
                                ];
    
        static readonly char[] tailskins = ['+', 'x', '~', '=', '-', 'o', '•'];

        static readonly char[] foodskins = ['*', '@', '$', '♥', '%', '¤', '&'];

        static readonly char[] randskins = ['█', '#', '▓', '░', '■'];

        // Level und Experience

        static int coins;
        static int xp;

        static bool[] freigeschaltetTail = new bool[tailskins.Length];
        static bool[] freigeschaltetFood = new bool[foodskins.Length];
        static bool[] freigeschaltetRand = new bool[randskins.Length];
        static bool[] freigeschaltetFarben = new bool[farben.Length];

        // Statistik

        static int spieleGesamt;
        static int highscore;
        static int gesamtcoins;

        static bool performancemode;


        // Main
        static void Main()

        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;

            Speichern_Laden("Laden");

            Eingaben();
            do
            {
             
                ShowMainMenue();

            } while (!exit);

        }

        // Speicher und Ladesystem
        static void Speichern_Laden(string speicher_laden)
        {
            string pfad = "spielstand.txt";
            //Nur wenn File nicht gefunden wird
            if (!File.Exists(pfad))
            {
                randzahl = 0;
                foodzahl = 0;
                skinzahl = 0;
                skin2zahl = 1;
                headfarbezahl = 0;
                headfarbe2zahl = 0;
                farbezahl = 0;
                farbe2zahl = 0;
                foodfarbezahl = 0;
                randfarbezahl = 0;

                freigeschaltetTail[0] = true;
                freigeschaltetTail[1] = true;
                freigeschaltetFood[0] = true;
                freigeschaltetRand[0] = true;
                freigeschaltetFarben[0] = true;

                performancemode = false;

                // Startguthaben
                coins = 0;
                xp = 0;

                // Standard Modi
                difficulty = "Mittel";
                gamemode = "Normal";
                multiplayer = false;

                // Standard Skins/Farben
                rand = '█';
                food = '*';
                skin = '+';
                skin2 = 'x';
                randfarbe = ConsoleColor.White;
                foodfarbe = ConsoleColor.White;
                farbe = ConsoleColor.White;
                farbe = ConsoleColor.White;
                farbe2 = ConsoleColor.White;
                headfarbe = ConsoleColor.White;
                headfarbe2 = ConsoleColor.White;

                gesamtcoins = 0;
                highscore = 0;
                spieleGesamt = 0;

                Speichern(pfad);
            }

            // Entscheidet ob geladen oder gespeichert wird
            switch(speicher_laden)
            {
                case "Speichern":
                    Speichern(pfad);
                    break;
                case "Laden":
                    Laden(pfad);
                    break;

            }
                

        }


        // Speicher Logik
        static void Speichern(string pfad)
        {
            //Liste was gespeichert wird
            var Zeilen = new List<string>
            {
                $"randzahl={randzahl}",
                $"foodzahl={foodzahl}",
                $"skinzahl={skinzahl}",
                $"skin2zahl={skin2zahl}",
                $"headfarbezahl={headfarbezahl}",
                $"headfarbe2zahl={headfarbe2zahl}",
                $"farbezahl={farbezahl}",
                $"farbe2zahl={farbe2zahl}",
                $"foodfarbezahl={foodfarbezahl}",
                $"randfarbezahl={randfarbezahl}",
                $"freigeschaltetTail0={freigeschaltetTail[0]}",
                $"freigeschaltetTail1={freigeschaltetTail[1]}",
                $"freigeschaltetTail2={freigeschaltetTail[2]}",
                $"freigeschaltetTail3={freigeschaltetTail[3]}",
                $"freigeschaltetTail4={freigeschaltetTail[4]}",
                $"freigeschaltetTail5={freigeschaltetTail[5]}",
                $"freigeschaltetTail6={freigeschaltetTail[6]}",
                $"freigeschaltetFood0={freigeschaltetFood[0]}",
                $"freigeschaltetFood1={freigeschaltetFood[1]}",
                $"freigeschaltetFood2={freigeschaltetFood[2]}",
                $"freigeschaltetFood3={freigeschaltetFood[3]}",
                $"freigeschaltetFood4={freigeschaltetFood[4]}",
                $"freigeschaltetFood5={freigeschaltetFood[5]}",
                $"freigeschaltetFood6={freigeschaltetFood[6]}",
                $"freigeschaltetRand0={freigeschaltetRand[0]}",
                $"freigeschaltetRand1={freigeschaltetRand[1]}",
                $"freigeschaltetRand2={freigeschaltetRand[2]}",
                $"freigeschaltetRand3={freigeschaltetRand[3]}",
                $"freigeschaltetRand4={freigeschaltetRand[4]}",
                $"freigeschaltetFarben0={freigeschaltetFarben[0]}",
                $"freigeschaltetFarben1={freigeschaltetFarben[1]}",
                $"freigeschaltetFarben2={freigeschaltetFarben[2]}",
                $"freigeschaltetFarben3={freigeschaltetFarben[3]}",
                $"freigeschaltetFarben4={freigeschaltetFarben[4]}",
                $"freigeschaltetFarben5={freigeschaltetFarben[5]}",
                $"freigeschaltetFarben6={freigeschaltetFarben[6]}",
                $"freigeschaltetFarben7={freigeschaltetFarben[7]}",
                $"freigeschaltetFarben8={freigeschaltetFarben[8]}",
                $"freigeschaltetFarben9={freigeschaltetFarben[9]}",
                $"freigeschaltetFarben10={freigeschaltetFarben[10]}",
                $"freigeschaltetFarben11={freigeschaltetFarben[11]}",
                $"freigeschaltetFarben12={freigeschaltetFarben[12]}",
                $"freigeschaltetFarben13={freigeschaltetFarben[13]}",
                $"freigeschaltetFarben14={freigeschaltetFarben[14]}",
                $"performancemode={performancemode}",
                $"coins={coins}",
                $"xp={xp}",
                $"spieleGesamt={spieleGesamt}",
                $"highscore={highscore}",
                $"gesamtcoins={gesamtcoins}",
                $"difficulty={difficulty}",
                $"gamemode={gamemode}",
                $"multiplayer={multiplayer}",
                $"rand={rand}",
                $"food={food}",
                $"skin={skin}",
                $"skin2={skin2}",
                $"randfarbe={randfarbe}",
                $"foodfarbe={foodfarbe}",
                $"farbe={farbe}",
                $"farbe2={farbe2}",
                $"headfarbe={headfarbe}",
                $"headfarbe2={headfarbe2}"
            };

            File.WriteAllLines(pfad, Zeilen);
        }

        // Speicher laden Logik 
        static void Laden(string pfad)
        {
            
            var Zeilen = File.ReadAllLines(pfad);

            foreach (var Zeile in Zeilen)
            {
                //Teilt was vor und nach dem = steht
                var Teil = Zeile.Split('=');

                string Variablenname = Teil[0];
                string Wert = Teil[1];

                //Entscheidet was in die Variablen eingespeichert wird
                switch (Variablenname)
                {
                    case "randzahl": randzahl = int.Parse(Wert); break;
                    case "foodzahl": foodzahl = int.Parse(Wert); break;
                    case "skinzahl": skinzahl = int.Parse(Wert); break;
                    case "skin2zahl": skin2zahl = int.Parse(Wert); break;
                    case "headfarbezahl": headfarbezahl = int.Parse(Wert); break;
                    case "headfarbe2zahl": headfarbe2zahl = int.Parse(Wert); break;
                    case "farbezahl": farbezahl = int.Parse(Wert); break;
                    case "farbe2zahl": farbe2zahl = int.Parse(Wert); break;
                    case "foodfarbezahl": foodfarbezahl = int.Parse(Wert); break;
                    case "randfarbezahl": randfarbezahl = int.Parse(Wert); break;

                    case "freigeschaltetTail0": freigeschaltetTail[0] = bool.Parse(Wert); break;
                    case "freigeschaltetTail1": freigeschaltetTail[1] = bool.Parse(Wert); break;
                    case "freigeschaltetTail2": freigeschaltetTail[2] = bool.Parse(Wert); break;
                    case "freigeschaltetTail3": freigeschaltetTail[3] = bool.Parse(Wert); break;
                    case "freigeschaltetTail4": freigeschaltetTail[4] = bool.Parse(Wert); break;
                    case "freigeschaltetTail5": freigeschaltetTail[5] = bool.Parse(Wert); break;
                    case "freigeschaltetTail6": freigeschaltetTail[6] = bool.Parse(Wert); break;

                    case "freigeschaltetFood0": freigeschaltetFood[0] = bool.Parse(Wert); break;
                    case "freigeschaltetFood1": freigeschaltetFood[1] = bool.Parse(Wert); break;
                    case "freigeschaltetFood2": freigeschaltetFood[2] = bool.Parse(Wert); break;
                    case "freigeschaltetFood3": freigeschaltetFood[3] = bool.Parse(Wert); break;
                    case "freigeschaltetFood4": freigeschaltetFood[4] = bool.Parse(Wert); break;
                    case "freigeschaltetFood5": freigeschaltetFood[5] = bool.Parse(Wert); break;
                    case "freigeschaltetFood6": freigeschaltetFood[6] = bool.Parse(Wert); break;

                    case "freigeschaltetRand0": freigeschaltetRand[0] = bool.Parse(Wert); break;
                    case "freigeschaltetRand1": freigeschaltetRand[1] = bool.Parse(Wert); break;
                    case "freigeschaltetRand2": freigeschaltetRand[2] = bool.Parse(Wert); break;
                    case "freigeschaltetRand3": freigeschaltetRand[3] = bool.Parse(Wert); break;
                    case "freigeschaltetRand4": freigeschaltetRand[4] = bool.Parse(Wert); break;

                    case "freigeschaltetFarben0": freigeschaltetFarben[0] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben1": freigeschaltetFarben[1] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben2": freigeschaltetFarben[2] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben3": freigeschaltetFarben[3] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben4": freigeschaltetFarben[4] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben5": freigeschaltetFarben[5] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben6": freigeschaltetFarben[6] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben7": freigeschaltetFarben[7] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben8": freigeschaltetFarben[8] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben9": freigeschaltetFarben[9] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben10": freigeschaltetFarben[10] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben11": freigeschaltetFarben[11] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben12": freigeschaltetFarben[12] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben13": freigeschaltetFarben[13] = bool.Parse(Wert); break;
                    case "freigeschaltetFarben14": freigeschaltetFarben[14] = bool.Parse(Wert); break;

                    case "performancemode": performancemode = bool.Parse(Wert); break;
                    case "coins": coins = int.Parse(Wert); break;
                    case "xp": xp = int.Parse(Wert); break;
                    case "gesamtcoins": gesamtcoins = int.Parse(Wert); break;
                    case "highscore": highscore = int.Parse(Wert); break;
                    case "spieleGesamt": spieleGesamt = int.Parse(Wert); break;

                    case "difficulty": difficulty = Wert; break;
                    case "gamemode": gamemode = Wert; break;
                    case "multiplayer": multiplayer = bool.Parse(Wert); break;

                    case "rand": rand = char.Parse(Wert); break;
                    case "food": food = char.Parse(Wert); break;
                    case "skin": skin = char.Parse(Wert); break;
                    case "skin2": skin2 = char.Parse(Wert); break;

                    case "randfarbe": randfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "foodfarbe": foodfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "farbe": farbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "farbe2": farbe2 = Enum.Parse<ConsoleColor>(Wert); break;
                    case "headfarbe": headfarbe = Enum.Parse<ConsoleColor>(Wert); break;
                    case "headfarbe2": headfarbe2 = Enum.Parse<ConsoleColor>(Wert); break;
                }
            }
        }

        // Allen Variablen den Startwert geben
        static void Neustart()
        {
            Speichern_Laden("Speichern");

            spiel = true;
            
            gameover = 0;

            unentschieden = false;

            kollisionPlayer = false;
            kollisionPlayer2 = false;
            kollisionRand = false;
            kollisionRand2 = false;

            // Taillängen zurücksetzen
            tail = 3;
            tail2 = 3;

            // Punkte zurücksetzen

            punkte = 0;
            punkte2 = 0;

            maxpunkte = 20;

            // Maximale Länge einstellen
            if (gamemode == "Normal" || gamemode == "Babymode")
            {
                playerX = new int[maxpunkte + tail + 2];
                playerY = new int[maxpunkte + tail + 2];
                playerX2 = new int[maxpunkte + tail2 + 2];
                playerY2 = new int[maxpunkte + tail2 + 2];
            }
            else if (gamemode == "Unendlich")
            {
                playerX = new int[weite * hoehe];
                playerY = new int[weite * hoehe];
                playerX2 = new int[weite * hoehe];
                playerY2 = new int[weite * hoehe];
            }


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


        // Spielablauf
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

                aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                aenderung2 = true;

            }

            Coins();

            inputThread.Join();   // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            ShowGameOverScreen(); // Spielende-Bildschirm

            while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
        }


        // Coins und xp hinzufügen
        static void Coins()
        {
            if (gamemode != "babymode")
            {
                if(highscore<punkte)
                {highscore = punkte;}
                else if (highscore < punkte2)
                {highscore = punkte2;}

                spieleGesamt++;

                switch (difficulty)
                {
                    case "Langsam":
                        gesamtcoins = (punkte + punkte2) + gesamtcoins;
                        coins = punkte + punkte2 + coins;
                        xp = punkte + punkte2 + xp;
                        break;

                    case "Mittel":
                        gesamtcoins = 2 * (punkte + punkte2) + gesamtcoins;
                        coins = 2 * (punkte + punkte2) + coins;
                        xp = 2 * (punkte + punkte2) + xp;
                        break;

                    case "Schnell":
                        gesamtcoins = 3 * (punkte + punkte2) + gesamtcoins;
                        coins = 3 * (punkte + punkte2) + coins;
                        xp = 3 * (punkte + punkte2) + xp;
                        break;
                }
            }

        }


        // Hauptmenü
        static void ShowMainMenue()
        {
            Speichern_Laden("Speichern");

            if (performancemode)
            {
                foodfarbe = ConsoleColor.White;
                randfarbe = ConsoleColor.White;
                farbe = ConsoleColor.White;
                headfarbe = ConsoleColor.White;
                farbe2 = ConsoleColor.White; ;
                headfarbe2 = ConsoleColor.White;
                headfarbezahl = 0;
                headfarbe2zahl = 0;
                farbezahl = 0;
                farbe2zahl = 0;
                foodfarbezahl = 0;
                randfarbezahl = 0;
            }

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
                        if (MenueOptions < 1) MenueOptions = 7;
                        break;

                    case ConsoleKey.DownArrow:
                        MenueOptions++;
                        if (MenueOptions > 7) MenueOptions = 1;
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
                case 6:
                    Anleitung();
                    break;
                case 5:
                    Statistiken();
                    break;
                case 7:
                    exit = true;
                    break;
            }
        }


        // Titelbild
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


        // Auswahlmöglichkeiten im Hauptmenü
        static void ShowMenuOptions(int selected)
        {
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       SMAKE MAIN MENU        ║");
            Console.WriteLine("╠══════════════════════════════╣");

            for (int i = 0; i < 7; i++)
            {

                string option = i switch
                {
                    0 => "Spiel starten",
                    1 => "Einstellungen",
                    2 => "Shop",
                    3 => "Skins/Farben",
                    5 => "Anleitung",
                    4 => "Statistiken",
                    6 => "Beenden",
                    _ => ""
                };
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {option,-25}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
        }


        // Eingaben für Spielernamen
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


        // Einstellungsmenü im Hauptmenü
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
                        if (einstellungsAuswahl < 1) einstellungsAuswahl = 5;
                        break;

                    case ConsoleKey.DownArrow:
                        einstellungsAuswahl++;
                        if (einstellungsAuswahl > 5) einstellungsAuswahl = 1;
                        break;
                    case ConsoleKey.Escape:
                        menu = false;
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
                                performancemode = !performancemode;
                                break;
                            case 5:
                                menu = false;
                                break;
                        }
                        break;
                }

            } while (menu);
        }


        // Einstellmöglichkeiten im Einstellungsmenü
        static void EinstellungenOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("EINSTELLUNGEN");
            Console.WriteLine("══════════════════════════════");


            for (int i = 0; i < 5; i++)
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
                        option = $"Performance mode       [Aktuell: {(performancemode ? "An" : "Aus")}]";
                        break;
                    case 4:
                        option = "Zurück zum Hauptmenü";
                        break;
                }

                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {option,-30}");
            }

            Console.WriteLine("══════════════════════════════");
        }


        // Auswahl der Spielgeschwindigkeit
        static void ChangeDifficulty()
        {
            if (difficulty == "Langsam") difficulty = "Mittel";
            else if (difficulty == "Mittel") difficulty = "Schnell";
            else difficulty = "Langsam";
        }


        // Auswahl der Verschiedenen Modi
        static void ChangeGamemode()
        {
            if (gamemode == "Normal") gamemode = "Unendlich";
            else if (gamemode == "Unendlich") gamemode = "Babymode";
            else gamemode = "Normal";
        }


        // Shop - Menü im Hauptmenü
        static void Shop()
        {
            Console.Clear();
            bool menu = true;
            int auswahl = 1;

            // Zähle alle Shop-Optionen zusammen
            int gesamtOptionenSkins = tailskins.Length + foodskins.Length + randskins.Length - 3 ;
            int gesamtOptionenFarben = farben.Length;

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
                        case ConsoleKey.Escape:
                            menu = false;
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
                            if (auswahl + 1 < tailskins.Length)
                            {
                                if (!freigeschaltetTail[auswahl + 1] && coins >= 10)
                                {
                                    freigeschaltetTail[auswahl + 1] = true;
                                    coins -= 10;
                                }
                            }
                            else if (auswahl + 2 < tailskins.Length + foodskins.Length)
                            {
                                int i = auswahl + 2 - tailskins.Length;
                                if (!freigeschaltetFood[i] && coins >= 10)
                                {
                                    freigeschaltetFood[i] = true;
                                    coins -= 10;
                                }
                            }
                            else if (auswahl + 3 < tailskins.Length + foodskins.Length + randskins.Length)
                            {
                                int i = auswahl + 3 - tailskins.Length - foodskins.Length;
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
                        case ConsoleKey.Escape:
                            menu = false;
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Spacebar:
                            Console.Clear();
                            if (auswahl == gesamtOptionenFarben)
                            {
                                menu = false;
                                break;
                            }

                            
                            if (!freigeschaltetFarben[auswahl] && coins >= 10)
                            {
                                freigeschaltetFarben[auswahl] = true;
                                coins -= 10;
                            }
                            break;

                    }
                }
                
                
            } while (menu);
        }


        // Zeigt die Farbenseite vom Shop
        static void ShopFarbenOptions(int selected)
        {

            Console.SetCursorPosition(0, 0);
            Console.WriteLine("           Shop           ");
            Console.WriteLine($"Coins: {coins}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
           
            int option = 0;

            Console.WriteLine("\nFarben:");
            for (int i = 1; i < farben.Length; i++, option++)
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


        // Zeigt die Skinseite vom Shop
        static void ShopSkinsOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("           Shop           ");
            Console.WriteLine($"Coins: {coins}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
            int option = 0;

            Console.WriteLine("\nTail Skins:");
            for (int i = 2; i < tailskins.Length; i++, option++)
            {
                string shoptext = freigeschaltetTail[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {tailskins[i]} {shoptext}");
            }

            Console.WriteLine("\nFood Skins:");
            for (int i = 1; i < foodskins.Length; i++, option++)
            {
                string shoptext = freigeschaltetFood[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {foodskins[i]} {shoptext}");
            }

            Console.WriteLine("\nRand Skins:");
            for (int i = 1; i < randskins.Length; i++, option++)
            {
                string shoptext = freigeschaltetRand[i] ? "[Freigeschaltet]" : "[10 Coins]";
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {randskins[i]} {shoptext}");
            }

            string zeiger2 = (option + 1 == selected) ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }


        // Logik für Skin und Farben menü
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

                    case ConsoleKey.Escape:
                        menu = false;
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
                                } while (!freigeschaltetTail[skinzahl] || skinzahl == skin2zahl);

                                if (freigeschaltetTail[skinzahl])
                                    skin = tailskins[skinzahl];
                                break;

                            case 2:
                                do
                                {
                                    skin2zahl = (skin2zahl + 1) % tailskins.Length;
                                } while (!freigeschaltetTail[skin2zahl] || skin2zahl == skinzahl);

                                if (freigeschaltetTail[skin2zahl])
                                    skin2 = tailskins[skin2zahl];
                                break;
                            case 3:
                                do
                                {
                                    foodzahl = (foodzahl + 1) % foodskins.Length;
                                } while (!freigeschaltetFood[foodzahl]);

                                if (freigeschaltetFood[foodzahl])
                                    food = foodskins[foodzahl];
                                break;
                            case 4:
                                do
                                {
                                    randzahl = (randzahl + 1) % randskins.Length;
                                } while ((!freigeschaltetRand[randzahl]));

                                if (freigeschaltetRand[randzahl])
                                    rand = randskins[randzahl];
                                break;
                            case 5:
                                do
                                {
                                    headfarbezahl = (headfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[headfarbezahl]);

                                if (freigeschaltetFarben[headfarbezahl])
                                    headfarbe = farben[headfarbezahl];
                                break;
                            case 6:
                                do
                                {
                                    headfarbe2zahl = (headfarbe2zahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[headfarbe2zahl]);

                                if (freigeschaltetFarben[headfarbe2zahl])
                                    headfarbe2 = farben[headfarbe2zahl];
                                break;
                            case 7:
                                do
                                {
                                    farbezahl = (farbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[farbezahl]);

                                if (freigeschaltetFarben[farbezahl])
                                    farbe = farben[farbezahl];
                                break;
                            case 8:
                                do
                                {
                                    farbe2zahl = (farbe2zahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[farbe2zahl]);

                                if (freigeschaltetFarben[farbe2zahl])
                                    farbe2 = farben[farbe2zahl];
                                break;
                            case 9:
                                do
                                {
                                    foodfarbezahl = (foodfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[foodfarbezahl]);

                                if (freigeschaltetFarben[foodfarbezahl])
                                    foodfarbe = farben[foodfarbezahl];
                                break;
                            case 10:
                                do
                                {
                                    randfarbezahl = (randfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[randfarbezahl]);

                                if (freigeschaltetFarben[randfarbezahl])
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


        // Auswahlmenü für Skins und Farben im Hauptmenü
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
                        option = $"Player 1 Tailskin ändern    [Aktuell: {skin}]";
                        color = false;
                        break;
                    case 1:
                        option = $"Player 2 Tailskin ändern    [Aktuell: {skin2}]";
                        color = false;
                        break;
                    case 2:
                        option = $"Foodskin ändern             [Aktuell: {food}]";
                        color = false;
                        break;
                    case 3:
                        option = $"Randskin ändern             [Aktuell: {rand}]";
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
                if (color)
                {
                    Console.Write(zeiger + " " + option);
                    if (performancemode)
                    {
                        
                        Console.Write("Performance Mode AN");
                        Console.WriteLine("]");

                    }
                    else
                    {
                        Console.ForegroundColor = farbemenue;
                        Console.Write(farbemenue);
                        Console.ResetColor();
                        Console.WriteLine("]");
                    }
                }
                else
                {
                    Console.WriteLine(zeiger + " " + option);
                }
            }
            Console.WriteLine("══════════════════════════════");
        }


        // Anleitung im Hauptmenü
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


        // Statistik - Untermenü im Hauptmenü
        static void Statistiken()
        {
            Console.Clear();
            Console.WriteLine("Statistiken");
            Console.WriteLine(" ");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine(" ");
            // Level-Berechnung (1 Level pro 100 XP)
            int level = xp / 100 + 1;
            int punktefürLevel = xp % 100;

            // Fortschrittsbalken
            int balkenLänge = 20; // Balken 20 Zeichen lang
            int gefüllteBlöcke = (punktefürLevel * balkenLänge) / 100;
            string Fortschrittsbalken = new string('█', gefüllteBlöcke).PadRight(balkenLänge, '-');

            Console.WriteLine($"Level:                    {level}");
            Console.WriteLine($"Fortschritt:              [{Fortschrittsbalken}] {punktefürLevel}/100");

            Console.WriteLine(" ");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Gesamte Spiele:           {spieleGesamt}");
            Console.WriteLine($"Höchste Punktzahl:        {highscore}");
            Console.WriteLine($"Durchschnittliche XP:     {(spieleGesamt > 0 ? xp / spieleGesamt : 0)}");
            Console.WriteLine($"Gesamte Coins:            {gesamtcoins}");
            Console.WriteLine($"Aktuelle Coins:           {coins}");

            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
            Console.ReadKey();
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
                    Console.WriteLine($"{name} hat {punkte} Punkte erreicht.");
                    Console.WriteLine($"{name2} hat {punkte2} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (gameover == 1)
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

            Kollision(newPlayerX, newPlayerY, newPlayerX2, newPlayerY2);
            Gameover();
            if (!spiel) return;
            TailShift();
            Bewegung(newPlayerX, newPlayerY, newPlayerX2, newPlayerY2);
            EsseFutter();
        }


        // Prüft die Kollision
        static void Kollision(int x, int y, int x2, int y2)
        {
            if (grid[y, x] == ' ' || grid[y, x] == food || grid[y, x] == head)
            {
                kollisionPlayer = false;
                kollisionRand = false;
            }
            else if (grid[y, x] == rand)
            {
                kollisionPlayer = false;
                kollisionRand = true;
            }
            else if (grid[y, x] == skin2 || grid[y, x] == head2 || grid[y, x] == skin)
            {
                kollisionPlayer = true;
                kollisionRand = false;
            }

            if (multiplayer)
            {
                if (grid[y2, x2] == ' ' || grid[y2, x2] == food || grid[y2, x2] == head2)

                {
                    kollisionPlayer2 = false;
                    kollisionRand2 = false;
                }
                else if (grid[y2, x2] == rand)
                {
                    kollisionPlayer2 = false;
                    kollisionRand2 = true;
                }
                else if (grid[y2, x2] == skin || grid[y2, x2] == head || grid[y2, x2] == skin2)
                {
                    kollisionPlayer2 = true;
                    kollisionRand2 = false;
                }
            }
        }


        // Tailkoordinaten berechnen
        static void TailShift()
        {
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
        }


        // Bewegt die Spieler
        static void Bewegung(int x, int y, int x2, int y2)
        {
            // Wenn das Zielfeld leer ist (kein Hindernis), bewege den Spieler
            if (gamemode != "Babymode")
            {
                if (!kollisionPlayer && !kollisionRand)
                {                    
                    for (int i = 0; i <= tail; i++)       // Tail des Spielers Zeichnen
                    {
                        grid[playerY[i], playerX[i]] = skin;
                    }

                    grid[playerY[tail + 1], playerX[tail + 1]] = ' ';        // Altes Feld leeren

                    grid[y, x] = head;  // Spieler auf neues Feld setzen

                    playerX[0] = x;

                    playerY[0] = y;
                }

                if (multiplayer)
                {
                    if (!kollisionPlayer2 && !kollisionRand2)
                    {
                        for (int i = 0; i <= tail2; i++)       // Tail des Spielers2 Zeichnen
                        {
                            grid[playerY2[i], playerX2[i]] = skin2;
                        }

                        grid[playerY2[tail2 + 1], playerX2[tail2 + 1]] = ' ';     // Altes Feld leeren

                        grid[y2, x2] = head2;  // Spieler2 auf neues Feld setzen

                        playerX2[0] = x2;

                        playerY2[0] = y2;
                    }
                }
            }
            else
            {
                if (!kollisionRand)
                {
                    grid[y, x] = head;  // Spieler auf neues Feld setzen

                    for (int i = 0; i <= tail; i++)       // Tail des Spielers Zeichnen
                    {
                        grid[playerY[i], playerX[i]] = skin;
                    }

                    grid[playerY[tail + 1], playerX[tail + 1]] = ' ';        // Altes Feld leeren

                    playerX[0] = x;

                    playerY[0] = y;
                }

                if (multiplayer)
                {
                    if (!kollisionRand2)
                    {
                        grid[y2, x2] = head2;  // Spieler2 auf neues Feld setzen

                        for (int i = 0; i <= tail2; i++)       // Tail des Spielers2 Zeichnen
                        {
                            grid[playerY2[i], playerX2[i]] = skin2;
                        }

                        grid[playerY2[tail2 + 1], playerX2[tail2 + 1]] = ' ';     // Altes Feld leeren

                        playerX2[0] = x2;

                        playerY2[0] = y2;
                    }
                }
            }
        }


        // Prüft, ob das Spiel vorbei ist
        static void Gameover()
        {
            bool spieler1Tot = false;
            bool spieler2Tot = false;

            if (gamemode == "Unendlich")
            {
                if (kollisionPlayer || kollisionRand)
                {
                    spieler1Tot = true;
                }

                if (multiplayer)
                {
                    if (kollisionPlayer2 || kollisionRand2)
                    {
                        spieler2Tot = true;
                    }
                }
            }
            else if (gamemode == "Normal")
            {
                if (kollisionPlayer || kollisionRand || punkte2 == maxpunkte)
                {
                    spieler1Tot = true;
                }

                if (multiplayer)
                {
                    if (kollisionPlayer2 || kollisionRand2 || punkte == maxpunkte)
                    {
                        spieler2Tot = true;
                    }
                }
                else
                {
                    if (punkte == maxpunkte)
                    {
                        spieler2Tot = true;
                    }
                }
            }
            else if (gamemode =="Babymode")
            {
                if (kollisionRand || punkte2 == maxpunkte)
                {
                    spieler1Tot = true;
                }

                if (multiplayer)
                {
                    if (kollisionRand2 || punkte == maxpunkte)
                    {
                        spieler2Tot = true;
                    }
                }
                else
                {
                    if (punkte == maxpunkte)
                    {
                        spieler2Tot = true;
                    }
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



            grid[futterY, futterX] = food; // Setze Futter an die berechnete Position
        }


        // Die Spieler Essen das Futter
        static void EsseFutter()
        {
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

                    if (!performancemode)
                    {
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
                    }

                    // Nur Farbe wechseln, wenn nötig
                    if (neueFarbe != aktuelleFarbe)
                    {
                        Console.ForegroundColor = neueFarbe;
                        aktuelleFarbe = neueFarbe;
                    }

                    Console.Write(zeichen);
                }
                // → Legende auf bestimmten Zeilen ausgeben
                Console.ForegroundColor = randfarbe;
                if (y == 1)
                {
                    Console.Write("  ══════════════════════════════");
                }
                else if (y == 2)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("  Punkte:");
                    Console.ForegroundColor = randfarbe;
                }
                else if (y == 3)
                {
                    Console.Write("  ══════════════════════════════");
                }
                else if (y == 4)
                {
                    Console.ForegroundColor = headfarbe;
                    Console.Write($"  {name}: {punkte}/{maxpunkte}");
                    Console.ForegroundColor = randfarbe;
                }
                else if (y == 5)
                {
                        Console.Write("  ══════════════════════════════");                    
                }
                else if (y == 6)
                {
                    if (multiplayer)
                    {
                        Console.ForegroundColor = headfarbe2;
                        Console.Write($"  {name2}: {punkte2}/{maxpunkte}");
                        Console.ForegroundColor = randfarbe;
                    }
                }
                else if (y == 7)
                {
                    if (multiplayer)
                        Console.Write("  ══════════════════════════════");
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

