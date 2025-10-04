using Smake.Values;
using Smake.Render;
using Smake.Speicher;

namespace Smake.Menues
{
    public class Shop : RendernMenue
    {
        public void ProcessInput()
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
            gesamtOptionenSkins = GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length - 3;
            gesamtOptionenFarben = GameData.Farben.Length;

            Menueloop();
        }

        private void Menueloop()
        {
            Musik.Currentmusik = GameData.MusikDaten.Menue.Shop;
            MenuTracker = 1;
            BuildMenu();
            InitialRender();
            StartInputstream();
            while (DoReadInput)
            {
                ProcessInput();
                Thread.Sleep(5); // kleine Pause, CPU schonen
            }
            _ = new Menue();
        }

        private void SelectMenu()
        {
            if (ShopSkins)
            {
                if (MenuTracker == gesamtOptionenSkins)
                {
                    StopInputstream();
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
