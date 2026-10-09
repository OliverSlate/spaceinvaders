using System;
using System.Collections.Generic;

namespace SpaceInvaders.Classes
{
    internal class GameWorld
    {
        public event Action<Enemy> EnemyDied;
        public event Action EnemyReachedEnd;

        public Player player;
        public List<Enemy> Enemies = [];
        public List<Projectile> Projectiles = [];
        public List<Pickup> PickUps = [];

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
            enemy.ReachedEnd += () => EnemyReachedEnd?.Invoke();
        }

        public void AddProjectile(Projectile projectile) => Projectiles.Add(projectile);
        
        public void AddPickup(Pickup pickup)
        {
            PickUps.Add(pickup);
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

            foreach (Pickup pickup in PickUps)
            {
                if (pickup.isAlive)
                    yield return pickup;
            }
        }

        private void OnDamageableDied(Damageable damageable)
        {
            if (damageable is Enemy enemy)
                EnemyDied?.Invoke(enemy);
        }
    }
}