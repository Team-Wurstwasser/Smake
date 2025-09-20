using Smake.io.Render;
using Smake.io.Spiel;
using System.Text;
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
            Musik.musikplay = true;
            Musik.soundplay = true;

            Menüs.freigeschaltetTail[0] = true;
            Menüs.freigeschaltetFood[0] = true;
            Menüs.freigeschaltetRand[0] = true;
            Menüs.freigeschaltetFarben[0] = true;

            RendernSpielfeld.performancemode = false;

            Spiellogik.coins = 0;
            Spiellogik.xp = 0;
            Menüs.gesamtcoins = 0;
            Menüs.highscore = 0;
            Menüs.spieleGesamt = 0;

            Spiellogik.difficulty = "Mittel";
            Spiellogik.gamemode = "Normal";
            Spiellogik.multiplayer = false;

            Spiellogik.rand = '█';
            Spiellogik.food = '*';
            Spiellogik.player.Skin = '+';
            Spiellogik.player2.Skin = 'x';

            Spiellogik.randfarbe = ConsoleColor.White;
            Spiellogik.foodfarbe = ConsoleColor.White;
            Spiellogik.player.Farbe = ConsoleColor.White;
            Spiellogik.player2.Farbe = ConsoleColor.White;
            Spiellogik.player.Headfarbe = ConsoleColor.White;
            Spiellogik.player2.Headfarbe = ConsoleColor.White;
        }

        private static void Speichern()
        {
            var zeilen = new List<string>
            {
                $"performancemode={RendernSpielfeld.performancemode}",
                $"coins={Spiellogik.coins}",
                $"xp={Spiellogik.xp}",
                $"spieleGesamt={Menüs.spieleGesamt}",
                $"maxfutter={Spiellogik.maxfutter}",
                $"highscore={Menüs.highscore}",
                $"gesamtcoins={Menüs.gesamtcoins}",
                $"difficulty={Spiellogik.difficulty}",
                $"gamemode={Spiellogik.gamemode}",
                $"multiplayer={Spiellogik.multiplayer}",
                $"rand={Spiellogik.rand}",
                $"food={Spiellogik.food}",
                $"player1.Skin={Spiellogik.player.Skin}",
                $"player2.Skin={Spiellogik.player2.Skin}",
                $"randfarbe={Spiellogik.randfarbe}",
                $"foodfarbe={Spiellogik.foodfarbe}",
                $"player1.Farbe={Spiellogik.player.Farbe}",
                $"player2.Farbe={Spiellogik.player2.Farbe}",
                $"player1.Headfarbe={Spiellogik.player.Headfarbe}",
                $"player2.Headfarbe={Spiellogik.player2.Headfarbe}",
                $"Musik={Musik.musikplay}",
                $"Sound={Musik.soundplay}"
            };

            for (int i = 0; i < Menüs.freigeschaltetTail.Length; i++)
                zeilen.Add($"freigeschaltetTail{i}={Menüs.freigeschaltetTail[i]}");
            for (int i = 0; i < Menüs.freigeschaltetFood.Length; i++)
                zeilen.Add($"freigeschaltetFood{i}={Menüs.freigeschaltetFood[i]}");
            for (int i = 0; i < Menüs.freigeschaltetRand.Length; i++)
                zeilen.Add($"freigeschaltetRand{i}={Menüs.freigeschaltetRand[i]}");
            for (int i = 0; i < Menüs.freigeschaltetFarben.Length; i++)
                zeilen.Add($"freigeschaltetFarben{i}={Menüs.freigeschaltetFarben[i]}");

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
                        case "performancemode": RendernSpielfeld.performancemode = bool.Parse(wert); break;
                        case "coins": Spiellogik.coins = int.Parse(wert); break;
                        case "xp": Spiellogik.xp = int.Parse(wert); break;
                        case "gesamtcoins": Menüs.gesamtcoins = int.Parse(wert); break;
                        case "highscore": Menüs.highscore = int.Parse(wert); break;
                        case "spieleGesamt": Menüs.spieleGesamt = int.Parse(wert); break;
                        case "maxfutter": Spiellogik.maxfutter = int.Parse(wert); break;

                        case "difficulty": Spiellogik.difficulty = wert; break;
                        case "gamemode": Spiellogik.gamemode = wert; break;
                        case "multiplayer": Spiellogik.multiplayer = bool.Parse(wert); break;

                        case "rand": Spiellogik.rand = wert[0]; break;
                        case "food": Spiellogik.food = wert[0]; break;
                        case "player1.Skin": Spiellogik.player.Skin = wert[0]; break;
                        case "player2.Skin": Spiellogik.player2.Skin = wert[0]; break;

                        case "randfarbe": Spiellogik.randfarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "foodfarbe": Spiellogik.foodfarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "player1.Farbe": Spiellogik.player.Farbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "player2.Farbe": Spiellogik.player2.Farbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "player1.Headfarbe": Spiellogik.player.Headfarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "player2.Headfarbe": Spiellogik.player2.Headfarbe = Enum.Parse<ConsoleColor>(wert); break;
                        case "Sound": Musik.soundplay = bool.Parse(wert); break;
                        case "Musik": Musik.musikplay = bool.Parse(wert); break;
                        default:
                            if (name.StartsWith("freigeschaltetTail"))
                                Menüs.freigeschaltetTail[int.Parse(name.Replace("freigeschaltetTail", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("freigeschaltetFood"))
                                Menüs.freigeschaltetFood[int.Parse(name.Replace("freigeschaltetFood", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("freigeschaltetRand"))
                                Menüs.freigeschaltetRand[int.Parse(name.Replace("freigeschaltetRand", ""))] = bool.Parse(wert);
                            else if (name.StartsWith("freigeschaltetFarben"))
                                Menüs.freigeschaltetFarben[int.Parse(name.Replace("freigeschaltetFarben", ""))] = bool.Parse(wert);
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
