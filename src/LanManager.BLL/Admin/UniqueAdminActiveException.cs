using System;

namespace LanManager.BLL.Admin
{
    /// <summary>
    /// Cliente não tem permissão do responsável pra se logar este horário
    /// </summary>
    [Serializable]
    public class UniqueAdminActiveException : Exception
    {
        public UniqueAdminActiveException() { }
        public UniqueAdminActiveException(string message) : base(message) { }
        public UniqueAdminActiveException(string message, Exception inner) : base(message, inner) { }
        protected UniqueAdminActiveException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}