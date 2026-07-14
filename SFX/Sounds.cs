using Ownaudio.Core;
using OwnaudioNET;
using OwnaudioNET.Mixing;
using OwnaudioNET.Sources;
using Smake.Speicher;

namespace Smake.SFX
{
    public static class Sounds
    {
        private static AudioMixer? _mixer;
        private static FileSource? _musikSource;
        private static SampleSource? _beepSource;
        public static bool AudioAvailable { get; private set; }

        public static void Init()
        {
            try
            {
                var config = new AudioConfig
                {
                    BufferSize = 1024,
                    HostType = GetPlatformHostType()
                };

                OwnaudioNet.Initialize(config);
                OwnaudioNet.Start();

                _mixer = new AudioMixer(OwnaudioNet.Engine!.UnderlyingEngine);
                _mixer.Start();

                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                string beepPfad = Path.Combine(basePath, "Sounds", GameData.BeepFile);

                if (File.Exists(beepPfad))
                {
                    float[] beepSamples = LoadWavAsFloatArray(beepPfad);
                    _beepSource = new SampleSource(beepSamples, config);
                    _mixer.AddSource(_beepSource);
                    _beepSource.Stop();
                }

                AudioAvailable = true;
            }
            catch
            {
                AudioAvailable = false;
            }
        }

        static EngineHostType GetPlatformHostType()
        {
            if (OperatingSystem.IsWindows())
            {
                return EngineHostType.WASAPI;
            }    
            else if (OperatingSystem.IsLinux())
            {
                return EngineHostType.ALSA;
            }
            if (OperatingSystem.IsMacOS())
            {
                return EngineHostType.COREAUDIO;
            }

            return EngineHostType.None;
        }

        private static float[] LoadWavAsFloatArray(string path)
        {
            using var fs = File.OpenRead(path);
            using var reader = new BinaryReader(fs);

            reader.ReadBytes(4);
            reader.ReadInt32();
            reader.ReadBytes(4);

            short bitsPerSample = 16;
            var samples = new List<float>();

            while (fs.Position < fs.Length - 8)
            {
                string chunkId = new(reader.ReadChars(4));
                int chunkSize = reader.ReadInt32();

                if (chunkId == "fmt ")
                {
                    reader.ReadInt16();
                    reader.ReadInt16();
                    reader.ReadInt32();
                    reader.ReadInt32();
                    reader.ReadInt16();
                    bitsPerSample = reader.ReadInt16();

                    int rest = chunkSize - 16;
                    if (rest > 0) reader.ReadBytes(rest);
                }
                else if (chunkId == "data")
                {
                    int count = chunkSize / (bitsPerSample / 8);
                    for (int i = 0; i < count; i++)
                    {
                        if (bitsPerSample == 16)
                            samples.Add(reader.ReadInt16() / 32768f);
                        else if (bitsPerSample == 8)
                            samples.Add((reader.ReadByte() - 128) / 128f);
                    }
                }
                else
                {
                    reader.ReadBytes(chunkSize);
                }

                if (chunkSize % 2 != 0) reader.ReadByte();
            }

            return [.. samples];
        }

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
            }
        }

        private static int? lastmusik = -1;
        private static bool lastPlayState = false;

        public static void Melodie(int Currentmusik)
        {
            if (!AudioAvailable) return;

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
                    _musikSource = new FileSource(dateipfad)
                    {
                        Loop = true
                    };
                    _mixer!.AddSource(_musikSource);
                    _musikSource.Play();
                    _musikSource.Seek(0);
                }

                lastmusik = Currentmusik;
                lastPlayState = Musikplay;
            }
        }

        private static void StopMusik()
        {
            if (_musikSource != null)
            {
                _musikSource.Stop();
                _mixer?.RemoveSource(_musikSource);
                _musikSource.Dispose();
                _musikSource = null;
            }
        }

        public static void Playbeep()
        {
            if (!AudioAvailable || !Soundplay || _beepSource == null) return;

            _beepSource.Seek(0);
            _beepSource.Play();
        }
    }
}