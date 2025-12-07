using Smake.Render;
using Smake.Speicher;
using Smake.Values;
using Smake.SFX;

namespace Smake.Menues
{
    public class Einstellungen : RendernMenue
    {
        private int menuTracker;
        public int MenuTracker
        {
            get { return menuTracker; }
            set
            {
                if (value != menuTracker)
                {
                    if (value > 10) menuTracker = 1;
                    else if (value < 1) menuTracker = 10;
                    else menuTracker = value;
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
            Sounds.Melodie(GameData.MusikDaten.Menue?.Einstellungen ?? 0);
            Title = LanguageManager.Get("settings.title");
            Display = BuildMenu();
            MenuTracker = 1;
            InitialRender();
            StartInputstream();

            while (DoReadInput)
            {
                Spielvalues.GamemodeInt ??= 1;
                var modes = LanguageManager.GetArray("settings.gamemodes");
                Spielvalues.Gamemode = modes[(int)Spielvalues.GamemodeInt - 1];

                Spielvalues.Difficulty = Spielvalues.DifficultyInt switch
                {
                    1 => LanguageManager.Get("settings.difficulty.slow"),
                    2 => LanguageManager.Get("settings.difficulty.medium"),
                    3 => LanguageManager.Get("settings.difficulty.fast"),
                    _ => LanguageManager.Get("settings.difficulty.medium") // Standardwert
                };

                ProcessInput();
                Thread.Sleep(5);
            }

            Program.CurrentView = 7;
        }

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

        private void SelectMenu()
        {
            switch (MenuTracker)
            {
                case 1: ChangeDifficulty(); break;
                case 2: Spielvalues.Multiplayer = !Spielvalues.Multiplayer; break;
                case 3: ChangeGamemode(); break;
                case 4: ChangeMaxFutter(); break;
                case 5: RendernSpielfeld.Performancemode = !RendernSpielfeld.Performancemode; break;
                case 6: Sounds.Musikplay = !Sounds.Musikplay;break;
                case 7: Sounds.Soundplay = !Sounds.Soundplay;break;
                case 8: ChangeLanguage(); break;
                case 9: ResetSpielstand(); break;
                case 10: StopInputstream(); break;
            }
        }


        private static string[] BuildMenu()
        {
            var items = LanguageManager.GetArray("settings.items");
            return
            [
                items[0].Replace("{difficulty}", Spielvalues.Difficulty),
                items[1].Replace("{multiplayer}", Spielvalues.Multiplayer ? LanguageManager.Get("settings.on") : LanguageManager.Get("settings.off")),
                items[2].Replace("{gamemode}", Spielvalues.Gamemode),
                items[3].Replace("{maxfutter}", Spielvalues.Maxfutter.ToString()),
                items[4].Replace("{performance}", RendernSpielfeld.Performancemode ? LanguageManager.Get("settings.on") : LanguageManager.Get("settings.off")),
                items[5].Replace("{music}", Sounds.Musikplay ? LanguageManager.Get("settings.on") : LanguageManager.Get("settings.off")),
                items[6].Replace("{sounds}", Sounds.Soundplay ? LanguageManager.Get("settings.on") : LanguageManager.Get("settings.off")),
                items[7].Replace("{language}", LanguageManager.Language?.ToUpper()),
                items[8],
                items[9]
            ];
        }

        static void ChangeLanguage()
        {
            bool valid = false;

            // Dynamisch verfügbare Sprachen (Code + Anzeigename)
            List<(string, string)> languages = LanguageManager.GetAvailableLanguages();

            do
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine(LanguageManager.Get("settings.languageMenuTitle"));
                Console.WriteLine("╠════════════════════════════════════════════╣");

                // Sprachen dynamisch auflisten
                for (int i = 0; i < languages.Count; i++)
                {
                    
                    var (langCode, displayName) = languages[i];

                    Console.Write("║ ");
                    if (LanguageManager.Language == langCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"[{i + 1}] {displayName,-10} ({langCode}) ← {LanguageManager.Get("settings.current")}".PadRight(43));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"[{i + 1}] {displayName,-10} ({langCode})".PadRight(43));
                    }
                    Console.WriteLine("║");
                }

                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.Write(LanguageManager.Get("settings.languageMenuPrompt"));

                string eingabe = Console.ReadLine()!.Trim().ToLower();
                string? newLang = null;

                // Eingabe prüfen (Index oder Sprachcode)
                if (int.TryParse(eingabe, out int auswahl) && auswahl >= 1 && auswahl <= languages.Count)
                {
                    var (langCode, _) = languages[auswahl - 1];
                    newLang = langCode;
                }
                else
                {
                    var match = languages.FirstOrDefault(l => string.Equals(l.Item1, eingabe, StringComparison.OrdinalIgnoreCase));
                    if (match != default)
                        newLang = match.Item1;
                }

                if (newLang != null)
                {
                    LanguageManager.Speichern_Laden("Speichern", newLang);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✔ {LanguageManager.Get("settings.languageChanged").Replace("{lang}", newLang.ToUpper())}");
                    Console.ResetColor();
                    valid = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n" + LanguageManager.Get("settings.invalidSelection"));
                    Console.ResetColor();

                    Console.WriteLine("\n" + LanguageManager.Get("settings.pressAnyKey"));
                    Console.ReadKey(true);
                }

            } while (!valid);

