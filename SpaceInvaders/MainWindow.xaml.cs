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
        private readonly GameStateManager gameState = new();
        private readonly AudioEngine audio = new();
        private SoundHandler soundHandler;
        public MainWindow()
        {
            InitializeComponent();
            soundHandler = new SoundHandler(gameState, audio);
        }
        public void HandleStateChange(State state)
        { 
           
        }
    
       
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            audio.PlaySfx("Resources/Audio/hover.wav");
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
        
        }
        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsGrid.Visibility = OptionsGrid.Visibility == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden;
        }
        private void QuitButton_Click(object sender, RoutedEventArgs e) { 
        }
    }
}
