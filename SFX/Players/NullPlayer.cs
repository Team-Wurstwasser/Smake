using Smake.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smake.SFX.Players
{
    internal class NullPlayer : IPlayer
    {
        public bool Playing => false;

        #pragma warning disable CS0067
        public event EventHandler? PlaybackFinished;
        #pragma warning restore CS0067

        public Task Play(string fileName, bool loop = false) => Task.CompletedTask;
        public Task Stop() => Task.CompletedTask;
    }
}
