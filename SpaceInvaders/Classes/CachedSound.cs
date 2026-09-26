using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Classes
{
    internal class CachedSound
    {
        public float[] AudioData { get; }
        public WaveFormat WaveFormat { get; }

        public CachedSound(string path)
        {
            using var reader = new AudioFileReader(path);
            WaveFormat = reader.WaveFormat;

            var all = new List<float>();
            var buffer = new float[reader.WaveFormat.SampleRate * reader.WaveFormat.Channels];
            int read;
            while ((read = reader.Read(buffer.AsSpan())) > 0)
                all.AddRange(buffer.Take(read));
            AudioData = all.ToArray();
            System.Diagnostics.Debug.WriteLine($"{path}: {reader.WaveFormat.SampleRate}Hz, {reader.WaveFormat.Channels}ch");
        }
    }

    class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound sound;
        private long position;
        public CachedSoundSampleProvider(CachedSound sound) => this.sound = sound;
        public WaveFormat WaveFormat => sound.WaveFormat;

        public int Read(Span<float> buffer)
        {
            long remaining = sound.AudioData.Length - position;
            int toCopy = (int)Math.Min(remaining, buffer.Length);
            sound.AudioData.AsSpan((int)position, toCopy).CopyTo(buffer);
            position += toCopy;
            return toCopy;
        }
    }
}
