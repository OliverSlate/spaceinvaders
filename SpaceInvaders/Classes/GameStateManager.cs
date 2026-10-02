using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.Classes
{
    internal class GameStateManager
    {
        public State CurrentState { get; private set; } = State.Menu;
        public event Action<State> StateChanged;

        public void SetState(State newState)
        {
            if (newState == CurrentState) return;
            CurrentState = newState;
            StateChanged?.Invoke(newState);
        }
        public void EndGame()
        {

        }
    }
}
