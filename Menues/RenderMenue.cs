using Smake.Game;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Menues
{
    public abstract class RenderMenue
    {
        protected string[] Display = [];
        protected int Selected;
        protected string? Title;
        protected object?[] GameValue = [];
        protected bool[] IsColor = [];
        protected volatile bool DoReadInput = true;
        protected ConsoleKey Input;
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

                case "SkinColors":
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
            Console.WriteLine("""
     ██████  ███▄ ▄███▓ ▄▄▄        ██ ▄█▀▓█████
    ▒██    ▒ ▓██▒▀█▀ ██▒▒████▄     ██▄█▒ ▓█   ▀ 
    ░ ▓██▄   ▓██    ▓██░▒██  ▀█▄   ▓███▄░ ▒███   
      ▒   ██▒▒██    ▒██ ░██▄▄▄▄██ ▓██ █▄ ▒▓█  ▄ 
    ▒██████▒▒▒██▒   ░██▒ ▓█   ▓██▒▒██▒ █▄░▒████▒
    ▒ ▒▓▒ ▒ ░░ ▒░   ░  ░ ▒▒   ▓▒█░▒ ▒▒ ▓▒░░ ▒░ ░
    ░ ░▒  ░ ░░  ░      ░  ▒   ▒▒ ░░ ░▒ ▒░ ░ ░  ░
    ░  ░  ░  ░      ░      ░   ▒   ░ ░░ ░    ░   
          ░          ░          ░  ░░  ░      ░  ░
    """);
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
            Console.WriteLine(LanguageSystem.Get("skins.title"));
            Console.WriteLine("══════════════════════════════");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = i + 1 == Selected ? ">>" : "  ";
                Console.Write($"{zeiger} {Display[i]}");

                if (GameValue[i] != null)
                {
                    if (IsColor[i] && !Spielvalues.Performancemode || !IsColor[i])
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
                        Console.Write(Spielvalues.Performancemode ? LanguageSystem.Get("skins.performancemode") : GameValue[i]);
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
            Console.WriteLine(LanguageSystem.Get("shop.title").PadLeft(14));
            Console.WriteLine(LanguageSystem.Get("shop.coins").Replace("{coins}", Spielstatus.Coins.ToString()).PadRight(50));
            Console.WriteLine(LanguageSystem.Get("shop.level").Replace("{level}", Spielstatus.Level.ToString()).PadRight(50));
            Console.WriteLine("═══════════════════════════");
            Console.WriteLine(LanguageSystem.Get("shop.switchPage"));
        }

        void RenderShopFooter(int option)
        {
            string zeiger = option + 1 == Selected ? ">>" : "  ";
            Console.WriteLine($"\n{zeiger} {LanguageSystem.Get("shop.back")}");
            Console.WriteLine("══════════════════════════");
        }

        static int RenderShopSection(string titleKey, int optionCounter, int selected1, char[] items, int[] levels, bool[] unlocked, int[] prices, int startIndex = 1)
        {
            Console.WriteLine($"\n{LanguageSystem.Get("shop." + titleKey)}:");
            for (int i = startIndex; i < items.Length; i++)
            {
                int shopItemIndex = i - startIndex;

                string shoptext = Spielstatus.Level < levels[shopItemIndex] ? LanguageSystem.Get("shop.requiredLevel").Replace("{level}", levels[shopItemIndex].ToString()) : unlocked[i] ? LanguageSystem.Get("shop.unlocked") : LanguageSystem.Get("shop.price").Replace("{price}", prices[shopItemIndex].ToString());

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

            option = RenderShopSection("tailSkins", option, Selected, ConfigSystem.Skins.TailSkins, ConfigSystem.Levels.TailLevel, Menüsvalues.FreigeschaltetTail, ConfigSystem.Prices.TailPreis, 2);
            option = RenderShopSection("foodSkins", option, Selected, ConfigSystem.Skins.FoodSkins, ConfigSystem.Levels.FoodLevel, Menüsvalues.FreigeschaltetFood, ConfigSystem.Prices.FoodPreis);
            option = RenderShopSection("randSkins", option, Selected, ConfigSystem.Skins.RandSkins, ConfigSystem.Levels.RandLevel, Menüsvalues.FreigeschaltetRand, ConfigSystem.Prices.RandPreis);

            RenderShopFooter(option);
        }

        void RenderShopFarbenLayout()
        {
            RenderShopHeader();
            Console.WriteLine($"\n{LanguageSystem.Get("shop.colors")}:");
            int option = 0;

            for (int i = 1; i < ConfigSystem.Skins.Farben.Length; i++, option++)
            {
                string shoptext = Spielstatus.Level < ConfigSystem.Levels.FarbenLevel[i - 1] ? LanguageSystem.Get("shop.requiredLevel").Replace("{level}", ConfigSystem.Levels.FarbenLevel[i - 1].ToString()) : Menüsvalues.FreigeschaltetFarben[i] ? LanguageSystem.Get("shop.unlocked") : LanguageSystem.Get("shop.price").Replace("{price}", ConfigSystem.Prices.FarbenPreis[i - 1].ToString());
                
                string zeiger = option + 1 == Selected ? ">>" : "  ";
                Console.ForegroundColor = ConfigSystem.Skins.Farben[i];
                Console.WriteLine($"{zeiger} {ConfigSystem.Skins.Farben[i],-12} {shoptext}".PadRight(50));
                Console.ResetColor();
            }

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
