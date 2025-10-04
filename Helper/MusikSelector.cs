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
                case "Normal":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Normal.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Normal.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Normal.Schnell,
                        _ => null
                    };
                    break;

                case "Unendlich":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Unendlich.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Unendlich.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Unendlich.Schnell,
                        _ => null
                    };
                    break;

                case "Babymode":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Babymode.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Babymode.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Babymode.Schnell,
                        _ => null
                    };
                    break;

                case "Mauer-Modus":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Mauer_Modus.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Mauer_Modus.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Mauer_Modus.Schnell,
                        _ => null
                    };
                    break;

                case "Schlüssel-Modus":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Schluessel_Modus.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Schluessel_Modus.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Schluessel_Modus.Schnell,
                        _ => null
                    };
                    break;

                case "Sprungfutter-Modus":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Sprungfutter_Modus.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Sprungfutter_Modus.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Sprungfutter_Modus.Schnell,
                        _ => null
                    };
                    break;

                case "Chaos-Steuerung":
                    currentMusik = Spielvalues.Difficulty switch
                    {
                        "Langsam" => GameData.MusikDaten.Game.Chaos_Steuerung.Langsam,
                        "Mittel" => GameData.MusikDaten.Game.Chaos_Steuerung.Mittel,
                        "Schnell" => GameData.MusikDaten.Game.Chaos_Steuerung.Schnell,
                        _ => null
                    };
                    break;
            }

            return currentMusik;
        }

    }
}
