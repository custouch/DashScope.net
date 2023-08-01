using System.Text.Json.Serialization;

namespace DashScope
{
    public class CompletionResponse : DashScopeResponse
    {
        [JsonPropertyName("output")]
        public CompletionOutput Output { get; set; }

        [JsonPropertyName("usage")]
        public CompletionUsage Usage { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }


    public class CompletionOutput
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class CompletionUsage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }



}
