using Smake.io.Speicher;
using System.Media;

namespace Smake.io
{
    public class Musik
    {
        public static bool Musikplay { get; set; }
        public static int? Currentmusik { private get; set; } = 0;
        public static bool Soundplay { get; set; }

        private static SoundPlayer? currentPlayer;
        private static int? lastmusik = -1;
        private static bool lastPlayState = false; // merkt sich, ob Musik an/aus war

        public static void Melodie()
        {
            // Prüfen, ob sich Musik oder der Status geändert hat
            if (Currentmusik != lastmusik || Musikplay != lastPlayState)
            {
                // Stoppe vorherige Musik
                currentPlayer?.Stop();
                currentPlayer = null;

                // Dateipfad bestimmen
                string dateipfad;
                if (Musikplay)
                {
                    dateipfad = Path.Combine("Sounds", GameData.Filenames[Currentmusik ?? 0]);
                }
                else
                {
                    dateipfad = Path.Combine("Sounds", GameData.NoMusikFile);
                }

                // Prüfen, ob Datei existiert
                if (File.Exists(dateipfad))
                {
                    currentPlayer = new SoundPlayer(dateipfad);
                    currentPlayer.PlayLooping();
                }

                // Status merken
                lastmusik = Currentmusik;
                lastPlayState = Musikplay;
            }

            Thread.Sleep(50); // CPU schonen
        }
    }
}
