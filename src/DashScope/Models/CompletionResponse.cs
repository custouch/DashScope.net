using System.Text.Json.Serialization;

namespace DashScope.Models
{
    public class CompletionResponse : DashScopeResponse
    {
        [JsonPropertyName("output")]
        public CompletionOutput Output { get; set; } = new CompletionOutput();

        [JsonPropertyName("usage")]
        public CompletionUsage Usage { get; set; } = new CompletionUsage();

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = string.Empty;
    }


    public class CompletionOutput
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }

    public class CompletionUsage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }



}
