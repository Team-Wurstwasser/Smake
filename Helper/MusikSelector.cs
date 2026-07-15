using Smake.Enums;
using Smake.Speicher;
using Smake.Values;

namespace Smake.Helper
{
    public static class MusikSelector
    {
        private static readonly int defaultMusik = 1;

        public static int Selector()
        {
            var game = GameData.MusikDaten.Game;
            if (game == null) return defaultMusik;

            var modusTracks = Spielvalues.Gamemode switch
            {
                Gamemodes.Normal => game.Normal,
                Gamemodes.Unendlich => game.Unendlich,
                Gamemodes.Babymode => game.Babymode,
                Gamemodes.BabymodeUnendlich => game.BabymodeUnendlich,
                Gamemodes.MauerModus => game.MauerModus,
                Gamemodes.SchluesselModus => game.SchluesselModus,
                Gamemodes.SprungfutterModus => game.SprungfutterModus,
                Gamemodes.BombenModus => game.BombenModus,
                Gamemodes.ChaosSteuerung => game.ChaosSteuerung,
                _ => null
            };

            return GetTrackByDifficulty(modusTracks, Spielvalues.Difficulty, defaultMusik);
        }

        private static int GetTrackByDifficulty(GameData.MusikSpeed? tracks, Difficultys difficulty, int defaultMusik)
        {
            if (tracks == null) return defaultMusik;

            return difficulty switch
            {
                Difficultys.Slow => tracks.Langsam,
                Difficultys.Medium => tracks.Mittel,
                Difficultys.Fast => tracks.Schnell,
                _ => defaultMusik
            };
        }
    }
}