using Smake.io.Spiel;
using Smake.io.Speicher;
using System;
using System.Collections.Generic;

namespace Smake.io.Render
{
    public static class MenüRenderer
    {
        public static void DrawTitle()
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

        public static void RenderMainMenueOptions(int selected)
        {
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       SMAKE MAIN MENU        ║");
            Console.WriteLine("╠══════════════════════════════╣");

            ReadOnlySpan<string> optionen =
            [
                "Spiel starten",
                "Einstellungen",
                "Shop",
                "Skins/Farben",
                "Statistiken",
                "Anleitung",
                "Beenden"
            ];

            for (int i = 0; i < optionen.Length; i++)
            {
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {optionen[i],-25}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
        }

        static void RenderGenericMenu(string title, string[] options, int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(title);
            Console.WriteLine("══════════════════════════════");

            for (int i = 0; i < options.Length; i++)
            {
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {options[i]}");
            }

            Console.WriteLine("══════════════════════════════");
        }

        public static void RenderEinstellungen(int selected)
        {
            var optionen = new[]
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
            RenderGenericMenu("EINSTELLUNGEN", optionen, selected);
        }

        public static void RenderSkin_FarbenOptions(int selected)
        {
            Console.SetCursorPosition(0, 0);
            Console.ResetColor();
            Console.WriteLine("Skins/Farben");
            Console.WriteLine("══════════════════════════════");

            var options = new List<(string Text, object? Value, bool IsColor)>
            {
                ($"Player 1 Tailskin ändern    [Aktuell: ", Spiellogik.player.Skin, false),
                ($"Player 2 Tailskin ändern    [Aktuell: ", Spiellogik.player2.Skin, false),
                ($"Foodskin ändern             [Aktuell: ", Spiellogik.food, false),
                ($"Randskin ändern             [Aktuell: ", Spiellogik.rand, false),
                ($"Player 1 Farbe ändern       [Aktuell: ", Spiellogik.player.Headfarbe, true),
                ($"Player 2 Farbe ändern       [Aktuell: ", Spiellogik.player2.Headfarbe, true),
                ($"Player 1 Tailfarbe ändern   [Aktuell: ", Spiellogik.player.Farbe, true),
                ($"Player 2 Tailfarbe ändern   [Aktuell: ", Spiellogik.player2.Farbe, true),
                ($"Foodfarbe ändern            [Aktuell: ", Spiellogik.foodfarbe, true),
                ($"Randfarbe ändern            [Aktuell: ", Spiellogik.randfarbe, true),
                ("Zurück zum Hauptmenü", null, false)
             };

            for (int i = 0; i < options.Count; i++)
            {
                string zeiger = (i + 1 == selected) ? ">>" : "  ";
                var (Text, Value, IsColor) = options[i];

                Console.Write($"{zeiger} {Text}");

                if (Value != null)
                {
                    if (IsColor && !RendernSpielfeld.performancemode)
                    {
                        Console.ForegroundColor = (ConsoleColor)Value;
                        Console.Write(Value);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(RendernSpielfeld.performancemode && IsColor ? "Performance Mode AN" : Value);
                    }
                    Console.Write("]");
                }
                Console.WriteLine();
            }
            Console.WriteLine("══════════════════════════════");
        }

        static void RenderShopHeader()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("           Shop           ");
            Console.WriteLine($"Coins: {Spiellogik.coins}");
            Console.WriteLine($"Level: {Spiellogik.level}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
        }

        static int RenderShopSection(string title, int optionCounter, int selected, char[] items, int[] levels, bool[] unlocked, int[] prices, int startIndex = 1)
        {
            Console.WriteLine($"\n{title}:");
            for (int i = startIndex; i < items.Length; i++)
            {
                // Korrigiert den Index für Preis- und Level-Arrays basierend auf dem StartIndex
                int shopItemIndex = i - startIndex;

                string shoptext = Spiellogik.level < levels[shopItemIndex]
                    ? $"[Benötigtes Level: {levels[shopItemIndex]}]"
                    : unlocked[i] ? "[Freigeschaltet]" : $"[{prices[shopItemIndex]} Coins]";

                string zeiger = (optionCounter + 1 == selected) ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {items[i]} {shoptext}");
                optionCounter++;
            }
            return optionCounter;
        }

        public static void RenderShopSkinsOptions(int selected)
        {
            RenderShopHeader();
            int option = 0;

            option = RenderShopSection("Tail Skins", option, selected, GameData.TailSkins, GameData.TailLevel, Menüs.freigeschaltetTail, GameData.TailPreis, 2);

            option = RenderShopSection("Food Skins", option, selected, GameData.FoodSkins, GameData.FoodLevel, Menüs.freigeschaltetFood, GameData.FoodPreis);
            option = RenderShopSection("Rand Skins", option, selected, GameData.RandSkins, GameData.RandLevel, Menüs.freigeschaltetRand, GameData.RandPreis);

            string zeiger = (option + 1 == selected) ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        public static void RenderShopFarbenOptions(int selected)
        {
            RenderShopHeader();
            Console.WriteLine("\nFarben:");
            int option = 0;
            for (int i = 1; i < GameData.Farben.Length; i++, option++)
            {
                string shoptext = Spiellogik.level < GameData.FarbenLevel[i - 1]
                    ? $"[Benötigtes Level: {GameData.FarbenLevel[i - 1]}]"
                    : Menüs.freigeschaltetFarben[i] ? "[Freigeschaltet]" : $"[{GameData.FarbenPreis[i - 1]} Coins]";

                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.ForegroundColor = GameData.Farben[i];
                Console.WriteLine($"{zeiger} {GameData.Farben[i],-12} {shoptext}");
                Console.ResetColor();
            }

            string zeiger2 = (option + 1 == selected) ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        public static void RenderAnleitung()
        {
            Console.Clear();
            Console.WriteLine("ANLEITUNG");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Ziel: Iss so viele {Spiellogik.food} wie möglich!");
            Console.WriteLine("\nSteuerung:\n");
            Console.WriteLine("Spieler 1:\n  ↑ - Hoch\n  ← - Links\n  ↓ - Runter\n  → - Rechts\n");
            Console.WriteLine("Spieler 2:\n  W - Hoch\n  A - Links\n  S - Runter\n  D - Rechts\n");
            Console.WriteLine("Vermeide Kollisionen mit dir selbst oder dem Rand!");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
            Console.ReadKey();
        }

        public static void RenderStatistiken()
        {
            Console.Clear();
            Console.WriteLine("Statistiken\n ");
            Console.WriteLine("══════════════════════════════\n ");

            int punktefürLevel = Spiellogik.xp % 100;
            int balkenLänge = 20;
            int gefüllt = (punktefürLevel * balkenLänge) / 100;
            string bar = new string('█', gefüllt).PadRight(balkenLänge, '-');

            Console.WriteLine($"Level:                    {Spiellogik.level}");
            Console.WriteLine($"Fortschritt:              [{bar}] {punktefürLevel}/100\n");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine($"Gesamte Spiele:           {Menüs.spieleGesamt}");
            Console.WriteLine($"Höchste Punktzahl:        {Menüs.highscore}");
            Console.WriteLine($"Durchschnittliche XP:     {(Menüs.spieleGesamt > 0 ? Spiellogik.xp / Menüs.spieleGesamt : 0)}");
            Console.WriteLine($"Gesamte Coins:            {Menüs.gesamtcoins}");
            Console.WriteLine($"Aktuelle Coins:           {Spiellogik.coins}");
            Console.WriteLine("══════════════════════════════");
            Console.WriteLine("Drücke eine beliebige Taste, um zum Menü zurückzukehren...");
            Console.ReadKey();
        }
    }
}