using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Classes
{
    public enum Flags {
        FORCE_BOSS = 0x00000001,
        INVINCIBLE = 0x00000010,
        SUPER_SPEED = 0x00000100,
        FAST_ATTACK = 0x00001000
    }
    public enum State
    {
        Menu,
        Wave,
        BossAlert,
        BossWave,
        Win,
        Loss
    }
    public enum SFX
    {
        Click,
        Explosion,
        Hover,
        Pickup,
        Shoot
    }
    public enum Direction
    {
        Up = -1,
        Down = 1,
        Right = 1,
        Left = -1
    }
    public enum Faction
    {
        Player,
        Enemy
    }
    public struct Position
    {
        public double x, y;
        public Position(double x, double y)
        {
            this.x = x; this.y = y;
        }
    }
    public struct Movement
    {
        public double x, y;
    }
    public struct Velocity
    {
        public double x, y;
    }
    public struct Dimensions
    {
        public float width, height;
        public Dimensions(float width, float height)
        {
            this.width = width;
            this.height = height;
        }
    }
}
