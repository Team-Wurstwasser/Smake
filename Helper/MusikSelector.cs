using Smake.Speicher;
using Smake.Enums;

namespace Smake.Helper
{
    public static class MusikSelector
    {
        public static readonly Dictionary<string, Dictionary<Difficulty, int>> MusikMap =
            new()
            {
                {
                    "Normal", new Dictionary<Difficulty, int>
                    {   
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Normal.Langsam },
                        { Difficulty.Mittel, GameData.MusikDaten.Game.Normal.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Normal.Schnell }
                    }
                },
                {
                    "Unendlich", new Dictionary<Difficulty, int>
                    {
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Unendlich.Langsam },
                        { Difficulty.Mittel, GameData.MusikDaten.Game.Unendlich.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Unendlich.Schnell }
                    }
                },
                {
                    "Babymode", new Dictionary<Difficulty, int>
                    {
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Babymode.Langsam },
                        { Difficulty.Mittel,  GameData.MusikDaten.Game.Babymode.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Babymode.Schnell }
                    }
                },
                {
                    "Mauer-Modus", new Dictionary<Difficulty, int>
                    {
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Mauer_Modus.Langsam },
                        { Difficulty.Mittel, GameData.MusikDaten.Game.Mauer_Modus.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Mauer_Modus.Schnell }
                    }
                },
                {
                    "Schlüssel-Modus", new Dictionary<Difficulty, int>
                    {
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Schluessel_Modus.Langsam },
                        { Difficulty.Mittel, GameData.MusikDaten.Game.Schluessel_Modus.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Schluessel_Modus.Schnell }
                    }
                },
                {
                    "Sprungfutter-Modus", new Dictionary<Difficulty, int>
                    {
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Sprungfutter_Modus.Langsam },
                        { Difficulty.Mittel, GameData.MusikDaten.Game.Sprungfutter_Modus.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Sprungfutter_Modus.Schnell }
                    }
                },
                {
                    "Chaos-Steuerung", new Dictionary<Difficulty, int>
                    {
                        { Difficulty.Langsam, GameData.MusikDaten.Game.Chaos_Steuerung.Langsam },
                        { Difficulty.Mittel, GameData.MusikDaten.Game.Chaos_Steuerung.Mittel },
                        { Difficulty.Schnell, GameData.MusikDaten.Game.Chaos_Steuerung.Schnell }
                    }
                }
            };
    }
}
