using Smake.io.Speicher;
using System.Media;

namespace Smake.io
{
    public class Musik
    {
        public static bool musikplay;
        public static int currentmusik = 0;
        public static bool soundplay;

        private static SoundPlayer? currentPlayer;
        private static int lastmusik = -1;
        private static bool lastPlayState = false; // merkt sich, ob Musik an/aus war

        public static void Melodie()
        {
            // Prüfen, ob sich Musik oder der Status geändert hat
            if (currentmusik != lastmusik || musikplay != lastPlayState)
            {
                // Stoppe vorherige Musik
                currentPlayer?.Stop();
                currentPlayer = null;

                // Dateipfad bestimmen
                string dateipfad;
                if (musikplay)
                {
                    dateipfad = Path.Combine("Sounds", GameData.Filenames[currentmusik]);
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
                lastmusik = currentmusik;
                lastPlayState = musikplay;
            }

            Thread.Sleep(50); // CPU schonen
        }
    }
}
