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
                        ShopSkins = !ShopSkins; // Seitenwechsel
                        MenuTracker = 1;
                        break;
                    case ConsoleKey.Escape:
                        Menu menu = new();
                        Thread.CurrentThread.Join();
                        break;
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        SelectMenu();
                        break;
                }

                GameValue = BuildMenu();
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

                    selected = MenuTracker;
                }
            }
        }

        private bool ShopSkins = true;
        private readonly int gesamtOptionenSkins;
        private readonly int gesamtOptionenFarben;

        public Shop()
        {
            title = "Shop";
            gesamtOptionenSkins = GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length - 3;
            gesamtOptionenFarben = GameData.Farben.Length;

            Display = BuildDisplay();
            GameValue = BuildMenu();
            MenuTracker = 1;

            InitialRender();
            StartInputstream();
        }

        private void SelectMenu()
        {
            if (ShopSkins)
            {
                if (MenuTracker == gesamtOptionenSkins)
                {
                    Menu menu = new();
                    Thread.CurrentThread.Join();
                    return;
                }

                // Kauflogik Skins
                if (MenuTracker + 1 < GameData.TailSkins.Length)
                {
                    if (!Menüsvalues.freigeschaltetTail[MenuTracker + 1] &&
                        Spiellogik.coins >= GameData.TailPreis[MenuTracker - 1] &&
                        Spiellogik.level >= GameData.TailLevel[MenuTracker - 1])
                    {
                        Menüsvalues.freigeschaltetTail[MenuTracker + 1] = true;
                        Spiellogik.coins -= GameData.TailPreis[MenuTracker - 1];
                    }
                }
                else if (MenuTracker + 2 < GameData.TailSkins.Length + GameData.FoodSkins.Length)
                {
                    int i = MenuTracker + 2 - GameData.TailSkins.Length;
                    int b = MenuTracker + 1 - GameData.TailSkins.Length;
                    if (!Menüsvalues.freigeschaltetFood[i] &&
                        Spiellogik.coins >= GameData.FoodPreis[b] &&
                        Spiellogik.level >= GameData.FoodLevel[b])
                    {
                        Menüsvalues.freigeschaltetFood[i] = true;
                        Spiellogik.coins -= GameData.FoodPreis[b];
                    }
                }
                else if (MenuTracker + 3 < GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length)
                {
                    int i = MenuTracker + 3 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                    int b = MenuTracker + 2 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                    if (!Menüsvalues.freigeschaltetRand[i] &&
                        Spiellogik.coins >= GameData.RandPreis[b] &&
                        Spiellogik.level >= GameData.RandLevel[b])
                    {
                        Menüsvalues.freigeschaltetRand[i] = true;
                        Spiellogik.coins -= GameData.RandPreis[b];
                    }
                }
            }
            else
            {
                if (MenuTracker == gesamtOptionenFarben)
                {
                    Menu menu = new();
                    Thread.CurrentThread.Join();
                    return;
                }

                // Kauflogik Farben
                if (!Menüsvalues.freigeschaltetFarben[MenuTracker] &&
                    Spiellogik.coins >= GameData.FarbenPreis[MenuTracker - 1] &&
                    Spiellogik.level >= GameData.FarbenLevel[MenuTracker - 1])
                {
                    Menüsvalues.freigeschaltetFarben[MenuTracker] = true;
                    Spiellogik.coins -= GameData.FarbenPreis[MenuTracker - 1];
                }
            }
        }

        private string[] BuildDisplay()
        {
            if (ShopSkins)
            {
                List<string> items = new();
                items.AddRange(GameData.TailSkins.Select(c => $"Tail Skin {c}"));
                items.AddRange(GameData.FoodSkins.Select(c => $"Food Skin {c}"));
                items.AddRange(GameData.RandSkins.Select(c => $"Rand Skin {c}"));
                items.Add("Zurück zum Hauptmenü");
                return items.ToArray();
            }
            else
            {
                List<string> items = new();
                items.AddRange(GameData.Farben.Select(f => f.ToString()));
                items.Add("Zurück zum Hauptmenü");
                return items.ToArray();
            }
        }

        private object[] BuildMenu()
        {
            if (ShopSkins)
            {
                object[] values = new object[GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length];
                Array.Copy(GameData.TailSkins, 0, values, 0, GameData.TailSkins.Length);
                Array.Copy(GameData.FoodSkins, 0, values, GameData.TailSkins.Length, GameData.FoodSkins.Length);
                Array.Copy(GameData.RandSkins, 0, values, GameData.TailSkins.Length + GameData.FoodSkins.Length, GameData.RandSkins.Length);
                return values;
            }
            else
            {
                return GameData.Farben.Cast<object>().ToArray();
            }
        }
    }
}
