using Smake.Speicher;
using Smake.Values;

namespace Smake.Render
{
    public abstract class RendernMenue
    {
        protected string[] Display { private get; set; } = [];
        protected int Selected { private get; set; }
        protected string? Title { private get; set; }
        protected object?[] GameValue { private get; set; } = [];
        protected  bool[] IsColor { private get; set; } = [];
        protected volatile bool DoReadInput = true;
        protected ConsoleKey Input { get; set; }
        Thread? InputThread;

        protected void InitialRender()
        {
            Console.Clear();
            Render();
        }

        protected void Render()
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

                case "Shop_Musik":
                    RenderShopMusikLayout();
                    break;

                default:
                    RenderDefaultLayout();
                    break;
            }
        }

        void RenderDefaultLayout()
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

        static void DrawTitle()
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

        void RenderMainMenuLayout()
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

        void RenderSkinFarbenLayout()
        {
            Console.WriteLine(LanguageManager.Get("skins.title"));
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
                        Console.Write(RendernSpielfeld.Performancemode ? LanguageManager.Get("skins.performancemode") : GameValue[i]);
                    }

                    Console.Write("]".PadRight(13));
                }

                Console.WriteLine();
            }
            Console.WriteLine("══════════════════════════════");
        }

        static void RenderShopHeader()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(LanguageManager.Get("shop.title").PadLeft(14));
            Console.WriteLine(LanguageManager.Get("shop.coins").Replace("{coins}", Spielstatus.Coins.ToString()).PadRight(50));
            Console.WriteLine(LanguageManager.Get("shop.level").Replace("{level}", Spielstatus.Level.ToString()).PadRight(50));
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine(LanguageManager.Get("shop.switchPage"));
        }

        void RenderShopFooter(int option)
        {
            string zeiger = option + 1 == Selected ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger} {LanguageManager.Get("shop.back")}");
            Console.WriteLine("══════════════════════════");
        }

        static int RenderShopSection(string titleKey, int optionCounter, int selected1, char[] items, int[] levels, bool[] unlocked, int[] prices, int startIndex = 1)
        {
            Console.WriteLine($"\n{LanguageManager.Get("shop." + titleKey)}:");
            for (int i = startIndex; i < items.Length; i++)
            {
                int shopItemIndex = i - startIndex;

                string shoptext = Spielstatus.Level < levels[shopItemIndex]
                    ? LanguageManager.Get("shop.requiredLevel").Replace("{level}", levels[shopItemIndex].ToString())
                    : unlocked[i] ? LanguageManager.Get("shop.unlocked")
                    : LanguageManager.Get("shop.price").Replace("{price}", prices[shopItemIndex].ToString());

                string zeiger = optionCounter + 1 == selected1 ? ">>" : "  ";
                Console.WriteLine($"{zeiger} {items[i]} {shoptext}".PadRight(50));
                optionCounter++;
            }
            return optionCounter;
        }


        void RenderShopSkinsLayout()
        {
            RenderShopHeader();
            int option = 0;

            option = RenderShopSection("tailSkins", option, Selected, GameData.TailSkins, GameData.TailLevel, Menüsvalues.FreigeschaltetTail, GameData.TailPreis, 2);
            option = RenderShopSection("foodSkins", option, Selected, GameData.FoodSkins, GameData.FoodLevel, Menüsvalues.FreigeschaltetFood, GameData.FoodPreis);
            option = RenderShopSection("randSkins", option, Selected, GameData.RandSkins, GameData.RandLevel, Menüsvalues.FreigeschaltetRand, GameData.RandPreis);

            RenderShopFooter(option);
        }

        void RenderShopFarbenLayout()
        {
            RenderShopHeader();
            Console.WriteLine($"\n{LanguageManager.Get("shop.colors")}:");
            int option = 0;

            for (int i = 1; i < GameData.Farben.Length; i++, option++)
            {
                string shoptext = Spielstatus.Level < GameData.FarbenLevel[i - 1]
                    ? LanguageManager.Get("shop.requiredLevel").Replace("{level}", GameData.FarbenLevel[i - 1].ToString())
                    : Menüsvalues.FreigeschaltetFarben[i] ? LanguageManager.Get("shop.unlocked")
                    : LanguageManager.Get("shop.price").Replace("{price}", GameData.FarbenPreis[i - 1].ToString());

                string zeiger = option + 1 == Selected ? ">>" : "  ";
                Console.ForegroundColor = GameData.Farben[i];
                Console.WriteLine($"{zeiger} {GameData.Farben[i],-12} {shoptext}".PadRight(50));
                Console.ResetColor();
            }
            RenderShopFooter(option);
        }

        private void RenderShopMusikLayout()
        {
            RenderShopHeader();
            int option = 0;

            option = RenderShopSection("tailSkins", option, Selected, GameData.TailSkins, GameData.TailLevel, Menüsvalues.FreigeschaltetTail, GameData.TailPreis, 2);

            RenderShopFooter(option);
        }

        public void StartInputstream()
        {
            InputThread = new(ReadInput);
            InputThread.Start();
        }

        public void StopInputstream()
        {
            DoReadInput = false;
            InputThread?.Join();
        }

        void ReadInput()
        {
            while (DoReadInput)
            {
                if (Console.KeyAvailable)
                {
                    Input = Console.ReadKey(true).Key;
                }
                else
                {
                    Thread.Sleep(10); // CPU schonen
                }
            }
        }
    }
}
