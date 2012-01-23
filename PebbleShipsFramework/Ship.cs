namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Represents a ship on a players board, its location and its 'sunken-ness'.
    /// </summary>
    public class Ship
    {
        /// <summary>
        /// Gets or sets the location of the ship on the board.
        /// </summary>
        public Coordinate Location { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the ship on the board.
        /// </summary>
        public ShipOrientation Orientation { get; set; }

        private ShipClass shipClass;

        /// <summary>
        /// Gets or sets the nautical class of the ship and therefore its length.
        /// </summary>
        public ShipClass Class
        {
            get
            {
                return this.shipClass;
            }
            set
            {
                this.shipClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the ship, indicating its nautical class.
        /// </summary>
        public int Length
        {
            get
            {
                return (int)shipClass;
            }
            set
            {
                switch (value)
                {
                    case 4:
                    case 5:
                        // Allowed value.
                        break;
                    default:
                        throw new ArgumentException("The specified length does not equate to a valid ship class.", "value");
                }

                this.shipClass = (ShipClass)value;
            }
        }

        internal int Hits { get; set; }

        /// <summary>
        /// Gets the location of the other end of the ship.
        /// </summary>
        public Coordinate EndCoordinate
        {
            get
            {
                switch (this.Orientation)
                {
                    case ShipOrientation.Horizontal:
                        return new Coordinate(this.Location.Long + Length - 1, this.Location.Lat);
                    default:
                        return new Coordinate(this.Location.Long, this.Location.Lat + Length - 1);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating if this ship has been sunk.
        /// </summary>
        public bool IsSunk
        {
            get
            {
                return this.Length == this.Hits;
            }
        }

        /// <summary>
        /// Gets a value indicating if the parameters of the ship are valid based on a 10 by 10 board.
        /// </summary>
        public bool IsValid
        {
            get
            {
                switch (this.Orientation)
                {
                    case ShipOrientation.Horizontal:
                        return this.Location.Long < 10 && (this.Location.Lat + Length) < 10;
                    case ShipOrientation.Vertical:
                        return this.Location.Lat < 10 && (this.Location.Long + Length) < 10;
                }

                throw new Exception("Invalid orientation.");
            }
        }

        /// <summary>
        /// Check if there are any collisions within a set of ships.
        /// </summary>
        /// <param name="ships">List of ships to check for collisions</param>
        /// <returns>Boolean indicating if any collisions were found.</returns>
        public static bool HasCollisions(IList<Ship> ships)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                for (int j = i + 1; j < ships.Count; j++)
                {
                    if (Ship.Intersects(ships[i], ships[j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determine if two ships are intersecting (overlapping).
        /// </summary>
        /// <param name="a">First ship.</param>
        /// <param name="b">Second ship.</param>
        /// <returns>Boolean indicating if the ships overlap.</returns>
        public static bool Intersects(Ship a, Ship b)
        {
            // This is a little hackish, would look for a better solution, 
            // but this appears to work against the tests and is not a performance bottleneck.

            switch (a.Orientation)
            {
                case ShipOrientation.Horizontal:
                    switch (b.Orientation)
                    {
                        case ShipOrientation.Horizontal:
                            return a.Location.Lat == b.Location.Lat
                                && ((a.Location.Long <= b.Location.Long && a.EndCoordinate.Long >= b.Location.Long)
                                || (b.Location.Long <= a.Location.Long && b.EndCoordinate.Long >= a.Location.Long));
                        default:
                            return (a.Location.Long <= b.Location.Long && a.EndCoordinate.Long >= b.Location.Long)
                                && (b.Location.Lat <= a.Location.Lat && b.EndCoordinate.Lat >= a.Location.Lat);
                    }
                default:
                    switch (b.Orientation)
                    {
                        case ShipOrientation.Vertical:
                            return a.Location.Long == b.Location.Long
                                && ((a.Location.Lat <= b.Location.Lat && a.EndCoordinate.Lat >= b.Location.Lat)
                                || (b.Location.Lat <= a.Location.Lat && b.EndCoordinate.Lat >= a.Location.Lat));
                        default:
                            return (a.Location.Lat <= b.Location.Lat && a.EndCoordinate.Lat >= b.Location.Lat)
                                && (b.Location.Long <= a.Location.Long && b.EndCoordinate.Long >= a.Location.Long);
                    }
            }
        }

        /// <summary>
        /// Check if a guess would hit the ship.
        /// </summary>
        /// <param name="guess">Coordinate of the guess to evaluate.</param>
        /// <returns>Boolean indicating if the guess will hit the ship.</returns>
        public bool IsHit(Coordinate guess)
        {
            switch (this.Orientation)
            {
                case ShipOrientation.Horizontal:
                    return this.Location.Lat == guess.Lat && guess.Long >= this.Location.Long && guess.Long < (this.Location.Long + this.Length);
                default:
                    return this.Location.Long == guess.Long && guess.Lat >= this.Location.Lat && guess.Lat < (this.Location.Lat + this.Length);
            }
        }

        /// <summary>
        /// Make a guess against this ship.
        /// </summary>
        /// <param name="guess">Coordinate of the guess to evaluate.</param>
        /// <returns>Boolean indicating if the ship was hit.</returns>
        public bool Guess(Coordinate guess)
        {
            bool isHit = IsHit(guess);
            if (isHit)
            {
                this.Hits++;
            }

            return isHit;
        }
    }
}
