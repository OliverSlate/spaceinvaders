using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Classes
{
    internal abstract class Enemy
    {
        public Position position;
        public int health;
        public float speed;
        public float projectileSpeed;
        public bool hasFastAttack = false;

        public Enemy() { }
        public void TakeDamage()
        {
            health--;
            if (health == 0) Die();
        }
        public void Die()
        {
            //implement Die invoking WaveManager
        }
    }
    internal class Enemy1 : Enemy {
        //Weak enemy
        public ImageBrush skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/enemy1.png", UriKind.Relative)));
        public Enemy1() : base() {
            health = 1;
            speed = 300;
            projectileSpeed = 300;
        }
    }
    internal class Enemy2 : Enemy {
        //strong enemy
        public ImageBrush skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/enemy2.png", UriKind.Relative)));
        public Enemy2() : base()
        {
            health = 3;
            speed = 250;
            projectileSpeed = 200;
        }
    }
    internal class Enemy3 : Enemy {
        //special enemy
        public ImageBrush skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/enemy3.png", UriKind.Relative)));
        public Enemy3() : base()
        {
            health = 1;
            speed = 350f;
            projectileSpeed = 0;
        }
    }
    internal class Boss : Enemy {
        public ImageBrush skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/boss.png", UriKind.Relative)));
        public Boss() : base()
        {
            health = 20;
            speed = 100;
        }
    }
}
