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
        private readonly AudioEngine audio;

        public SoundHandler(GameStateManager gameState, AudioEngine audio)
        {
            this.audio = audio;
            gameState.StateChanged += OnStateChanged;
            OnStateChanged(gameState.CurrentState);
        }

        private void OnStateChanged(State newState)
        {
            string track = newState switch
            {
                State.Menu => "Resources/Audio/mainmenu.wav",
                State.Wave => "Resources/Audio/game.wav",
                State.BossAlert => "Resources/Audio/bossalert.wav",
                State.BossWave => "Resources/Audio/boss.wav",
                State.Win => "Resources/Audio/win.wav",
                State.Loss => "Resources/Audio/lose.wav",
                _ => throw new ArgumentOutOfRangeException()
            };
            audio.PlayMusic(track);
        }
    }
}
