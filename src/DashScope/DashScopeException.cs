using System.Runtime.Serialization;

namespace DashScope
{
    [Serializable]
    public class DashScopeException : Exception
    {
        public string Code { get; set; } = "Unknown";

        public DashScopeException(string message) : base(message)
        {
        }
        public DashScopeException(string code, string message) : base(message)
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