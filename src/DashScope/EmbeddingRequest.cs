using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DashScope
{
    /// <summary>
    ///  <see href="https://help.aliyun.com/document_detail/2399495.html"/>
    /// </summary>
    public class EmbeddingRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = Models.TextEmbeddingV1;

        [JsonPropertyName("input")]
        public EmbeddingInput Input { get; set; } = new EmbeddingInput();

        [JsonPropertyName("parameters")]
        public EmbeddingParameters Parameters { get; set; } = new EmbeddingParameters();
    }

    public class EmbeddingInput
    {
        [JsonPropertyName("texts")]
        public string[] Texts { get; set; } = Array.Empty<string>();

    }
    public class EmbeddingParameters
    {
        [JsonPropertyName("text_type")]
        public string TextType { get; set; } = "document";
    }
}
