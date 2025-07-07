using Smake.io.Render;
using Smake.io.Spiel;

namespace Smake.io
{
    public class Menüs
    {
        // Shop variablen
        public static int foodzahl;
        public static int foodfarbezahl;
        public static int randfarbezahl;
        public static int randzahl;

        //Freischaltbare Skins/Farbenf
        public readonly static ConsoleColor[] farben = [
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
        public readonly static char[] tailskins = ['+', 'x', '~', '=', '-', 'o', '•'];
        public readonly static char[] foodskins = ['*', '@', '$', '♥', '%', '¤', '&'];
        public readonly static char[] randskins = ['█', '#', '▓', '░', '■', '▌', '▒'];

        //Freigeschalteneskins/farben
        public static bool[] freigeschaltetTail = new bool[tailskins.Length];
        public static bool[] freigeschaltetFood = new bool[foodskins.Length];
        public static bool[] freigeschaltetRand = new bool[randskins.Length];
        public static bool[] freigeschaltetFarben = new bool[farben.Length];

        //Preise Skin/Farben

        public static readonly int[] TailPreis = [300, 350, 400, 450, 500];
        public static readonly int[] FoodPreis = [350, 500, 400, 300, 250, 250];
        public static readonly int[] RandPreis = [200, 400, 400, 350, 300, 400];
        public static readonly int[] FarbenPreis = [100, 259, 100, 250, 200, 300, 100, 300, 175, 250, 100, 250, 450, 500];

        // Level für Skin/Farben
        public static readonly int[] TailLevel = [2, 4, 6, 8, 10];
        public static readonly int[] FoodLevel = [2, 4, 6, 8, 10, 12, 14];
        public static readonly int[] RandLevel = [2, 4, 6, 8, 10, 12, 14];
        public static readonly int[] FarbenLevel = [0, 4, 0, 6, 8, 10, 0, 10, 20, 25, 0, 20, 25, 30];

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            Console.Clear();
            Console.Write("Spieler 1, gib deinen Namen ein: ");
            Spiellogik.player.Name = Console.ReadLine();
            Console.Clear();

            Console.Clear();
            Console.Write("Spieler 2, gib deinen Namen ein: ");
            Spiellogik.player2.Name = Console.ReadLine();
            Console.Clear();
        }

        //Logik des Mainmenüs
        public static void ShowMainMenue()
        {
            Musik.currentmusik = 0;
            SpeicherSytem.Speichern_Laden("Speichern");

            // Level-Berechnung (1 Level pro 100 XP)
            Program.level = Program.xp / 100 + 1;

            if (RendernSpielfeld.performancemode)
            {
                Spiellogik.foodfarbe = ConsoleColor.White;
                Spiellogik.randfarbe = ConsoleColor.White;
                Spiellogik.player.Farbe = ConsoleColor.White;
                Spiellogik.player.Headfarbe = ConsoleColor.White;
                Spiellogik.player2.Farbe = ConsoleColor.White; ;
                Spiellogik.player2.Headfarbe = ConsoleColor.White;
                Spiellogik.player.Headfarbezahl = 0;
                Spiellogik.player2.Headfarbezahl = 0;
                Spiellogik.player.Farbezahl = 0;
                Spiellogik.player2.Farbezahl = 0;
                foodfarbezahl = 0;
                randfarbezahl = 0;
            }

            Console.Clear();
            MenüRenderer.DrawTitle();
            bool menu = true;
            int MenueOptions = 1;

            do
            {
                Console.SetCursorPosition(0, 11);
                MenüRenderer.RenderMainMenueOptions(MenueOptions);

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
                    Spiellogik.Spiel();
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
                    MenüRenderer.RenderAnleitung();
                    break;
                case 5:
                    MenüRenderer.RenderStatistiken();
                    break;
                case 7:
                    Spiellogik.exit = true;
                    break;
            }
        }

