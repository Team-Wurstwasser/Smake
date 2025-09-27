using Smake.io.Values;
using Smake.io.Speicher;

namespace Smake.io.Render
{
    public class Screen
    {
        public string[] Display { get; set; }
        public int Selected { get; set; }
        public string Title { get; set; }
        public object?[] GameValue { get; set; }
        public bool[] IsColor { get; set; }
        public virtual ConsoleKey Input { get; set; }
        public bool DoReadInput { get; set; } = true;

        public Screen() 
        {
            Console.Clear();
        }

        public void InitialRender()
        {
            Musik.Melodie();
            Render();
        }

        public void Render()
        {
            Console.SetCursorPosition(0, 0);

            switch (Title)
            {
                case "Menü":
                    DrawTitle();
                    RenderMainMenuLayout();
                    break;

                case "Skin_Farben":
                    RenderSkinFarbenLayout();
                    break;

                case "Shop_Skins":
                    RenderShopSkinsLayout();
                    break;

                case "Shop_Farben":
                    RenderShopFarbenLayout();
                    break;

                default:
                    RenderDefaultLayout();
                    break;
            }
        }

        private void RenderDefaultLayout()
        {
            Console.WriteLine(Title);
            Console.WriteLine("══════════════════════════════");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = i + 1 == Selected ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {Display[i]}");
            }

            Console.WriteLine("══════════════════════════════");
        }

        static private void DrawTitle()
        {
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

        private void RenderMainMenuLayout()
        {
            Console.SetCursorPosition(0, 11);
            Console.WriteLine("╔══════════════════════════════╗");
            Console.WriteLine("║       SMAKE MAIN MENU        ║");
            Console.WriteLine("╠══════════════════════════════╣");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = i + 1 == Selected ? ">>" : "  ";
                Console.WriteLine($"║  {zeiger} {Display[i],-25}║");
            }

            Console.WriteLine("╚══════════════════════════════╝");
        }

        private void RenderSkinFarbenLayout()
        {
            Console.WriteLine("Skins/Farben");
            Console.WriteLine("══════════════════════════════");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = i + 1 == Selected ? ">>" : "  ";
                Console.Write($"{zeiger} {Display[i]}");

                if (GameValue[i] != null)
                {
                    if (IsColor[i] && !RendernSpielfeld.performancemode)
                    {
                        if (GameValue[i] is ConsoleColor color)
                        {
                            Console.ForegroundColor = color;
                            Console.Write(color);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.Write(GameValue[i]);
                        }
                    }
                    else
                    {
                        Console.Write(RendernSpielfeld.performancemode ? "Performance Mode AN" : GameValue[i]);
                    }

                    Console.Write("]".PadRight(13));
                }

                Console.WriteLine();
            }
            Console.WriteLine("══════════════════════════════");
        }

        private static void RenderShopHeader()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("           Shop           ");
            Console.WriteLine($"Coins: {Spielstatus.coins}");
            Console.WriteLine($"Level: {Spielstatus.level}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
        }

        private static int RenderShopSection(string title1, int optionCounter, int selected1, char[] items, int[] levels, bool[] unlocked, int[] prices, int startIndex = 1)
        {
            Console.WriteLine($"\n{title1}:");
            for (int i = startIndex; i < items.Length; i++)
            {
                int shopItemIndex = i - startIndex;

                string shoptext = Spielstatus.level < levels[shopItemIndex]
                    ? $"[Benötigtes Level: {levels[shopItemIndex]}]"
                    : unlocked[i] ? "[Freigeschaltet]" : $"[{prices[shopItemIndex]} Coins]";

                string zeiger = optionCounter + 1 == selected1 ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {items[i]} {shoptext}");
                optionCounter++;
            }
            return optionCounter;
        }

        private void RenderShopSkinsLayout()
        {
            RenderShopHeader();
            int option = 0;

            option = RenderShopSection("Tail Skins", option, Selected, GameData.TailSkins, GameData.TailLevel, Menüsvalues.freigeschaltetTail, GameData.TailPreis, 2);
            option = RenderShopSection("Food Skins", option, Selected, GameData.FoodSkins, GameData.FoodLevel, Menüsvalues.freigeschaltetFood, GameData.FoodPreis);
            option = RenderShopSection("Rand Skins", option, Selected, GameData.RandSkins, GameData.RandLevel, Menüsvalues.freigeschaltetRand, GameData.RandPreis);

            string zeiger = option + 1 == Selected ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        private void RenderShopFarbenLayout()
        {
            RenderShopHeader();
            Console.WriteLine("\nFarben:");
            int option = 0;
            for (int i = 1; i < GameData.Farben.Length; i++, option++)
            {
                string shoptext = Spielstatus.level < GameData.FarbenLevel[i - 1]
                    ? $"[Benötigtes Level: {GameData.FarbenLevel[i - 1]}]"
                    : Menüsvalues.freigeschaltetFarben[i] ? "[Freigeschaltet]" : $"[{GameData.FarbenPreis[i - 1]} Coins]";

                string zeiger = option + 1 == Selected ? ">>" : "  ";
                Console.ForegroundColor = GameData.Farben[i];
                Console.WriteLine($"{zeiger} {GameData.Farben[i],-12} {shoptext}");
                Console.ResetColor();
            }

            string zeiger2 = option + 1 == Selected ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        public void StartInputstream()
        {
            Thread InputThread = new(Readinput);
            InputThread.Start();
        }

        private void Readinput()
        {
            while (DoReadInput)
            {
                if (Console.KeyAvailable)
                {
                    Input = Console.ReadKey().Key;
                }
            }
            Thread.CurrentThread.Join();
        }
    }
}
