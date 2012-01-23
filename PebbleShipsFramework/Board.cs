namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a players board, the ship positions and the guesses that have been made against the board.
    /// </summary>
    public class Board
    {
        private Guess[] attempts = new Guess[100];
        private List<Ship> placedShips = new List<Ship>(3);

        /// <summary>
        /// The array of guesses against board positions.
        /// </summary>
        public Guess[] Attempts
        {
            get
            {
                var copy = new Guess[100];
                attempts.CopyTo(copy, 0);
                return copy;
            }
        }

        /// <summary>
        /// Attempt to place a ship on the board.
        /// </summary>
        /// <param name="ship">Details of the ship to place.</param>
        /// <exception cref="ShipCollisionException">Thrown if placing the ship would overlap with an existing ship.</exception>
        public void PlaceShip(Ship ship)
        {
            var newPlacedShips = new List<Ship>(placedShips);
            newPlacedShips.Add(ship);

            if (Ship.HasCollisions(newPlacedShips))
            {
                throw new ShipCollisionException("Placing this ship would overlap with an existing ship.");
            }

            placedShips = newPlacedShips;
        }

        /// <summary>
        /// Make a guess against the board.
        /// </summary>
        /// <param name="guess">Position requested to check.</param>
        /// <returns>Result of the guess.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The point is not valid for at 10 by 10 board.</exception>
        /// <exception cref="AlreadyGuessedException">Thrown if the user guesses a square multiple times.</exception>
        public GuessResult Guess(Coordinate guess)
        {
            if (guess.Lat > 9 || guess.Long > 9 || guess.Lat < 0 || guess.Long < 0)
            {
                throw new ArgumentOutOfRangeException("guess", "Guess point must be within a 0-indexed, 10 by 10 board.");
            }

            var linearPosition = guess.Long + (10 * guess.Lat);
            if (attempts[linearPosition] != PebbleShips.Guess.Unknown)
            {
                throw new AlreadyGuessedException("A guess has already been made at that location");
            }

            foreach (var ship in placedShips)
            {
                var isHit = ship.Guess(guess);
                if (isHit)
                {
                    attempts[linearPosition] = PebbleShips.Guess.Hit;
                    var result = new GuessResult() { IsHit = true };
                    if (ship.IsSunk)
                    {
                        result.ShipSank = (ShipClass?)ship.Class;
                    }

                    return result;
                }
            }

            attempts[linearPosition] = PebbleShips.Guess.Miss;
            return new GuessResult();
        }

        /// <summary>
        /// Gets a list of ships placed on the board.
        /// </summary>
        public IList<Ship> PlacedShips
        {
            get
            {
                return placedShips.ToList();
            }
        }

        /// <summary>
        /// Gets the number of hits still remaining to sink all ships on the board.
        /// </summary>
        public int RemainingHis
        {
            get
            {
                return placedShips.Sum(s => s.Length - s.Hits);
            }
        }
    }
}
