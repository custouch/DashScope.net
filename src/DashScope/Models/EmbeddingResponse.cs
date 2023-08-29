using System.Text.Json.Serialization;

namespace DashScope.Models
{
    public class EmbeddingResponse : DashScopeResponse
    {
        [JsonPropertyName("output")]
        public EmbeddingOutput Output { get; set; }

        [JsonPropertyName("usage")]
        public EmbeddingUsage Usage { get; set; }
    }

    public class EmbeddingOutput
    {
        [JsonPropertyName("embeddings")]
        public EmbeddingItem[] Embeddings { get; set; } = Array.Empty<EmbeddingItem>();
    }

    public class EmbeddingItem
    {
        [JsonPropertyName("embedding")]
        public double[] Embedding { get; set; } = Array.Empty<double>();

        [JsonPropertyName("text_index")]
        public int TextIndex { get; set; }
    }

    public class EmbeddingUsage
    {
        [JsonPropertyName("total_tokens")]
        public int TotalTokens { get; set; }
    }
}
