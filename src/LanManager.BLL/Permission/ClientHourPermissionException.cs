using System;

namespace LanManager
{
    /// <summary>
    /// Cliente não tem permissão do responsável pra se logar este horário
    /// </summary>
    [Serializable]
    public class ClientHourPermissionException : Exception
    {
        public ClientHourPermissionException() { }
        public ClientHourPermissionException(string message) : base(message) { }
        public ClientHourPermissionException(string message, Exception inner) : base(message, inner) { }
        protected ClientHourPermissionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
