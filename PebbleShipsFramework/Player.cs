namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Player
    {
        public Player(Guid playerId, Board opponentBoard = null)
        {
            this.OwnBoard = new Board();
            this.playerId = playerId;
            this.OpponentBoard = opponentBoard;
        }

        private Guid playerId;
        public Board OwnBoard { get; private set; }
        public Board OpponentBoard { get; internal set; }

        public Guid PlayerId
        {
            get
            {
                return playerId;
            }
        }

        public bool Ready { get; set; }
        public string Name { get; set; }

        public void PlaceShip(Ship ship)
        {
            this.OwnBoard.PlaceShip(ship);
        }

        public GuessResult Guess(Coordinate guess)
        {
            return this.OpponentBoard.Guess(guess);
        }
    }
}
