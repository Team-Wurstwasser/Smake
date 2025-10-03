using Smake.Render;
using Smake.Speicher;
using Smake.Values;
using System.Diagnostics.Metrics;
using System.Reflection;

namespace Smake.Menues
{
    public class Einstellungen : RendernMenue
    {

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
                        _ = new Menue();
                        StopInputstream();
                        Thread.Sleep(5);
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
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
                    Selected = MenuTracker;
                }
            }
        }

        public Einstellungen()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Einstellungen;

            Title = "Einstellungen";
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
                    Spielvalues.Multiplayer = !Spielvalues.Multiplayer;
                    break;
                case 3:
                    ChangeGamemode();
                    break;
                case 4:
                    ChangeMaxFutter();
                    break;
                case 5:
                    RendernSpielfeld.Performancemode = !RendernSpielfeld.Performancemode;
                    break;
                case 6:
                    Musik.Musikplay = !Musik.Musikplay;
                    Musik.Melodie();
                    break;
                case 7:
                    Musik.Soundplay = !Musik.Soundplay;
                    Musik.Melodie();
                    break;
                case 8:
                    ResetSpielstand();
                    break;
                case 9:
                    _ = new Menue();
                    StopInputstream();
                    Thread.Sleep(5);
                    break;
                default:
                    break;
            }
        }

        private static string[] BuildMenu()
        {
            return
            [
        $"Schwierigkeit ändern   [Aktuell: {Spielvalues.Difficulty}]",
        $"Multiplayer            [Aktuell: {(Spielvalues.Multiplayer ? "An" : "Aus")}]",
        $"Gamemode ändern        [Aktuell: {Spielvalues.Gamemode}]",
        $"MaxFutter ändern       [Aktuell: {Spielvalues.Maxfutter}]",
        $"Performance mode       [Aktuell: {(RendernSpielfeld.Performancemode ? "An" : "Aus")}]",
        $"Musik AN/AUS           [Aktuell: {(Musik.Musikplay ? "An" : "Aus")}]",
        $"Sounds AN/AUS          [Aktuell: {(Musik.Soundplay ? "An" : "Aus")}]",
        "Spielstand zurücksetzen",
        "Zurück zum Hauptmenü"
            ];
        }


        // Auswahl der Spielgeschwindigkeit
        static void ChangeDifficulty()
        {
            if (Spielvalues.Difficulty == "Langsam") Spielvalues.Difficulty = "Mittel";
            else if (Spielvalues.Difficulty == "Mittel") Spielvalues.Difficulty = "Schnell";
            else Spielvalues.Difficulty = "Langsam";
        }

        // Auswahl der Verschiedenen Modi
        static void ChangeGamemode()
        {
            if (Spielvalues.Gamemode == "Normal") Spielvalues.Gamemode = "Unendlich";
            else if (Spielvalues.Gamemode == "Unendlich") Spielvalues.Gamemode = "Babymode";
            else if (Spielvalues.Gamemode == "Babymode") Spielvalues.Gamemode = "Mauer-Modus";
            else if (Spielvalues.Gamemode == "Mauer-Modus") Spielvalues.Gamemode = "Schlüssel-Modus";
            else if (Spielvalues.Gamemode == "Schlüssel-Modus") Spielvalues.Gamemode = "Sprungfutter-Modus";
            else if (Spielvalues.Gamemode == "Sprungfutter-Modus") Spielvalues.Gamemode = "Chaos-Steuerung";
            else Spielvalues.Gamemode = "Normal";
        }

        static void ChangeMaxFutter()
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
                        Spielvalues.Maxfutter = wert;
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

        static void ResetSpielstand()
        {
            Console.Clear();
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
