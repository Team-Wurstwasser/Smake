using Smake.Enums;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Helper
{
    public static class MusikSelector
    {
        public static int? Selector()
        {
            int? currentMusik = null;

            switch (Spielvalues.Gamemode)
            {
                case Gamemodes.Normal:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.Unendlich:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.Unendlich?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.Unendlich?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.Unendlich?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.Babymode:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.Babymode?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.Babymode?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.Babymode?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.BabymodeUnendlich:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.BabymodeUnendlich?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.BabymodeUnendlich?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.BabymodeUnendlich?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.MauerModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.MauerModus?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.MauerModus?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.MauerModus?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.SchluesselModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.SchluesselModus?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.SchluesselModus?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.SchluesselModus?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.SprungfutterModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.SprungfutterModus?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.SprungfutterModus?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.SprungfutterModus?.Schnell ?? 1,
                        _ => null
                    };
                    break;
                case Gamemodes.BombenModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.BombenModus?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.BombenModus?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.BombenModus?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.ChaosSteuerung:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => GameData.MusikDaten.Game?.ChaosSteuerung?.Langsam ?? 1,
                        Difficultys.Medium => GameData.MusikDaten.Game?.ChaosSteuerung?.Mittel ?? 1,
                        Difficultys.Fast => GameData.MusikDaten.Game?.ChaosSteuerung?.Schnell ?? 1,
                        _ => null
                    };
                    break;
            }

            return currentMusik;
        }

    }
}
