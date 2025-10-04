using Smake.Helper;
using Smake.Render;
using Smake.Spiel;
using Smake.Values;

namespace Smake.Speicher
{
    public class SpeicherSystem
    {
        private const string SpeicherDatei = "spielstand.bin";
        private const string BackupDatei = "spielstand.bak";

        public static void Speichern_Laden(string aktion)
        {
            // Wenn die Hauptdatei fehlt, aber ein Backup existiert, lade das Backup
            if (!File.Exists(SpeicherDatei) && File.Exists(BackupDatei))
            {
                Console.WriteLine(LanguageManager.Get("saveLoad.mainMissing"));
                Console.ReadKey();
                if (!Laden(BackupDatei))
                {
                    Console.WriteLine(LanguageManager.Get("saveLoad.backupCorrupt"));
                    Console.ReadKey();
                    SetzeStandardwerte();
                    Speichern();
                }
                else
                {
                    // Backup erfolgreich geladen → Backup als Hauptdatei speichern
                    File.Copy(BackupDatei, SpeicherDatei, true);
                }
                return; // Aktion bereits erledigt
            }

            // Wenn weder Hauptdatei noch Backup existieren
            if (!File.Exists(SpeicherDatei) && !File.Exists(BackupDatei))
            {
                SetzeStandardwerte();
                Speichern();
            }

            switch (aktion)
            {
                case "Zurücksetzen":
                    SetzeStandardwerte();
                    Speichern();
                    break;
                case "Speichern":
                    Speichern();
                    break;
                case "Laden":
                    if (!Laden(SpeicherDatei))
                    {
                        Console.WriteLine(LanguageManager.Get("saveLoad.loadError"));
                        Console.ReadKey();
                        if (!Laden(BackupDatei))
                        {
                            Console.WriteLine(LanguageManager.Get("saveLoad.backupAlsoCorrupt"));
                            Console.ReadKey();
                            SetzeStandardwerte();
                            Speichern();
                        }
                        else
                        {
                            // Bei erfolgreichem Laden ein Backup anlegen
                            File.Copy(BackupDatei, SpeicherDatei, true);
                        }
                    }
                    else
                    {
                        // Bei erfolgreichem Laden ein Backup anlegen
                        File.Copy(SpeicherDatei, BackupDatei, true);
                    }
                    break;
            }
        }

        private static void SetzeStandardwerte()
        {
            Musik.Musikplay = true;
            Musik.Soundplay = true;

            // Erst alle auf false setzen
            Array.Clear(Menüsvalues.FreigeschaltetTail);
            Array.Clear(Menüsvalues.FreigeschaltetFood);
            Array.Clear(Menüsvalues.FreigeschaltetRand);
            Array.Clear(Menüsvalues.FreigeschaltetFarben);

            Menüsvalues.FreigeschaltetTail[0] = true;
            Menüsvalues.FreigeschaltetTail[1] = true;
            Menüsvalues.FreigeschaltetFood[0] = true;
            Menüsvalues.FreigeschaltetRand[0] = true;
            Menüsvalues.FreigeschaltetFarben[0] = true;

            RendernSpielfeld.Performancemode = false;

            Spielstatus.Coins = 0;
            Spielstatus.Xp = 0;
            Spielstatus.Gesamtcoins = 0;
            Spielstatus.Highscore = 0;
            Spielstatus.SpieleGesamt = 0;
            Spielvalues.Maxfutter = 1;

            Spielvalues.DifficultyInt = 2;

            Spielvalues.Difficulty = Spielvalues.DifficultyInt switch
            {
                1 => LanguageManager.Get("settings.difficulty.slow"),
                2 => LanguageManager.Get("settings.difficulty.medium"),
                3 => LanguageManager.Get("settings.difficulty.fast"),
                _ => LanguageManager.Get("settings.difficulty.medium") // Standardwert
            };

            Spielvalues.GamemodeInt = 1;
            var modes = LanguageManager.GetArray("settings.gamemodes");
            Spielvalues.Gamemode = modes[(int)Spielvalues.GamemodeInt - 1];
            Spielvalues.Multiplayer = false;

            Skinvalues.RandSkin = GameData.RandSkins[0];
            Skinvalues.FoodSkin = GameData.FoodSkins[0];
            Spiellogik.Player.TailSkin = GameData.TailSkins[0];
            Spiellogik.Player2.TailSkin = GameData.TailSkins[1];

            Skinvalues.RandFarbe = GameData.Farben[0];
            Skinvalues.FoodFarbe = GameData.Farben[0];
            Skinvalues.FoodfarbeRandom = false;
            Spiellogik.Player.TailFarbe = GameData.Farben[0];
            Spiellogik.Player2.TailFarbe = GameData.Farben[0];
            Spiellogik.Player.HeadFarbe = GameData.Farben[0];
            Spiellogik.Player2.HeadFarbe = GameData.Farben[0];
        }

