using Smake.io.Spiel;
using Smake.io.Speicher;
using Smake.io.Menus;

namespace Smake.io.Render
{
    public static class MenüRenderer
    {
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
                ($"Foodfarbe ändern            [Aktuell: ", Spiellogik.foodfarbeRandom ? "Random" : (object)Spiellogik.foodfarbe, !Spiellogik.foodfarbeRandom),
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
                        Console.Write(RendernSpielfeld.performancemode ? "Performance Mode AN" : Value);
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

            option = RenderShopSection("Tail Skins", option, selected, GameData.TailSkins, GameData.TailLevel, Menüsvalues.freigeschaltetTail, GameData.TailPreis, 2);

            option = RenderShopSection("Food Skins", option, selected, GameData.FoodSkins, GameData.FoodLevel, Menüsvalues.freigeschaltetFood, GameData.FoodPreis);
            option = RenderShopSection("Rand Skins", option, selected, GameData.RandSkins, GameData.RandLevel, Menüsvalues.freigeschaltetRand, GameData.RandPreis);

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
                    : Menüsvalues.freigeschaltetFarben[i] ? "[Freigeschaltet]" : $"[{GameData.FarbenPreis[i - 1]} Coins]";

                string zeiger = (option + 1 == selected) ? ">>" : "  ";
                Console.ForegroundColor = GameData.Farben[i];
                Console.WriteLine($"{zeiger} {GameData.Farben[i],-12} {shoptext}");
                Console.ResetColor();
            }

            string zeiger2 = (option + 1 == selected) ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }
    }
}