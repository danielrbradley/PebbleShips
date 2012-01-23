namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StateChangedArgs : EventArgs
    {
        public StateChangedArgs(GameState oldState, GameState newState)
        {
            this.OldState = oldState;
            this.NewState = newState;
        }

        public GameState OldState { get; private set; }
        public GameState NewState { get; private set; }
    }
}
