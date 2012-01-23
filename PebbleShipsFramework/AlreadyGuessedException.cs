namespace PebbleShips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;

    public class AlreadyGuessedException : Exception
    {
        public AlreadyGuessedException() : base() { }
        public AlreadyGuessedException(string message) : base(message) { }
        public AlreadyGuessedException(string message, Exception innerException) : base(message, innerException) { }
        public AlreadyGuessedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
