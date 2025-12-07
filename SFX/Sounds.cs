using NAudio.Wave;
using Smake.Speicher;

namespace Smake.SFX
{
    public class Sounds
    {
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

                Melodie(lastmusik > -1 ? lastmusik.Value : 0);
            }
        }

        private static int? lastmusik = -1;
        private static bool lastPlayState = false; // merkt sich, ob Musik an/aus war

        private static WaveOutEvent? waveOutMusik;
        private static AudioFileReader? audioFile;
        private static LoopStream? loopStream;

        public static void Melodie(int Currentmusik)
        {
            if (!OperatingSystem.IsWindows()) return;
            
            if (!Musikplay && !Soundplay)
            {
                StopMusik();
                lastPlayState = false;
                return;
            }

            if (Currentmusik != lastmusik || Musikplay != lastPlayState)
            {
                StopMusik();

                string basePath = AppDomain.CurrentDomain.BaseDirectory;

                string dateipfad = Musikplay
                                    ? Path.Combine(basePath, "Sounds", GameData.Filenames[Currentmusik])
                                    : Path.Combine(basePath, "Sounds", GameData.NoMusikFile);

                if (File.Exists(dateipfad))
                {
                    waveOutMusik = new WaveOutEvent();
                    audioFile = new AudioFileReader(dateipfad);
                    loopStream = new LoopStream(audioFile);
                    waveOutMusik.Init(loopStream);
                    waveOutMusik.Play();
                }

                lastmusik = Currentmusik;
                lastPlayState = Musikplay;
            }
        }

        private static void StopMusik()
        {
            if (waveOutMusik != null)
            {
                waveOutMusik.Stop();
                waveOutMusik.Dispose();
                waveOutMusik = null;
            }

            audioFile?.Dispose();
            audioFile = null;

            loopStream = null;
        }

        public static void Playbeep()
        {
            if (!Soundplay || !OperatingSystem.IsWindows()) return;

            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string dateipfad = Path.Combine(basePath, "Sounds", GameData.BeepFile);

            if (File.Exists(dateipfad))
            {
                var waveOutBeep = new WaveOutEvent();
                var audioFile = new AudioFileReader(dateipfad);

                waveOutBeep.Init(audioFile);
                waveOutBeep.Play();

                waveOutBeep.PlaybackStopped += (s, e) =>
                {
                    waveOutBeep.Dispose();
                    audioFile.Dispose();
                };
            }
        }
    }
}