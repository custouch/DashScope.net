using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DashScope.Models
{
    public class TokenizerRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = DashScopeModels.QWenTurbo;

        [JsonPropertyName("input")]
        public CompletionInput Input { get; set; } = new();
    }

    public class TokenizerResponse : DashScopeResponse
    {
        [JsonPropertyName("tokens")]
        public List<string> Tokens { get; set; } = new List<string>();

        [JsonPropertyName("token_ids")]
        public List<int> TokenIds { get; set; } = new List<int>();

        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }
    }
}