            Console.WriteLine("\n" + LanguageManager.Get("settings.pressAnyKey"));
            Console.ReadKey(true);
            Console.Clear();
        }

        // Auswahl der Spielgeschwindigkeit
        static void ChangeDifficulty()
        {
            if (Spielvalues.DifficultyInt == 1)
            {
                Spielvalues.Difficulty = LanguageManager.Get("settings.difficulty.medium");
                Spielvalues.DifficultyInt = 2;
            }
            else if (Spielvalues.DifficultyInt == 2)
            {
                Spielvalues.Difficulty = LanguageManager.Get("settings.difficulty.fast");
                Spielvalues.DifficultyInt = 3;
            }
            else
            {
                Spielvalues.Difficulty = LanguageManager.Get("settings.difficulty.slow");
                Spielvalues.DifficultyInt = 1;
            } 
        }

        static void ChangeGamemode()
        {
            var modes = LanguageManager.GetArray("settings.gamemodes");
            bool valid = false;

            do
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine(LanguageManager.Get("settings.gamemodeHeaderTitle"));
                Console.WriteLine("╠════════════════════════════════════════════╣");

                for (int i = 0; i < modes.Length; i++)
                {
                    Console.Write($"║ ");
                    if (Spielvalues.Gamemode == modes[i])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"[{i + 1}] {modes[i],-25} ← {LanguageManager.Get("settings.current")}".PadRight(43));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"[{i + 1}] {modes[i],-39}");
                    }
                    Console.WriteLine("║");
                }

                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.Write(LanguageManager.Get("settings.gamemodePrompt"));

                string eingabe = Console.ReadLine()!;
                if (int.TryParse(eingabe, out int auswahl) && auswahl >= 1 && auswahl <= modes.Length)
                {
                    Spielvalues.Gamemode = modes[auswahl - 1];
                    Spielvalues.GamemodeInt = auswahl;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✔ {LanguageManager.Get("settings.gamemodeChanged")} {Spielvalues.Gamemode}");
                    Console.ResetColor();
                    valid = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n" + LanguageManager.Get("settings.invalidSelection"));
                    Console.ResetColor();
                }

                if (!valid)
                {
                    Console.WriteLine("\n" + LanguageManager.Get("settings.pressAnyKey"));
                    Console.ReadKey(true);
                }

            } while (!valid);

            Console.WriteLine("\n" + LanguageManager.Get("settings.pressAnyKey"));
            Console.ReadKey(true);
            Console.Clear();
        }

        static void ChangeMaxFutter()
        {
            bool valid = false;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("╔════════════════════════════════════════════╗");
                Console.WriteLine(LanguageManager.Get("settings.maxFoodHeaderTitle"));
                Console.WriteLine("╠════════════════════════════════════════════╣");
                Console.WriteLine(LanguageManager.Get("settings.maxFoodLimit").Replace("{limit}", GameData.MaxFutterconfig.ToString().PadRight(17)));
                Console.WriteLine("╚════════════════════════════════════════════╝");
                Console.Write(LanguageManager.Get("settings.maxFoodPrompt"));
                string input = Console.ReadLine()!;
                if (int.TryParse(input, out int wert) && wert > 0)
                {
                    if (wert > GameData.MaxFutterconfig)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n" + LanguageManager.Get("settings.maxFoodTooHigh").Replace("{limit}", GameData.MaxFutterconfig.ToString()));
                        Console.ResetColor();
                    }
                    else
                    {
                        Spielvalues.Maxfutter = wert;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n" + LanguageManager.Get("settings.maxFoodChanged").Replace("{value}", wert.ToString()));
                        Console.ResetColor();
                        valid = true;
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n" + LanguageManager.Get("settings.invalidInput"));
                    Console.ResetColor();
                }

                if (!valid)
                {
                    Console.WriteLine("\n" + LanguageManager.Get("settings.pressAnyKey"));
                    Console.ReadKey(true);
                }

            } while (!valid);

            Console.WriteLine("\n" + LanguageManager.Get("settings.pressAnyKey"));
            Console.ReadKey(true);
            Console.Clear();
        }

        static void ResetSpielstand()
        {
            Console.Clear();
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine(LanguageManager.Get("settings.resetPrompt").Replace("{count}", i.ToString()));
                string? input = Console.ReadLine()?.Trim().ToLower();
                Console.Clear();
                if (input != LanguageManager.Get("settings.resetConfirm"))
                {
                    Console.WriteLine(LanguageManager.Get("settings.resetCancelled"));
                    Console.ReadKey(true);
                    Console.Clear();
                    return;
                }
            }

            SpeicherSystem.Speichern_Laden("Zurücksetzen");
            Console.WriteLine(LanguageManager.Get("settings.resetDone"));
            Console.WriteLine(LanguageManager.Get("settings.pressAnyKey"));
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
