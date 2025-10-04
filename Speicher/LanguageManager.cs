using System.Text.Json;

namespace Smake.Speicher
{
    public static class LanguageManager
    {
        private static Dictionary<string, object> _data = [];

        public static void Load(string lang)
        {
            string path = $"Languages/{lang}.json";
            string json = File.ReadAllText(path);
            _data = JsonSerializer.Deserialize<Dictionary<string, object>>(json)!;
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

        // Platzhalter ersetzen: {name}
        public static string Format(string key, Dictionary<string, string> values)
        {
            string text = Get(key);
            foreach (var kv in values)
                text = text.Replace("{" + kv.Key + "}", kv.Value);
            return text;
        }
    }
}
