using Smake.Render;
using Smake.Speicher;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Menues
{
    public class Shop : RendernMenue
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
                    Console.Clear();
                    MenuTracker = 1;
                    ShopTab++; // Seitenwechsel
                    break;
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    MenuTracker = 1;
                    ShopTab--; // Seitenwechsel
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
                    int max = gesamtOptionen[ShopTab];
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

        int shopTab;
        public int ShopTab
        {
            get { return shopTab; }
            set
            {
                if (value != shopTab)
                {
                    if (value > 2)
                        shopTab = 0;
                    else if (value < 0)
                        shopTab = 2;
                    else
                        shopTab = value;
                }
            }
        }
        readonly int[] gesamtOptionen = new int[3];

        public Shop()
        {
            gesamtOptionen[0] = GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length - 3;
            gesamtOptionen[1] = GameData.Farben.Length;
            gesamtOptionen[2] = 6;

            Menueloop();
        }

        void Menueloop()
        {
            Sounds.Melodie(GameData.MusikDaten.Menue?.Shop ?? 0);
            MenuTracker = 1;
            ShopTab = 0;
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
            switch (ShopTab)
            {
                case 0:
                    if (MenuTracker == gesamtOptionen[ShopTab])
                    {
                        StopInputstream();
                    }

                    // Kauflogik Skins
                    if (MenuTracker + 1 < GameData.TailSkins.Length)
                    {
                        // Kauflogik TailSkins
                        if (!Menüsvalues.FreigeschaltetTail[MenuTracker + 1] &&
                            Spielstatus.Coins >= GameData.TailPreis[MenuTracker - 1] &&
                            Spielstatus.Level >= GameData.TailLevel[MenuTracker - 1])
                        {
                            Menüsvalues.FreigeschaltetTail[MenuTracker + 1] = true;
                            Spielstatus.Coins -= GameData.TailPreis[MenuTracker - 1];
                        }
                    }
                    else if (MenuTracker + 2 < GameData.TailSkins.Length + GameData.FoodSkins.Length)
                    {
                        // Kauflogik FoodSkins
                        int i = MenuTracker + 2 - GameData.TailSkins.Length;
                        int b = MenuTracker + 1 - GameData.TailSkins.Length;
                        if (!Menüsvalues.FreigeschaltetFood[i] &&
                            Spielstatus.Coins >= GameData.FoodPreis[b] &&
                            Spielstatus.Level >= GameData.FoodLevel[b])
                        {
                            Menüsvalues.FreigeschaltetFood[i] = true;
                            Spielstatus.Coins -= GameData.FoodPreis[b];
                        }
                    }
                    else if (MenuTracker + 3 < GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length)
                    {
                        // Kauflogik RandSkins
                        int i = MenuTracker + 3 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                        int b = MenuTracker + 2 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                        if (!Menüsvalues.FreigeschaltetRand[i] &&
                            Spielstatus.Coins >= GameData.RandPreis[b] &&
                            Spielstatus.Level >= GameData.RandLevel[b])
                        {
                            Menüsvalues.FreigeschaltetRand[i] = true;
                            Spielstatus.Coins -= GameData.RandPreis[b];
                        }
                    }
                    break;

                case 1:
                    if (MenuTracker == gesamtOptionen[ShopTab])
                    {
                        StopInputstream();
                    }

                    // Kauflogik Farben
                    if (!Menüsvalues.FreigeschaltetFarben[MenuTracker] &&
                             Spielstatus.Coins >= GameData.FarbenPreis[MenuTracker - 1] &&
                             Spielstatus.Level >= GameData.FarbenLevel[MenuTracker - 1])
                    {
                        Menüsvalues.FreigeschaltetFarben[MenuTracker] = true;
                        Spielstatus.Coins -= GameData.FarbenPreis[MenuTracker - 1];
                    }
                    break;

                case 2:
                    if (MenuTracker == gesamtOptionen[ShopTab])
                    {
                        StopInputstream();
                    }
                    break;
            }
        }

        void BuildMenu()
        {
            switch (ShopTab)
            {
                case 0:
                    Title = "Shop_Skins";
                    break;

                case 1:
                    Title = "Shop_Farben";
                    break;

                case 2:
                    Title = "Shop_Musik";
                    break;
            }
        }
    }
}
