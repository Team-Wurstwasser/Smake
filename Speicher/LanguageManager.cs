using System.Text.Json;

namespace Smake.Speicher
{
    public static class LanguageManager
    {
        private static Dictionary<string, object> _data = [];

        public static string? Language { get; private set; }

        private static readonly string ConfigPath = "config.json";
        private static readonly string DefaultLanguage = "de";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public static void Load()
        {
            // 1️. Config laden oder Standard anlegen
            if (!File.Exists(ConfigPath))
            {
                Save();
            }

            // 2️. Sprache aus config.json lesen
            try
            {
                var configJson = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<Dictionary<string, string>>(configJson);

                if (config is { } && config.TryGetValue("language", out var lang) && !string.IsNullOrWhiteSpace(lang))
                {
                    Language = lang;
                }
                else
                {
                    Console.WriteLine("⚠ Ungültiger Spracheintrag – Standard 'de' wird verwendet.");
                    Console.ReadKey(true);
                    SetLanguage(DefaultLanguage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Fehler beim Laden der Konfigurationsdatei: {ex.Message}");
                Console.ReadKey(true);
                SetLanguage(DefaultLanguage);
            }

            // 3️. Sprachdatei laden (mit Fallback auf 'de')
            string langPath = $"Languages/{Language}.json";
            if (!File.Exists(langPath))
            {
                Console.WriteLine($"⚠ Sprachdatei '{Language}.json' nicht gefunden – Fallback auf '{DefaultLanguage}.json'.");
                Console.ReadKey(true);
                SetLanguage(DefaultLanguage);
                langPath = $"Languages/{DefaultLanguage}.json";
            }

            try
            {
                string json = File.ReadAllText(langPath);
                var data = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                if (data is null || data.Count == 0)
                {
                    Console.WriteLine($"⚠ Sprachdatei '{Language}.json' ist leer oder ungültig.");
                    Console.ReadKey(true);
                    _data = new Dictionary<string, object> { ["error"] = "Invalid language file" };
                }
                else
                {
                    _data = data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Fehler beim Laden der Sprachdatei ({ex.Message}).");
                Console.ReadKey(true);
                _data = new Dictionary<string, object>
                {
                    ["fallback"] = "Language data unavailable."
                };
            }
        }

        private static void Save()
        {
            var config = new Dictionary<string, string?>
            {
                { "language", Language }
            };

            string json = JsonSerializer.Serialize(config, JsonOptions);
            File.WriteAllText(ConfigPath, json);
        }

        public static void SetLanguage(string newLang)
        {
            Language = newLang;
            Save();
            Load();
        }

        public static Dictionary<string, string> GetAvailableLanguages()
        {
            var result = new Dictionary<string, string>();

            if (!Directory.Exists("Languages"))
                return result;

            foreach (var file in Directory.GetFiles("Languages", "*.json"))
            {
                string code = Path.GetFileNameWithoutExtension(file);

                try
                {
                    string json = File.ReadAllText(file);
                    using var doc = JsonDocument.Parse(json);

                    string displayName = doc.RootElement.TryGetProperty("languageName", out var prop)
                        ? prop.GetString() ?? code.ToUpper()
                        : code.ToUpper();

                    result[code] = displayName;
                }
                catch
                {
                    result[code] = code.ToUpper();
                }
            }

            return result;
        }

        private static object? ResolveKey(string key)
        {
            string[] parts = key.Split('.');
            object? current = _data;
            foreach (var part in parts)
            {
                if (current is JsonElement elem)
                {
                    if (elem.ValueKind == JsonValueKind.Object && elem.TryGetProperty(part, out var child))
                        current = child;
                    else
                        return null;
                }
                else if (current is Dictionary<string, object> dict && dict.TryGetValue(part, out var next))
                {
                    current = next;
                }
                else return null;
            }
            return current;
        }

        public static string Get(string key)
        {
            var val = ResolveKey(key);
            if (val is JsonElement elem)
            {
                if (elem.ValueKind == JsonValueKind.String) return elem.GetString() ?? $"[{key}]";
            }
            return $"[{key}]";
        }

        public static string[] GetArray(string key)
        {
            var val = ResolveKey(key);
            if (val is JsonElement elem && elem.ValueKind == JsonValueKind.Array)
            {
                return [.. elem.EnumerateArray().Select(x => x.GetString() ?? "")];
            }
            return [];
        }

        public static string Format(string key, Dictionary<string, string> values)
        {
            string text = Get(key);
            foreach (var kv in values)
                text = text.Replace("{" + kv.Key + "}", kv.Value);
            return text;
        }
    }
}