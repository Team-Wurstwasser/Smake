namespace Snake.io
{
    using System;
    using System.Media;
    public class Spieler
    {
        // Eingabe-Richtung (durch Pfeiltasten)
        public int InputX { get; set; }

        public int InputY { get; set; }

        public bool Aenderung { get; set; }

        // Position des Spielers (Startkoordinaten)
        public int[] PlayerX { get; set; }

        public int[] PlayerY { get; set; }

        // Kollisionsvariablen
        public bool KollisionRand { get; set; }

        public bool KollisionPlayer { get; set; }

        // Länge des Spielers

        public int Tail { get; set; }

        //Punkte des Spielers
        public int Punkte { get; set; }

        // Namen der Spieler
        public string? Name { get; set; }

        // Aussehen des Spielers
        public char Head { get; set; }

        public char Skin { get; set; }

        public  ConsoleColor Farbe { get; set; }

        public  ConsoleColor Headfarbe { get; set; }

        // Shop variablen
        public int Skinzahl { get; set; }

        public int Farbezahl { get; set; }

        public int Headfarbezahl { get; set; }

    }
    public class Musik
    {
        public static bool musikplay = true;
        public static string[] filenames = ["Smake2.wav", "Smake.wav"];
        public static int currentmusik = 0;

        private static int lastmusik = -1;

        public static void Melodie()
        {
            bool musikda = false;


            while (!Program.exit)
            {
                SoundPlayer musik = new(filenames[currentmusik]);

                if (musikplay)
                {
                    // Wenn Musik nicht läuft oder ein anderes Lied gewählt wurde
                    if (!musikda || currentmusik != lastmusik)
                    {
                        //stop alte musik
                        musik.Stop();
                        //Neues Musikstück
                        musik.PlayLooping();

                        musikda = true;
                        lastmusik = currentmusik;
                    }
                }
                else
                {
                   musik.Stop();
                   musikda = false;
                   lastmusik = -1;
   
                }

                Thread.Sleep(100);
            }
        }
    }

    public class Program
    {

        // Spielstatus: true = Spiel läuft, false = Spiel beendet
        public static bool spiel = true;
        public static int gameover;
        public static bool unentschieden;
        public static bool exit = false;
        public static bool performancemode;


        // Spielfeldgröße (Breite x Höhe)
        static readonly int weite = 41;
        static readonly int hoehe = 20;

        // Das Spielfeld als zweidimensionales Zeichen-Array
        static char[,] grid = new char[hoehe, weite];

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
        static int maxpunkte;

        // Spielgeschwindigkeit
        static int zeit;

        // Level und Experience
        public static int coins;
        public static int xp;
        public static int level;


        // Statistik
        public static int spieleGesamt;
        public static int highscore;
        public static int gesamtcoins;

        public static Spieler player = new();

        public static Spieler player2 = new();

        // Main
        static void Main()
        {
            
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            // Mauszeiger im Konsolenfenster ausblenden
            Console.CursorVisible = false;

            SpeicherSytem.Speichern_Laden("Laden");
    
            Thread melodieThread = new(Musik.Melodie);
            melodieThread.Start();

            Menüs.Eingaben();
            do
            {

                Menüs.ShowMainMenue();

            } while (!exit);
            melodieThread.Join();
        }

        // Allen Variablen den Startwert geben
        static void Neustart()
        {
            Musik.currentmusik = 1;
            SpeicherSytem.Speichern_Laden("Speichern");

            spiel = true;
            
            gameover = 0;

            unentschieden = false;

            player.KollisionPlayer = false;
            player2.KollisionPlayer = false;
            player.KollisionRand = false;
            player2.KollisionRand = false;

            // Taillängen zurücksetzen
            player.Tail = 3;
            player2.Tail = 3;

            // Punkte zurücksetzen

            player.Punkte = 0;
            player2.Punkte = 0;

            maxpunkte = 20;

            // Maximale Länge einstellen
            if (gamemode == "Normal" || gamemode == "Babymode")
            {
                player.PlayerX = new int[maxpunkte + player.Tail + 2];
                player.PlayerY = new int[maxpunkte + player.Tail + 2];
                player2.PlayerX = new int[maxpunkte + player2.Tail + 2];
                player2.PlayerY = new int[maxpunkte + player2.Tail + 2];
            }
            else if (gamemode == "Unendlich")
            {
                player.PlayerX = new int[weite * hoehe];
                player.PlayerY = new int[weite * hoehe];
                player2.PlayerX = new int[weite * hoehe];
                player2.PlayerY = new int[weite * hoehe];
            }


            // Arrays zurücksetzen
            Array.Clear(player.PlayerX, 0, player.PlayerX.Length);
            Array.Clear(player.PlayerY, 0, player.PlayerY.Length);
            Array.Clear(player2.PlayerX, 0, player2.PlayerX.Length);
            Array.Clear(player2.PlayerY, 0, player2.PlayerY.Length);

            // Spieler-Positionen auf Startwerte setzen
            player.PlayerX[0] = 36;
            player.PlayerY[0] = 4;
            player2.PlayerX[0] = 4;
            player2.PlayerY[0] = 4;

            player.Aenderung = true;
            player2.Aenderung = true;

            // Aussehen einstellen

            player.Head = player.Skin;

            player2.Head = player2.Skin;

            // Zeit einstellen

           
            if (difficulty == "Langsam") zeit = 150;
            else if (difficulty == "Mittel") zeit = 100;
            else zeit = 50;

            // Alle Eingabewerte zurücksetzen
            player.InputX = 0;
            player.InputY = 0;
            player2.InputX = 0;
            player2.InputY = 0;

        }

        // Spielablauf
        public static void Spiel()
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

                player.Aenderung = true; // Eingaben auf 1 pro Tick Beschränken

                player2.Aenderung = true;

            }

            Coins();

            inputThread.Join();   // Warte auf Ende des Eingabethreads sodass das Spiel sauber beendet wird

            Menüs.ShowGameOverScreen(); // Spielende-Bildschirm

            while (Console.KeyAvailable) Console.ReadKey(true);   // Leere Eingabepuffer vollständig
        }

        // Coins und xp hinzufügen
        static void Coins()
        {
            if (gamemode != "babymode")
            {
                if(highscore< player.Punkte)
                {highscore = player.Punkte;}
                else if (highscore < player2.Punkte)
                {highscore = player2.Punkte;}

                spieleGesamt++;

                switch (difficulty)
                {
                    case "Langsam":
                        gesamtcoins = player.Punkte + player2.Punkte + gesamtcoins;
                        coins = player.Punkte + player2.Punkte + coins;
                        xp = player.Punkte + player2.Punkte + xp;
                        break;

                    case "Mittel":
                        gesamtcoins = 2 * (player.Punkte + player2.Punkte) + gesamtcoins;
                        coins = 2 * (player.Punkte + player2.Punkte) + coins;
                        xp = 2 * (player.Punkte + player2.Punkte) + xp;
                        break;

                    case "Schnell":
                        gesamtcoins = 3 * (player.Punkte + player2.Punkte) + gesamtcoins;
                        coins = 3 * (player.Punkte + player2.Punkte) + coins;
                        xp = 3 * (player.Punkte + player2.Punkte) + xp;
                        break;
                }
            }

        }

        // Aktualisiert die Position des Spielers anhand der Eingabe
        static void Update()
        {

            // Neue Zielkoordinaten berechnen

            int newPlayerX = player.PlayerX[0] + 2 * player.InputX;

            int newPlayerY = player.PlayerY[0] + player.InputY;

            int newPlayerX2 = player2.PlayerX[0] + 2 * player2.InputX;

            int newPlayerY2 = player2.PlayerY[0] + player2.InputY;

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
            if (grid[y, x] == ' ' || grid[y, x] == food || grid[y, x] == player.Head)
            {
                player.KollisionPlayer = false;
                player.KollisionRand = false;
            }
            else if (grid[y, x] == rand)
            {
                player.KollisionPlayer = false;
                player.KollisionRand = true;
            }
            else if (grid[y, x] == player2.Skin || grid[y, x] == player2.Head || grid[y, x] == player.Skin)
            {
                player.KollisionPlayer = true;
                player.KollisionRand = false;
            }

            if (multiplayer)
            {
                if (grid[y2, x2] == ' ' || grid[y2, x2] == food || grid[y2, x2] == player2.Head)

                {
                    player2.KollisionPlayer = false;
                    player2.KollisionRand = false;
                }
                else if (grid[y2, x2] == rand)
                {
                    player2.KollisionPlayer = false;
                    player2.KollisionRand = true;
                }
                else if (grid[y2, x2] == player.Skin || grid[y2, x2] == player.Head || grid[y2, x2] == player2.Skin)
                {
                    player2.KollisionPlayer = true;
                    player2.KollisionRand = false;
                }
            }
        }

        // Tailkoordinaten berechnen
        static void TailShift()
        {
            for (int i = player.PlayerX.Length - 1; i > 0; i--)
            {
                player.PlayerX[i] = player.PlayerX[i - 1];
            }

            for (int i = player.PlayerY.Length - 1; i > 0; i--)
            {
                player.PlayerY[i] = player.PlayerY[i - 1];
            }

            if (multiplayer)
            {
                for (int i = player2.PlayerX.Length - 1; i > 0; i--)
                {
                    player2.PlayerX[i] = player2.PlayerX[i - 1];
                }

                for (int i = player2.PlayerY.Length - 1; i > 0; i--)
                {
                    player2.PlayerY[i] = player2.PlayerY[i - 1];
                }
            }
        }

        // Bewegt die Spieler
        static void Bewegung(int x, int y, int x2, int y2)
        {
            // Wenn das Zielfeld leer ist (kein Hindernis), bewege den Spieler
            if (gamemode != "Babymode")
            {
                if (!player.KollisionPlayer && !player.KollisionRand)
                {                    
                    for (int i = 0; i <= player.Tail; i++)       // Tail des Spielers Zeichnen
                    {
                        grid[player.PlayerY[i], player.PlayerX[i]] = player.Skin;
                    }

                    grid[player.PlayerY[player.Tail + 1], player.PlayerX[player.Tail + 1]] = ' ';        // Altes Feld leeren

                    grid[y, x] = player.Head;  // Spieler auf neues Feld setzen

                    player.PlayerX[0] = x;

                    player.PlayerY[0] = y;
                }

                if (multiplayer)
                {
                    if (!player2.KollisionPlayer && !player2.KollisionRand)
                    {
                        for (int i = 0; i <= player2.Tail; i++)       // Tail des Spielers2 Zeichnen
                        {
                            grid[player2.PlayerY[i], player2.PlayerX[i]] = player2.Skin;
                        }

                        grid[player2.PlayerY[player2.Tail + 1], player2.PlayerX[player2.Tail + 1]] = ' ';     // Altes Feld leeren

                        grid[y2, x2] = player2.Head;  // Spieler2 auf neues Feld setzen

                        player2.PlayerX[0] = x2;

                        player2.PlayerY[0] = y2;
                    }
                }
            }
            else
            {
                if (!player.KollisionRand)
                {
                    grid[y, x] = player.Head;  // Spieler auf neues Feld setzen

                    for (int i = 0; i <= player.Tail; i++)       // Tail des Spielers Zeichnen
                    {
                        grid[player.PlayerY[i], player.PlayerX[i]] = player.Skin;
                    }

                    grid[player.PlayerY[player.Tail + 1], player.PlayerX[player.Tail + 1]] = ' ';        // Altes Feld leeren

                    player.PlayerX[0] = x;

                    player.PlayerY[0] = y;
                }

                if (multiplayer)
                {
                    if (!player2.KollisionRand)
                    {
                        grid[y2, x2] = player2.Head;  // Spieler2 auf neues Feld setzen

                        for (int i = 0; i <= player2.Tail; i++)       // Tail des Spielers2 Zeichnen
                        {
                            grid[player2.PlayerY[i], player2.PlayerX[i]] = player2.Skin;
                        }

                        grid[player.PlayerY[player.Tail + 1], player2.PlayerX[player2.Tail + 1]] = ' ';     // Altes Feld leeren

                        player2.PlayerX[0] = x2;

                        player2.PlayerY[0] = y2;
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
            else if (gamemode =="Babymode")
            {
                if (player.KollisionRand || player2.Punkte == maxpunkte)
                {
                    spieler1Tot = true;
                }

                if (multiplayer)
                {
                    if (player2.KollisionRand || player.Punkte == maxpunkte)
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
            if (player.PlayerX[0] == futterX && player.PlayerY[0] == futterY)
            {
                player.Tail++;
                player.Punkte++;
                Thread.Sleep(10);
                SetzeFutter();
            }

            // Spieler 2 frisst Futter
            if (player2.PlayerX[0] == futterX && player2.PlayerY[0] == futterY && multiplayer)
            {
                player2.Tail++;
                player2.Punkte++;
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
                                player.Head = 'V';
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

                            break;

                        case ConsoleKey.S:

                            if (player2.InputY != -1 && player2.Aenderung && multiplayer)
                            {
                                player2.InputY = 1;
                                player2.InputX = 0;
                                player2.Aenderung = false;
                                player2.Head = 'V';
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

                            break;

                        case ConsoleKey.A:

                            if (player2.InputX != 1 && player2.Aenderung && multiplayer)
                            {
                                player2.InputY = 0;
                                player2.InputX = -1;
                                player2.Aenderung = false;
                                player2.Head = '<';
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
                        if (x == player.PlayerX[0] && y == player.PlayerY[0])
                            neueFarbe = player.Headfarbe;
                        else if (x == player2.PlayerX[0] && y == player2.PlayerY[0] && multiplayer)
                            neueFarbe = player2.Headfarbe;
                        else if (zeichen == player.Skin)
                            neueFarbe = player.Farbe;
                        else if (zeichen == player2.Skin)
                            neueFarbe = player2.Farbe;
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
                    Console.ForegroundColor = player.Headfarbe;
                    if (gamemode != "Unendlich")
                    {
                        Console.Write($"  {player.Name}: {player.Punkte}/{maxpunkte}");
                    }
                    else
                    {
                        Console.Write($"  {player.Name}: {player.Punkte}/∞");
                    }
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
                        Console.ForegroundColor = player2.Headfarbe;
                        if (gamemode != "Unendlich")
                        {
                            Console.Write($"  {player2.Name}: {player2.Punkte}/{maxpunkte}");
                        }
                        else
                        {
                            Console.Write($"  {player2.Name}: {player2.Punkte}/∞");
                        }
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

            grid[player.PlayerY[0], player.PlayerX[0]] = player.Head;

            if (multiplayer)
            {
                grid[player2.PlayerY[0], player2.PlayerX[0]] = player2.Head;
            }

        }

    }

}