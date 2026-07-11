using Smake.Speicher;

namespace Smake.SFX
{
    public class Sounds
    {
        private static readonly SoundPlayer _musicPlayer = new();
        private static readonly SoundPlayer _sfxPlayer = new();

        private static bool _musikplay;

        public static bool Musikplay
        {
            get => _musikplay;
            set
            {
                if (_musikplay == value)
                {
                    return;
                }
                _musikplay = value;

                Melodie(lastmusik > -1 ? lastmusik.Value : 0);
            }
        }

        private static bool _soundplay;

        public static bool Soundplay
        {
            get => _soundplay;
            set
            {
                if (_soundplay == value)
                {
                    return;
                }
                _soundplay = value;
            }
        }

        private static int? lastmusik = -1;
        private static bool lastPlayState = false; // merkt sich, ob Musik an/aus war

        public static void Melodie(int Currentmusik)
        {
            if (!Musikplay)
            {
                StopMusik();
                lastPlayState = false;
                return;
            }

            if (Currentmusik != lastmusik || Musikplay != lastPlayState)
            {
                StopMusik();

                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                string dateipfad = Path.Combine(basePath, "Sounds", GameData.Filenames[Currentmusik]);

                if (File.Exists(dateipfad))
                {
                    Task.Run(async () => await _musicPlayer.Play(dateipfad, loop: true));
                }

                lastmusik = Currentmusik;
                lastPlayState = Musikplay;
            }
        }

        private static void StopMusik()
        {
            Task.Run(async () => await _musicPlayer.Stop());
        }

        public static void Playbeep()
        {
            if (!Soundplay) return;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dateipfad = Path.Combine(basePath, "Sounds", GameData.BeepFile);

            if (File.Exists(dateipfad))
            {
                Task.Run(async () => await _sfxPlayer.Play(dateipfad));
            }
        }
    }
}