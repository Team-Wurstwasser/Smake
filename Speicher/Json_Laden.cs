using Smake.Helper;
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
        public static int TeleportInterval { get; private set; }
        public static int TailStartLaenge { get; private set; }
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
                TeleportInterval = data?.TeleportInterval ?? 0;
                TailStartLaenge = data?.TailStartLaenge ?? 0;
                Startpositionen = data?.Startpositionen ?? new Positionen();
                SpielSchwierigkeit = data?.Difficulty ?? new Difficulty();
            });
        }

        static void Load<T>(string path, Action<T?> setData)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException(LanguageManager.Get("jsonload.fileNotFound").Replace("{path}", path));

                string json = XorCryptoHelper.Decrypt(path);
                var data = JsonSerializer.Deserialize<T>(json, JsonOptions);
                setData(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(LanguageManager.Get("jsonload.loadError").Replace("{path}", path).Replace("{message}", ex.Message));
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
            public MusikSpeed? Babymode_Unendlich { get; set; }
            public MusikSpeed? Mauer_Modus { get; set; }
            public MusikSpeed? Schluessel_Modus { get; set; }
            public MusikSpeed? Sprungfutter_Modus { get; set; }
            public MusikSpeed? Bomben_Modus { get; set; }
            public MusikSpeed? Chaos_Steuerung { get; set; }
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
            public int TeleportInterval { get; set; }
            public int TailStartLaenge { get; set; }
            public Positionen? Startpositionen { get; set; }
            public Difficulty? Difficulty { get; set; }
        }
    }
}
