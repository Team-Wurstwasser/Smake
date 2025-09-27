using Smake.io.Values;
using Smake.io.Render;
using Smake.io.Speicher;
using Smake.io.Spiel;

namespace Smake.io.Menus
{
    public class Shop : Screen
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
                        _ = new Menu();
                        DoReadInput = false;
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

        private bool ShopSkins = true;
        private readonly int gesamtOptionenSkins;
        private readonly int gesamtOptionenFarben;

        public Shop()
        {
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
                    _ = new Menu();
                    DoReadInput = false;
                    Thread.Sleep(5);
                }

                // Kauflogik Skins
                if (MenuTracker + 1 < GameData.TailSkins.Length)
                {
                    if (!Menüsvalues.freigeschaltetTail[MenuTracker + 1] &&
                        Spielstatus.coins >= GameData.TailPreis[MenuTracker - 1] &&
                        Spielstatus.level >= GameData.TailLevel[MenuTracker - 1])
                    {
                        Menüsvalues.freigeschaltetTail[MenuTracker + 1] = true;
                        Spielstatus.coins -= GameData.TailPreis[MenuTracker - 1];
                    }
                }
                else if (MenuTracker + 2 < GameData.TailSkins.Length + GameData.FoodSkins.Length)
                {
                    int i = MenuTracker + 2 - GameData.TailSkins.Length;
                    int b = MenuTracker + 1 - GameData.TailSkins.Length;
                    if (!Menüsvalues.freigeschaltetFood[i] &&
                        Spielstatus.coins >= GameData.FoodPreis[b] &&
                        Spielstatus.level >= GameData.FoodLevel[b])
                    {
                        Menüsvalues.freigeschaltetFood[i] = true;
                        Spielstatus.coins -= GameData.FoodPreis[b];
                    }
                }
                else if (MenuTracker + 3 < GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length)
                {
                    int i = MenuTracker + 3 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                    int b = MenuTracker + 2 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                    if (!Menüsvalues.freigeschaltetRand[i] &&
                        Spielstatus.coins >= GameData.RandPreis[b] &&
                        Spielstatus.level >= GameData.RandLevel[b])
                    {
                        Menüsvalues.freigeschaltetRand[i] = true;
                        Spielstatus.coins -= GameData.RandPreis[b];
                    }
                }
            }
            else
            {
                if (MenuTracker == gesamtOptionenFarben)
                {
                    _ = new Menu();
                    DoReadInput = false;
                    Thread.Sleep(5);
                }

                // Kauflogik Farben
                if (!Menüsvalues.freigeschaltetFarben[MenuTracker] &&
                    Spielstatus.coins >= GameData.FarbenPreis[MenuTracker - 1] &&
                    Spielstatus.level >= GameData.FarbenLevel[MenuTracker - 1])
                {
                    Menüsvalues.freigeschaltetFarben[MenuTracker] = true;
                    Spielstatus.coins -= GameData.FarbenPreis[MenuTracker - 1];
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
