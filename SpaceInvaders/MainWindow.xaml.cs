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
        public HashSet<Key> _keysDown = new();
        private TimeSpan lastRender = TimeSpan.Zero;
        //UI
        private Canvas gameCanvas;
        private Rectangle playerRect;
        //SYSTEMS
        private readonly GameStateManager gameState = new();
        private WaveManager waveManager;
        private Player player;
        private readonly AudioEngine audio = new();
        private SoundHandler soundHandler;
        private CanvasRenderer canvasRenderer;
        private CollisionSystem collisionSystem;
        private GameSession gameSession;
        private GameWorld world;
        private UIController controller;
 
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
        private void QuitButton_Click(object sender, RoutedEventArgs e) { this.Close(); }

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
            lastRender = TimeSpan.Zero;
            CompositionTarget.Rendering += GameLoop;
        }
        private void GameLoop(object sender, EventArgs e) 
        {
            var args = (RenderingEventArgs)e;
            if (args.RenderingTime == lastRender) return; //safeguard so you dont get 2 frames at the same time

            double deltaTime = lastRender == TimeSpan.Zero ? 0 : (args.RenderingTime - lastRender).TotalSeconds;
            lastRender = args.RenderingTime;
            deltaTime = Math.Min(deltaTime, 0.05); //get a minimum for the same reason basically
            HandleInput(deltaTime);
            Render();
            //update i fixed the choppy ass look
        }
        private void HandleInput(double dT)
        {
            //get movement vector
            Movement movement = new Movement();
            if (_keysDown.Contains(Key.W)) movement.y += (int)Direction.Up;
            if (_keysDown.Contains(Key.S)) movement.y += (int)Direction.Down;
            if (_keysDown.Contains(Key.A)) movement.x += (int)Direction.Left;
            if (_keysDown.Contains(Key.D)) movement.x += (int)Direction.Right;

            //normalize
            double len = Math.Sqrt(movement.x * movement.x + movement.y * movement.y);
            double targetVelX = len > 0 ? (movement.x / len) * player.speed : 0;
            double targetVelY = len > 0 ? (movement.y / len) * player.speed : 0;

            //lerp (we are in space)
            double t = Math.Clamp(player.acceleration * dT, 0, 1);
            player.velocity.x += (targetVelX - player.velocity.x) * t;
            player.velocity.y += (targetVelY - player.velocity.y) * t;

            //move that fat ass 
            player.position.x = Math.Clamp(player.position.x + player.velocity.x * dT, 0, gameCanvas.ActualWidth - 51);
            player.position.y = Math.Clamp(player.position.y + player.velocity.y * dT, 0, gameCanvas.ActualHeight - 51);
        }
        private void Render()
        {
            Canvas.SetLeft(playerRect, player.position.x);
            Canvas.SetTop(playerRect, player.position.y);
            //gameCanvas.UpdateLayout();
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
