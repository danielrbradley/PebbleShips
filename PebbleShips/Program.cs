using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PebbleShips
{
    class Program
    {
        static void Main(string[] args)
        {
            // Leave the test mode here in case it's useful later!
            if (args.Any(a => a.ToLower() == "/test"))
            {
                TestMode.Run();
                return;
            }

            var game = new Game();
            var cpuPlayerId = game.RegisterPlayer("Cpu player");
            var humanPlayerId = game.RegisterPlayer("Human Player");

            RandomlyPlaceShips(game, cpuPlayerId);
            game.StartGame(cpuPlayerId);
            RandomlyPlaceShips(game, humanPlayerId);
            game.StartGame(humanPlayerId);

            while (game.State == GameState.Playing)
            {
                if (game.IsAwaitingPlayer(cpuPlayerId))
                {
                    CpuGuess(game, cpuPlayerId);
                }
                else
                {
                    HumanGuess(game, humanPlayerId);
                }
            }

            if (game.HasPlayerWon(humanPlayerId))
            {
                Console.WriteLine("You have won!");
            }
            else
            {
                Console.WriteLine("Sorry, you lost this time.");
            }
        }

        private static void HumanGuess(Game game, Guid humanPlayerId)
        {
            try
            {
                Console.WriteLine("Please enter a cooridinate to guess.");
                var input = Console.ReadLine().ToUpper();
                var guess = new Coordinate(input);
                var result = game.Guess(humanPlayerId, guess);
                Console.WriteLine("It was a {0}!", result);
            }
            catch (FormatException)
            {
                Console.WriteLine("User input must be in the form \"A5\".");
            }
            catch (AlreadyGuessedException)
            {
                Console.WriteLine("You've already guessed that! Have another go.");
            }
        }

        private static void CpuGuess(Game game, Guid playerId)
        {
            var random = new Random();
            try
            {
                var guess = new Coordinate(random.Next(0, 10), random.Next(0, 10));
                var result = game.Guess(playerId, guess);
                Console.WriteLine("Computer guessed {0}, it was a {1}!", guess.ToString(true), result);
            }
            catch (AlreadyGuessedException)
            {
                // Just continue and guess again.
            }
        }

        private static void RandomlyPlaceShips(Game game, Guid playerId)
        {
            RandomlyPlaceShip(game, playerId, 4);
            RandomlyPlaceShip(game, playerId, 4);
            RandomlyPlaceShip(game, playerId, 5);
        }

        private static void RandomlyPlaceShip(Game game, Guid playerId, int length)
        {
            var random = new Random();
            while (true)
            {
                try
                {
                    ShipOrientation orientation;
                    if (random.Next(0, 1) == 0)
                    {
                        orientation = ShipOrientation.Horizontal;
                    }
                    else
                    {
                        orientation = ShipOrientation.Vertical;
                    }

                    Coordinate location;
                    var longSide = random.Next(0, 9);
                    var shortSide = random.Next(0, 9 - length);

                    switch (orientation)
                    {
                        case ShipOrientation.Horizontal:
                            location = new Coordinate(shortSide, longSide);
                            break;
                        default:
                            location = new Coordinate(longSide, shortSide);
                            break;
                    }

                    game.PutShip(playerId, new Ship()
                    {
                        Length = length,
                        Orientation = orientation,
                        Location = location,
                    });

                    // Break out of the while loop.
                    return;
                }
                catch (ShipCollisionException)
                {
                    // Just try again!
                }
            }
        }
    }
}
