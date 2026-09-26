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
        private readonly AudioEngine audio = new();
        private SoundHandler soundHandler;
        public int targetFrameRate = 60;
        public MainWindow()
        {
            InitializeComponent();
            soundHandler = new SoundHandler(gameState, audio);

            VolumeSlider.ValueChanged += VolumeSlider_ValueChanged;
            VolumeSlider_ValueChanged(VolumeSlider, null);
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            audio.PlaySfx(sfx[SFX.Hover]);
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            audio.PlaySfx(sfx[SFX.Click]);
            MenuGrid.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
            gameState.SetState(State.Wave);
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
    }
}
