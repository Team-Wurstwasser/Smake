using System.Text.Json;

namespace Smake.Speicher
{
    public static class LanguageManager
    {
        private static Dictionary<string, object> data = [];

        public static string? Language { get; private set; }

        private const string Config = "config.config";
        private const string DefaultLanguage = "de";

        public static void Speichern_Laden(string aktion, string newLang = "")
        {
            if (!File.Exists(Config))
            {
                newLang = DefaultLanguage;
                Speichern(newLang);
            }

            switch (aktion)
            {
                case "Speichern":
                    Speichern(newLang);
                    break;
                case "Laden":
                    if (!Laden())
                    {
                        Console.WriteLine($"Fehler beim Laden der konfigurierten Sprache. Versuche Standard-Sprache: {DefaultLanguage.ToUpper()}");
                        Console.ReadKey(true);

                        Language = DefaultLanguage;
                        if (!Laden())
                        {
                            Console.Error.WriteLine($"Fehler: Das Laden der Standard-Sprache '{DefaultLanguage.ToUpper()}' ist fehlgeschlagen. Das Programm wird beendet.");
                            Console.ReadKey(true);
                            Environment.Exit(1);
                        }
                        else
                        {
                            Console.WriteLine($"Standard-Sprache '{DefaultLanguage.ToUpper()}' erfolgreich geladen.");
                            Console.ReadKey(true);
                        }
                    }
                    break;
            }
        }

        static void Speichern(string newLang)
        {
            File.WriteAllText(Config, newLang);
            Laden();
        }

        static bool Laden()
        {
            string configContent = File.ReadAllText(Config);

            if (string.IsNullOrEmpty(configContent))
            {
                return false;
            }

            Language = configContent.Trim();

            string langPath = $"Languages/{Language}.json";

            if (!File.Exists(langPath))
            {
                return false;
            }

            string json = File.ReadAllText(langPath);

            try
            {
                var deserializedData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                data = deserializedData ?? [];

                return true;
            }
            catch 
            {
                data = [];
                return false;
            }
        }

        public static List<(string, string)> GetAvailableLanguages()
        {
            List<(string, string)> results = [];

            if (!Directory.Exists("Languages"))
                return results;

            foreach (var file in Directory.GetFiles("Languages", "*.json"))
            {
                string code = Path.GetFileNameWithoutExtension(file);

                try
                {
                    string json = File.ReadAllText(file);
                    using var doc = JsonDocument.Parse(json);

                    string displayName = doc.RootElement.TryGetProperty("languageName", out var prop) ? prop.GetString() ?? code.ToUpper() : code.ToUpper();

                    results.Add((code, displayName));
                }
                catch
                {
                    results.Add((code, code.ToUpper()));
                }
            }

            return results;
        }

        private static object? ResolveKey(string key)
        {
            string[] parts = key.Split('.');
            object? current = data;
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
    }
}