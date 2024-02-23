using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Responses
{
    public class ErrorResponse : ResponseBase<string>
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("httpStatusCode")]
        public int HttpStatusCode { get; set; }

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonPropertyName("accessDeniedDetail")]
        public string AccessDeniedDetail { get; set; }
    }
}
