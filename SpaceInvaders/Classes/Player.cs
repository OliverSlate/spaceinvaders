using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Classes
{
    internal class Player
    {
        public Position position;
        public int health = 3;
        public int shield = 0;

        public void Move(float x, float y)
        {
            position.x += x;
            position.y += y;
        }
        public void Shoot()
        {

        }
    }
}
