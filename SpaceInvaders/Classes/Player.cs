using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Classes
{
    internal class Player
    {
        public Position position;
        public Velocity velocity;
        public ImageBrush skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", uriKind: UriKind.Relative)));
        public int health = 3;
        public const int maxHealth = 5;
        public int shield = 0;
        public const int maxShield = 5;
        public float speed = 400f;
        public float acceleration = 10f;
        
        public void Shoot()
        {

        }
        public void TakeDamage(int dmg)
        {
            while(dmg != 0)
            {
                if (shield > 0) shield--;
                else health--;
                if (health == 0) Die();
                dmg--;
            }
        }
        public void Die() { 
        
        }
        public void Heal()
        {
            if (health >= maxHealth) return;
            health++;
        }
        public void AddShield()
        {
            if (shield >= maxShield) return;
            shield++;
        }
    }
}
