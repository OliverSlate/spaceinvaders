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
        public List<Explosion> Explosions = [];

        public void Reset()
        {
            player = null;
            Enemies.Clear();
            Projectiles.Clear();
            PickUps.Clear();
            Explosions.Clear();
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
        public void AddProjectiles(List<EnemyProjectile> projectiles) => Projectiles.AddRange(projectiles);
        public void AddPickup(Pickup pickup)
        {
            PickUps.Add(pickup);
        }

        public void AddExplosion(Explosion explosion)
        {
            Explosions.Add(explosion);
        }

        public void UpdateExplosions(double deltaTime)
        {
            foreach (Explosion explosion in Explosions)
                explosion.Update(deltaTime);
        }

        public void RemoveInactiveEntities()
        {
            Enemies.RemoveAll(enemy => !enemy.isAlive);
            Projectiles.RemoveAll(projectile => !projectile.isAlive);
            PickUps.RemoveAll(pickup => !pickup.isAlive);
            Explosions.RemoveAll(explosion => !explosion.isAlive);
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

            foreach (Explosion explosion in Explosions)
            {
                if (explosion.isAlive)
                    yield return explosion;
            }
        }

        private void OnDamageableDied(Damageable damageable)
        {
            if (damageable is Enemy enemy)
                EnemyDied?.Invoke(enemy);
        }
    }
}
