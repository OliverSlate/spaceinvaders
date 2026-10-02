using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders.Classes
{
    internal class GameSession
    {
        public float sessionScore = 0;
        public float sessionMultiplier = 1.0f;
        private GameStateManager stateManager;

        public GameSession(GameStateManager stateManager)
        {
            this.stateManager = stateManager;
        }
       
    }
}
