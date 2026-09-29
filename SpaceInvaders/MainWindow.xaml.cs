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
        private readonly GameStateManager gameState = new();
        private Canvas gameCanvas;
        private WaveManager waveManager;
        private Player player;
        private Rectangle playerRect;
        private readonly AudioEngine audio = new();
        private SoundHandler soundHandler;
        public int targetFrameRate = 60;
        public DispatcherTimer gameTimer = new DispatcherTimer();
        public HashSet<Key> _keysDown = new();
        public MainWindow()
        {
            InitializeComponent();
            soundHandler = new SoundHandler(gameState, audio);

            VolumeSlider.ValueChanged += VolumeSlider_ValueChanged;
            VolumeSlider_ValueChanged(VolumeSlider, null);

            gameCanvas = GameCanvas;
            Focusable = true;
            Focus();
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
            Deactivated += (_, _) => _keysDown.Clear();
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
        private void QuitButton_Click(object sender, RoutedEventArgs e) { 
              this.Close();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audio.Volume = (float)VolumeSlider.Value;
            VolumeText.Text = (audio.Volume * 100).ToString("0") + "%";
        }
        private void StartGame()
        {
            //UI
            MenuGrid.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
            gameState.SetState(State.Wave);

            //Player
            waveManager = new();
            player = new();
            playerRect = new Rectangle()
            {
                Tag = "player",
                Width = 51,
                Height = 51,
                Fill = player.skin
            };
            player.position.x = (gameCanvas.ActualWidth - playerRect.Width) / 2;
            player.position.y = gameCanvas.ActualHeight - playerRect.Height;
            Canvas.SetLeft(playerRect, player.position.x);
            Canvas.SetTop(playerRect, player.position.y);
            gameCanvas.Children.Add(playerRect);

            //Game loop setup
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(1000 / targetFrameRate);
            gameTimer.Start();
        }
        private void GameLoop(object sender, EventArgs e) 
        {
            HandleMovement();
            Render();
        }
        private void HandleMovement()
        {
            //get movement vector
            Movement movement = new Movement();
            if(_keysDown.Contains(Key.W) && player.position.y > 0)
                movement.y += (int)Direction.Up;
            if (_keysDown.Contains(Key.S) && player.position.y < gameCanvas.ActualHeight - 51)
                movement.y += (int)Direction.Down;
            if (_keysDown.Contains(Key.A) && player.position.x > 0)
                movement.x += (int)Direction.Left;
            if (_keysDown.Contains(Key.D) && player.position.x < gameCanvas.ActualWidth - 51)
                movement.x += (int)Direction.Right;

            player.position.x += movement.x * player.speed;
            player.position.y += movement.y * player.speed;
        }
        private void Render()
        {
            Canvas.SetLeft(playerRect, player.position.x);
            Canvas.SetTop(playerRect, player.position.y);
            gameCanvas.UpdateLayout();
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _keysDown.Add(e.Key);
        }
        private void OnKeyUp(object sender, KeyEventArgs e) 
        {
            _keysDown.Remove(e.Key);
        }
    }
}
