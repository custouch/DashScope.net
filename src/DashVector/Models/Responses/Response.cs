using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Responses
{
    public class Response<T>
    {
        [JsonPropertyName("request_id")]
        public string Code { get; set; }

        [JsonPropertyName("code")]
        public string Message { get; set; }

        [JsonPropertyName("message")]
        public string RequestId { get; set; }

        [JsonPropertyName("output")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T OutPut { get; set; }
    }
}
