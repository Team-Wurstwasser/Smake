using Smake.Speicher;
using System.Media;

namespace Smake
{
    public class Sounds
    {
        
        #pragma warning disable CA1416 // Plattformkompatibilität überprüfen

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

        public static void Playbeep()
        {
            if(Soundplay)
            {
                Console.Beep(700, 100);
            }

        }

        #pragma warning restore CA1416 // Plattformkompatibilität überprüfen

    }
}
