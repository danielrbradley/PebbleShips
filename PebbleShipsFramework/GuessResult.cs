namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GuessResult
    {
        public bool IsHit { get; set; }
        public ShipClass? ShipSank { get; set; }

        public override string ToString()
        {
            if (ShipSank.HasValue)
            {
                return string.Format("hit and sank a {0}", this.ShipSank.Value);
            }

            if (this.IsHit)
            {
                return "hit";
            }

            return "miss";
        }
    }
}
