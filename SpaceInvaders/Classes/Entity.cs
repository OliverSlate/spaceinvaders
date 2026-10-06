using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Classes
{
    internal abstract class Entity
    {
        public string Id;
        public PointF position;
        public Dimensions dimensions = new Dimensions(51, 51);
        public bool isAlive = true;
        public float speed;
        public ImageBrush skin;
        public Entity()
        {
            Id = Guid.NewGuid().ToString();
        }

        public RectangleF GetBounds()
        {
            PointF origin = new(position.X, position.Y);
            SizeF size = new(dimensions.width, dimensions.height);
            return new RectangleF(origin, size);
        }
    }
    internal abstract class Damageable : Entity
    {
        public event Action<Damageable> Died;
        public int health;
        public int maxHealth;

        public void TakeDamage()
        {
            if (!isAlive) return;
            health--;
            if (health <= 0) Die();
        }
        public void Die()
        {
            if (!isAlive) return;
            isAlive = false;
            Died?.Invoke(this);
        }
        public Damageable() : base() { }
    }
    internal abstract class Enemy : Damageable
    {
        public int scoreValue;
        double fireCooldownRemaining;
        double fireInterval;
        float projectileSpeed;
        bool canShoot;
        public Enemy() : base() { }
        public void Update(double deltaTime)
        {
            if(fireCooldownRemaining > 0) fireCooldownRemaining -= deltaTime;
            //enemy movement
        }
        public EnemyProjectile TryShoot()
        {
            if (!canShoot || fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new(position.X + 25, position.Y + 51);
            EnemyProjectile projectile = new(projectilePos, projectileSpeed);
            return projectile;
        }
    }
    internal class Enemy1 : Enemy {
        //Weak enemy
        public float projectileSpeed = 300;
        public Enemy1() : base() {
            health = 1;
            speed = 300;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 100;
        }
    }
    internal class Enemy2 : Enemy {
        //strong enemy
        public float projectileSpeed = 200;
        public Enemy2() : base()
        {
            health = 3;
            speed = 250;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 220;
        }
    }
    internal class Enemy3 : Enemy {
        //special enemy
        public float  projectileSpeed = 0;
        public Enemy3() : base()
        {
            health = 1;
            speed = 350f;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 350;
        }
    }
    internal class Boss : Enemy { 
        public Boss() : base()
        {
            health = 20;
            speed = 100;
            dimensions = new(306, 51);
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/boss.png", UriKind.Relative)));
            scoreValue = 2000;
        }
    }
    internal class Player : Damageable
    {
        public Velocity velocity;
        public int shield = 0;
        public const int maxShield = 5;
        public float acceleration = 10f;
        public double fireCooldownRemaining = 0;
        public double fireInterval = 0.7f;
        public float projectileSpeed = 1000;
        public Player() : base()
        {
            health = 3;
            maxHealth = 5;
            speed = 400f;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
        }
        public PlayerProjectile TryShoot()
        {
            if (fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new(position.X + 25, position.Y - 20);
            PlayerProjectile projectile = new(projectilePos, projectileSpeed);
            return projectile;
        }
        public void Update(double dT)
        {
            if(fireCooldownRemaining > 0) fireCooldownRemaining -= dT;
        }
        public void Move(double dT, HashSet<Key> keysDown, double width, double height)
        {
            //get movement vector
            Movement movement = new Movement();
            if (keysDown.Contains(Key.W)) movement.y += (int)Direction.Up;
            if (keysDown.Contains(Key.S)) movement.y += (int)Direction.Down;
            if (keysDown.Contains(Key.A)) movement.x += (int)Direction.Left;
            if (keysDown.Contains(Key.D)) movement.x += (int)Direction.Right;

            //normalize
            double len = Math.Sqrt(movement.x * movement.x + movement.y * movement.y);
            double targetVelX = len > 0 ? (movement.x / len) * speed : 0;
            double targetVelY = len > 0 ? (movement.y / len) * speed : 0;

            //lerp (we are in space)
            double t = Math.Clamp(acceleration * dT, 0, 1);
            velocity.x += (targetVelX - velocity.x) * t;
            velocity.y += (targetVelY - velocity.y) * t;

            position.X = (float)Math.Clamp(position.X + velocity.x * dT, 0, width - 51);
            position.Y = (float)Math.Clamp(position.Y + velocity.y * dT, 0, height - 51);
        }
        public void TakeDamage(int dmg)
        {
            while (dmg != 0)
            {
                if (shield > 0) shield--;
                else health--;
                if (health == 0) Die();
                dmg--;
            }
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
