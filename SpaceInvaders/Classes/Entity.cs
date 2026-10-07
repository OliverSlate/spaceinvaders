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
        public static Random random = new Random();
        public int scoreValue;
        public double fireCooldownRemaining;
        public double fireInterval;
        public float projectileSpeed;
        public Direction horizontalDir = Direction.Right;
        public bool movingDown = false;
        public float dropStartY;
        public Enemy() : base() => dropStartY = position.Y;
        public void Update(double dT, double canvasWidth)
        {
            if (fireCooldownRemaining > 0) fireCooldownRemaining -= dT;
            if (movingDown)
            {
                position.Y += speed * (float)dT;
                if (position.Y >= dropStartY + 51)
                {
                    position.Y = dropStartY + 51;
                    movingDown = false;
                }
                return;
            }
            position.X += speed * (int)horizontalDir * (float)dT;
            if (position.X > canvasWidth - 63 || position.X < 12)
            {
                position.X = Math.Clamp(position.X, 12f, (float)canvasWidth - 63f);
                horizontalDir = horizontalDir == Direction.Right ? Direction.Left : Direction.Right;
                dropStartY = position.Y;
                movingDown = true;
            }
        }
        public abstract EnemyProjectile TryShoot();
    }
    internal class Enemy1 : Enemy {
        //Weak enemy
        public Enemy1() : base() {
            health = 1;
            speed = 60;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 35;
            projectileSpeed = 300;
            fireInterval = 5;
            fireCooldownRemaining = random.NextDouble() * fireInterval;
        }
        public override EnemyProjectile TryShoot()
        {
            if (fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new(position.X + 25, position.Y + 51);
            EnemyProjectile projectile = new(projectilePos, projectileSpeed);
            return projectile;
        }
    }
    internal class Enemy2 : Enemy {
        //strong enemy
        public Enemy2() : base()
        {
            health = 3;
            speed = 90;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 100;
            projectileSpeed = 200;
            fireInterval = 4;
            fireCooldownRemaining = random.NextDouble() * fireInterval;
        }
        public override EnemyProjectile TryShoot()
        {
            if (fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new(position.X + 25, position.Y + 51);
            EnemyProjectile projectile = new(projectilePos, projectileSpeed);
            return projectile;
        }
    }
    internal class Enemy3 : Enemy {
        //special enemy
        public Enemy3() : base()
        {
            health = 1;
            speed = 50;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 150;
            projectileSpeed = 50;
            fireInterval = 8;
            fireCooldownRemaining = random.NextDouble() * fireInterval;
        }
        public override EnemyProjectile TryShoot()
        {
            if (fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new(position.X + 25, position.Y + 51);
            EnemyProjectile projectile = new(projectilePos, projectileSpeed);
            return projectile;
        }
    }
    internal class Boss : Enemy { 
        public Boss() : base()
        {
            health = 40;
            speed = 100;
            dimensions = new(306, 306);
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
            scoreValue = 5000;
            projectileSpeed = 600;
            fireInterval = Math.Max(random.NextDouble() * 1.5f, 0.35f);
            fireCooldownRemaining = fireInterval;
        }
        public override EnemyProjectile TryShoot()
        {
            if (fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new((position.X + (position.X + dimensions.width)) / 2, position.Y + dimensions.height);
            EnemyProjectile projectile = new(projectilePos, projectileSpeed);
            return projectile;
        }
    }
    internal class Player : Damageable
    {
        public event Action Damaged;
        public Velocity velocity;
        public int shield = 0;
        public const int maxShield = 5;
        public float acceleration = 10f;
        public double fireCooldownRemaining = 0;
        public double fireInterval;
        public float projectileSpeed = 1000;
        public bool invincible;
        public Player(int flags) : base()
        {
            health = 3;
            maxHealth = 5;
            speed = (flags & (int)Flags.SUPER_SPEED) > 0 ? 1600 : 400;
            invincible = (flags & (int)Flags.INVINCIBLE) > 0 ? true : false;
            fireInterval = (flags & (int)Flags.FAST_ATTACK) > 0 ? 0.01f : 0.7f;
            skin = new ImageBrush(new BitmapImage(new Uri("Resources/Images/player.png", UriKind.Relative)));
        }
        public PlayerProjectile TryShoot()
        {
            if (fireCooldownRemaining > 0) return null;
            fireCooldownRemaining = fireInterval;
            PointF projectilePos = new(position.X + 21, position.Y - 16);
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
            if (invincible) return;
            Damaged?.Invoke();
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
