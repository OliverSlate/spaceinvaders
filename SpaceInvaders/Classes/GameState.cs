using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Classes
{
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
    public struct Position
    {
        public float x, y;
    }
    public enum Direction
    {
        Up = -1,
        Down = 1
    }
}
