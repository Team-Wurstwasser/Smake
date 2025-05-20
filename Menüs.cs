namespace Snake.io
{
    public class Menüs
    {
        // Shop variablen
        public static int foodzahl;
        public static int foodfarbezahl;
        public static int randfarbezahl;
        public static int randzahl;

        //Freischaltbare Skins/Farben
        static readonly ConsoleColor[] farben = [
                            ConsoleColor.White,
                            ConsoleColor.Blue,
                            ConsoleColor.DarkBlue,
                            ConsoleColor.Green,
                            ConsoleColor.DarkGreen,
                            ConsoleColor.Cyan,
                            ConsoleColor.DarkCyan,
                            ConsoleColor.Red,
                            ConsoleColor.DarkRed,
                            ConsoleColor.Magenta,
                            ConsoleColor.DarkMagenta,
                            ConsoleColor.Yellow,
                            ConsoleColor.DarkYellow,
                            ConsoleColor.Gray,
                            ConsoleColor.DarkGray,
                                ];
        static readonly char[] tailskins = ['+', 'x', '~', '=', '-', 'o', '•'];
        static readonly char[] foodskins = ['*', '@', '$', '♥', '%', '¤', '&'];
        static readonly char[] randskins = ['█', '#', '▓', '░', '■', '▌', '▒'];

        //Freigeschalteneskins/farben
        public static bool[] freigeschaltetTail = new bool[tailskins.Length];
        public static bool[] freigeschaltetFood = new bool[foodskins.Length];
        public static bool[] freigeschaltetRand = new bool[randskins.Length];
        public static bool[] freigeschaltetFarben = new bool[farben.Length];

        //Preise Skin/Farben

        static readonly int[] TailPreis = [30, 40, 50, 60, 70];
        static readonly int[] FoodPreis = [20, 30, 40, 50, 60, 70];
        static readonly int[] RandPreis = [20, 30, 40, 50, 60, 70];
        static readonly int[] FarbenPreis = [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140];

        // Level für Skin/Farben
        static readonly int[] TailLevel = [0, 1, 4, 6, 20];
        static readonly int[] FoodLevel = [0, 0, 0, 3, 4, 7, 15];
        static readonly int[] RandLevel = [0, 0, 0, 2, 4, 6, 8];
        static readonly int[] FarbenLevel = [0, 0, 8, 10, 10, 10, 10, 10, 10, 10, 20, 30, 40, 50];

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            Console.Clear();
            Console.Write("Spieler 1, gib deinen Namen ein: ");
            Program.player.Name = Console.ReadLine();
            Console.Clear();

            Console.Clear();
            Console.Write("Spieler 2, gib deinen Namen ein: ");
            Program.player2.Name = Console.ReadLine();
            Console.Clear();
        }

        //Logik des Mainmenüs
        public static void ShowMainMenue()
        {
            SpeicherSytem.Speichern_Laden("Speichern");

            // Level-Berechnung (1 Level pro 100 XP)
            Program.level = Program.xp / 100 + 1;

            if (Program.performancemode)
            {
                Program.foodfarbe = ConsoleColor.White;
                Program.randfarbe = ConsoleColor.White;
                Program.player.Farbe = ConsoleColor.White;
                Program.player.Headfarbe = ConsoleColor.White;
                Program.player2.Farbe = ConsoleColor.White; ;
                Program.player2.Headfarbe = ConsoleColor.White;
                Program.player.Headfarbezahl = 0;
                Program.player2.Headfarbezahl = 0;
                Program.player.Farbezahl = 0;
                Program.player2.Farbezahl = 0;
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
                    Program.Spiel();
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
                    Program.exit = true;
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
                    4 => "Statistiken",
                    5 => "Anleitung",
                    6 => "Beenden",
                    _ => ""
                };
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {option,-25}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
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
                                Program.multiplayer = !Program.multiplayer;
                                break;
                            case 3:
                                ChangeGamemode();
                                break;
                            case 4:
                                Program.performancemode = !Program.performancemode;
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
                string option = i switch
                {
                    0 => $"Schwierigkeit ändern   [Aktuell: {Program.difficulty}]",
                    1 => $"Multiplayer            [Aktuell: {(Program.multiplayer ? "An" : "Aus")}]",
                    2 => $"Gamemode ändern        [Aktuell: {Program.gamemode}]",
                    3 => $"Performance mode       [Aktuell: {(Program.performancemode ? "An" : "Aus")}]",
                    4 => "Zurück zum Hauptmenü",
                    _ => ""
                };
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {option,-25}");
            }

            Console.WriteLine("══════════════════════════════");
        }

        // Auswahl der Spielgeschwindigkeit
        static void ChangeDifficulty()
        {
            if (Program.difficulty == "Langsam") Program.difficulty = "Mittel";
            else if (Program.difficulty == "Mittel") Program.difficulty = "Schnell";
            else Program.difficulty = "Langsam";
        }

        // Auswahl der Verschiedenen Modi
        static void ChangeGamemode()
        {
            if (Program.gamemode == "Normal") Program.gamemode = "Unendlich";
            else if (Program.gamemode == "Unendlich") Program.gamemode = "Babymode";
            else Program.gamemode = "Normal";
        }

        // Shop - Menü im Hauptmenü
        static void Shop()
        {
            Console.Clear();
            bool menu = true;
            int auswahl = 1;

            // Zähle alle Shop-Optionen zusammen
            int gesamtOptionenSkins = tailskins.Length + foodskins.Length + randskins.Length - 3;
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
                            else if (auswahl + 1 < tailskins.Length)
                            {
                                if (!freigeschaltetTail[auswahl + 1] && Program.coins >= TailPreis[auswahl - 1] && Program.level >= TailLevel[auswahl - 1])
                                {
                                    freigeschaltetTail[auswahl + 1] = true;
                                    Program.coins -= TailPreis[auswahl - 1];
                                }
                            }
                            else if (auswahl + 2 < tailskins.Length + foodskins.Length)
                            {
                                int i = auswahl + 2 - tailskins.Length;
                                if (!freigeschaltetFood[i] && Program.coins >= FoodPreis[auswahl - 6] && Program.level >= FoodLevel[auswahl - 6])
                                {
                                    freigeschaltetFood[i] = true;
                                    Program.coins -= FoodPreis[auswahl - 6];
                                }
                            }
                            else if (auswahl + 3 < tailskins.Length + foodskins.Length + randskins.Length)
                            {
                                int i = auswahl + 3 - tailskins.Length - foodskins.Length;
                                if (!freigeschaltetRand[i] && Program.coins >= RandPreis[auswahl - 12] && Program.level >= RandLevel[auswahl - 12])
                                {
                                    freigeschaltetRand[i] = true;
                                    Program.coins -= RandPreis[auswahl - 12];
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
                            }
                            else if (!freigeschaltetFarben[auswahl] && Program.coins >= FarbenPreis[auswahl - 1] && Program.level >= FarbenLevel[auswahl - 1])
                            {
                                freigeschaltetFarben[auswahl] = true;
                                Program.coins -= FarbenPreis[auswahl - 1];
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
            Console.WriteLine($"Coins: {Program.coins}");
            Console.WriteLine($"Level: {Program.level}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");

            int option = 0;

            Console.WriteLine("\nFarben:");
            for (int i = 1; i < farben.Length; i++, option++)
            {
                string shoptext;

                if (Program.level < FarbenLevel[i - 1])
                {
                    shoptext = $"[Benötigtes Level: {FarbenLevel[i - 1]}]";
                }
                else
                {
                    shoptext = freigeschaltetFarben[i] ? "[Freigeschaltet]" : "[" + FarbenPreis[i - 1] + " Coins]";
                }
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
            Console.WriteLine($"Coins: {Program.coins}");
            Console.WriteLine($"Level: {Program.level}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
            int option = 0;

            Console.WriteLine("\nTail Skins:");
            for (int i = 2; i < tailskins.Length; i++, option++)
            {
                string shoptext;

                if (Program.level < TailLevel[i - 2])
                {
                    shoptext = $"[Benötigtes Level: {TailLevel[i - 2]}]";
                }
                else
                {
                    shoptext = freigeschaltetTail[i] ? "[Freigeschaltet]" : "[" + TailPreis[i - 2] + " Coins]";
                }
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {tailskins[i]} {shoptext}");
            }

            Console.WriteLine("\nFood Skins:");
            for (int i = 1; i < foodskins.Length; i++, option++)
            {
                string shoptext;

                if (Program.level < FoodLevel[i - 1])
                {
                    shoptext = $"[Benötigtes Level: {FoodLevel[i - 1]}]";
                }
                else
                {
                    shoptext = freigeschaltetFood[i] ? "[Freigeschaltet]" : "[" + FoodPreis[i - 1] + " Coins]";
                }
                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {foodskins[i]} {shoptext}");
            }

            Console.WriteLine("\nRand Skins:");
            for (int i = 1; i < randskins.Length; i++, option++)
            {
                string shoptext;

                if (Program.level < RandLevel[i - 1])
                {
                    shoptext = $"[Benötigtes Level: {RandLevel[i - 1]}";
                }
                else
                {
                    shoptext = freigeschaltetRand[i] ? "[Freigeschaltet]" : "[" + RandPreis[i - 1] + " Coins]";
                }
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
                                    Program.player.Skinzahl = (Program.player.Skinzahl + 1) % tailskins.Length;
                                } while (!freigeschaltetTail[Program.player.Skinzahl] || Program.player.Skinzahl == Program.player2.Skinzahl);

                                if (freigeschaltetTail[Program.player.Skinzahl])
                                    Program.player.Skin = tailskins[Program.player.Skinzahl];
                                break;

                            case 2:
                                do
                                {
                                    Program.player2.Skinzahl = (Program.player2.Skinzahl + 1) % tailskins.Length;
                                } while (!freigeschaltetTail[Program.player2.Skinzahl] || Program.player2.Skinzahl == Program.player.Skinzahl);

                                if (freigeschaltetTail[Program.player2.Skinzahl])
                                    Program.player2.Skin = tailskins[Program.player2.Skinzahl];
                                break;
                            case 3:
                                do
                                {
                                    foodzahl = (foodzahl + 1) % foodskins.Length;
                                } while (!freigeschaltetFood[foodzahl]);

                                if (freigeschaltetFood[foodzahl])
                                    Program.food = foodskins[foodzahl];
                                break;
                            case 4:
                                do
                                {
                                    randzahl = (randzahl + 1) % randskins.Length;
                                } while ((!freigeschaltetRand[randzahl]));

                                if (freigeschaltetRand[randzahl])
                                    Program.rand = randskins[randzahl];
                                break;
                            case 5:
                                do
                                {
                                    Program.player.Headfarbezahl = (Program.player.Headfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Program.player.Headfarbezahl]);

                                if (freigeschaltetFarben[Program.player.Headfarbezahl])
                                    Program.player.Headfarbe = farben[Program.player.Headfarbezahl];
                                break;
                            case 6:
                                do
                                {
                                    Program.player2.Headfarbezahl = (Program.player2.Headfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Program.player2.Headfarbezahl]);

                                if (freigeschaltetFarben[Program.player2.Headfarbezahl])
                                    Program.player2.Headfarbe = farben[Program.player2.Headfarbezahl];
                                break;
                            case 7:
                                do
                                {
                                    Program.player.Farbezahl = (Program.player.Farbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Program.player.Farbezahl]);

                                if (freigeschaltetFarben[Program.player.Farbezahl])
                                    Program.player.Farbe = farben[Program.player.Farbezahl];
                                break;
                            case 8:
                                do
                                {
                                    Program.player2.Farbezahl = (Program.player2.Farbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Program.player2.Farbezahl]);

                                if (freigeschaltetFarben[Program.player2.Farbezahl])
                                    Program.player2.Farbe = farben[Program.player2.Farbezahl];
                                break;
                            case 9:
                                do
                                {
                                    foodfarbezahl = (foodfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[foodfarbezahl]);

                                if (freigeschaltetFarben[foodfarbezahl])
                                    Program.foodfarbe = farben[foodfarbezahl];
                                break;
                            case 10:
                                do
                                {
                                    randfarbezahl = (randfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[randfarbezahl]);

                                if (freigeschaltetFarben[randfarbezahl])
                                    Program.randfarbe = farben[randfarbezahl];
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
                        option = $"Player 1 Tailskin ändern    [Aktuell: {Program.player.Skin}]";
                        color = false;
                        break;
                    case 1:
                        option = $"Player 2 Tailskin ändern    [Aktuell: {Program.player2.Skin}]";
                        color = false;
                        break;
                    case 2:
                        option = $"Foodskin ändern             [Aktuell: {Program.food}]";
                        color = false;
                        break;
                    case 3:
                        option = $"Randskin ändern             [Aktuell: {Program.rand}]";
                        color = false;
                        break;
                    case 4:
                        option = $"Player 1 Farbe ändern       [Aktuell: ";
                        farbemenue = Program.player.Headfarbe;
                        color = true;
                        break;
                    case 5:
                        option = $"Player 2 Farbe ändern       [Aktuell: ";
                        farbemenue = Program.player2.Headfarbe;
                        color = true;
                        break;
                    case 6:
                        option = $"Player 1 Tailfarbe ändern   [Aktuell: ";
                        farbemenue = Program.player.Farbe;
                        color = true;
                        break;
                    case 7:
                        option = $"Player 2 Tailfarbe ändern   [Aktuell: ";
                        farbemenue = Program.player2.Farbe;
                        color = true;
                        break;

                    case 8:
                        option = $"Foodfarbe ändern            [Aktuell: ";
                        farbemenue = Program.foodfarbe;
                        color = true;
                        break;

                    case 9:
                        option = $"Randfarbe ändern            [Aktuell: ";
                        farbemenue = Program.randfarbe;
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
                    if (Program.performancemode)
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
            Console.WriteLine($"Ziel: Iss so viele {Program.food} wie möglich!");
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

            int punktefürLevel = Program.xp % 100;

            // Fortschrittsbalken
            int balkenLänge = 20; // Balken 20 Zeichen lang
            int gefüllteBlöcke = (punktefürLevel * balkenLänge) / 100;
            string Fortschrittsbalken = new string('█', gefüllteBlöcke).PadRight(balkenLänge, '-');

            Console.WriteLine($"Level:                    {Program.level}");
            Console.WriteLine($"Fortschritt:              [{Fortschrittsbalken}] {punktefürLevel}/100");

            Console.WriteLine(" ");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Gesamte Spiele:           {Program.spieleGesamt}");
            Console.WriteLine($"Höchste Punktzahl:        {Program.highscore}");
            Console.WriteLine($"Durchschnittliche XP:     {(Program.spieleGesamt > 0 ? Program.xp / Program.spieleGesamt : 0)}");
            Console.WriteLine($"Gesamte Coins:            {Program.gesamtcoins}");
            Console.WriteLine($"Aktuelle Coins:           {Program.coins}");

            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
            Console.ReadKey();
        }

        // Zeigt den Game-Over-Screen an
        public static void ShowGameOverScreen()
        {
            Console.Clear();
            Console.WriteLine("═════════════════════════════════════");
            Console.WriteLine("             GAME OVER              ");
            Console.WriteLine("═════════════════════════════════════");

            if (Program.multiplayer)
            {
                if (Program.unentschieden)
                {
                    Console.WriteLine();
                    Console.WriteLine("Unentschieden!");
                    Console.WriteLine($"{Program.player.Name} hat {Program.player.Punkte} Punkte erreicht.");
                    Console.WriteLine($"{Program.player2.Name} hat {Program.player2.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (Program.gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{Program.player2.Name} gewinnt!");
                    Console.WriteLine($"Punkte: {Program.player2.Punkte}");
                    Console.WriteLine($"{Program.player.Name} hat {Program.player.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (Program.gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{Program.player.Name} gewinnt!");
                    Console.WriteLine($"Punkte: {Program.player.Punkte}");
                    Console.WriteLine($"{Program.player2.Name} hat {Program.player2.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
            }
            else
            {
                if (Program.gameover == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Leider verloren – versuch's noch einmal!");
                    Console.WriteLine($"Du hast {Program.player.Punkte} Punkte erreicht.");
                    Console.WriteLine();
                }
                else if (Program.gameover == 2)
                {
                    Console.WriteLine();
                    Console.WriteLine("Glückwunsch! Du hast gewonnen!");
                    Console.WriteLine($"Deine Punktzahl: {Program.player.Punkte}");
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
                        Program.Spiel();
                        continue;

                    case ConsoleKey.Escape:
                        check = true;
                        break;

                }
            }
            while (!check);
        }
    }
}