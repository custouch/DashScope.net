using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DashScope.Models
{
    public class CompletionResponse : DashScopeResponse
    {
        [JsonPropertyName("output")]
        public CompletionOutput Output { get; set; } = new CompletionOutput();

        [JsonPropertyName("usage")]
        public CompletionUsage Usage { get; set; } = new CompletionUsage();

        public bool IsTextResponse => Output.IsTextOutput;

    }

    public class CompletionUsage
    {
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
        
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }

    public class CompletionOutput
    {
        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
        
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        
        [JsonPropertyName("choices")]
        public List<Choice>? Choices { get; set; }

        public bool IsTextOutput => Choices == null;
    }
    public class CompletionTextOutput
    {
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = string.Empty;
        
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
    
    public class CompletionMessageOutput
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    
    public class Choice
    {
        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; } = string.Empty;

        [JsonPropertyName("message")] 
        public Message Message { get; set; } = new Message();
    }
    
}
