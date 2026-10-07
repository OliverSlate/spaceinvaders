using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceInvaders.Classes
{
    internal abstract class Projectile : Entity
    {
        public Faction faction;
        public int damage;
        public Direction direction;
        public void Update(double dT, double canvasHeight)
        {
            float change = this.speed * (int)this.direction * (float)dT;
            position.Y += change;
            if (position.Y > canvasHeight || position.Y < -51) isAlive = false;
        }
        public void Deactivate() => isAlive = false;
    }
    internal class PlayerProjectile : Projectile
    {
        public PlayerProjectile(PointF position, float speed) : base()
        {
            this.position = position;
            this.speed = speed;
            this.direction = Direction.Up;
            skinFrames = new[] { LoadBrush("Resources/Images/playerprojectile.png") };
            damage = 1;
            faction = Faction.Player;
            dimensions = new Dimensions(9, 20);
        }
    }
    internal class EnemyProjectile : Projectile
    {
        public EnemyProjectile(PointF position, float speed) : base()
        {
            this.position = position;
            this.speed = speed;
            this.direction = Direction.Down;
            skinFrames = new[] { LoadBrush("Resources/Images/enemyprojectile.png") };
            damage = 1;
            faction = Faction.Enemy;
            dimensions = new Dimensions(9, 20);
        }
    }
    internal class SpecialProjectile : Projectile
    {
        public SpecialProjectile(PointF position) : base()
        {
            this.position = position;
            this.speed = 0;
            this.direction = Direction.Down;
            skinFrames = new[] { LoadBrush("Resources/Images/specialattack.png") };
            damage = 3;
            faction = Faction.Enemy;
        }
    }
}
