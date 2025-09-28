using Smake.io.Values;
using Smake.io.Render;
using Smake.io.Spiel;
using System.Security.Cryptography;

namespace Smake.io.Speicher
{
    public static class CryptoHelper
    {
        private static readonly string Passwort = "djsghiowrhurt9iwezriwehgfokweh9tfhwoirthweoihtoeriwh";

        // Länge des Salts in Bytes
        private const int SaltLength = 16;

        public static byte[] Encrypt(string plainText)
        {
            // Zufälligen Salt erzeugen
            byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);

            using var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(Passwort, salt, 100_000, HashAlgorithmName.SHA256);

            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            byte[] encryptedData = ms.ToArray();

            // Salt vorne anfügen, damit es beim Entschlüsseln verfügbar ist
            byte[] result = new byte[SaltLength + encryptedData.Length];
            Array.Copy(salt, 0, result, 0, SaltLength);
            Array.Copy(encryptedData, 0, result, SaltLength, encryptedData.Length);

            return result;
        }

        public static string Decrypt(byte[] cipherWithSalt)
        {
            if (cipherWithSalt.Length < SaltLength)
                throw new ArgumentException("Ungültige verschlüsselte Daten");

            // Salt aus den ersten Bytes auslesen
            byte[] salt = new byte[SaltLength];
            Array.Copy(cipherWithSalt, 0, salt, 0, SaltLength);

            // Restliche Daten = verschlüsselter Text
            byte[] cipherText = new byte[cipherWithSalt.Length - SaltLength];
            Array.Copy(cipherWithSalt, SaltLength, cipherText, 0, cipherText.Length);

            using var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(Passwort, salt, 100_000, HashAlgorithmName.SHA256);

            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }

    public class SpeicherSystem
    {
        private const string SpeicherDatei = "spielstand.bin";
        private const string BackupDatei = "spielstand.bak";

