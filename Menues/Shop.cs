using Smake.Values;
using Smake.Render;
using Smake.Speicher;

namespace Smake.Menues
{
    public class Shop : RendernMenue
    {
        private ConsoleKey input;
        public override ConsoleKey Input
        {
            get { return input; }
            set
            {
                input = value;

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
                        ShopSkins = !ShopSkins; // Seitenwechsel
                        break;
                    case ConsoleKey.Escape:
                        _ = new Menue();
                        StopInputstream();
                        Thread.Sleep(5);
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        SelectMenu();
                        break;
                }
                BuildMenu();
                Render();
            }
        }

        private int menuTracker;
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

        private bool ShopSkins;
        private readonly int gesamtOptionenSkins;
        private readonly int gesamtOptionenFarben;

        public Shop()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Shop;

            gesamtOptionenSkins = GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length - 3;
            gesamtOptionenFarben = GameData.Farben.Length;

            MenuTracker = 1;
            BuildMenu();
            InitialRender();
            StartInputstream();
        }

        private void SelectMenu()
        {
            if (ShopSkins)
            {
                if (MenuTracker == gesamtOptionenSkins)
                {
                    _ = new Menue();
                    StopInputstream();
                    Thread.Sleep(5);
                }

                // Kauflogik Skins
                if (MenuTracker + 1 < GameData.TailSkins.Length)
                {
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
            }
            else
            {
                if (MenuTracker == gesamtOptionenFarben)
                {
                    _ = new Menue();
                    StopInputstream();
                    Thread.Sleep(5);
                }

                // Kauflogik Farben
                if (!Menüsvalues.FreigeschaltetFarben[MenuTracker] &&
                    Spielstatus.Coins >= GameData.FarbenPreis[MenuTracker - 1] &&
                    Spielstatus.Level >= GameData.FarbenLevel[MenuTracker - 1])
                {
                    Menüsvalues.FreigeschaltetFarben[MenuTracker] = true;
                    Spielstatus.Coins -= GameData.FarbenPreis[MenuTracker - 1];
                }
            }
        }

        private  void BuildMenu()
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
