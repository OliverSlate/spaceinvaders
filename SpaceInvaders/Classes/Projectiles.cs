using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Classes
{
    internal abstract class Projectile
    {
        public Position position;
        public float speed;
        public Direction direction;
        public Projectile() { }
        public abstract void Move();
    }
    internal class PlayerProjectile : Projectile
    {
        public PlayerProjectile(Position position, float speed, Direction direction = Direction.Up) : base()
        {
            this.position = position;
            this.speed = speed;
            this.direction = direction;
        }
        public override void Move()
        {
            float change = this.speed * (int)this.direction;
            this.position.y += change;
        }
    }
    internal class EnemyProjectile : Projectile
    {
        public EnemyProjectile(Position position, float speed, Direction direction = Direction.Down) : base()
        {
            this.position = position;
            this.speed = speed;
            this.direction = direction;
        }
        public override void Move()
        {
            float change = this.speed * (int)this.direction;
            this.position.y += change;
        }
    }
}
