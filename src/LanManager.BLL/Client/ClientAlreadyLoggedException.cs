using System;

namespace LanManager
{
    [Serializable]
    public class ClientAlreadyLoggedException : Exception
    {
        public ClientAlreadyLoggedException() { }
        public ClientAlreadyLoggedException(string message) : base(message) { }
        public ClientAlreadyLoggedException(string message, System.Exception inner) : base(message, inner) { }
        protected ClientAlreadyLoggedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}