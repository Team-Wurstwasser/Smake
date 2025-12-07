using NAudio.Wave;
using System.Runtime.InteropServices;
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
                if (_musikplay == value) return;
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
                if (_soundplay == value) return;
                _soundplay = value;
                Melodie(lastmusik > -1 ? lastmusik.Value : 0);
            }
        }

        private static int? lastmusik = -1;
        private static bool lastPlayState = false;

        private static IWavePlayer? waveOutMusik;
        private static AudioFileReader? audioFile;
        private static LoopStream? loopStream;

        private static IWavePlayer CreateOutputDevice()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WaveOutEvent();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return new WaveOutEvent();
            }
            return new WaveOutEvent();
        }

        public static void Melodie(int Currentmusik)
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
                                        ? Path.Combine(basePath, "Sounds", GameData.Filenames[Currentmusik])
                                        : Path.Combine(basePath, "Sounds", GameData.NoMusikFile);

                if (File.Exists(dateipfad))
                {
                    waveOutMusik = CreateOutputDevice();

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
                var waveOutBeep = CreateOutputDevice();

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