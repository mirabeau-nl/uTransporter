using System;
using System.Runtime.Serialization;

namespace Mirabeau.uTransporter.Exceptions
{
    [Serializable]
    public class AliasNullException : Exception
    {
        public AliasNullException()
        {
        }

        public AliasNullException(string message)
            : base(message)
        {
        }

        public AliasNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AliasNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}