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
            if (!Musikplay && !Soundplay)
            {
                currentPlayer?.Stop();
                lastPlayState = false;
                return;
            }

            if (Currentmusik != lastmusik || Musikplay != lastPlayState)
            {
                currentPlayer?.Stop();
                currentPlayer = null;

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string dateipfad;

                if (Musikplay)
                {
                    dateipfad = Path.Combine(basePath, "Sounds", GameData.Filenames[Currentmusik ?? 0]);
                }
                else
                {
                    dateipfad = Path.Combine(basePath, "Sounds", GameData.NoMusikFile);
                }

                if (File.Exists(dateipfad))
                {
                    currentPlayer = new SoundPlayer(dateipfad);
                    currentPlayer.PlayLooping();
                }

                lastmusik = Currentmusik;
                lastPlayState = Musikplay;
            }
        }
    }
}
