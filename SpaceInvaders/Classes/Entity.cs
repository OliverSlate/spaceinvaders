using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Classes
{
    internal abstract class Entity
    {
        public string id;
        public Position position;
        public Velocity velocity;
        public int health;
        public float speed;
        public ImageBrush skin;

        public Entity() { }
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
    internal class Enemy1 : Entity {
        //Weak enemy
        public Enemy1() : base() {
            health = 1;
            speed = 300;
            projectileSpeed = 300;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/enemy1.png", UriKind.Relative)));

        }
    }
    internal class Enemy2 : Entity {
        //strong enemy
        public Enemy2() : base()
        {
            health = 3;
            speed = 250;
            projectileSpeed = 200;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/enemy2.png", UriKind.Relative)));
        }
    }
    internal class Enemy3 : Entity {
        //special enemy
        public Enemy3() : base()
        {
            health = 1;
            speed = 350f;
            projectileSpeed = 0;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/enemy3.png", UriKind.Relative)));
        }
    }
    internal class Boss : Entity { 
        public Boss() : base()
        {
            health = 20;
            speed = 100;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/boss.png", UriKind.Relative)));
        }
    }
    internal class Player : Entity
    {
        public const int maxHealth = 5;
        public int shield = 0;
        public const int maxShield = 5;
        public float acceleration = 10f;
        public Player() : base()
        {
            health = 3;
            speed = 400f;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", uriKind: UriKind.Relative)));
        }
        public void Shoot()
        {

        }
        public override void TakeDamage(int dmg)
        {
            while (dmg != 0)
            {
                if (shield > 0) shield--;
                else health--;
                if (health == 0) Die();
                dmg--;
            }
        }
        public override void Die()
        {

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
