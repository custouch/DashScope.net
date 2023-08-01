using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DashScope
{
    public class CompletionRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("input")]
        public CompletionInput Input { get; set; } = new ();

        [JsonPropertyName("parameters")]
        public CompletionParameters Parameters { get; set; } = new();
    }
    public class CompletionInput
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("history")]
        public List<CompletionHistoryItem> History { get; set; }

    }
    public class CompletionHistoryItem
    {
        public string User { get; set; }
        public string Bot { get; set; }
    }

    public class CompletionParameters
    {
        [JsonPropertyName("top_p")]
        public float? TopP { get; set; } = 0.8f;

        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }

        [JsonPropertyName("seed")]
        public int? Seed { get; set; } = 1234;

        [JsonPropertyName("enable_search")]
        public bool? EnableSearch { get; set; } = false;
    }
}
