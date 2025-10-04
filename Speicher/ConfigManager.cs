using System.Text.Json;

namespace Smake.Speicher
{
    public static class ConfigManager
    {
        private static readonly string ConfigPath = "config.json";

        static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public static string Language { get; private set; } = "de"; // Standard

        // Lädt die Konfigurationsdatei
        public static void Load()
        {
            if (!File.Exists(ConfigPath))
            {
                Save(); // Erstellt Standard-Datei
                return;
            }

            try
            {
                string json = File.ReadAllText(ConfigPath);
                var config = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (config != null && config.TryGetValue("language", out string? lang) && !string.IsNullOrWhiteSpace(lang))
                {
                    Language = lang;
                }
                else
                {
                    Save(); // Falls kaputt → neu schreiben
                }
            }
            catch
            {
                Console.WriteLine("⚠ Fehler beim Laden der Konfigurationsdatei – Sprache auf 'de' gesetzt.");
                Save(); // Neu schreiben
            }
        }

        // Speichert die aktuelle Sprache
        public static void Save()
        {
            var config = new Dictionary<string, string>
            {
                { "language", Language }
            };

            string json = JsonSerializer.Serialize(config, JsonOptions);
            File.WriteAllText(ConfigPath, json);
        }

        // Ändert die Sprache und speichert sie sofort
        public static void SetLanguage(string newLang)
        {
            Language = newLang;
            Save();
        }
    }
}
