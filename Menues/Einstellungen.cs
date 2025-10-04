using Smake.Render;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Menues
{
    public class Einstellungen : RendernMenue
    {
        void ProcessInput()
        {
            switch (Input)
            {
                case ConsoleKey.UpArrow:
                    MenuTracker--;
                    break;
                case ConsoleKey.DownArrow:
                    MenuTracker++;
                    break;
                case ConsoleKey.Escape:
                    StopInputstream();
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    SelectMenu();
                    break;
            }

            Input = 0;
            Display = BuildMenu();
            Render();
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
            Menueloop();
        }

        private void Menueloop()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Einstellungen;

            Title = "Einstellungen";
            Display = BuildMenu();
            MenuTracker = 1;
            InitialRender();
            StartInputstream();
            while (DoReadInput)
            {
                ProcessInput();
                Thread.Sleep(5); // kleine Pause, CPU schonen
            }

            Program.CurrentView = 7;
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
                    StopInputstream();
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

        // Liste aller Modi
        static readonly string[] Modi =
        [
            "Normal",
            "Unendlich",
            "Babymode",
            "Babymode-Unendlich",
            "Mauer-Modus",
            "Schlüssel-Modus",
            "Sprungfutter-Modus",
            "Chaos-Steuerung"
        ];

        static void ChangeGamemode()
        {
            bool gueltig = false;

            do
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine("║             Spielmodus auswählen           ║");
                Console.WriteLine("╠════════════════════════════════════════════╣");

                // Alle Modi anzeigen, aktueller Modus wird markiert
                for (int i = 0; i < Modi.Length; i++)
                {
                    Console.Write($"║ ");
                    if (Spielvalues.Gamemode == Modi[i])
                    {

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"[{i + 1}] {Modi[i],-25} ← aktuell".PadRight(43));
                        Console.ResetColor();

                    }
                    else
                    {
                        Console.Write($"[{i + 1}] {Modi[i],-39}");
                    }
                    Console.WriteLine("║");
                }

                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.Write("Auswahl: ");

                string eingabe = Console.ReadLine()!;

                if (int.TryParse(eingabe, out int auswahl))
                {
                    if (auswahl >= 1 && auswahl <= Modi.Length)
                    {
                        Spielvalues.Gamemode = Modi[auswahl - 1];

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n✔ Spielmodus gewechselt: {Spielvalues.Gamemode}");
                        Console.ResetColor();
                        gueltig = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n⚠ Ungültige Auswahl!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n⚠ Bitte eine Zahl eingeben!");
                    Console.ResetColor();
                }

                if (!gueltig)
                {
                    Console.WriteLine("\nDrücke eine beliebige Taste, um es erneut zu versuchen...");
                    Console.ReadKey(true);
                }
            }
            while (!gueltig);

            Console.WriteLine("\nDrücke eine beliebige Taste, um zurückzukehren...");
            Console.ReadKey(true);
            Console.Clear();
        }


        static void ChangeMaxFutter()
        {
            bool gueltig = false;

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
                        gueltig = true;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n⚠ Ungültige Eingabe. Bitte eine gültige positive Zahl eingeben.");
                    Console.ResetColor();
                }

                if (!gueltig)
                {
                    Console.WriteLine("\nDrücke eine beliebige Taste, um es erneut zu versuchen...");
                    Console.ReadKey(true);
                }

            } while (!gueltig);

            Console.WriteLine("\nDrücke eine beliebige Taste, um zurückzukehren...");
            Console.ReadKey(true);
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
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                }
            }

            // Wenn alle drei Bestätigungen "ja" waren:
            SpeicherSystem.Speichern_Laden("Zurücksetzen");
            Console.WriteLine("Dein Spielstand wurde zurückgesetzt!");
            Console.WriteLine("Drücke eine Taste, um zurückzukehren...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
