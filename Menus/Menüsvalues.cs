using Smake.io.Spiel;
using Smake.io.Speicher;
using Smake.io.Render;

namespace Smake.io.Menus
{
    public class Menüsvalues
    {
        //Freigeschalteneskins/farben
        public static bool[] freigeschaltetTail = new bool[GameData.TailSkins.Length];
        public static bool[] freigeschaltetFood = new bool[GameData.FoodSkins.Length];
        public static bool[] freigeschaltetRand = new bool[GameData.RandSkins.Length];
        public static bool[] freigeschaltetFarben = new bool[GameData.Farben.Length];

        // Statistik
        public static int spieleGesamt;
        public static int highscore;
        public static int gesamtcoins;

        // Shop - Menü im Hauptmenü
        static void Shop()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Shop;
            Console.Clear();
            bool menu = true;
            int auswahl = 1;

            // Zähle alle Shop-Optionen zusammen
            int gesamtOptionenSkins = GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length - 3;
            int gesamtOptionenFarben = GameData.Farben.Length;

            bool Shopskins = false;
            do
            {
                if (Shopskins)
                {
                    if (auswahl < 1) auswahl = gesamtOptionenSkins;
                    if (auswahl > gesamtOptionenSkins) auswahl = 1;

                    MenüRenderer.RenderShopSkinsOptions(auswahl);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            auswahl--;
                            break;

                        case ConsoleKey.DownArrow:
                            auswahl++;
                            break;
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.LeftArrow:
                            Console.Clear();
                            Shopskins = false;
                            break;
                        case ConsoleKey.Escape:
                            menu = false;
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Spacebar:
                            Console.Clear();
                            if (auswahl == gesamtOptionenSkins)
                            {
                                menu = false; // Zurück zum Hauptmenü
                                break;
                            }

                            // Kauflogik für Skins
                            else if (auswahl + 1 < GameData.TailSkins.Length)
                            {
                                if (!freigeschaltetTail[auswahl + 1] && Spiellogik.coins >= GameData.TailPreis[auswahl - 1] && Spiellogik.level >= GameData.TailLevel[auswahl - 1])
                                {
                                    freigeschaltetTail[auswahl + 1] = true;
                                    Spiellogik.coins -= GameData.TailPreis[auswahl - 1];
                                }
                            }
                            else if (auswahl + 2 < GameData.TailSkins.Length + GameData.FoodSkins.Length)
                            {
                                int i = auswahl + 2 - GameData.TailSkins.Length;
                                int b = auswahl + 1 - GameData.TailSkins.Length;
                                if (!freigeschaltetFood[i] && Spiellogik.coins >= GameData.FoodPreis[b] && Spiellogik.level >= GameData.FoodLevel[b])
                                {
                                    freigeschaltetFood[i] = true;
                                    Spiellogik.coins -= GameData.FoodPreis[b];
                                }
                            }
                            else if (auswahl + 3 < GameData.TailSkins.Length + GameData.FoodSkins.Length + GameData.RandSkins.Length)
                            {
                                int i = auswahl + 3 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                                int b = auswahl + 2 - GameData.TailSkins.Length - GameData.FoodSkins.Length;
                                if (!freigeschaltetRand[i] && Spiellogik.coins >= GameData.RandPreis[b] && Spiellogik.level >= GameData.RandLevel[b])
                                {
                                    freigeschaltetRand[i] = true;
                                    Spiellogik.coins -= GameData.RandPreis[b];
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (auswahl < 1) auswahl = gesamtOptionenFarben;
                    if (auswahl > gesamtOptionenFarben) auswahl = 1;

                    MenüRenderer.RenderShopFarbenOptions(auswahl);

                    while (Console.KeyAvailable)
                        Console.ReadKey(true);

                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow:
                            auswahl--;
                            break;

                        case ConsoleKey.DownArrow:
                            auswahl++;
                            break;
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.LeftArrow:
                            Console.Clear();
                            Shopskins = true;
                            break;
                        case ConsoleKey.Escape:
                            menu = false;
                            break;
                        case ConsoleKey.Enter:
                        case ConsoleKey.Spacebar:
                            Console.Clear();
                            if (auswahl == gesamtOptionenFarben)
                            {
                                menu = false;
                            }
                            else if (!freigeschaltetFarben[auswahl] && Spiellogik.coins >= GameData.FarbenPreis[auswahl - 1] && Spiellogik.level >= GameData.FarbenLevel[auswahl - 1])
                            {
                                freigeschaltetFarben[auswahl] = true;
                                Spiellogik.coins -= GameData.FarbenPreis[auswahl - 1];
                            }
                            break;

                    }
                }


            } while (menu);
        }

