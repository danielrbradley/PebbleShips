namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    public class GameFullException : Exception
    {
        public GameFullException() : base() { }
        public GameFullException(string message) : base(message) { }
        public GameFullException(string message, Exception innerException) : base(message, innerException) { }
        public GameFullException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