        public static void Speichern_Laden(string aktion)
        {
            // Wenn die Hauptdatei fehlt, aber ein Backup existiert, lade das Backup
            if (!File.Exists(SpeicherDatei) && File.Exists(BackupDatei))
            {
                Console.WriteLine("Hauptdatei fehlt, lade Backup...");
                Console.ReadKey();
                if (!Laden(BackupDatei))
                {
                    Console.WriteLine("❌ Backup beschädigt – Standardwerte werden gesetzt.");
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
                        Console.WriteLine("⚠ Fehler beim Laden, versuche Backup...");
                        Console.ReadKey();
                        if (!Laden(BackupDatei))
                        {
                            Console.WriteLine("❌ Backup auch beschädigt – Standardwerte werden gesetzt.");
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
            Array.Clear(Menüsvalues.freigeschaltetTail);
            Array.Clear(Menüsvalues.freigeschaltetFood);
            Array.Clear(Menüsvalues.freigeschaltetRand);
            Array.Clear(Menüsvalues.freigeschaltetFarben);

            Menüsvalues.freigeschaltetTail[0] = true;
            Menüsvalues.freigeschaltetTail[1] = true;
            Menüsvalues.freigeschaltetFood[0] = true;
            Menüsvalues.freigeschaltetRand[0] = true;
            Menüsvalues.freigeschaltetFarben[0] = true;

            RendernSpielfeld.Performancemode = false;

            Spielstatus.coins = 0;
            Spielstatus.xp = 0;
            Menüsvalues.gesamtcoins = 0;
            Menüsvalues.highscore = 0;
            Menüsvalues.spieleGesamt = 0;
            Spielvalues.maxfutter = 1;

            Spielvalues.difficulty = "Mittel";
            Spielvalues.gamemode = "Normal";
            Spielvalues.multiplayer = false;

            Skinvalues.rand = GameData.RandSkins[0];
            Skinvalues.food = GameData.FoodSkins[0];
            Spiellogik.Player.TailSkin = GameData.TailSkins[0];
            Spiellogik.Player2.TailSkin = GameData.TailSkins[1];

            Skinvalues.randfarbe = GameData.Farben[0];
            Skinvalues.foodfarbe = GameData.Farben[0];
            Skinvalues.foodfarbeRandom = false;
            Spiellogik.Player.TailFarbe = GameData.Farben[0];
            Spiellogik.Player2.TailFarbe = GameData.Farben[0];
            Spiellogik.Player.HeadFarbe = GameData.Farben[0];
            Spiellogik.Player2.HeadFarbe = GameData.Farben[0];
        }

        private static void Speichern()
        {
            var zeilen = new List<string>
            {
                $"performancemode={RendernSpielfeld.Performancemode}",
                $"coins={Spielstatus.coins}",
                $"xp={Spielstatus.xp}",
                $"spieleGesamt={Menüsvalues.spieleGesamt}",
                $"maxfutter={Spielvalues.maxfutter}",
                $"highscore={Menüsvalues.highscore}",
                $"gesamtcoins={Menüsvalues.gesamtcoins}",
                $"difficulty={Spielvalues.difficulty}",
                $"gamemode={Spielvalues.gamemode}",
                $"multiplayer={Spielvalues.multiplayer}",
                $"rand={Skinvalues.rand}",
                $"food={Skinvalues.food}",
                $"Player1.TailSkin={Spiellogik.Player.TailSkin}",
                $"Player2.TailSkin={Spiellogik.Player2.TailSkin}",
                $"randfarbe={Skinvalues.randfarbe}",
                $"foodfarbe={Skinvalues.foodfarbe}",
                $"foodfarbeRandom={Skinvalues.foodfarbeRandom}",
                $"Player1.TailFarbe={Spiellogik.Player.TailFarbe}",
                $"Player2.TailFarbe={Spiellogik.Player2.TailFarbe}",
                $"Player1.HeadFarbe={Spiellogik.Player.HeadFarbe}",
                $"Player2.HeadFarbe={Spiellogik.Player2.HeadFarbe}",
                $"Musik={Musik.Musikplay}",
                $"Sound={Musik.Soundplay}"
            };

            for (int i = 0; i < Menüsvalues.freigeschaltetTail.Length; i++)
                zeilen.Add($"freigeschaltetTail{i}={Menüsvalues.freigeschaltetTail[i]}");
            for (int i = 0; i < Menüsvalues.freigeschaltetFood.Length; i++)
                zeilen.Add($"freigeschaltetFood{i}={Menüsvalues.freigeschaltetFood[i]}");
            for (int i = 0; i < Menüsvalues.freigeschaltetRand.Length; i++)
                zeilen.Add($"freigeschaltetRand{i}={Menüsvalues.freigeschaltetRand[i]}");
            for (int i = 0; i < Menüsvalues.freigeschaltetFarben.Length; i++)
                zeilen.Add($"freigeschaltetFarben{i}={Menüsvalues.freigeschaltetFarben[i]}");

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
                var teil = zeile.Split('=');
                if (teil.Length != 2) continue;

                string name = teil[0];
                string wert = teil[1];

                try
                {
                    switch (name)
                    {
                        case "performancemode": RendernSpielfeld.Performancemode = bool.Parse(wert); break;
                        case "coins": Spielstatus.coins = int.Parse(wert); break;
                        case "xp": Spielstatus.xp = int.Parse(wert); break;
                        case "gesamtcoins": Menüsvalues.gesamtcoins = int.Parse(wert); break;
                        case "highscore": Menüsvalues.highscore = int.Parse(wert); break;
                        case "spieleGesamt": Menüsvalues.spieleGesamt = int.Parse(wert); break;
                        case "maxfutter": Spielvalues.maxfutter = int.Parse(wert); break;

                        case "difficulty": Spielvalues.difficulty = wert; break;
                        case "gamemode": Spielvalues.gamemode = wert; break;
                        case "multiplayer": Spielvalues.multiplayer = bool.Parse(wert); break;

                        case "rand": Skinvalues.rand = wert[0]; break;
                        case "food": Skinvalues.food = wert[0]; break;
                        case "Player1.TailSkin": Spiellogik.Player.TailSkin = wert[0]; break;
                        case "Player2.TailSkin": Spiellogik.Player2.TailSkin = wert[0]; break;

                        case "randfarbe": Skinvalues.randfarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "foodfarbe": Skinvalues.foodfarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "foodfarbeRandom": Skinvalues.foodfarbeRandom = bool.Parse(wert); break;
                        case "Player1.TailFarbe": Spiellogik.Player.TailFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Player2.TailFarbe": Spiellogik.Player2.TailFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Player1.HeadFarbe": Spiellogik.Player.HeadFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Player2.HeadFarbe": Spiellogik.Player2.HeadFarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Sound": Musik.Soundplay = bool.Parse(wert); break;
                        case "Musik": Musik.Musikplay = bool.Parse(wert); break;
                        default:
                            if (name.StartsWith("freigeschaltetTail"))
                                Menüsvalues.freigeschaltetTail[int.Parse(name.Replace("freigeschaltetTail", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("freigeschaltetFood"))
                                Menüsvalues.freigeschaltetFood[int.Parse(name.Replace("freigeschaltetFood", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("freigeschaltetRand"))
                                Menüsvalues.freigeschaltetRand[int.Parse(name.Replace("freigeschaltetRand", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("freigeschaltetFarben"))
                                Menüsvalues.freigeschaltetFarben[int.Parse(name.Replace("freigeschaltetFarben", ""))] = bool.Parse(wert);
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
