using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace SpaceInvaders.Classes
{
    internal class CollisionSystem
    {
        public event Action<Pickup> PickedUp;
        public void Resolve(GameWorld world)
        {
            foreach (Projectile projectile in world.Projectiles)
            {
                if (!projectile.isAlive) continue;
                RectangleF projectileBounds = projectile.GetBounds();
                if (projectile.faction == Faction.Player)
                {
                    foreach (Enemy enemy in world.Enemies)
                    {
                        if (!enemy.isAlive) continue;
                        if(projectileBounds.IntersectsWith(enemy.GetBounds()))
                        {
                            enemy.TakeDamage();
                            projectile.isAlive = false;
                            break;
                        }
                        
                    }
                }
                else
                {
                    if(world.player.isAlive &&
                        projectileBounds.IntersectsWith(world.player.GetBounds()))
                    {
                        world.player.TakeDamage(projectile.damage);
                        projectile.isAlive = false;
                    }
                }
            }
            foreach (Enemy enemy in world.Enemies)
            {
                if (!world.player.isAlive) return;
                if (!enemy.isAlive) continue;
                RectangleF enemyBounds = enemy.GetBounds();
                if(enemyBounds.IntersectsWith(world.player.GetBounds()))
                {
                    world.player.TakeDamage(2137);
                    enemy.Die();
                }
            }
            foreach(Pickup pickup in world.PickUps)
            {
                if (!pickup.isAlive) continue;
                RectangleF pickupBounds = pickup.GetBounds();
                if (pickupBounds.IntersectsWith(world.player.GetBounds()))
                {
                    world.player.PickUp(pickup);
                    pickup.isAlive = false;
                    PickedUp?.Invoke(pickup);
                }
            }
        }
    }
}
