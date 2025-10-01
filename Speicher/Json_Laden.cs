using System.Text;
using System.Text.Json;

namespace Smake.Speicher
{
    public static class GameData
    {
        // Sounds
        public static string[] Filenames { get; private set; } = [];
        public static string NoMusikFile { get; private set; } = "";
        public static Musik MusikDaten { get; private set; } = new();

        // Preise
        public static int[] TailPreis { get; private set; } = [];
        public static int[] FoodPreis { get; private set; } = [];
        public static int[] RandPreis { get; private set; } = [];
        public static int[] FarbenPreis { get; private set; } = [];

        // Level
        public static int[] TailLevel { get; private set; } = [];
        public static int[] FoodLevel { get; private set; } = [];
        public static int[] RandLevel { get; private set; } = [];
        public static int[] FarbenLevel { get; private set; } = [];

        // Skins / Farben
        public static ConsoleColor[] Farben { get; private set; } = [];
        public static char[] TailSkins { get; private set; } = [];
        public static char[] FoodSkins { get; private set; } = [];
        public static char[] RandSkins { get; private set; } = [];

        // Spielkonfiguration
        public static int Weite { get; private set; }
        public static int Hoehe { get; private set; }
        public static int MaxPunkte { get; private set; }
        public static int MaxFutterconfig { get; private set; }
        public static Positionen Startpositionen { get; private set; } = new();
        public static Difficulty SpielSchwierigkeit { get; private set; } = new();

        static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static void LoadAllConfigs()
        {
            Load<Sounds>("Json/sounds.json", data =>
            {
                Filenames = data?.Filenames ?? [];
                NoMusikFile = data?.NoMusikFile ?? "";
                MusikDaten = data?.Musik ?? new();
            });

            Load<Preise>("Json/preise.json", data =>
            {
                TailPreis = data?.TailPreis ?? [];
                FoodPreis = data?.FoodPreis ?? [];
                RandPreis = data?.RandPreis ?? [];
                FarbenPreis = data?.FarbenPreis ?? [];
            });

            Load<Level>("Json/level.json", data =>
            {
                TailLevel = data?.TailLevel ?? [];
                FoodLevel = data?.FoodLevel ?? [];
                RandLevel = data?.RandLevel ?? [];
                FarbenLevel = data?.FarbenLevel ?? [];
            });

            Load<Skins>("Json/skins.json", data =>
            {
                Farben = data?.Farben?
                    .Select(f => Enum.TryParse(f, true, out ConsoleColor c) ? (ConsoleColor?)c : null)
                    .Where(c => c.HasValue)
                    .Select(c => c!.Value)
                    .ToArray() ?? [];

                TailSkins = data?.TailSkins ?? [];
                FoodSkins = data?.FoodSkins ?? [];
                RandSkins = data?.RandSkins ?? [];
            });

            Load<GameConfig>("Json/game_config.json", data =>
            {
                Weite = data?.Weite ?? 0;
                Hoehe = data?.Hoehe ?? 0;
                MaxPunkte = data?.MaxPunkte ?? 0;
                MaxFutterconfig = data?.MaxFutterconfig ?? 0;
                Startpositionen = data?.Startpositionen ?? new Positionen();
                SpielSchwierigkeit = data?.Difficulty ?? new Difficulty();
            });
        }

        static void Load<T>(string path, Action<T?> setData)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException($"JSON-Datei nicht gefunden: {path}");

                string json = XorCrypt.DecryptJsonFileToString(path);
                var data = JsonSerializer.Deserialize<T>(json, JsonOptions);
                setData(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der Datei {path}: {ex.Message}");
                Console.ReadKey();  
                setData(default);
            }
        }

        // Hilfsklassen für JSON-Struktur
        public class Positionen
        {
            public SpielerPosition Spieler1 { get; set; } = new();
            public SpielerPosition Spieler2 { get; set; } = new();
        }

        public class SpielerPosition { public int X { get; set; } public int Y { get; set; } }
        public class Difficulty { public int Langsam { get; set; } public int Mittel { get; set; } public int Schnell { get; set; } }

        private class Sounds
        {
            public string[]? Filenames { get; set; }
            public string? NoMusikFile { get; set; }
            public Musik? Musik { get; set; }
        }

        public class Musik
        {
            public GameMusik? Game { get; set; }
            public MenueMusik? Menue { get; set; }
        }

        public class GameMusik
        {
            public MusikSpeed? Normal { get; set; }
            public MusikSpeed? Unendlich { get; set; }
            public MusikSpeed? Babymode { get; set; }
        }

        public class MusikSpeed
        {
            public int Langsam { get; set; }
            public int Mittel { get; set; }
            public int Schnell { get; set; }
        }

        public class MenueMusik
        {
            public int Eingabe { get; set; }
            public int Main { get; set; }
            public int Einstellungen { get; set; }
            public int Shop { get; set; }
            public int SkinFarben { get; set; }
            public int Anleitung { get; set; }
            public int Statistiken { get; set; }
        }
        class Preise
        {
            public int[]? TailPreis { get; set; }
            public int[]? FoodPreis { get; set; }
            public int[]? RandPreis { get; set; }
            public int[]? FarbenPreis { get; set; }
        }
        class Level
        {
            public int[]? TailLevel { get; set; }
            public int[]? FoodLevel { get; set; }
            public int[]? RandLevel { get; set; }
            public int[]? FarbenLevel { get; set; }
        }
        class Skins
        {
            public string[]? Farben { get; set; }
            public char[]? TailSkins { get; set; }
            public char[]? FoodSkins { get; set; }
            public char[]? RandSkins { get; set; }
        }
        class GameConfig
        {
            public int Weite { get; set; }
            public int Hoehe { get; set; }
            public int MaxPunkte { get; set; }
            public int MaxFutterconfig { get; set; }
            public Positionen? Startpositionen { get; set; }
            public Difficulty? Difficulty { get; set; }
        }

        // XOR-Entschlüsselung
        static class XorCrypt
        {
            static readonly byte[] key =
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
                var encrypted = File.ReadAllBytes(path);
                if (encrypted.Length <= 5) return string.Empty;

                var decrypted = new byte[encrypted.Length - 5]; //Enfernt HEADER SMAKE
                for (int i = 5; i < encrypted.Length; i++)
                    decrypted[i - 5] = (byte)(encrypted[i] ^ key[(i - 5) % key.Length]);

                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}
