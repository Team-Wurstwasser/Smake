using Smake.Speicher;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Menues
{
    public class Shop : RenderMenue
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
                case ConsoleKey.RightArrow:
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    MenuTracker = 1;
                    ShopSkins = !ShopSkins; // Seitenwechsel
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
            BuildMenu();
            Render();
        }

        int menuTracker;
        public int MenuTracker
        {
            get { return menuTracker; }
            set
            {
                if (value != menuTracker)
                {
                    int max = ShopSkins ? gesamtOptionenSkins : gesamtOptionenFarben;
                    if (value > max)
                        menuTracker = 1;
                    else if (value < 1)
                        menuTracker = max;
                    else
                        menuTracker = value;

                    Selected = MenuTracker;
                }
            }
        }

        bool ShopSkins;
        readonly int gesamtOptionenSkins;
        readonly int gesamtOptionenFarben;

        public Shop()
        {
            gesamtOptionenSkins = ConfigSystem.Skins.Tail.Length + ConfigSystem.Skins.Food.Length + ConfigSystem.Skins.Rand.Length - 3;
            gesamtOptionenFarben = ConfigSystem.Skins.Farben.Length;

            Menueloop();
        }

        void Menueloop()
        {
            Sounds.Melodie(ConfigSystem.Sounds.Musik.Menue.Shop);
            MenuTracker = 1;
            BuildMenu();
            InitialRender();
            StartInputstream();
            while (DoReadInput)
            {
                ProcessInput();
                Thread.Sleep(5); // kleine Pause, CPU schonen
            }
            Program.CurrentView = ViewType.MainMenu;
        }

        void SelectMenu()
        {
            if (ShopSkins)
            {
                if (MenuTracker == gesamtOptionenSkins)
                {
                    StopInputstream();
                }

                // Kauflogik Skins
                if (MenuTracker + 1 < ConfigSystem.Skins.Tail.Length)
                {
                    if (!Menüsvalues.FreigeschaltetTail[MenuTracker + 1] && Spielstatus.Coins >= ConfigSystem.Prices.Tail[MenuTracker - 1] && Spielstatus.Level >= ConfigSystem.Levels.Tail[MenuTracker - 1])
                    {
                        Menüsvalues.FreigeschaltetTail[MenuTracker + 1] = true;
                        Spielstatus.Coins -= ConfigSystem.Prices.Tail[MenuTracker - 1];
                    }
                }
                else if (MenuTracker + 2 < ConfigSystem.Skins.Tail.Length + ConfigSystem.Skins.Food.Length)
                {
                    int i = MenuTracker + 2 - ConfigSystem.Skins.Tail.Length;
                    int b = MenuTracker + 1 - ConfigSystem.Skins.Tail.Length;
                    if (!Menüsvalues.FreigeschaltetFood[i] && Spielstatus.Coins >= ConfigSystem.Prices.Food[b] && Spielstatus.Level >= ConfigSystem.Levels.Food[b])
                    {
                        Menüsvalues.FreigeschaltetFood[i] = true;
                        Spielstatus.Coins -= ConfigSystem.Prices.Food[b];
                    }
                }
                else if (MenuTracker + 3 < ConfigSystem.Skins.Tail.Length + ConfigSystem.Skins.Food.Length + ConfigSystem.Skins.Rand.Length)
                {
                    int i = MenuTracker + 3 - ConfigSystem.Skins.Tail.Length - ConfigSystem.Skins.Food.Length;
                    int b = MenuTracker + 2 - ConfigSystem.Skins.Tail.Length - ConfigSystem.Skins.Food.Length;
                    if (!Menüsvalues.FreigeschaltetRand[i] && Spielstatus.Coins >= ConfigSystem.Prices.Rand[b] && Spielstatus.Level >= ConfigSystem.Prices.Rand[b])
                    {
                        Menüsvalues.FreigeschaltetRand[i] = true;
                        Spielstatus.Coins -= ConfigSystem.Prices.Rand[b];
                    }
                }
            }
            else
            {
                if (MenuTracker == gesamtOptionenFarben)
                {
                    StopInputstream();
                    
                }

                // Kauflogik Farben
                else if (!Menüsvalues.FreigeschaltetFarben[MenuTracker] && Spielstatus.Coins >= ConfigSystem.Prices.Farben[MenuTracker - 1] && Spielstatus.Level >= ConfigSystem.Levels.Farben[MenuTracker - 1])
                {
                    Menüsvalues.FreigeschaltetFarben[MenuTracker] = true;
                    Spielstatus.Coins -= ConfigSystem.Prices.Farben[MenuTracker - 1];
                }
            }
        }

        void BuildMenu()
        {
            if (ShopSkins)
            {
                Title = "Shop_Skins";
            }
            else
            {
                Title = "Shop_Farben";
            }
        }

    }
}
