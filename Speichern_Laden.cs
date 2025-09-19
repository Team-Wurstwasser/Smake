using Smake.io.Render;
using Smake.io.Spiel;
using System.Text;
using System.Security.Cryptography;

namespace Smake.io
{
    public static class CryptoHelper
    {
        private static readonly string Passwort = "i4ui63uz6o5z6uzu3zuzm,fcngihdoihf";
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("546354636453654");

        public static byte[] Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(Passwort, Salt, 10000);
            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using var sw = new StreamWriter(cs);
            sw.Write(plainText);
            sw.Close();
            return ms.ToArray();
        }

        public static string Decrypt(byte[] cipherText)
        {
            using var aes = Aes.Create();
            var key = new Rfc2898DeriveBytes(Passwort, Salt, 10000);
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

        public static void Speichern_Laden(string aktion)
        {
            if (!File.Exists(SpeicherDatei))
            {
                SetzeStandardwerte();
                Speichern();
            }

            switch (aktion)
            {
                case "Speichern":
                    Speichern();
                    break;
                case "Laden":
                    Laden();
                    break;
            }
        }

        private static void SetzeStandardwerte()
        {
            Menüs.randzahl = 0;
            Menüs.foodzahl = 0;
            Spiellogik.player.Skinzahl = 0;
            Spiellogik.player2.Skinzahl = 1;
            Spiellogik.player.Headfarbezahl = 0;
            Spiellogik.player2.Headfarbezahl = 0;
            Spiellogik.player.Farbezahl = 0;
            Spiellogik.player2.Farbezahl = 0;
            Menüs.foodfarbezahl = 0;
            Menüs.randfarbezahl = 0;

            Musik.musikplay = true;
            Musik.soundplay = true;

            Menüs.freigeschaltetTail[0] = true;
            Menüs.freigeschaltetFood[0] = true;
            Menüs.freigeschaltetRand[0] = true;
            Menüs.freigeschaltetFarben[0] = true;

            RendernSpielfeld.performancemode = false;

            Program.coins = 0;
            Program.xp = 0;
            Program.gesamtcoins = 0;
            Program.highscore = 0;
            Program.spieleGesamt = 0;

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
                $"randzahl={Menüs.randzahl}",
                $"foodzahl={Menüs.foodzahl}",
                $"player1.Skinzahl={Spiellogik.player.Skinzahl}",
                $"player2.Skinzahl={Spiellogik.player2.Skinzahl}",
                $"player1.Headfarbezahl={Spiellogik.player.Headfarbezahl}",
                $"player2.Headfarbezahl={Spiellogik.player2.Headfarbezahl}",
                $"player1.Farbezahl={Spiellogik.player.Farbezahl}",
                $"player2.Farbezahl={Spiellogik.player2.Farbezahl}",
                $"foodfarbezahl={Menüs.foodfarbezahl}",
                $"randfarbezahl={Menüs.randfarbezahl}",
                $"performancemode={RendernSpielfeld.performancemode}",
                $"coins={Program.coins}",
                $"xp={Program.xp}",
                $"spieleGesamt={Program.spieleGesamt}",
                $"highscore={Program.highscore}",
                $"gesamtcoins={Program.gesamtcoins}",
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

        private static void Laden()
        {
            if (!File.Exists(SpeicherDatei)) return;

            byte[] encrypted = File.ReadAllBytes(SpeicherDatei);
            string plainText;

            try
            {
                plainText = CryptoHelper.Decrypt(encrypted);
            }
            catch
            {
                Console.WriteLine("Fehler beim Entschlüsseln! Datei beschädigt!");
                return;
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
                        case "randzahl": Menüs.randzahl = int.Parse(wert); break;
                        case "foodzahl": Menüs.foodzahl = int.Parse(wert); break;
                        case "player1.Skinzahl": Spiellogik.player.Skinzahl = int.Parse(wert); break;
                        case "player2.Skinzahl": Spiellogik.player2.Skinzahl = int.Parse(wert); break;
                        case "player1.Headfarbezahl": Spiellogik.player.Headfarbezahl = int.Parse(wert); break;
                        case "player2.Headfarbezahl": Spiellogik.player2.Headfarbezahl = int.Parse(wert); break;
                        case "player1.Farbezahl": Spiellogik.player.Farbezahl = int.Parse(wert); break;
                        case "player2.Farbezahl": Spiellogik.player2.Farbezahl = int.Parse(wert); break;
                        case "foodfarbezahl": Menüs.foodfarbezahl = int.Parse(wert); break;
                        case "randfarbezahl": Menüs.randfarbezahl = int.Parse(wert); break;

                        case "performancemode": RendernSpielfeld.performancemode = bool.Parse(wert); break;
                        case "coins": Program.coins = int.Parse(wert); break;
                        case "xp": Program.xp = int.Parse(wert); break;
                        case "gesamtcoins": Program.gesamtcoins = int.Parse(wert); break;
                        case "highscore": Program.highscore = int.Parse(wert); break;
                        case "spieleGesamt": Program.spieleGesamt = int.Parse(wert); break;

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
                    // Fehlerhafte Werte ignorieren
                }
            }
        }
    }
}