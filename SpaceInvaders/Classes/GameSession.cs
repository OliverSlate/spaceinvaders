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
        private Random rnd = new();
        private double canvasWidth;
        private double canvasHeight;

        public int sessionScore = 0;
        public double sessionMultiplier = 1.0f;
        public int highScore = 0;
        public int flags;
        public double animationTimer = 0f;
        public int animationFrame = 0;
        public double gracePeriod = 1;
        public double graceTimer = 1;
        public double pickupTimer;
        public double basePickupTimer = 5;
        public int bonusPoints = 1;
        public double bonusPointsCounter = 0;

        public GameSession(
            GameStateManager stateManager,
            Canvas canvas,
            UIController uiController,
            Action<SFX> playSfx)
        {
            this.stateManager = stateManager;
            this.uiController = uiController;
            this.playSfx = playSfx;
            pickupTimer = basePickupTimer;

            canvasRenderer = new CanvasRenderer(canvas);
            world.EnemyDied += OnEnemyDied;
            world.EnemyReachedEnd += OnEnemyReachedEnd;
            collisionSystem.PickedUp += (Pickup pickup) =>
            {
                playSfx(SFX.Pickup);
                if (pickup.type == PickUpType.BonusPoints) bonusPointsCounter += 5;
            };
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
            uiController.UpdateHealth(player);

            if (stateManager.CurrentState != State.Wave &&
                stateManager.CurrentState != State.BossWave) return;

            if (player == null || !player.isAlive) return;

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
                if (!enemy.isAlive) continue;
                if (world.Enemies.Count < 5 && world.Enemies[0].GetType() != typeof(Boss)) enemy.speed = 250;
                enemy.Update(deltaTime, canvasWidth, canvasHeight);

                EnemyProjectile projectile = enemy.TryShoot();

                if (projectile != null)
                    world.AddProjectile(projectile);
            }

            foreach (Projectile projectile in world.Projectiles.ToList())
            {
                projectile.Update(deltaTime, canvasHeight);
            }

            if (bonusPointsCounter > 0) bonusPoints = 2;
            else bonusPoints = 1;
            bonusPointsCounter = Math.Max(bonusPointsCounter - deltaTime, 0);

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
            {
                if(graceTimer > 0)
                {
                    graceTimer -= deltaTime;
                }
                else
                {
                    StartNextWave();
                    graceTimer = gracePeriod;
                    gracePeriod -= 0.02;
                }
            }

            pickupTimer -= deltaTime;
            if(pickupTimer <= 0)
            {
                if(rnd.NextDouble() > 0.6)
                {
                    Pickup pickup = new(canvasWidth, canvasHeight);
                    world.AddPickup(pickup);
                }
                pickupTimer = basePickupTimer;
            } 

            canvasRenderer.Synchronize(world, animationFrame);

            uiController.UpdateHud(
                waveManager.currentWave,
                sessionScore,
                sessionMultiplier * bonusPoints,
                highScore,
                player);
            uiController.UpdatePowerups(player, bonusPointsCounter);
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
            uiController.ShowState(stateManager.CurrentState);
            playSfx(SFX.Explosion);
        }

        private void OnEnemyReachedEnd()
        {
            stateManager.SetState(State.Loss);
            uiController.ShowState(stateManager.CurrentState);
        }

        private void OnEnemyDied(Enemy enemy)
        {
            sessionScore += (int)(enemy.scoreValue * sessionMultiplier * bonusPoints);
            sessionMultiplier += 0.02f;

            if (sessionScore > highScore)
                highScore = sessionScore;

            playSfx(SFX.Explosion);
        }
    }
}