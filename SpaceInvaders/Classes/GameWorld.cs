using System;
using System.Collections.Generic;

namespace SpaceInvaders.Classes
{
    internal class GameWorld
    {
        public event Action<Enemy> EnemyDied;

        public Player player;
        public List<Enemy> Enemies = new();
        public List<Projectile> Projectiles = new();

        public void Reset()
        {
            player = null;
            Enemies.Clear();
            Projectiles.Clear();
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
        }

        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
            enemy.Died += OnDamageableDied;
        }

        public void AddProjectile(Projectile projectile)
        {
            Projectiles.Add(projectile);
        }

        public void RemoveInactiveEntities()
        {
            Enemies.RemoveAll(enemy => !enemy.isAlive);
            Projectiles.RemoveAll(projectile => !projectile.isAlive);
        }

        public IEnumerable<Entity> GetLiveEntities()
        {
            if (player != null && player.isAlive)
                yield return player;

            foreach (Enemy enemy in Enemies)
            {
                if (enemy.isAlive)
                    yield return enemy;
            }

            foreach (Projectile projectile in Projectiles)
            {
                if (projectile.isAlive)
                    yield return projectile;
            }
        }

        private void OnDamageableDied(Damageable damageable)
        {
            if (damageable is Enemy enemy)
                EnemyDied?.Invoke(enemy);
        }
    }
}