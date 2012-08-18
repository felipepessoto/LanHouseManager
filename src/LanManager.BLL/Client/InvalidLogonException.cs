using System;

namespace LanManager
{
    [Serializable]
    public class InvalidLogOnException : Exception
    {
        public InvalidLogOnException() { }
        public InvalidLogOnException(string message) : base(message) { }
        public InvalidLogOnException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidLogOnException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}