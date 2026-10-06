using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Media;

namespace SpaceInvaders.Classes
{
    internal class CollisionSystem
    {
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
        }
    }
}
