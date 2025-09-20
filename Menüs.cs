using Smake.io.Render;
using Smake.io.Spiel;
using Smake.io.Speicher;

namespace Smake.io
{
    public class Menüs
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

        // Eingaben für Spielernamen
        public static void Eingaben()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Eingabe;

            Console.Clear();
            Console.Write("Spieler 1, gib deinen Namen ein: ");
            Spiellogik.player.Name = Console.ReadLine();
            Console.Clear();

            Console.Clear();
            Console.Write("Spieler 2, gib deinen Namen ein: ");
            Spiellogik.player2.Name = Console.ReadLine();
            Console.Clear();
        }

        //Logik des Mainmenüs
        public static void ShowMainMenue()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Main;

            SpeicherSystem.Speichern_Laden("Speichern");

            // Level-Berechnung (1 Level pro 100 XP)
            Spiellogik.level = Spiellogik.xp / 100 + 1;

            if (RendernSpielfeld.performancemode)
            {
                Spiellogik.foodfarbe = ConsoleColor.White;
                Spiellogik.randfarbe = ConsoleColor.White;
                Spiellogik.player.Farbe = ConsoleColor.White;
                Spiellogik.player.Headfarbe = ConsoleColor.White;
                Spiellogik.player2.Farbe = ConsoleColor.White; ;
                Spiellogik.player2.Headfarbe = ConsoleColor.White;
            }

            Console.Clear();
            MenüRenderer.DrawTitle();
            bool menu = true;
            int MenueOptions = 1;

            do
            {
                Console.SetCursorPosition(0, 11);
                MenüRenderer.RenderMainMenueOptions(MenueOptions);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        MenueOptions--;
                        if (MenueOptions < 1) MenueOptions = 7;
                        break;

                    case ConsoleKey.DownArrow:
                        MenueOptions++;
                        if (MenueOptions > 7) MenueOptions = 1;
                        break;

                    case ConsoleKey.Spacebar:
                    case ConsoleKey.Enter:
                        menu = false;
                        break;
                }

            } while (menu);

            switch (MenueOptions)
            {
                case 1:
                    Console.Clear();
                    Spiellogik.Spiel();
                    break;
                case 2:
                    Einstellungen();
                    break;
                case 3:
                    Shop();
                    break;
                case 4:
                    Skin_Farben();
                    break;
                case 6:
                    // Zuweisung an dein Musiksystem
                    Musik.currentmusik = GameData.MusikDaten.Menue.Anleitung;
                    MenüRenderer.RenderAnleitung();
                    break;
                case 5:
                    // Zuweisung an dein Musiksystem
                    Musik.currentmusik = GameData.MusikDaten.Menue.Statistiken;
                    MenüRenderer.RenderStatistiken();
                    break;
                case 7:
                    Spiellogik.exit = true;
                    break;
            }
        }

        // Einstellungsmenü im Hauptmenü
        static void Einstellungen()
        {
            // Zuweisung an dein Musiksystem
            Musik.currentmusik = GameData.MusikDaten.Menue.Einstellungen;

            Console.Clear();
            bool menu = true;
            int einstellungsAuswahl = 1;

            do
            {
                MenüRenderer.RenderEinstellungen(einstellungsAuswahl);

                while (Console.KeyAvailable)
                    Console.ReadKey(true);

                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        einstellungsAuswahl--;
                        if (einstellungsAuswahl < 1) einstellungsAuswahl = 8;
                        break;

                    case ConsoleKey.DownArrow:
                        einstellungsAuswahl++;
                        if (einstellungsAuswahl > 8) einstellungsAuswahl = 1;
                        break;
                    case ConsoleKey.Escape:
                        menu = false;
                        break;

                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        Console.Clear();
                        switch (einstellungsAuswahl)
                        {
                            case 1:
                                ChangeDifficulty();
                                break;
                            case 2:
                                Spiellogik.multiplayer = !Spiellogik.multiplayer;
                                break;
                            case 3:
                                ChangeGamemode();
                                break;
                            case 4:
                                RendernSpielfeld.performancemode = !RendernSpielfeld.performancemode;
                                break;
                            case 5:
                                Musik.musikplay = !Musik.musikplay;
                                break;
                            case 6:
                                Musik.soundplay = !Musik.soundplay;
                                break;
                            case 7:
                                ResetSpielstand();
                                break;
                            case 8:
                                menu = false;
                                break;
                        }
                        break;
                }

            } while (menu);
        }

        // Auswahl der Spielgeschwindigkeit
        static void ChangeDifficulty()
        {
            if (Spiellogik.difficulty == "Langsam") Spiellogik.difficulty = "Mittel";
            else if (Spiellogik.difficulty == "Mittel") Spiellogik.difficulty = "Schnell";
            else Spiellogik.difficulty = "Langsam";
        }

        // Auswahl der Verschiedenen Modi
        static void ChangeGamemode()
        {
            if (Spiellogik.gamemode == "Normal") Spiellogik.gamemode = "Unendlich";
            else if (Spiellogik.gamemode == "Unendlich") Spiellogik.gamemode = "Babymode";
            else Spiellogik.gamemode = "Normal";
        }

        static void ResetSpielstand()
        {
            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"Willst du deinen Spielstand wirklich zurücksetzen? ({i}/3) [ja/nein]");
                string? eingabe = Console.ReadLine()?.Trim().ToLower();
                Console.Clear();
                if (eingabe != "ja")
                {
                    Console.WriteLine("Zurücksetzen abgebrochen.");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }
            }

            // Wenn alle drei Bestätigungen "ja" waren:
            SpeicherSystem.Speichern_Laden("Zurücksetzen");
            Console.WriteLine("Dein Spielstand wurde zurückgesetzt!");
            Console.ReadKey();
            Console.Clear();
        }

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
                            case 9: WechselFarbe(ref Spiellogik.foodfarbe); break;
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
            } while ((!freigeschaltet[idx] || (verboteneSkin.HasValue && skins[idx] == verboteneSkin.Value)) && idx != start);
            aktuellesSkin = skins[idx];
        }

        // Helper für Farben
        static void WechselFarbe(ref ConsoleColor aktuelleFarbe)
        {
            if (GameData.Farben.Length == 0) return;
            int idx = Array.IndexOf(GameData.Farben, aktuelleFarbe);
            int start = idx;
            do
            {
                idx = (idx + 1) % GameData.Farben.Length;
            } while (!freigeschaltetFarben[idx] && idx != start);
            aktuelleFarbe = GameData.Farben[idx];
        }
    }
}