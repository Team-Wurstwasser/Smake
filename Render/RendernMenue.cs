using Smake.Values;
using Smake.Speicher;

namespace Smake.Render
{
    public class RendernMenue
    {
        public string[] Display { private get; set; } = [];
        public int Selected { private get; set; }
        public string? Title { private get; set; }
        public object?[] GameValue { private get; set; } = [];
        public bool[] IsColor { private get; set; } = [];
        public virtual ConsoleKey Input { get; set; }
        bool DoReadInput = true;
        Thread? InputThread;

        public void InitialRender() 
        {
            Musik.Melodie();
            Console.Clear();
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
                Console.WriteLine($"{zeiger} {Display[i],-70}");
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
                    if (IsColor[i] && !RendernSpielfeld.Performancemode || !IsColor[i])
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
                        Console.Write(RendernSpielfeld.Performancemode ? "Performance Mode AN" : GameValue[i]);
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
            Console.WriteLine($"Coins: {Spielstatus.Coins,-50}");
            Console.WriteLine($"Level: {Spielstatus.Level,-50}");
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine("←  Wechsle die Shopseite  →");
        }

        private static int RenderShopSection(string title1, int optionCounter, int selected1, char[] items, int[] levels, bool[] unlocked, int[] prices, int startIndex = 1)
        {
            Console.WriteLine($"\n{title1}:");
            for (int i = startIndex; i < items.Length; i++)
            {
                int shopItemIndex = i - startIndex;

                string shoptext = Spielstatus.Level < levels[shopItemIndex]
                    ? $"[Benötigtes Level: {levels[shopItemIndex]}]"
                    : unlocked[i] ? "[Freigeschaltet]" : $"[{prices[shopItemIndex]} Coins]";

                string zeiger = optionCounter + 1 == selected1 ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {items[i]} {shoptext}".PadRight(50));
                optionCounter++;
            }
            return optionCounter;
        }

        private void RenderShopSkinsLayout()
        {
            RenderShopHeader();
            int option = 0;

            option = RenderShopSection("Tail Skins", option, Selected, GameData.TailSkins, GameData.TailLevel, Menüsvalues.FreigeschaltetTail, GameData.TailPreis, 2);
            option = RenderShopSection("Food Skins", option, Selected, GameData.FoodSkins, GameData.FoodLevel, Menüsvalues.FreigeschaltetFood, GameData.FoodPreis);
            option = RenderShopSection("Rand Skins", option, Selected, GameData.RandSkins, GameData.RandLevel, Menüsvalues.FreigeschaltetRand, GameData.RandPreis);

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
                string shoptext = Spielstatus.Level < GameData.FarbenLevel[i - 1]
                    ? $"[Benötigtes Level: {GameData.FarbenLevel[i - 1]}]"
                    : Menüsvalues.FreigeschaltetFarben[i] ? "[Freigeschaltet]" : $"[{GameData.FarbenPreis[i - 1]} Coins]";

                string zeiger = option + 1 == Selected ? ">>" : "  ";
                Console.ForegroundColor = GameData.Farben[i];
                Console.WriteLine($"{zeiger} {GameData.Farben[i],-12} {shoptext}".PadRight(50));
                Console.ResetColor();
            }

            string zeiger2 = option + 1 == Selected ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger2} Zurück zum Hauptmenü");
            Console.WriteLine("══════════════════════════");
        }

        public void StartInputstream()
        {
            InputThread = new(Readinput);
            InputThread.Start();
        }

        public void StopInputstream()
        {
            DoReadInput = false;
            InputThread?.Join();
        }

        private void Readinput()
        {
            while (DoReadInput)
            {
                if (Console.KeyAvailable)
                {
                    Input = Console.ReadKey(true).Key;
                }
            }
        }
    }
}
