namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class TestMode
    {
        public static void Run()
        {
            var game = new Game();
            game.StateChanged += new EventHandler<StateChangedArgs>(StateChangeHandler);
            game.CurrentPlayerChanged += new EventHandler(PlayerChangeHandler);

            var player1Id = game.RegisterPlayer("Player 1");
            var player2Id = game.RegisterPlayer("Player 2");

            // Get player 1 ready.
            game.PutShip(player1Id, new Ship()
            {
                Location = new Coordinate(3, 0),
                Class = ShipClass.Battleship,
                Orientation = ShipOrientation.Horizontal
            });
            game.PutShip(player1Id, new Ship()
            {
                Location = new Coordinate(2, 1),
                Class = ShipClass.Destroyer,
                Orientation = ShipOrientation.Horizontal
            });
            game.PutShip(player1Id, new Ship()
            {
                Location = new Coordinate(5, 3),
                Class = ShipClass.Destroyer,
                Orientation = ShipOrientation.Vertical
            });
            game.StartGame(player1Id);

            // Get player 2 ready.
            game.PutShip(player2Id, new Ship()
            {
                Location = new Coordinate(3, 3),
                Class = ShipClass.Battleship,
                Orientation = ShipOrientation.Horizontal
            });
            game.PutShip(player2Id, new Ship()
            {
                Location = new Coordinate(2, 4),
                Class = ShipClass.Destroyer,
                Orientation = ShipOrientation.Horizontal
            });
            game.PutShip(player2Id, new Ship()
            {
                Location = new Coordinate(5, 5),
                Class = ShipClass.Destroyer,
                Orientation = ShipOrientation.Vertical
            });
            game.StartGame(player2Id);

            var random = new Random();

            while (game.State == GameState.Playing)
            {
                Guid currentPlayer;
                int playerNum;
                if (game.IsAwaitingPlayer(player1Id))
                {
                    playerNum = 1;
                    currentPlayer = player1Id;
                }
                else
                {
                    playerNum = 2;
                    currentPlayer = player2Id;
                }

                try
                {
                    var guess = new Coordinate(random.Next(0, 10), random.Next(0, 10));
                    var result = game.Guess(currentPlayer, guess);
                    Console.WriteLine("Player {0} guessed {1}, it was a {2}!", playerNum, guess, result);
                }
                catch (AlreadyGuessedException)
                {
                    // Just continue and guess again.
                }
            }

            if (game.HasPlayerWon(player1Id))
            {
                Console.WriteLine("Player 1 has won.");
            }
            else
            {
                Console.WriteLine("Player 2 has won.");
            }
        }

        private static void StateChangeHandler(object src, StateChangedArgs stateArgs)
        {
            Console.WriteLine("State changed from {0} to {1}.", stateArgs.OldState, stateArgs.NewState);
        }

        private static void PlayerChangeHandler(object src, EventArgs args)
        {
            Console.WriteLine("Awaiting next player.");
        }
    }
}