        private static void Speichern()
        {
            var zeilen = new List<string>
            {
                $"Performancemode={RendernSpielfeld.Performancemode}",
                $"Coins={Spielstatus.Coins}",
                $"Xp={Spielstatus.Xp}",
                $"SpieleGesamt={Spielstatus.SpieleGesamt}",
                $"Maxfutter={Spielvalues.Maxfutter}",
                $"Highscore={Spielstatus.Highscore}",
                $"Gesamtcoins={Spielstatus.Gesamtcoins}",
                $"Difficulty={Spielvalues.DifficultyInt}",
                $"Gamemode={Spielvalues.GamemodeInt}",
                $"Multiplayer={Spielvalues.Multiplayer}",
                $"RandSkin={Skinvalues.RandSkin}",
                $"FoodSkin={Skinvalues.FoodSkin}",
                $"Player1.TailSkin={Spiellogik.Player.TailSkin}",
                $"Player2.TailSkin={Spiellogik.Player2.TailSkin}",
                $"RandFarbe={Skinvalues.RandFarbe}",
                $"FoodFarbe={Skinvalues.FoodFarbe}",
                $"FoodfarbeRandom={Skinvalues.FoodfarbeRandom}",
                $"Player1.TailFarbe={Spiellogik.Player.TailFarbe}",
                $"Player2.TailFarbe={Spiellogik.Player2.TailFarbe}",
                $"Player1.HeadFarbe={Spiellogik.Player.HeadFarbe}",
                $"Player2.HeadFarbe={Spiellogik.Player2.HeadFarbe}",
                $"Musikplay={Musik.Musikplay}",
                $"Soundplay={Musik.Soundplay}"
            };

            for (int i = 0; i < Menüsvalues.FreigeschaltetTail.Length; i++)
                zeilen.Add($"FreigeschaltetTail{i}={Menüsvalues.FreigeschaltetTail[i]}");
            for (int i = 0; i < Menüsvalues.FreigeschaltetFood.Length; i++)
                zeilen.Add($"FreigeschaltetFood{i}={Menüsvalues.FreigeschaltetFood[i]}");
            for (int i = 0; i < Menüsvalues.FreigeschaltetRand.Length; i++)
                zeilen.Add($"FreigeschaltetRand{i}={Menüsvalues.FreigeschaltetRand[i]}");
            for (int i = 0; i < Menüsvalues.FreigeschaltetFarben.Length; i++)
                zeilen.Add($"FreigeschaltetFarben{i}={Menüsvalues.FreigeschaltetFarben[i]}");

            string plainText = string.Join(Environment.NewLine, zeilen);
            byte[] encrypted = CryptoHelper.Encrypt(plainText);

            File.WriteAllBytes(SpeicherDatei, encrypted);
        }

        private static bool Laden(string datei)
        {
            if (!File.Exists(datei)) return false;

            byte[] encrypted = File.ReadAllBytes(datei);
            string plainText;

            try
            {
                plainText = CryptoHelper.Decrypt(encrypted);
            }
            catch
            {
                return false;
            }

            var zeilen = plainText.Split(Environment.NewLine);

            foreach (var zeile in zeilen)
            {
                var teil = zeile.Split('=', 2);
                if (teil.Length != 2) continue;

                string name = teil[0];
                string wert = teil[1];

                try
                {
                    switch (name)
                    {
                        case "Performancemode": RendernSpielfeld.Performancemode = bool.Parse(wert); break;
                        case "Coins": Spielstatus.Coins = int.Parse(wert); break;
                        case "Xp": Spielstatus.Xp = int.Parse(wert); break;
                        case "Gesamtcoins": Spielstatus.Gesamtcoins = int.Parse(wert); break;
                        case "Highscore": Spielstatus.Highscore = int.Parse(wert); break;
                        case "SpieleGesamt": Spielstatus.SpieleGesamt = int.Parse(wert); break;
                        case "Maxfutter": Spielvalues.Maxfutter = int.Parse(wert); break;

                        case "Difficulty": Spielvalues.DifficultyInt = int.Parse(wert); break;
                        case "Gamemode": Spielvalues.GamemodeInt = int.Parse(wert); break;
                        case "Multiplayer": Spielvalues.Multiplayer = bool.Parse(wert); break;

                        case "RandSkin": Skinvalues.RandSkin = wert[0]; break;
                        case "FoodSkin": Skinvalues.FoodSkin = wert[0]; break;
                        case "Player1.TailSkin": Spiellogik.Player.TailSkin = wert[0]; break;
                        case "Player2.TailSkin": Spiellogik.Player2.TailSkin = wert[0]; break;

                        case "RandFarbe": Skinvalues.RandFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "FoodFarbe": Skinvalues.FoodFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "FoodfarbeRandom": Skinvalues.FoodfarbeRandom = bool.Parse(wert); break;
                        case "Player1.TailFarbe": Spiellogik.Player.TailFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Player2.TailFarbe": Spiellogik.Player2.TailFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Player1.HeadFarbe": Spiellogik.Player.HeadFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Player2.HeadFarbe": Spiellogik.Player2.HeadFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Soundplay": Musik.Soundplay = bool.Parse(wert); break;
                        case "Musikplay": Musik.Musikplay = bool.Parse(wert); break;
                        default:
                            if (name.StartsWith("FreigeschaltetTail"))
                                Menüsvalues.FreigeschaltetTail[int.Parse(name.Replace("FreigeschaltetTail", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("FreigeschaltetFood"))
                                Menüsvalues.FreigeschaltetFood[int.Parse(name.Replace("FreigeschaltetFood", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("FreigeschaltetRand"))
                                Menüsvalues.FreigeschaltetRand[int.Parse(name.Replace("FreigeschaltetRand", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("FreigeschaltetFarben"))
                                Menüsvalues.FreigeschaltetFarben[int.Parse(name.Replace("FreigeschaltetFarben", ""))] = bool.Parse(wert);
                            break;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}
