namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents the context of a single game of pebbleships.
    /// This manages the game state and all player interactions.
    /// </summary>
    /// <remarks>
    /// States:
    /// Games start in the Setup state while players register and place their ships.
    /// Once both players have indicated they are ready, the game moves to the Playing state.
    /// When one player sinks all of the other players ships the game moves into the finished state.
    /// </remarks>
    public class Game
    {
        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        public Game()
        {
            this.State = GameState.Setup;
            this.players = new Dictionary<Guid,Player>();
            playerNumbers = new List<Guid>(2);

            // Pick a random player to start.
            var r = new Random();
            awaitingPlayer = r.Next(0, 1);
        }

        private int awaitingPlayer;
        private List<Guid> playerNumbers;
        private Dictionary<Guid, Player> players;
        private object playerRegistrationLock = new object();

        /// <summary>
        /// Gets the current state of the game.
        /// </summary>
        public GameState State { get; private set; }

        /// <summary>
        /// Check if the game is awaiting a move by the specified player.
        /// </summary>
        /// <param name="playerId">Identifier of the player to check.</param>
        /// <returns>Boolean indicating if the game is awaiting the player.</returns>
        public bool IsAwaitingPlayer(Guid playerId)
        {
            if (this.State != GameState.Playing)
            {
                return false;
            }

            var index = playerNumbers.IndexOf(playerId);
            if (index < 0)
            {
                return false;
            }

            return index == awaitingPlayer;
        }

        /// <summary>
        /// Register a new player in the game. A maximum of 2 players are allowed in any game instance.
        /// </summary>
        /// <param name="name">Name of the player to register.</param>
        /// <returns>The identifier of the player, used in all subsequent interactions with the game.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a player attempts to register once the game has already started.</exception>
        /// <exception cref="GameFullException">Thrown if two players are already registered in this game.</exception>
        public Guid RegisterPlayer(string name)
        {
            if (this.State != GameState.Setup)
            {
                throw new InvalidOperationException("Players can only register during game setup.");
            }

            lock (playerRegistrationLock)
            {
                if (players.Count >= 2)
                {
                    throw new GameFullException("Two players are already registered in this game.");
                }

                var playerId = Guid.NewGuid();
                var playerContext = new Player(playerId) { Name = name };
                if (players.Count > 0)
                {
                    playerContext.OpponentBoard = players[playerNumbers[0]].OwnBoard;
                    players[playerNumbers[0]].OpponentBoard = playerContext.OwnBoard;
                }

                players.Add(playerId, playerContext);
                playerNumbers.Add(playerId);
                return playerId;
            }
        }

        /// <summary>
        /// Place a ship on a players board.
        /// </summary>
        /// <param name="playerId">Identifier of the player to place the ship for.</param>
        /// <param name="ship">Details of the ship to place.</param>
        /// <exception cref="InvalidOperationException">Thrown if a ship is placed in any other state than during setup.</exception>
        /// <exception cref="ShipCollisionException">Thrown if placing the ship would overlap with an existing ship.</exception>
        public void PutShip(Guid playerId, Ship ship)
        {
            if (this.State != GameState.Setup)
            {
                throw new InvalidOperationException("Cannot put ship in a game that has already started.");
            }

            players[playerId].PlaceShip(ship);
        }

        /// <summary>
        /// Indicate that the player is ready to start the game. This is used during game setup.
        /// </summary>
        /// <param name="playerId">Identifier of the player marking themselves as ready.</param>
        /// <exception cref="InvalidOperationException">Thrown if the game is not in the setup state.</exception>
        public void StartGame(Guid playerId)
        {
            if (this.State != GameState.Setup)
            {
                throw new InvalidOperationException("Cannot start game that is not in the setup state.");
            }

            players[playerId].Ready = true;

            if (players.Values.Count(p => p.Ready) == 2)
            {
                ChangeState(GameState.Playing);
            }
        }

        /// <summary>
        /// Perform a guess for a player.
        /// </summary>
        /// <param name="playerId">Identifier of the player making the guess.</param>
        /// <param name="guess">Coordinate of the guess.</param>
        /// <returns>Result of the guess.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a guess is made when the state is not Playing or if it is not the players turn.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The point is not valid for at 10 by 10 board.</exception>
        /// <exception cref="AlreadyGuessedException">Thrown if the user guesses a square multiple times.</exception>
        public GuessResult Guess(Guid playerId, Coordinate guess)
        {
            if (this.State != GameState.Playing)
            {
                throw new InvalidOperationException("Can only make guesses while in the game play state.");
            }

            if (playerNumbers.IndexOf(playerId) != awaitingPlayer)
            {
                throw new InvalidOperationException("Cannot make guess while awaiting other player.");
            }

            Player player = players[playerId];
            var result = player.Guess(guess);
            if (result.IsHit)
            {
                // Check if we've got a win.
                if (player.OpponentBoard.RemainingHis == 0)
                {
                    ChangeState(GameState.Finished);
                }
            }

            ChangePlayer();

            return result;
        }

        /// <summary>
        /// Indicates if a specific player has won the game.
        /// </summary>
        /// <param name="playerId">Identifier of the player to check.</param>
        /// <returns>Boolean indicating if the player has won the game.</returns>
        public bool HasPlayerWon(Guid playerId)
        {
            if (this.State != GameState.Finished)
            {
                return false;
            }

            return players[playerId].OpponentBoard.RemainingHis == 0;
        }

        #region Events

        public event EventHandler<StateChangedArgs> StateChanged;
        private object stateChangeLock = new object();

        private void ChangeState(GameState newState)
        {
            lock (stateChangeLock)
            {
                var oldState = this.State;
                this.State = newState;
                var temp = StateChanged;
                if (temp != null)
                {
                    temp(this, new StateChangedArgs(oldState, newState));
                }
            }
        }

        public event EventHandler CurrentPlayerChanged;
        private object playerChangeLock = new object();

        private void ChangePlayer()
        {
            lock (playerChangeLock)
            {
                awaitingPlayer = (awaitingPlayer + 1) % 2;
                var temp = CurrentPlayerChanged;
                if (temp != null)
                {
                    temp(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}
