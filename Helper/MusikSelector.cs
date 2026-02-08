using Smake.Enums;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Helper
{
    public class MusikSelector
    {
        public static int? Selector()
        {
            int? currentMusik = null;

            switch (Spielvalues.Gamemode)
            {
                case Gamemodes.Normal:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.Unendlich:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.Babymode:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.BabymodeUnendlich:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.MauerModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.SchlüsselModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.SprungfutterModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;
                case Gamemodes.BombenModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;

                case Gamemodes.ChaosSteuerung:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.slow => GameData.MusikDaten.Game?.Normal?.Langsam ?? 1,
                        Difficultys.medium => GameData.MusikDaten.Game?.Normal?.Mittel ?? 1,
                        Difficultys.fast => GameData.MusikDaten.Game?.Normal?.Schnell ?? 1,
                        _ => null
                    };
                    break;
            }

            return currentMusik;
        }

    }
}
