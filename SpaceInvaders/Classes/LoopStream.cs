using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Classes
{
    public class LoopStream : WaveStream
    {
        private readonly WaveStream source;
        public bool EnableLooping { get; set; } = true;

        public LoopStream(WaveStream source) => this.source = source;

        public override WaveFormat WaveFormat => source.WaveFormat;
        public override long Length => source.Length;
        public override long Position
        {
            get => source.Position;
            set => source.Position = value;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = source.Read(buffer, offset + totalRead, count - totalRead);
                if (read == 0)
                {
                    if (source.Position == 0 || !EnableLooping) break;
                    source.Position = 0;
                    continue;
                }
                totalRead += read;
            }
            return totalRead;
        }
    }
}
