using System;

namespace LanManager
{
    [Serializable]
    public class ClientOutDatedException : Exception
    {
        public ClientOutDatedException() { }
        public ClientOutDatedException(string message) : base(message) { }
        public ClientOutDatedException(string message, Exception inner) : base(message, inner) { }
        protected ClientOutDatedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
