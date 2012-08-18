using System;

namespace LanManager
{
    [Serializable]
    public class ClientLowCreditException : Exception
    {
        public ClientLowCreditException() { }
        public ClientLowCreditException(string message) : base(message) { }
        public ClientLowCreditException(string message, Exception inner) : base(message, inner) { }
        protected ClientLowCreditException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
