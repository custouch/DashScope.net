using DashVector.Enums;
using System.Text.Json.Serialization;

namespace DashVector.Models.Requests
{

    public class CreateCollectionRequest
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("dimension")]
        public int Dimension { get; set; }

        [JsonPropertyName("dataType")]
        public CollectionInfo.DataType DataType { get; set; }

        [JsonPropertyName("metric")]
        public CollectionInfo.Metric Metric { get; set; }

        [JsonPropertyName("fieldsSchema")]
        public Dictionary<string, FieldType> FieldsSchema { get; set; }

        [JsonPropertyName("extraParams")]
        public Dictionary<string, string> ExtraParams { get; set; }

        [JsonPropertyName("timeout")]
        public int Timeout { get; set; }



    }
}
