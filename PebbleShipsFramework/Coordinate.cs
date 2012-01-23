namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a position on a board.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Initializes a new instance of the Coordinate class.
        /// </summary>
        public Coordinate()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Coordinate class from a coordinate string.
        /// </summary>
        /// <param name="alphaNumericCoordinate">Alpha-numeric coordinate string in the format "[A-J][1-10]".</param>
        /// <exception cref="FormatException">Thrown if the string is not in an acceptable format.</exception>
        public Coordinate(string alphaNumericCoordinate)
        {
            alphaNumericCoordinate = alphaNumericCoordinate.ToUpper();
            if (!Regex.IsMatch(alphaNumericCoordinate, "^[A-J]([1-9]|10)$", RegexOptions.IgnoreCase))
            {
                throw new FormatException("The coordinate must be in the format [Letter][1-10].");
            }
            else
            {
                var letter = alphaNumericCoordinate.First();
                var number = int.Parse(alphaNumericCoordinate.Substring(1));
                this.Long = (int)(letter - 'A');
                this.Lat = number - 1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Coordinate class with its position.
        /// </summary>
        /// <param name="longitude">The horizontal position.</param>
        /// <param name="latitude">The vertical position.</param>
        public Coordinate(int longitude, int latitude)
        {
            this.Long = longitude;
            this.Lat = latitude;
        }

        /// <summary>
        /// Gets the longitude, the horizontal position.
        /// </summary>
        public int Long { get; set; }

        /// <summary>
        /// Gets the latitude, the virtical position.
        /// </summary>
        public int Lat { get; set; }

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool alphaNumeric)
        {
            if (alphaNumeric)
            {
                // The more human-readable format with letters and a 1-based index.
                return string.Format("{0}{1}", (char)(this.Long + (int)'A'), this.Lat + 1);
            }
            else
            {
                return string.Format("({0},{1})", this.Long, this.Lat);
            }
        }
    }
}
