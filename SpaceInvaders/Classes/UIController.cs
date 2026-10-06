using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SpaceInvaders.Classes
{
    internal class UIController
    {
        Grid _mainMenu;
        Grid _gameMenu;
        TextBlock _wave;
        TextBlock _score;
        TextBlock _mult;
        TextBlock _hi;
        public UIController(Grid mainMenu, Grid gameMenu, ref TextBlock wave, ref TextBlock score, ref TextBlock mult, ref TextBlock hi) {
            _mainMenu = mainMenu;
            _gameMenu = gameMenu;
            _wave = wave;
            _score = score;
            _mult = mult;
            _hi = hi;
        }
        public void ShowState(State state)
        {
            switch (state)
            {
                case State.Menu:
                    _mainMenu.Visibility = Visibility.Visible;
                    _gameMenu.Visibility = Visibility.Hidden;
                    break;
                case State.Wave:
                    _mainMenu.Visibility= Visibility.Hidden;
                    _gameMenu.Visibility = Visibility.Visible;
                    break;
                case State.BossAlert:
                    break;
                case State.BossWave:
                    break;
                case State.Win:
                    break;
                case State.Loss:
                    break;
                default:
                    break;
            }
        }
        public void ResetHud()
        {
            _wave.Text = "Wave 1";
            _score.Text = "Score: 0";
            _mult.Text = "x1.0";
            _hi.Text = "0";
        }
        public void UpdateHud(int wave, int score, float multiplier, int hi, Player player)
        {
            _wave.Text = "Wave " + wave;
            _score.Text = "Score: " + score;
            _mult.Text = "x" + multiplier;
            //to do: draw hp, shield and bonuses
        }
    }
}
