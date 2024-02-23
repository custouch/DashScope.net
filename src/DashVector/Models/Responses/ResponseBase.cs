using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Responses
{
    public class ResponseBase<T>
    {
        [JsonPropertyName("code")]
        public T Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
