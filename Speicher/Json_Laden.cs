using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        public class Difficulty
        {
            public int Langsam { get; set; }
            public int Mittel { get; set; }
            public int Schnell { get; set; }
        }


        // Allgemeine Optionen für JSON: Groß-/Kleinschreibung wird ignoriert
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

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

            // Farben
            Farben = data?.Farben?.Select(f => Enum.TryParse(f, true, out ConsoleColor color) ? (ConsoleColor?)color : null).Where(c => c.HasValue).Select(c => c.Value).ToArray() ?? [];
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

        // Allgemeine Funktion zum Laden von JSON-Dateien
        private static T LoadJson<T>(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"JSON file not found: {path}");

            var json = File.ReadAllText(path);
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
    }
}
