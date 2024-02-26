using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Requests
{
    public class QueryDocRequest
    {
        [JsonPropertyName("vector")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float[]? Vector { get; set; }

        [JsonPropertyName("sparse_vector")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<int, float>? SparseVector { get; set; }


        [JsonPropertyName("id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Id { get; set; }

        [JsonPropertyName("filter")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Filter { get; set; }

        [JsonPropertyName("topk")]
        public int TopK { get; set; } = 10;

        [JsonPropertyName("include_vector")]
        public bool IncludeVector { get; set; } = false;

        [JsonPropertyName("partition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Partition { get; set; }
    }
}
