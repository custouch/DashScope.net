using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models
{
    public class CollectionStats
    {
        [JsonPropertyName("total_doc_count")]
        [JsonConverter(typeof(LongToStringConverter))]
        public long TotalDocCount { get; set; }

        [JsonPropertyName("index_completeness")]
        public float IndexCompleteness { get; set; }

        [JsonPropertyName("partitions")]
        public Dictionary<string, PartitionStats>? Partitions { get; set; }
    }
}
