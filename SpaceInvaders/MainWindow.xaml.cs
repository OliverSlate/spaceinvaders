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
    public enum State
    {
        Menu,
        Wave,
        BossAlert,
        BossWave,
        Loss
    }
    
    public partial class MainWindow : Window
    {
        State globalState = State.Menu;
        SoundHandler soundHandler;
        GameState gameState;
        public MainWindow()
        {
            InitializeComponent();
            gameState = new GameState();
            soundHandler = SoundHandler.GetInstance();
        }
        public void HandleStateChange(State state)
        {
            globalState = state;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
        }
    }
}
