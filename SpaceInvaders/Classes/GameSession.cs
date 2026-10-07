using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;

namespace SpaceInvaders.Classes
{
    internal class GameSession
    {
        private readonly GameStateManager stateManager;
        private readonly GameWorld world = new();
        private WaveManager waveManager;
        private readonly CollisionSystem collisionSystem = new();
        private readonly CanvasRenderer canvasRenderer;
        private readonly UIController uiController;
        private readonly Action<SFX> playSfx;

        private Player player;
        private double canvasWidth;
        private double canvasHeight;

        public int sessionScore = 0;
        public float sessionMultiplier = 1.0f;
        public int highScore = 0;
        public int flags;
        public double animationTimer = 0f;
        public int animationFrame = 0;

        public GameSession(
            GameStateManager stateManager,
            Canvas canvas,
            UIController uiController,
            Action<SFX> playSfx)
        {
            this.stateManager = stateManager;
            this.uiController = uiController;
            this.playSfx = playSfx;

            canvasRenderer = new CanvasRenderer(canvas);
            world.EnemyDied += OnEnemyDied;
        }

        public void Start(double width, double height, int flags)
        {
            this.flags = flags;
            waveManager = new(flags);
            canvasWidth = width;
            canvasHeight = height;

            world.Reset();
            canvasRenderer.Clear();
            waveManager.Reset();

            sessionScore = 0;
            sessionMultiplier = 1.0f;

            player = new Player(flags)
            {
                position = new PointF(
                    (float)((canvasWidth - 51) / 2),
                    (float)(canvasHeight - 51))
            };

            player.Died += OnPlayerDied;
            player.Damaged += () => sessionMultiplier = 1f;
            world.SetPlayer(player);

            stateManager.SetState(State.Wave);
            StartNextWave();

            canvasRenderer.Synchronize(world, animationFrame);

            uiController.UpdateHud(
                waveManager.currentWave,
                sessionScore,
                sessionMultiplier,
                highScore,
                player);
        }

        public void Update(double deltaTime, HashSet<Key> keysDown)
        {
            if (stateManager.CurrentState != State.Wave &&
                stateManager.CurrentState != State.BossWave)
            {
                return;
            }

            if (player == null || !player.isAlive)
                return;

            player.Update(deltaTime);
            player.Move(deltaTime, keysDown, canvasWidth, canvasHeight);

            if (keysDown.Contains(Key.Space))
            {
                PlayerProjectile projectile = player.TryShoot();

                if (projectile != null)
                {
                    world.AddProjectile(projectile);
                    playSfx(SFX.Shoot);
                }
            }

            foreach (Enemy enemy in world.Enemies.ToList())
            {
                if (!enemy.isAlive)
                    continue;

                enemy.Update(deltaTime, canvasWidth);

                EnemyProjectile projectile = enemy.TryShoot();

                if (projectile != null)
                    world.AddProjectile(projectile);
            }

            foreach (Projectile projectile in world.Projectiles.ToList())
            {
                projectile.Update(deltaTime, canvasHeight);
            }

            collisionSystem.Resolve(world);
            world.RemoveInactiveEntities();

            animationTimer += deltaTime;
            if (animationTimer >= 0.5)
            {
                animationTimer -= 0.5;
                animationFrame++;
            }

            if (!player.isAlive)
            {
                canvasRenderer.Synchronize(world, animationFrame);
                return;
            }

            if (world.Enemies.Count == 0)
                StartNextWave();

            canvasRenderer.Synchronize(world, animationFrame);

            uiController.UpdateHud(
                waveManager.currentWave,
                sessionScore,
                sessionMultiplier,
                highScore,
                player);
        }

        private void StartNextWave()
        {
            List<Enemy> enemies = waveManager.CreateNextWave(canvasWidth);

            foreach (Enemy enemy in enemies)
                world.AddEnemy(enemy);

            stateManager.SetState(
                waveManager.CurrentWaveKind == WaveKind.Boss
                    ? State.BossWave
                    : State.Wave);
        }

        private void OnPlayerDied(Damageable damageable)
        {
            stateManager.SetState(State.Loss);
            playSfx(SFX.Explosion);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            sessionScore += (int)(enemy.scoreValue * sessionMultiplier);
            sessionMultiplier += 0.02f;

            if (sessionScore > highScore)
                highScore = sessionScore;

            playSfx(SFX.Explosion);
        }
    }
}