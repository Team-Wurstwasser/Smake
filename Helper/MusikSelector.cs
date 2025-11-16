using Smake.Speicher;
using Smake.Values;

namespace Smake.Helper
{
    public class MusikSelector
    {
        public static int? Selector()
        {
            int? currentMusik = null;

            switch (Spielvalues.GamemodeInt)
            {
                case 1:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Normal.Langsam,
                        2 => GameData.MusikDaten.Game.Normal.Mittel,
                        3 => GameData.MusikDaten.Game.Normal.Schnell,
                        _ => null
                    };
                    break;

                case 2:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Unendlich.Langsam,
                        2 => GameData.MusikDaten.Game.Unendlich.Mittel,
                        3 => GameData.MusikDaten.Game.Unendlich.Schnell,
                        _ => null
                    };
                    break;

                case 3:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Babymode.Langsam,
                        2 => GameData.MusikDaten.Game.Babymode.Mittel,
                        3 => GameData.MusikDaten.Game.Babymode.Schnell,
                        _ => null
                    };
                    break;

                case 4:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Babymode_Unendlich.Langsam,
                        2 => GameData.MusikDaten.Game.Babymode_Unendlich.Mittel,
                        3 => GameData.MusikDaten.Game.Babymode_Unendlich.Schnell,
                        _ => null
                    };
                    break;

                case 5:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Mauer_Modus.Langsam,
                        2 => GameData.MusikDaten.Game.Mauer_Modus.Mittel,
                        3 => GameData.MusikDaten.Game.Mauer_Modus.Schnell,
                        _ => null
                    };
                    break;

                case 6:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Schluessel_Modus.Langsam,
                        2 => GameData.MusikDaten.Game.Schluessel_Modus.Mittel,
                        3 => GameData.MusikDaten.Game.Schluessel_Modus.Schnell,
                        _ => null
                    };
                    break;

                case 7:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Sprungfutter_Modus.Langsam,
                        2 => GameData.MusikDaten.Game.Sprungfutter_Modus.Mittel,
                        3 => GameData.MusikDaten.Game.Sprungfutter_Modus.Schnell,
                        _ => null
                    };
                    break;
                case 8:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Bomben_Modus.Langsam,
                        2 => GameData.MusikDaten.Game.Bomben_Modus.Mittel,
                        3 => GameData.MusikDaten.Game.Bomben_Modus.Schnell,
                        _ => null
                    };
                    break;

                case 9:
                    currentMusik = Spielvalues.DifficultyInt switch
                    {
                        1 => GameData.MusikDaten.Game.Chaos_Steuerung.Langsam,
                        2 => GameData.MusikDaten.Game.Chaos_Steuerung.Mittel,
                        3 => GameData.MusikDaten.Game.Chaos_Steuerung.Schnell,
                        _ => null
                    };
                    break;
            }

            return currentMusik;
        }

    }
}
