using DashVector.Enums;
using System.Text.Json.Serialization;

namespace DashVector.Models.Requests
{

    public class CreateCollectionRequest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("dimension")]
        public int Dimension { get; set; }

        [JsonPropertyName("dtype")]
        public DataType DataType { get; set; } = DataType.FLOAT;

        [JsonPropertyName("metric")]
        public string Metric { get; set; } = CollectionInfo.Metric.Cosine;

        [JsonPropertyName("fields_schema")]
        public Dictionary<string, FieldType>? FieldsSchema { get; set; }

        [JsonPropertyName("extra_params")]
        public Dictionary<string, string>? ExtraParams { get; set; }
    }
}
