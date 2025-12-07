using Smake.Speicher;
using NAudio.Wave;

namespace Smake.SFX
{
    public class Sounds
    {
        public static bool Musikplay { get; set; }
        public static int? Currentmusik { private get; set; } = 0;
        public static bool Soundplay { get; set; }
        private static int? lastmusik = -1;
        private static bool lastPlayState = false; // merkt sich, ob Musik an/aus war

        private static WaveOutEvent? waveOutMusik;
        private static AudioFileReader? audioFile;
        private static LoopStream? loopStream;

        public static void Melodie()
        {
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
                                    ? Path.Combine(basePath, "Sounds", GameData.Filenames[Currentmusik ?? 0])
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
            if (!Soundplay) return;

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