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
}
