using Smake.Interfaces;
using NAudio.Wave;

namespace Smake.SFX.Players
{
    internal class WindowsPlayer : IPlayer
    {
        private WaveOutEvent? _outputDevice;
        private AudioFileReader? _audioFile;
        private string? _fileName;
        private bool _loop;

        public event EventHandler? PlaybackFinished;

        public bool Playing { get; private set; }

        public async Task Play(string fileName, bool loop = false)
        {
            _fileName = fileName;
            _loop = loop;

            await StartPlayback();
        }

        private Task StartPlayback()
        {
            CleanupDevice();

            _audioFile = new AudioFileReader(_fileName!);
            _outputDevice = new WaveOutEvent();
            _outputDevice.Init(_audioFile);
            _outputDevice.PlaybackStopped += HandlePlaybackFinished;
            _outputDevice.Play();

            Playing = true;

            return Task.CompletedTask;
        }

        public Task Stop()
        {
            _loop = false;

            if (_outputDevice != null)
            {
                _outputDevice.PlaybackStopped -= HandlePlaybackFinished;
                _outputDevice.Stop();
            }

            Playing = false;
            CleanupDevice();

            return Task.CompletedTask;
        }

        private void CleanupDevice()
        {
            _outputDevice?.Dispose();
            _outputDevice = null;

            _audioFile?.Dispose();
            _audioFile = null;
        }

        private async void HandlePlaybackFinished(object? sender, StoppedEventArgs e)
        {
            Playing = false;

            if (e.Exception != null)
            {
                _loop = false;
            }

            if (_loop && _fileName != null)
            {
                await StartPlayback();
                return;
            }

            PlaybackFinished?.Invoke(this, EventArgs.Empty);
            CleanupDevice();
        }
    }
}