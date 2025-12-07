using NAudio.Wave;

namespace Smake.SFX
{
    public class LoopStream(WaveStream sourceStream) : WaveStream
    {
        readonly WaveStream sourceStream = sourceStream;

        public override WaveFormat WaveFormat => sourceStream.WaveFormat;

        public override long Position
        {
            get => sourceStream.Position;
            set => sourceStream.Position = value;
        }

        public override long Length => sourceStream.Length;

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);

                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0)
                    {
                        break;
                    }
                    sourceStream.Position = 0;
                }

                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }
    }
}