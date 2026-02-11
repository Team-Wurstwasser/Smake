using Smake.Enums;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Helper
{
    public static class MusikSelector
    {
        public static int Selector()
        {
            int currentMusik = 1;

            switch (Spielvalues.Gamemode)
            {
                case Gamemodes.Normal:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.Normal.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.Normal.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.Normal.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.Unendlich:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.Unendlich.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.Unendlich.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.Unendlich.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.Babymode:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.Babymode.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.Babymode.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.Babymode.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.BabymodeUnendlich:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.BabymodeUnendlich.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.BabymodeUnendlich.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.BabymodeUnendlich.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.MauerModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.MauerModus.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.MauerModus.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.MauerModus.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.SchluesselModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.SchluesselModus.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.SchluesselModus.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.SchluesselModus.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.SprungfutterModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.SprungfutterModus.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.SprungfutterModus.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.SprungfutterModus.Fast,
                        _ => 1
                    };
                    break;
                case Gamemodes.BombenModus:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.BombenModus.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.BombenModus.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.BombenModus.Fast,
                        _ => 1
                    };
                    break;

                case Gamemodes.ChaosSteuerung:
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        Difficultys.Slow => ConfigSystem.Sounds.Musik.Game.ChaosSteuerung.Slow,
                        Difficultys.Medium => ConfigSystem.Sounds.Musik.Game.ChaosSteuerung.Medium,
                        Difficultys.Fast => ConfigSystem.Sounds.Musik.Game.ChaosSteuerung.Fast,
                        _ => 1
                    };
                    break;
            }

            return currentMusik;
        }

    }
}
