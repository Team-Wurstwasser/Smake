using Smake.Helper;
using Smake.Values;
using Smake.SFX;
using Smake.Enums;

namespace Smake.Speicher
{
    public static class SpeicherSystem
    {
        const string SpeicherDatei = "spielstand.bin";
        const string BackupDatei = "spielstand.bak";

        public static void Speichern_Laden(StorageAction aktion)
        {
            // Wenn die Hauptdatei fehlt, aber ein Backup existiert, lade das Backup
            if (!File.Exists(SpeicherDatei) && File.Exists(BackupDatei))
            {
                Console.WriteLine(LanguageSystem.Get("saveLoad.mainMissing"));
                Console.ReadKey();
                if (!Laden(BackupDatei))
                {
                    Console.WriteLine(LanguageSystem.Get("saveLoad.backupCorrupt"));
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
                case StorageAction.Reset:
                    SetzeStandardwerte();
                    Speichern();
                    break;
                case StorageAction.Save:
                    Speichern();
                    break;
                case StorageAction.Load:
                    if (!Laden(SpeicherDatei))
                    {
                        Console.WriteLine(LanguageSystem.Get("saveLoad.loadError"));
                        Console.ReadKey(true);
                        if (!Laden(BackupDatei))
                        {
                            Console.WriteLine(LanguageSystem.Get("saveLoad.backupAlsoCorrupt"));
                            Console.ReadKey(true);
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

        static void SetzeStandardwerte()
        {
            Sounds.Musikplay = true;
            Sounds.Soundplay = true;

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

            Spielvalues.Performancemode = false;

            Spielstatus.Coins = 0;
            Spielstatus.Xp = 0;
            Spielstatus.Gesamtcoins = 0;
            Spielstatus.Highscore = 0;
            Spielstatus.SpieleGesamt = 0;
            Spielvalues.Maxfutter = 1;

            Spielvalues.Difficulty = Difficultys.Medium;

            Spielvalues.Gamemode = Gamemodes.Normal;
            Spielvalues.Multiplayer = false;

            Skinvalues.RandSkin = ConfigSystem.Skins.Rand[0];
            Skinvalues.FoodSkin = ConfigSystem.Skins.Food[0];
            Skinvalues.TailSkin[0] = ConfigSystem.Skins.Tail[0];
            Skinvalues.TailSkin[1] = ConfigSystem.Skins.Tail[1];

            Skinvalues.RandFarbe = ConfigSystem.Skins.Farben[0];
            Skinvalues.FoodFarbe = ConfigSystem.Skins.Farben[0];
            Skinvalues.FoodfarbeRandom = false;
            Array.Fill(Skinvalues.TailFarbe, ConfigSystem.Skins.Farben[0]);
            Array.Fill(Skinvalues.HeadFarbe, ConfigSystem.Skins.Farben[0]);
        }

        static void Speichern()
        {
            var zeilen = new List<string>
            {
                $"Performancemode={Spielvalues.Performancemode}",
                $"Musikplay={Sounds.Musikplay}",
                $"Soundplay={Sounds.Soundplay}",
                $"Coins={Spielstatus.Coins}",
                $"Xp={Spielstatus.Xp}",
                $"SpieleGesamt={Spielstatus.SpieleGesamt}",
                $"Maxfutter={Spielvalues.Maxfutter}",
                $"Highscore={Spielstatus.Highscore}",
                $"Gesamtcoins={Spielstatus.Gesamtcoins}",
                $"Difficulty={Spielvalues.Difficulty}",
                $"Gamemode={Spielvalues.Gamemode}",
                $"RandSkin={Skinvalues.RandSkin}",
                $"FoodSkin={Skinvalues.FoodSkin}",
                $"TailSkin[0]={Skinvalues.TailSkin[0]}",
                $"TailSkin[1]={Skinvalues.TailSkin[1]}",
                $"RandFarbe={Skinvalues.RandFarbe}",
                $"FoodFarbe={Skinvalues.FoodFarbe}",
                $"FoodfarbeRandom={Skinvalues.FoodfarbeRandom}",
                $"TailFarbe[0]={Skinvalues.TailFarbe[0]}",
                $"TailFarbe[1]={Skinvalues.TailFarbe[1]}",
                $"HeadFarbe[0]={Skinvalues.HeadFarbe[0]}",
                $"HeadFarbe[1]={Skinvalues.HeadFarbe[1]}"
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
            byte[] encrypted = AesCryptoHelper.Encrypt(plainText);

            File.WriteAllBytes(SpeicherDatei, encrypted);
        }

        static bool Laden(string datei)
        {
            if (!File.Exists(datei)) return false;

            byte[] encrypted = File.ReadAllBytes(datei);
            string plainText;

            try
            {
                plainText = AesCryptoHelper.Decrypt(encrypted);
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
                        case "Performancemode": Spielvalues.Performancemode = bool.Parse(wert); break;
                        case "Soundplay": Sounds.Soundplay = bool.Parse(wert); break;
                        case "Musikplay": Sounds.Musikplay = bool.Parse(wert); break;
                        case "Coins": Spielstatus.Coins = int.Parse(wert); break;
                        case "Xp": Spielstatus.Xp = int.Parse(wert); break;
                        case "Gesamtcoins": Spielstatus.Gesamtcoins = int.Parse(wert); break;
                        case "Highscore": Spielstatus.Highscore = int.Parse(wert); break;
                        case "SpieleGesamt": Spielstatus.SpieleGesamt = int.Parse(wert); break;
                        case "Maxfutter": Spielvalues.Maxfutter = int.Parse(wert); break;

                        case "Difficulty": Spielvalues.Difficulty = Enum.Parse<Difficultys>(wert); ; break;
                        case "Gamemode": Spielvalues.Gamemode = Enum.Parse<Gamemodes>(wert); break;
                        case "Multiplayer": Spielvalues.Multiplayer = bool.Parse(wert); break;

                        case "RandSkin": Skinvalues.RandSkin = wert[0]; break;
                        case "FoodSkin": Skinvalues.FoodSkin = wert[0]; break;
                        case "TailSkin[0]": Skinvalues.TailSkin[1] = wert[0]; break;
                        case "TailSkin[1]": Skinvalues.TailSkin[0] = wert[0]; break;

                        case "RandFarbe": Skinvalues.RandFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "FoodFarbe": Skinvalues.FoodFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "FoodfarbeRandom": Skinvalues.FoodfarbeRandom = bool.Parse(wert); break;
                        case "TailFarbe[0]": Skinvalues.TailFarbe[0] = Enum.Parse<ConsoleColor>(wert); break;
                        case "TailFarbe[1]": Skinvalues.TailFarbe[1] = Enum.Parse<ConsoleColor>(wert); break;
                        case "HeadFarbe[0]": Skinvalues.HeadFarbe[0] = Enum.Parse<ConsoleColor>(wert); break;
                        case "HeadFarbe[1]": Skinvalues.HeadFarbe[1] = Enum.Parse<ConsoleColor>(wert); break;
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
