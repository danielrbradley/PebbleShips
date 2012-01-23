using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PebbleShips.UnitTests.ShipTests
{
    [TestClass]
    public class IntersectsTests
    {
        [TestMethod]
        public void ShipIntersectOverlayed()
        {
            var a = new Ship()
            {
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            var b = new Ship()
            {
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            Assert.IsTrue(Ship.Intersects(a, b));
        }

        [TestMethod]
        public void ShipIntersectOffset()
        {
            var a = new Ship()
            {
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            var b = new Ship()
            {
                Location = new Coordinate(3, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            Assert.IsTrue(Ship.Intersects(a, b));
        }

        [TestMethod]
        public void ShipIntersectEndToEnd()
        {
            var a = new Ship()
            {
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            var b = new Ship()
            {
                Location = new Coordinate(4, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            Assert.IsFalse(Ship.Intersects(a, b));
        }

        [TestMethod]
        public void ShipIntersectOpposingAtRoot()
        {
            var a = new Ship()
            {
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            var b = new Ship()
            {
                Location = new Coordinate(0, 0),
                Orientation = ShipOrientation.Vertical,
                Length = 4,
            };
            Assert.IsTrue(Ship.Intersects(a, b));
        }

        [TestMethod]
        public void ShipIntersectOpposingAtEnd()
        {
            var a = new Ship()
            {
                Location = new Coordinate(0, 3),
                Orientation = ShipOrientation.Horizontal,
                Length = 4,
            };
            var b = new Ship()
            {
                Location = new Coordinate(3, 0),
                Orientation = ShipOrientation.Vertical,
                Length = 4,
            };
            Assert.IsTrue(Ship.Intersects(a, b));
        }

        [TestMethod]
        public void ShipIntersectMidCross()
        {
            var a = new Ship()
            {
                Location = new Coordinate(0, 2),
                Orientation = ShipOrientation.Horizontal,
                Length = 5,
            };
            var b = new Ship()
            {
                Location = new Coordinate(2, 0),
                Orientation = ShipOrientation.Vertical,
                Length = 5,
            };
            Assert.IsTrue(Ship.Intersects(a, b));
        }
    }
}
