using Smake.io.Render;
using Smake.io.Speicher;
using Smake.io.Spiel;

namespace Smake.io.Menus
{
    public class Einstellungen : Screen
    {
        string[] einstellungen = {
                $"Schwierigkeit ändern   [Aktuell: {Spiellogik.difficulty}]",
                $"Multiplayer            [Aktuell: {(Spiellogik.multiplayer ? "An" : "Aus")}]",
                $"Gamemode ändern        [Aktuell: {Spiellogik.gamemode}]",
                $"MaxFutter ändern       [Aktuell: {Spiellogik.maxfutter}]",
                $"Performance mode       [Aktuell: {(RendernSpielfeld.performancemode ? "An" : "Aus")}]",
                $"Musik AN/AUS           [Aktuell: {(Musik.musikplay ? "An" : "Aus")}]",
                $"Sounds AN/AUS          [Aktuell: {(Musik.soundplay ? "An" : "Aus")}]",
                "Spielstand zurücksetzen",
                "Zurück zum Hauptmenü"
            };


        private ConsoleKey input;
        public override ConsoleKey Input
        {
            get { return input; }
            set
            {
                input = value;

                switch (Input)
                {
                    case ConsoleKey.UpArrow:
                        MenuTracker--;
                        break;
                    case ConsoleKey.DownArrow:
                        MenuTracker++;
                        break;
                    case ConsoleKey.Escape:
                        Menu menu = new();
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        SelectMenu();
                        break;
                    default:
                        break;
                }
                Display = BuildMenu();
                Render();
            }
        }

        private int menuTracker;
        public int MenuTracker
        {
            get { return menuTracker; }
            set
            {
                if (value != menuTracker) // loop index
                {
                    if (value > 9)
                    {
                        menuTracker = 1;
                    }
                    else if (value < 1)
                    {
                        menuTracker = 9;
                    }
                    else
                    {
                        menuTracker = value;
                    }
                    selected = MenuTracker;
                }
            }
        }

        public Einstellungen()
        {
            title = "Einstellungen";
            Display = BuildMenu();
            MenuTracker = 1;
            InitialRender();
            StartInputstream();
        }

        private void SelectMenu()
        {
            switch (MenuTracker)
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
                    ChangeMaxFutter();
                    break;
                case 5:
                    RendernSpielfeld.performancemode = !RendernSpielfeld.performancemode;
                    break;
                case 6:
                    Musik.musikplay = !Musik.musikplay;
                    break;
                case 7:
                    Musik.soundplay = !Musik.soundplay;
                    break;
                case 8:
                    ResetSpielstand();
                    break;
                case 9:
                    Menu menu = new();
                    Thread.CurrentThread.Join();
                    break;
                default:
                    break;
            }
        }

        private string[] BuildMenu()
        {
            return new string[]
            {
        $"Schwierigkeit ändern   [Aktuell: {Spiellogik.difficulty}]",
        $"Multiplayer            [Aktuell: {(Spiellogik.multiplayer ? "An" : "Aus")}]",
        $"Gamemode ändern        [Aktuell: {Spiellogik.gamemode}]",
        $"MaxFutter ändern       [Aktuell: {Spiellogik.maxfutter}]",
        $"Performance mode       [Aktuell: {(RendernSpielfeld.performancemode ? "An" : "Aus")}]",
        $"Musik AN/AUS           [Aktuell: {(Musik.musikplay ? "An" : "Aus")}]",
        $"Sounds AN/AUS          [Aktuell: {(Musik.soundplay ? "An" : "Aus")}]",
        "Spielstand zurücksetzen",
        "Zurück zum Hauptmenü"
            };
        }


        // Auswahl der Spielgeschwindigkeit
        void ChangeDifficulty()
        {
            if (Spiellogik.difficulty == "Langsam") Spiellogik.difficulty = "Mittel";
            else if (Spiellogik.difficulty == "Mittel") Spiellogik.difficulty = "Schnell";
            else Spiellogik.difficulty = "Langsam";
        }

        // Auswahl der Verschiedenen Modi
        void ChangeGamemode()
        {
            if (Spiellogik.gamemode == "Normal") Spiellogik.gamemode = "Unendlich";
            else if (Spiellogik.gamemode == "Unendlich") Spiellogik.gamemode = "Babymode";
            else Spiellogik.gamemode = "Normal";
        }

        void ChangeMaxFutter()
        {
            bool gültig = false;

            do
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);

                // Box anzeigen
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║          Maximal Futter einstellen         ║");
                Console.WriteLine("╠════════════════════════════════════════════╣");
                Console.WriteLine($"║ Maximal erlaubte Anzahl: {GameData.MaxFutterconfig,-18}║");
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.Write("Eingabe: ");

                string input = Console.ReadLine()!;

                if (int.TryParse(input, out int wert) && wert > 0)
                {
                    if (wert > GameData.MaxFutterconfig)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n⚠ Der Wert darf maximal {GameData.MaxFutterconfig} sein.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Spiellogik.maxfutter = wert;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✔ MaxFutter wurde auf {wert} gesetzt!");
                        Console.ResetColor();
                        gültig = true;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n⚠ Ungültige Eingabe. Bitte eine gültige positive Zahl eingeben.");
                    Console.ResetColor();
                }

                if (!gültig)
                {
                    Console.WriteLine("\nDrücke eine beliebige Taste, um es erneut zu versuchen...");
                    Console.ReadKey();
                }

            } while (!gültig);

            Console.WriteLine("\nDrücke eine beliebige Taste, um zurückzukehren...");
            Console.ReadKey();
            Console.Clear();
        }

        void ResetSpielstand()
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Willst du deinen Spielstand wirklich zurücksetzen? ({i}/3) [ja/nein]");
                string? eingabe = Console.ReadLine()?.Trim().ToLower();
                Console.Clear();
                if (eingabe != "ja")
                {
                    Console.WriteLine("Zurücksetzen abgebrochen.");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }

            // Wenn alle drei Bestätigungen "ja" waren:
            SpeicherSystem.Speichern_Laden("Zurücksetzen");
            Console.WriteLine("Dein Spielstand wurde zurückgesetzt!");
            Console.WriteLine("Drücke eine Taste, um zurückzukehren...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
