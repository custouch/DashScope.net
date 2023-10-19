using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace DashScope
{
    [Serializable]
    public class DashScopeException : Exception
    {
        public string Code { get; set; } = string.Empty;

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

        public DashScopeException(string code, string message, Exception innerException) : base(message, innerException)
        {
            this.Code = code;
        }

        protected DashScopeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}