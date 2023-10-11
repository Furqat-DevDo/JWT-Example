using System.Runtime.Serialization;

namespace JWT.Managers
{
    [Serializable]
    internal class PermissionAlreadyExistsException : Exception
    {
        public PermissionAlreadyExistsException()
        {
        }

        public PermissionAlreadyExistsException(string? message) : base(message)
        {
        }

        public PermissionAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PermissionAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}