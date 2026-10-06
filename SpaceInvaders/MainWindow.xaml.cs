using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using SpaceInvaders.Classes;

namespace SpaceInvaders
{
  
    
    public partial class MainWindow : Window
    {
        private readonly Dictionary<SFX, string> sfx = new() {
            {SFX.Pickup, "Resources/Audio/pickup.wav" },
            {SFX.Explosion, "Resources/Audio/explosion.wav" },
            {SFX.Shoot, "Resources/Audio/shoot.wav" },
            {SFX.Hover, "Resources/Audio/hover.wav" },
            {SFX.Click, "Resources/Audio/click.wav" }
        };
        public HashSet<Key> keysDown = new();
        private TimeSpan lastRender = TimeSpan.Zero;
        private bool gameLoopSubscribed = false;
        private GameStateManager gameState = new();
        private AudioEngine audio = new();
        private SoundHandler soundHandler;
        private CanvasRenderer canvasRenderer;
        private UIController uiController;
        private GameSession gameSession;

        public MainWindow()
        {
            InitializeComponent(); 
            soundHandler = new SoundHandler(gameState, audio);
            uiController = new(MenuGrid, GameGrid, ref WaveText, ref ScoreText, ref MultiplierText, ref HighScoreText);
            gameSession = new(gameState, GameCanvas, uiController, sfxType => audio.PlaySfx(sfx[sfxType]));

            VolumeSlider.ValueChanged += VolumeSlider_ValueChanged;
            VolumeSlider_ValueChanged(VolumeSlider, null);

            Focusable = true;
            Focus();
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            Deactivated += (_, _) => keysDown.Clear();

            CompositionTarget.Rendering += GameLoop;
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e) { audio.PlaySfx(sfx[SFX.Hover]); }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            audio.PlaySfx(sfx[SFX.Click]);
            StartGame();
        }
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            audio.PlaySfx(sfx[SFX.Click]);
            OptionsGrid.Visibility = OptionsGrid.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
        private void QuitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.Volume = (float)VolumeSlider.Value;
            VolumeText.Text = (audio.Volume * 100).ToString("0") + "%";
        }
        private void StartGame()
        {
            //UI
            gameState.SetState(State.Wave);
            uiController.ShowState(gameState.CurrentState);
            gameSession.Start(GameCanvas.ActualWidth, GameCanvas.ActualHeight);
            //Game loop setup
            lastRender = TimeSpan.Zero;
        }
        private void GameLoop(object sender, EventArgs e) 
        {
            var args = (RenderingEventArgs)e;
            if (args.RenderingTime == lastRender) return; //safeguard so you dont get 2 frames at the same time

            double deltaTime = lastRender == TimeSpan.Zero ? 0 : (args.RenderingTime - lastRender).TotalSeconds;
            lastRender = args.RenderingTime;
            deltaTime = Math.Min(deltaTime, 0.05); //get a minimum for the same reason basically
            gameSession.Update(deltaTime, keysDown);
        }
        private void HandleInput() //this function will be moved to an input handler
        {
           
        }
        private void OnKeyDown(object sender, KeyEventArgs e) => keysDown.Add(e.Key);
        private void OnKeyUp(object sender, KeyEventArgs e) => keysDown.Remove(e.Key);
    }
}
