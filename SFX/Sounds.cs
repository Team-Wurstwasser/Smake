using Smake.Speicher;
using System.Media;

namespace Smake.SFX
{
    public class Sounds
    {
        public static bool Musikplay { get; set; }
        public static int Currentmusik { private get; set; }
        public static bool Soundplay { get; set; }

        private static SoundPlayer? currentPlayer;
        private static int? lastmusik = -1;
        private static bool lastPlayState; // merkt sich, ob Musik an/aus war

        public static void Melodie(int Currentmusik)
        {
            if (!OperatingSystem.IsWindows()) return;

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
                    dateipfad = Path.Combine(basePath, "Sounds", GameData.Filenames[Currentmusik]);
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
            if (!Soundplay || !OperatingSystem.IsWindows()) return;

            Console.Beep(700, 100);
        }
    }
}