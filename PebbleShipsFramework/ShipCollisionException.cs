namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    public class ShipCollisionException : Exception
    {
        public ShipCollisionException() : base() { }
        public ShipCollisionException(string message) : base(message) { }
        public ShipCollisionException(string message, Exception innerException) : base(message, innerException) { }
        public ShipCollisionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
