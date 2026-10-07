using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Classes
{
    enum WaveKind
    {
        Normal, Swarm, Boss
    }
    internal class WaveManager
    {
        public WaveKind CurrentWaveKind;
        public int currentWave = 0;
        private const int BossInterval = 10;
        private const int SwarmInterval = 6;
        private const float enemy1chance = 0.45f;
        private const float enemy2chance = 0.35f;
        private const float enemy3chance = 0.20f;
        private bool ForceBossWave = false;
        private Random random = new Random();
        public WaveManager(int flags)
        {
            ForceBossWave = (flags & (int)Flags.FORCE_BOSS) > 0;
        }
        public void Reset()
        {
            currentWave = 0;
            CurrentWaveKind = WaveKind.Normal;
        }
        
        public List<Enemy> CreateNextWave(double canvasWidth)
        {
            currentWave++;
            List<Enemy> list = new();
            int capacity = GetEnemyCount();
            CurrentWaveKind = DetermineWaveKind(currentWave);
            switch (CurrentWaveKind)
            {
                case WaveKind.Normal:
                    for(int i = 0; i < capacity; i++)
                    {
                        if(random.NextDouble() > 1 - enemy3chance)
                        {
                            Enemy3 enemy = new();
                            enemy.position.X = i * enemy.dimensions.width;
                            enemy.position.Y = 0;
                            list.Add(enemy);
                        }
                        else if(random.NextDouble() > 1 - enemy2chance)
                        {
                            Enemy2 enemy = new();
                            enemy.position.X = i * enemy.dimensions.width;
                            enemy.position.Y = 0;
                            list.Add(enemy);
                        }
                        else
                        {
                            Enemy1 enemy = new();
                            enemy.position.X = i * enemy.dimensions.width;
                            enemy.position.Y = 0;
                            list.Add(enemy);
                        }
                    }
                    break;
                case WaveKind.Swarm:
                    for(int i = 0; i < capacity/2; i++)
                    {
                        Enemy3 enemy = new();
                        enemy.position.X = i * enemy.dimensions.width;
                        enemy.position.Y = 0;
                        list.Add(enemy);
                    }
                    break;
                case WaveKind.Boss:
                    Boss boss = new();
                    boss.position.X = ((float)canvasWidth - boss.dimensions.width) / 2;
                    boss.position.Y = 0;
                    list.Add(boss);
                    break;
                default:
                    break;
            }
            PositionEnemies(list, canvasWidth);
            return list;
        }
        public void PositionEnemies(List<Enemy> enemies, double canvasWidth)
        {
            const float padding = 25;
            const float spacingX = 12;
            const float spacingY = 12;

            float enemyWidth = 51;
            int columns = Math.Max(1, (int)((canvasWidth - padding * 2) / (enemyWidth + spacingX)));
            for (int i = 0; i < enemies.Count; i++)
            {
                int row = i / columns;
                int column = i % columns;
                Enemy enemy = enemies[i];
                enemy.position.X = padding + column * (enemy.dimensions.width + spacingX);
                enemy.position.Y = padding + row * (enemy.dimensions.height + spacingY);
            }
        }
        public WaveKind DetermineWaveKind(int wave)
        {
            if (ForceBossWave || wave % BossInterval == 0) return WaveKind.Boss;
            if(wave % SwarmInterval == 0 && wave % BossInterval != 0) return WaveKind.Swarm;
            return WaveKind.Normal;
        }
        public int GetEnemyCount()
        {
            float t = 1f - MathF.Exp(-0.15f * (currentWave - 1));
            int count = (int)MathF.Round(8 + (32 - 8) * t);

            return Math.Clamp(count, 8, 32);
        }
    }
}
