using System.Text.Json;
using Smake.Helper;

namespace Smake.Speicher
{
    public static class ConfigSystem
    {
        public static SoundConfig Sounds { get; private set; } = GetDefaultSounds();
        public static MarketConfig Prices { get; private set; } = GetDefaultPrices();
        public static MarketConfig Levels { get; private set; } = GetDefaultLevels();
        public static SkinsConfig Skins { get; private set; } = GetDefaultSkins();
        public static GameSettings Game { get; private set; } = new();

        static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static void LoadAllConfigs()
        {
            Sounds = Load<SoundConfig>("Jsons/sounds.json") ?? Sounds;
            Prices = Load<MarketConfig>("Jsons/preise.json") ?? Prices;
            Levels = Load<MarketConfig>("Jsons/level.json") ?? Levels;
            Game = Load<GameSettings>("Jsons/game.json") ?? Game;

            var loadedSkins = LoadSkins("Jsons/skins.json");
            if (loadedSkins != null) Skins = loadedSkins;
        }

        static MarketConfig GetDefaultPrices() => new()
        {
            Tail = [300, 350, 400, 450, 500],
            Food = [350, 500, 400, 300, 250, 250],
            Rand = [200, 400, 400, 350, 300, 400],
            Farben = [100, 259, 100, 250, 200, 300, 100, 300, 175, 250, 100, 250, 450, 500]
        };

        static MarketConfig GetDefaultLevels() => new()
        {
            Tail = [2, 4, 6, 8, 10],
            Food = [2, 4, 6, 8, 10, 12, 14],
            Rand = [2, 4, 6, 8, 10, 12, 14, 0],
            Farben = [0, 4, 0, 6, 8, 10, 0, 10, 20, 25, 0, 20, 25, 30]
        };

        static SkinsConfig GetDefaultSkins() => new()
        {
            Farben = [ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.DarkMagenta, ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Gray, ConsoleColor.DarkGray],
            Tail = ['+', 'x', '~', '=', '-', 'o', '•'],
            Food = ['*', '@', '$', '♥', '%', '¤', '&'],
            Rand = ['█', '#', '▓', '░', '■', '▌', '▒']
        };

        static SoundConfig GetDefaultSounds() => new()
        {
            Filenames = ["Smake.wav", "Smake2.wav"],
            NoMusikFile = "NoMusik.wav"
        };

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

        static SkinsConfig? LoadSkins(string path)
        {
            var raw = Load<SkinsRaw>(path);
            if (raw == null) return null;

            return new SkinsConfig
            {
                Tail = raw.Tail ?? GetDefaultSkins().Tail,
                Food = raw.Food ?? GetDefaultSkins().Food,
                Rand = raw.Rand ?? GetDefaultSkins().Rand,
                Farben = raw.Farben?.Select(f => Enum.TryParse(f, true, out ConsoleColor c) ? c : ConsoleColor.White).ToArray() ?? GetDefaultSkins().Farben
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
            public int[] Tail { get; set; } = [];
            public int[] Food { get; set; } = [];
            public int[] Rand { get; set; } = [];
            public int[] Farben { get; set; } = [];
        }

        public class SoundConfig
        {
            public string[] Filenames { get; set; } = [];
            public string NoMusikFile { get; set; } = "";
            public Musik Musik { get; set; } = new();
        }

        public class SkinsConfig
        {
            public char[] Tail { get; set; } = [];
            public char[] Food { get; set; } = [];
            public char[] Rand { get; set; } = [];
            public ConsoleColor[] Farben { get; set; } = [];
        }

        class SkinsRaw
        {
            public string[]? Farben { get; set; }
            public char[]? Tail { get; set; }
            public char[]? Food { get; set; }
            public char[]? Rand { get; set; }
        }

        public class GameSettings
        {
            public int Weite { get; set; } = 41;
            public int Hoehe { get; set; } = 20;
            public int MaxPunkte { get; set; } = 20;
            public int MaxFutterconfig { get; set; } = 10;
            public int TeleportInterval { get; set; } = 60;
            public int TailStartLaenge { get; set; } = 3;
            public Positionen Startpositionen { get; set; } = new();
            public Difficulty Difficulty { get; set; } = new();
        }

        public class Positionen
        {
            public Point Spieler1 { get; set; } = new() { X = 36, Y = 4 };
            public Point Spieler2 { get; set; } = new() { X = 4, Y = 4 };
        }

        public class Point { public int X { get; set; } public int Y { get; set; } }

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
            public int Input { get; set; } = 0;
        }

        public class MusikSpeed
        {
            public int Slow { get; set; } = 1;
            public int Medium { get; set; } = 1;
            public int Fast { get; set; } = 1;
        }

        public class MenueMusik
        {
            public int Settings { get; set; } = 0;
            public int Shop { get; set; } = 0;
            public int SkinColors { get; set; } = 0;
            public int Statistics { get; set; } = 0;
            public int Instructions { get; set; } = 0;
            public int MainMenu { get; set; } = 0;
        }
    }
}