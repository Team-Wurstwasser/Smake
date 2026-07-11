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
            Console.WriteLine(LanguageSystem.Get("skins.title"));
            Console.WriteLine("══════════════════════════════");

            for (int i = 0; i < Display.Length; i++)
            {
                string zeiger = i + 1 == Selected ? ">>" : "  ";
                Console.Write($"{zeiger} ");

                string[] parts = Display[i].Split(["{item}"], StringSplitOptions.None);
                Console.Write(parts[0]);

                if (IsColor[i] && !RenderSpielfeld.Performancemode && GameValue[i] is ConsoleColor color)
                {
                    Console.ForegroundColor = color;
                    Console.Write(color);
                    Console.ResetColor();
                }
                else
                {
                    string val = (IsColor[i] && RenderSpielfeld.Performancemode)
                        ? LanguageSystem.Get("skins.performancemode")
                        : (GameValue[i]?.ToString() ?? "");
                    Console.Write(val);
                }

                if (parts.Length > 1) Console.Write(parts[1].PadRight(15));

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

                string shoptext = Spielstatus.Level < levels[shopItemIndex]
                    ? LanguageSystem.Get("shop.requiredLevel").Replace("{level}", levels[shopItemIndex].ToString())
                    : unlocked[i] ? LanguageSystem.Get("shop.unlocked")
                    : LanguageSystem.Get("shop.price").Replace("{price}", prices[shopItemIndex].ToString());

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
            Console.WriteLine($"\n{LanguageSystem.Get("shop.colors")}:");
            int option = 0;

            for (int i = 1; i < GameData.Farben.Length; i++, option++)
            {
                string shoptext = Spielstatus.Level < GameData.FarbenLevel[i - 1]
                    ? LanguageSystem.Get("shop.requiredLevel").Replace("{level}", GameData.FarbenLevel[i - 1].ToString())
                    : Menüsvalues.FreigeschaltetFarben[i] ? LanguageSystem.Get("shop.unlocked")
                    : LanguageSystem.Get("shop.price").Replace("{price}", GameData.FarbenPreis[i - 1].ToString());

                string zeiger = option + 1 == Selected ? ">>" : "  ";
                Console.ForegroundColor = GameData.Farben[i];
                Console.WriteLine($"{zeiger} {GameData.Farben[i],-12} {shoptext}".PadRight(50));
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