using System;
using System.Runtime.Serialization;

namespace Mirabeau.uTransporter.Exceptions
{
    [Serializable]
    public class CustomDataTypeArgumentNullException : Exception
    {
        public CustomDataTypeArgumentNullException()
        {
        }

        public CustomDataTypeArgumentNullException(string message)
            : base(message)
        {
        }

        public CustomDataTypeArgumentNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CustomDataTypeArgumentNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
