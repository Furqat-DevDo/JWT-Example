using System.Runtime.Serialization;

namespace JWT.Exceptions
{
    [Serializable]
    internal class PermissionNotFoundException : Exception
    {
        public PermissionNotFoundException()
        {
        }

        public PermissionNotFoundException(string? message) : base(message)
        {
        }

        public PermissionNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PermissionNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}