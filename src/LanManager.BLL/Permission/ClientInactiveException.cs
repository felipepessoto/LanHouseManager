using System;

namespace LanManager
{
    [Serializable]
    public class ClientInactiveException : Exception
    {
        public ClientInactiveException() { }
        public ClientInactiveException(string message) : base(message) { }
        public ClientInactiveException(string message, Exception inner) : base(message, inner) { }
        protected ClientInactiveException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
