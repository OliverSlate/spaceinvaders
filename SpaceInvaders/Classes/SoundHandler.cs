using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NAudio;
using NAudio.Wave;

namespace SpaceInvaders.Classes
{
    
    internal class SoundHandler
    {
        private State state;
        static SoundHandler _instance;
        private AudioFileReader _audioFileReader;
        private WaveOutEvent _event;
        private SoundHandler() {}
        public static SoundHandler GetInstance()
        {
            if (_instance == null) { 
                _instance = new SoundHandler();
            }
            return _instance;
        }
        public void SetState(State state)
        {
            this.state = state;
        }
        public void Play(string path)
        {
            if (_audioFileReader == null) { _audioFileReader = new AudioFileReader(path); }
            if (_event == null) { _event = new WaveOutEvent(); }
            _event.Init(_audioFileReader);
            _event.Play();
        }
        public void PlaySFX(string path)
        {

        }
        public void ChangeBackgroundState(State newState)
        {

        }
    }
}
