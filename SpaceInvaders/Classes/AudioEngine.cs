using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Classes
{
    internal class AudioEngine
    {
        private readonly MixingSampleProvider mixer;
        private readonly WaveOut outputDevice;
        private FadeInOutSampleProvider currentMusic;
        private readonly Dictionary<string, CachedSound> sfxCache = new();

        public AudioEngine()
        {
            var format = WaveFormat.CreateIeeeFloatWaveFormat(44100, 2);
            mixer = new MixingSampleProvider(format) { ReadFully = true };
            outputDevice = new WaveOut();
            outputDevice.Init(mixer);
            outputDevice.Play();
        }
        public void PlayMusic(string relativePath, double fadeSeconds = 1.5)
        {
            var looped = new LoopStream(new AudioFileReader(ResolveAudioPath(relativePath)));
            var incoming = new FadeInOutSampleProvider(looped.ToSampleProvider(), true);
            incoming.BeginFadeIn(fadeSeconds * 1000);

            var outgoing = currentMusic;
            if (outgoing != null)
            {
                outgoing.BeginFadeOut(fadeSeconds * 1000);
                var timer = new System.Timers.Timer(fadeSeconds * 1000) { AutoReset = false };
                timer.Elapsed += (_, _) => mixer.RemoveMixerInput(outgoing);
                timer.Start();
            }

            mixer.AddMixerInput(incoming);
            currentMusic = incoming;
        }
        public void PlaySfx(string relativePath)
        {
            if (!sfxCache.TryGetValue(relativePath, out var sound))
                sfxCache[relativePath] = sound = new CachedSound(ResolveAudioPath(relativePath));

            mixer.AddMixerInput(new CachedSoundSampleProvider(sound));
        }
        public static string ResolveAudioPath(string relativePath)
        {
            return AppDomain.CurrentDomain.BaseDirectory + relativePath;
        }
    }
}
