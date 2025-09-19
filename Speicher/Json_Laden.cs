using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Smake.io.Speicher
{
    public static class GameData
    {
        // Sounds
        public static string[] Filenames;

        // Preise
        public static int[] TailPreis;
        public static int[] FoodPreis;
        public static int[] RandPreis;
        public static int[] FarbenPreis;

        // Level
        public static int[] TailLevel;
        public static int[] FoodLevel;
        public static int[] RandLevel;
        public static int[] FarbenLevel;

        // Skins / Farben
        public static ConsoleColor[] Farben;
        public static char[] TailSkins;
        public static char[] FoodSkins;
        public static char[] RandSkins;

        // Spielkonfiguration
        public static int Weite;
        public static int Hoehe;
        public static int MaxPunkte;
        public static Positionen Startpositionen;
        public static Difficulty SpielSchwierigkeit;

        // Positionen
        public class Positionen
        {
            public SpielerPosition Spieler1 { get; set; }
            public SpielerPosition Spieler2 { get; set; }
        }

        public class SpielerPosition
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        // Schwierigkeit
        public class Difficulty
        {
            public int Langsam { get; set; }
            public int Mittel { get; set; }
            public int Schnell { get; set; }
        }

        // Allgemeine Optionen für JSON: Groß-/Kleinschreibung ignorieren
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // Lade alle Konfigurationen
        public static void LoadAllConfigs()
        {
            LoadSounds(Path.Combine("Json", "sounds.json"));
            LoadPreise(Path.Combine("Json", "preise.json"));
            LoadLevel(Path.Combine("Json", "level.json"));
            LoadSkins(Path.Combine("Json", "skins.json"));
            LoadGameConfig(Path.Combine("Json", "game_config.json"));
        }

        // Ladefunktionen
        static void LoadSounds(string path)
        {
            var data = LoadJson<Sounds>(path);
            Filenames = data?.Filenames ?? [];
        }

        static void LoadPreise(string path)
        {
            var data = LoadJson<Preise>(path);
            TailPreis = data?.TailPreis ?? [];
            FoodPreis = data?.FoodPreis ?? [];
            RandPreis = data?.RandPreis ?? [];
            FarbenPreis = data?.FarbenPreis ?? [];
        }

        static void LoadLevel(string path)
        {
            var data = LoadJson<Level>(path);
            TailLevel = data?.TailLevel ?? [];
            FoodLevel = data?.FoodLevel ?? [];
            RandLevel = data?.RandLevel ?? [];
            FarbenLevel = data?.FarbenLevel ?? [];
        }

        static void LoadSkins(string path)
        {
            var data = LoadJson<Skins>(path);
            Farben = data?.Farben?.Select(f => Enum.TryParse(f, true, out ConsoleColor c) ? (ConsoleColor?)c : null)
                                  .Where(c => c.HasValue)
                                  .Select(c => c.Value)
                                  .ToArray() ?? [];
            TailSkins = data?.TailSkins ?? [];
            FoodSkins = data?.FoodSkins ?? [];
            RandSkins = data?.RandSkins ?? [];
        }

        static void LoadGameConfig(string path)
        {
            var data = LoadJson<GameConfig>(path);
            Weite = data?.Weite ?? 0;
            Hoehe = data?.Hoehe ?? 0;
            MaxPunkte = data?.MaxPunkte ?? 0;
            Startpositionen = data?.Startpositionen ?? new Positionen();
            SpielSchwierigkeit = data?.Difficulty ?? new Difficulty();
        }

        // Generische Funktion zum Laden von JSON-Dateien mit XOR-Entschlüsselung
        private static T LoadJson<T>(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"JSON-Datei nicht gefunden: {path}");

            string json = XorCrypt.DecryptJsonFileToString(path);
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }

        // Hilfsklassen für JSON-Struktur
        private class Sounds { public string[] Filenames { get; set; } }
        private class Preise
        {
            public int[] TailPreis { get; set; }
            public int[] FoodPreis { get; set; }
            public int[] RandPreis { get; set; }
            public int[] FarbenPreis { get; set; }
        }
        private class Level
        {
            public int[] TailLevel { get; set; }
            public int[] FoodLevel { get; set; }
            public int[] RandLevel { get; set; }
            public int[] FarbenLevel { get; set; }
        }
        private class Skins
        {
            public string[] Farben { get; set; }
            public char[] TailSkins { get; set; }
            public char[] FoodSkins { get; set; }
            public char[] RandSkins { get; set; }
        }
        private class GameConfig
        {
            public int Weite { get; set; }
            public int Hoehe { get; set; }
            public int MaxPunkte { get; set; }
            public Positionen Startpositionen { get; set; }
            public Difficulty Difficulty { get; set; }
        }

        // XOR-Entschlüsselung
        private static class XorCrypt
        {
            public static readonly byte[] key =
            [
                0x5B, 0x42, 0x9D, 0xB1, 0xB4, 0x40, 0xDB, 0x83, 0x85,
                0x35, 0x79, 0x37, 0xF6, 0xB3, 0xF8, 0x9C, 0x47, 0xB5,
                0xE1, 0x96, 0x74, 0x55, 0x92, 0x43, 0xAD, 0x49, 0x90,
                0xBB, 0x7C, 0x7A, 0xC7, 0xD1, 0x22, 0x38, 0xED, 0xB4,
                0xAF, 0x12, 0xA8, 0x31, 0xE0, 0x25, 0x78, 0xD9, 0xF9,
                0x1E, 0xF2, 0x9D, 0x08, 0xB4, 0xF0, 0x58, 0x42, 0x8A,
                0xFA, 0x02, 0xE1, 0x80, 0xA1, 0x0C, 0x12, 0x69, 0xDD,
                0x91, 0x19, 0xD0, 0x1F, 0x18, 0x1F, 0x93, 0x6E, 0x24,
                0xA0, 0x76, 0x07, 0x30, 0xB5, 0xD3, 0xEF, 0xC9, 0xA6,
                0x0D, 0xD9, 0xEB, 0x58, 0xFC, 0x39, 0x73, 0x80, 0x24,
                0x36, 0xD4, 0xCC, 0xC3, 0x6C, 0x32, 0x66, 0x6E, 0x2E,
                0xAB, 0xE0, 0x72, 0xF6, 0x12, 0x98, 0x97, 0x78, 0x66,
                0x6A, 0x49, 0xB5, 0x05, 0xF5, 0x01, 0xB3, 0xF8, 0xAC,
                0xA7, 0x03, 0x3C, 0x40, 0xA5, 0xEF, 0x97, 0x8E, 0x30,
                0x56, 0x42, 0x41, 0x7D, 0x37, 0x1E, 0x2C, 0x35, 0xD4,
                0xB6, 0xD1, 0x0A, 0x45, 0x92, 0x5E, 0xE3, 0x70, 0xD9,
                0x98, 0xB1, 0xAE, 0x80, 0xD1, 0x07, 0x5A, 0x84, 0x85,
                0x19, 0xDE, 0x75, 0xA8, 0xCE, 0xCE, 0x14, 0x5E, 0xA6,
                0x7E, 0x6A, 0x15, 0xC9, 0x03, 0x1A, 0x0C, 0x0E, 0x9D,
                0x92, 0xFA, 0x89, 0x64, 0x55, 0xF2, 0x5C, 0xBB, 0x0A,
                0x44, 0x2A, 0x78, 0x82, 0xB1, 0xFF, 0xDA, 0x1B, 0x6A,
                0x1E, 0x16, 0x5A, 0x74, 0x77, 0xAC, 0x6B, 0xFA, 0x04,
                0x38, 0xD9, 0xF7, 0x04, 0x94, 0x2D, 0xB4, 0x0E, 0x2F,
                0x83, 0xAF, 0xFB, 0xA9, 0xE7, 0x6F, 0x3B, 0x48, 0xEC,
                0xB0, 0x71, 0x4C, 0x85, 0x06, 0xA5, 0xF2, 0xF2, 0xBB,
                0x5F, 0x56, 0x98, 0x8F, 0xFC, 0x54, 0x4F, 0xBD, 0xA4,
                0x0A, 0x71, 0xAF, 0xE2, 0x8D, 0xBF, 0x28, 0xC5, 0x99,
                0xE2, 0x7E, 0xA8, 0x4F, 0x5D, 0x80, 0x12, 0x12, 0x9B,
                0x62, 0xE3, 0x1B, 0x1D, 0x8F, 0xA3, 0xE9, 0xE9, 0xC4,
                0xF4, 0x32
            ];

            public static string DecryptJsonFileToString(string path)
            {
                byte[] encryptedData = File.ReadAllBytes(path);
                if (encryptedData.Length <= 3) return string.Empty;

                byte[] decryptedData = new byte[encryptedData.Length - 5]; // Header SMAKE entfernen
                for (int i = 5; i < encryptedData.Length; i++)
                {
                    int k = (i - 5) % key.Length;
                    decryptedData[i - 5] = (byte)(encryptedData[i] ^ key[k]);
                }

                return Encoding.UTF8.GetString(decryptedData);
            }
        }
    }
}