        // Einstellungsmenü im Hauptmenü
        static void Einstellungen()
        {
            Console.Clear();
            bool menu = true;
            int einstellungsAuswahl = 1;

            do
            {
                MenüRenderer.RenderEinstellungen(einstellungsAuswahl);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        einstellungsAuswahl--;
                        if (einstellungsAuswahl < 1) einstellungsAuswahl = 7;
                        break;

                    case ConsoleKey.DownArrow:
                        einstellungsAuswahl++;
                        if (einstellungsAuswahl > 7) einstellungsAuswahl = 1;
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
                                Spiellogik.multiplayer = !Spiellogik.multiplayer;
                                break;
                            case 3:
                                ChangeGamemode();
                                break;
                            case 4:
                                RendernSpielfeld.performancemode = !RendernSpielfeld.performancemode;
                                break;
                            case 5:
                                Musik.musikplay = !Musik.musikplay;
                                break;
                            case 6:
                                Musik.soundplay = !Musik.soundplay;
                                break;
                            case 7:
                                menu = false;
                                break;
                        }
                        break;
                }

            } while (menu);
        }

        // Auswahl der Spielgeschwindigkeit
        static void ChangeDifficulty()
        {
            if (Spiellogik.difficulty == "Langsam") Spiellogik.difficulty = "Mittel";
            else if (Spiellogik.difficulty == "Mittel") Spiellogik.difficulty = "Schnell";
            else Spiellogik.difficulty = "Langsam";
        }

        // Auswahl der Verschiedenen Modi
        static void ChangeGamemode()
        {
            if (Spiellogik.gamemode == "Normal") Spiellogik.gamemode = "Unendlich";
            else if (Spiellogik.gamemode == "Unendlich") Spiellogik.gamemode = "Babymode";
            else Spiellogik.gamemode = "Normal";
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

                    MenüRenderer.RenderShopSkinsOptions(auswahl);

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

                    MenüRenderer.RenderShopFarbenOptions(auswahl);

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

        // Logik für Skin und Farben menü
        static void Skin_Farben()
        {
            Console.Clear();
            bool menu = true;
            int Skin_FarbenAuswahl = 1;

            do
            {
                MenüRenderer.RenderSkin_FarbenOptions(Skin_FarbenAuswahl);

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
                                    Spiellogik.player.Skinzahl = (Spiellogik.player.Skinzahl + 1) % tailskins.Length;
                                } while (!freigeschaltetTail[Spiellogik.player.Skinzahl] || Spiellogik.player.Skinzahl == Spiellogik.player2.Skinzahl);

                                if (freigeschaltetTail[Spiellogik.player.Skinzahl])
                                    Spiellogik.player.Skin = tailskins[Spiellogik.player.Skinzahl];
                                break;

                            case 2:
                                do
                                {
                                    Spiellogik.player2.Skinzahl = (Spiellogik.player2.Skinzahl + 1) % tailskins.Length;
                                } while (!freigeschaltetTail[Spiellogik.player2.Skinzahl] || Spiellogik.player2.Skinzahl == Spiellogik.player.Skinzahl);

                                if (freigeschaltetTail[Spiellogik.player2.Skinzahl])
                                    Spiellogik.player2.Skin = tailskins[Spiellogik.player2.Skinzahl];
                                break;
                            case 3:
                                do
                                {
                                    foodzahl = (foodzahl + 1) % foodskins.Length;
                                } while (!freigeschaltetFood[foodzahl]);

                                if (freigeschaltetFood[foodzahl])
                                    Spiellogik.food = foodskins[foodzahl];
                                break;
                            case 4:
                                do
                                {
                                    randzahl = (randzahl + 1) % randskins.Length;
                                } while ((!freigeschaltetRand[randzahl]));

                                if (freigeschaltetRand[randzahl])
                                    Spiellogik.rand = randskins[randzahl];
                                break;
                            case 5:
                                do
                                {
                                    Spiellogik.player.Headfarbezahl = (Spiellogik.player.Headfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Spiellogik.player.Headfarbezahl]);

                                if (freigeschaltetFarben[Spiellogik.player.Headfarbezahl])
                                    Spiellogik.player.Headfarbe = farben[Spiellogik.player.Headfarbezahl];
                                break;
                            case 6:
                                do
                                {
                                    Spiellogik.player2.Headfarbezahl = (Spiellogik.player2.Headfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Spiellogik.player2.Headfarbezahl]);

                                if (freigeschaltetFarben[Spiellogik.player2.Headfarbezahl])
                                    Spiellogik.player2.Headfarbe = farben[Spiellogik.player2.Headfarbezahl];
                                break;
                            case 7:
                                do
                                {
                                    Spiellogik.player.Farbezahl = (Spiellogik.player.Farbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Spiellogik.player.Farbezahl]);

                                if (freigeschaltetFarben[Spiellogik.player.Farbezahl])
                                    Spiellogik.player.Farbe = farben[Spiellogik.player.Farbezahl];
                                break;
                            case 8:
                                do
                                {
                                    Spiellogik.player2.Farbezahl = (Spiellogik.player2.Farbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[Spiellogik.player2.Farbezahl]);

                                if (freigeschaltetFarben[Spiellogik.player2.Farbezahl])
                                    Spiellogik.player2.Farbe = farben[Spiellogik.player2.Farbezahl];
                                break;
                            case 9:
                                do
                                {
                                    foodfarbezahl = (foodfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[foodfarbezahl]);

                                if (freigeschaltetFarben[foodfarbezahl])
                                    Spiellogik.foodfarbe = farben[foodfarbezahl];
                                break;
                            case 10:
                                do
                                {
                                    randfarbezahl = (randfarbezahl + 1) % farben.Length;
                                } while (!freigeschaltetFarben[randfarbezahl]);

                                if (freigeschaltetFarben[randfarbezahl])
                                    Spiellogik.randfarbe = farben[randfarbezahl];
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
    }
}