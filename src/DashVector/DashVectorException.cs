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
        public int Code { get; set; }

        public DashVectorException(string message) : base(message)
        {
        }

        public DashVectorException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
