using System.Runtime.Serialization;

namespace DashScope
{
    [Serializable]
    public class DashScopeException : Exception
    {
        public int Code { get; set; } = -1;

        public DashScopeException(string message) : base(message)
        {
        }
        public DashScopeException(int code, string message) : base(message)
        {
            this.Code = code;
        }

        public DashScopeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DashScopeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}