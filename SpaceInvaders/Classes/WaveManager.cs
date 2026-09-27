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
        private static int BossInterval = 10;
        private static int SwarmInterval = 6;
        private int wave = 0;
        private int enemiesLeft = 0;
        public WaveManager() { }
        public void makeEnemies()
        {

        }
        public void bulletSpawner(double x, double y, Direction dir)
        {

        }
        public WaveKind DetermineWaveKind(int wave)
        {
            if (wave % BossInterval == 0) return WaveKind.Boss;
            if(wave % SwarmInterval == 0 && wave % BossInterval != 0) return WaveKind.Swarm;
            return WaveKind.Normal;
        }
    }
}
