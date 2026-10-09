using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceInvaders.Classes
{
    internal class UIController
    {
        Grid _mainMenu;
        Grid _gameMenu;
        Grid _loseMenu;
        Canvas _healthCanvas;
        Canvas _powerupCanvas;
        TextBlock _wave;
        TextBlock _score;
        TextBlock _mult;
        TextBlock _hi;
        public UIController(Grid mainMenu, Grid gameMenu, Grid loseMenu, Canvas healthCanvas, Canvas powerupCanvas, ref TextBlock wave, ref TextBlock score, ref TextBlock mult, ref TextBlock hi) {
            _mainMenu = mainMenu;
            _gameMenu = gameMenu;
            _loseMenu = loseMenu;
            _healthCanvas = healthCanvas;
            _powerupCanvas = powerupCanvas;
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
                    _loseMenu.Visibility = Visibility.Hidden;
                    break;
                case State.Wave:
                    _mainMenu.Visibility= Visibility.Hidden;
                    _gameMenu.Visibility = Visibility.Visible;
                    _loseMenu.Visibility= Visibility.Hidden;
                    break;
                case State.BossAlert:
                    break;
                case State.BossWave:
                    break;
                case State.Win:
                    break;
                case State.Loss:
                    _loseMenu.Visibility = Visibility.Visible;
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
        public void UpdateHud(int wave, int score, double multiplier, int hi, Player player)
        {
            _wave.Text = "Wave " + wave;
            _score.Text = "Score: " + score;
            _mult.Text = "x" + Math.Round(multiplier, 2);
            _hi.Text = "HI: " + hi;
        }
        public void UpdateHealth(Player player)
        {
            _healthCanvas.Children.Clear();
            if (player == null) return;
            for (int i = 0; i < player.shield; i++)
            {
                Rectangle rect = new Rectangle()
                {
                    Tag = "shield",
                    Width = 51,
                    Height = 51,
                    Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/shieldfull.png", UriKind.Relative)))
                };
                Canvas.SetTop(rect, 12);
                Canvas.SetLeft(rect, 12 + i * 51);
                _healthCanvas.Children.Add(rect);
            }
            for (int i = 0; i < player.maxHealth; i++)
            {
                string source = i + 1 > player.health ? "Resources/Images/heartempty.png" : "Resources/Images/heartfull.png";
                Rectangle rect = new Rectangle()
                {
                    Tag = "hp",
                    Width = 51,
                    Height = 51,
                    Fill = new ImageBrush(new BitmapImage(new Uri(source, UriKind.Relative)))
                };
                Canvas.SetTop(rect, 12);
                Canvas.SetLeft(rect, 12 + i * 51);
                _healthCanvas.Children.Add(rect);
            }
        }
        public void UpdatePowerups(Player player, double bonusPointsCounter)
        {
            _powerupCanvas.Children.Clear();
            if (player.fastShootTimer > 0)
            {
                Rectangle rect = new Rectangle()
                {
                    Tag = "fastShoot",
                    Width = 51,
                    Height = 51,
                    Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/pickup_shoot_0.png", UriKind.Relative)))
                };
                Canvas.SetTop(rect, 12);
                Canvas.SetLeft(rect, 12);
                TextBlock text = new TextBlock()
                {
                    Tag = "fastShoot",
                    FontSize = 36,
                    Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#F8BC04"),
                    Text = Math.Round(player.fastShootTimer, 1).ToString()
                };
                Canvas.SetTop(text, 16);
                Canvas.SetLeft(text, 24 + 51);
                _powerupCanvas.Children.Add(rect);
                _powerupCanvas.Children.Add(text);
            }
            if (bonusPointsCounter > 0)
            {
                Rectangle rect = new Rectangle()
                {
                    Tag = "bonusPoints",
                    Width = 51,
                    Height = 51,
                    Fill = new ImageBrush(new BitmapImage(new Uri("Resources/Images/pickup_points_0.png", UriKind.Relative)))
                };
                Canvas.SetTop(rect, 12 + 51);
                Canvas.SetLeft(rect, 12);
                TextBlock text = new TextBlock()
                {
                    Tag = "bonusPoints",
                    FontSize = 36,
                    Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#F8BC04"),
                    Text = Math.Round(bonusPointsCounter, 1).ToString()
                };
                Canvas.SetTop(text, 16 + 51);
                Canvas.SetLeft(text, 24 + 51);
                _powerupCanvas.Children.Add(rect);
                _powerupCanvas.Children.Add(text);
            }
        }
    }
}