        // Logik für Skin und Farben menü
        static void Skin_Farben()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.SkinFarben;
            Console.Clear();

            bool menu = true;
            int Skin_FarbenAuswahl = 1;

            while (menu)
            {
                // Menü rendern
                MenüRenderer.RenderSkin_FarbenOptions(Skin_FarbenAuswahl);

                // Warten auf Eingabe
                var keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        Skin_FarbenAuswahl--;
                        if (Skin_FarbenAuswahl < 1) Skin_FarbenAuswahl = 11;
                        break;

                    case ConsoleKey.DownArrow:
                        Skin_FarbenAuswahl++;
                        if (Skin_FarbenAuswahl > 11) Skin_FarbenAuswahl = 1;
                        break;

                    case ConsoleKey.Escape:
                        menu = false;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        switch (Skin_FarbenAuswahl)
                        {
                            case 1: WechselSkin(ref Spiellogik.player.Skin, GameData.TailSkins, freigeschaltetTail, Spiellogik.player2.Skin); break;
                            case 2: WechselSkin(ref Spiellogik.player2.Skin, GameData.TailSkins, freigeschaltetTail, Spiellogik.player.Skin); break;
                            case 3: WechselSkin(ref Spiellogik.food, GameData.FoodSkins, freigeschaltetFood); break;
                            case 4: WechselSkin(ref Spiellogik.rand, GameData.RandSkins, freigeschaltetRand); break;
                            case 5: WechselFarbe(ref Spiellogik.player.Headfarbe); break;
                            case 6: WechselFarbe(ref Spiellogik.player2.Headfarbe); break;
                            case 7: WechselFarbe(ref Spiellogik.player.Farbe); break;
                            case 8: WechselFarbe(ref Spiellogik.player2.Farbe); break;
                            case 9: WechselFarbe(ref Spiellogik.foodfarbe, true); break;
                            case 10: WechselFarbe(ref Spiellogik.randfarbe); break;
                            case 11: menu = false; break; // Zurück
                        }

                        break;
                }
            }
        }

        // Helper für Tail/Food/Rand
        static void WechselSkin(ref char aktuellesSkin, char[] skins, bool[] freigeschaltet, char? verboteneSkin = null)
        {
            if (skins.Length == 0) return;
            int idx = Array.IndexOf(skins, aktuellesSkin);
            int start = idx;
            do
            {
                idx = (idx + 1) % skins.Length;
            } while ((!freigeschaltet[idx] || verboteneSkin.HasValue && skins[idx] == verboteneSkin.Value) && idx != start);
            aktuellesSkin = skins[idx];
        }

        // Helper für Farben
        static void WechselFarbe(ref ConsoleColor aktuelleFarbe, bool isFood = false)
        {
            if (GameData.Farben.Length == 0) return;

            // Nächste freigeschaltete Farbe suchen
            int idx = Array.IndexOf(GameData.Farben, aktuelleFarbe);
            int start = idx;
            do
            {
                idx = (idx + 1) % GameData.Farben.Length;
            } while (!freigeschaltetFarben[idx] && idx != start);

            aktuelleFarbe = GameData.Farben[idx];

            if (isFood)
            {
                int lastIndex = -1;

                for (int i = 0; i < freigeschaltetFarben.Length; i++)
                {
                    if (freigeschaltetFarben[i])
                    {
                        lastIndex = i; // letzte Position merken
                    }
                }

                // Nur für foodfarbe: Random aktivieren, wenn letzte freigeschaltete Farbe erreicht
                if (!Spiellogik.foodfarbeRandom)
                {
                    if (lastIndex == idx)
                    {
                        Spiellogik.foodfarbeRandom = true;
                    }
                }
                else
                {
                    Spiellogik.foodfarbeRandom = false;
                }
            }
        }
    }
}