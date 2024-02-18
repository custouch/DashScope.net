using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DashVector
{
    public class DashVectorException : Exception
    {
        public string Code { get; set; } = string.Empty;

        public DashVectorException(string message) : base(message)
        {
        }

        public DashVectorException(string code, string message) : base(message)
        {
            this.Code = code;
        }

        public DashVectorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DashVectorException(string code, string message, Exception innerException) : base(message, innerException)
        {
            this.Code = code;
        }

        protected DashVectorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
