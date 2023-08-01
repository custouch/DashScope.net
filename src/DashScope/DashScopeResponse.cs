using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DashScope
{
    public class DashScopeResponse
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }

        [JsonPropertyName("request_Id")]
        public string RequestId { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("output")]
        public Output Output { get; set; }

        [JsonPropertyName("usage")]
        public Usage Usage { get; set; }
        

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }

    public class Output
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class Usage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }
}
