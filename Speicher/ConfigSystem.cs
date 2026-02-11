using System.Text.Json;
using System.Text.Json.Serialization;
using Smake.Helper;

namespace Smake.Speicher
{
    public static class ConfigSystem
    {
        public static SoundConfig Sounds { get; private set; } = new();
        public static MarketConfig Prices { get; private set; } = new();
        public static MarketConfig Levels { get; private set; } = new();
        public static SkinsConfig Skins { get; private set; } = new();
        public static GameSettings Game { get; private set; } = new();

        static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static void LoadAllConfigs()
        {
            Sounds = Load<SoundConfig>("Jsons/sounds.json") ?? new();
            Prices = Load<MarketConfig>("Jsons/preise.json") ?? new();
            Levels = Load<MarketConfig>("Jsons/level.json") ?? new();
            Game = Load<GameSettings>("Jsons/game.json") ?? new();
            Skins = LoadSkins("Jsons/skins.json");
        }

        static T? Load<T>(string path) where T : class
        {
            try
            {
                if (!File.Exists(path))
                {
                    HandleError("jsonload.fileNotFound", path);
                    return null;
                }

                string json = XorCryptoHelper.Decrypt(path);
                return JsonSerializer.Deserialize<T>(json, JsonOptions);
            }
            catch (Exception ex)
            {
                HandleError("jsonload.loadError", path, ex.Message);
                return null;
            }
        }

        static SkinsConfig LoadSkins(string path)
        {
            var raw = Load<SkinsRaw>(path);
            if (raw == null) return new SkinsConfig();

            return new SkinsConfig
            {
                TailSkins = raw.TailSkins ?? [],
                FoodSkins = raw.FoodSkins ?? [],
                RandSkins = raw.RandSkins ?? [],
                Farben = raw.FarbenRaw?.Select(f => Enum.TryParse(f, true, out ConsoleColor c) ? c : ConsoleColor.Gray).ToArray() ?? []
            };
        }

        static void HandleError(string key, string path, string message = "")
        {
            string error = LanguageSystem.Get(key).Replace("{path}", path).Replace("{message}", message);
            Console.WriteLine(error);
            Console.ReadKey(true);
        }

        public class MarketConfig
        {
            [JsonPropertyName("TailPreis")] public int[] TailPreis { get; set; } = [];
            [JsonPropertyName("FoodPreis")] public int[] FoodPreis { get; set; } = [];
            [JsonPropertyName("RandPreis")] public int[] RandPreis { get; set; } = [];
            [JsonPropertyName("FarbenPreis")] public int[] FarbenPreis { get; set; } = [];

            [JsonPropertyName("TailLevel")] public int[] TailLevel { get; set; } = [];
            [JsonPropertyName("FoodLevel")] public int[] FoodLevel { get; set; } = [];
            [JsonPropertyName("RandLevel")] public int[] RandLevel { get; set; } = [];
            [JsonPropertyName("FarbenLevel")] public int[] FarbenLevel { get; set; } = [];
        }

        public class SoundConfig
        {
            public string[] Filenames { get; set; } = [];
            public string NoMusikFile { get; set; } = "";
            public string BeepFile { get; set; } = "";
            public Musik Musik { get; set; } = new();
        }

        public class SkinsConfig
        {
            public char[] TailSkins { get; set; } = [];
            public char[] FoodSkins { get; set; } = [];
            public char[] RandSkins { get; set; } = [];
            public ConsoleColor[] Farben { get; set; } = [];
        }

        class SkinsRaw
        {
            [JsonPropertyName("Farben")] public string[]? FarbenRaw { get; set; }
            [JsonPropertyName("Tailskins")] public char[]? TailSkins { get; set; }
            [JsonPropertyName("Foodskins")] public char[]? FoodSkins { get; set; }
            [JsonPropertyName("Randskins")] public char[]? RandSkins { get; set; }
        }

        public class GameSettings
        {
            public int Weite { get; set; }
            public int Hoehe { get; set; }
            public int MaxPunkte { get; set; }
            public int MaxFutterconfig { get; set; }
            public int TeleportInterval { get; set; }
            public int TailStartLaenge { get; set; }
            public Positionen Startpositionen { get; set; } = new();
            public Difficulty Difficulty { get; set; } = new();
        }

        public class Positionen
        {
            public Point Spieler1 { get; set; } = new();
            public Point Spieler2 { get; set; } = new();
        }

        public class Point 
        { 
            public int X { get; set; } 
            public int Y { get; set; } 
        }

        public class Difficulty
        {
            public int Slow { get; set; } = 150;
            public int Medium { get; set; } = 100;
            public int Fast { get; set; } = 50;
        }

        public class Musik
        {
            public GameMusik Game { get; set; } = new();
            public MenueMusik Menue { get; set; } = new();
        }

        public class GameMusik
        {
            public MusikSpeed Normal { get; set; } = new();
            public MusikSpeed Unendlich { get; set; } = new();
            public MusikSpeed Babymode { get; set; } = new();
            public MusikSpeed BabymodeUnendlich { get; set; } = new();
            public MusikSpeed MauerModus { get; set; } = new();
            public MusikSpeed SchluesselModus { get; set; } = new();
            public MusikSpeed SprungfutterModus { get; set; } = new();
            public MusikSpeed BombenModus { get; set; } = new();
            public MusikSpeed ChaosSteuerung { get; set; } = new();
            public int Input { get; set; }
        }

        public class MusikSpeed
        {
            public int Slow { get; set; }
            public int Medium { get; set; }
            public int Fast { get; set; }
        }

        public class MenueMusik
        {
            public int Settings { get; set; }
            public int Shop { get; set; }
            public int SkinColors { get; set; }
            public int Statistics { get; set; }
            public int Instructions { get; set; }
            public int MainMenu { get; set; }
        }
    }
}