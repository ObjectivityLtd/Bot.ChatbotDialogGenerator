namespace RoslynWrapper.Tests.Utils
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidSyntaxTreeException : Exception
    {
        public InvalidSyntaxTreeException()
        {
        }

        public InvalidSyntaxTreeException(string message)
            : base(message)
        {
        }

        public InvalidSyntaxTreeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected InvalidSyntaxTreeException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }
    }
}