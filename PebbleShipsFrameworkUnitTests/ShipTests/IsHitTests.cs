using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PebbleShips.UnitTests.ShipTests
{
    [TestClass]
    public class ShipGuessTests
    {
        [TestMethod]
        public void HorizontalShipHit1()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
            };
            Assert.IsTrue(ship.IsHit(new Coordinate(0, 0)));
        }

        [TestMethod]
        public void HorizontalShipHit2()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
            };
            Assert.IsTrue(ship.IsHit(new Coordinate(4, 0)));
        }

        [TestMethod]
        public void HorizontalShipMiss1()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
            };
            Assert.IsFalse(ship.IsHit(new Coordinate(5, 0)));
        }

        [TestMethod]
        public void HorizontalShipMiss2()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
            };
            Assert.IsFalse(ship.IsHit(new Coordinate(0, 1)));
        }

        [TestMethod]
        public void HorizontalShipMiss3()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(1, 0),
                Orientation = ShipOrientation.Horizontal,
            };
            Assert.IsFalse(ship.IsHit(new Coordinate(0, 0)));
        }

        [TestMethod]
        public void VerticalShipHit1()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Vertical,
            };
            Assert.IsTrue(ship.IsHit(new Coordinate(0, 0)));
        }

        [TestMethod]
        public void VerticalShipHit2()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Vertical,
            };
            Assert.IsTrue(ship.IsHit(new Coordinate(0, 4)));
        }

        [TestMethod]
        public void VerticalShipMiss1()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Vertical,
            };
            Assert.IsFalse(ship.IsHit(new Coordinate(0, 5)));
        }

        [TestMethod]
        public void VerticalShipMiss2()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Vertical,
            };
            Assert.IsFalse(ship.IsHit(new Coordinate(1, 0)));
        }

        [TestMethod]
        public void VerticalShipMiss3()
        {
            var ship = new Ship()
            {
                Length = 5,
                Location = new Coordinate(0, 1),
                Orientation = ShipOrientation.Vertical,
            };
            Assert.IsFalse(ship.IsHit(new Coordinate(0, 0)));
        }
    }
}
