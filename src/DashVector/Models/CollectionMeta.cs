using DashVector.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models
{
    public class CollectionMeta
    {
        /// <summary>
        /// Collection Name
        /// </summary>
        [Required]
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Collection Dimension(1,20000]
        /// </summary>
        [Required]
        [JsonPropertyName("Dimension")]
        public int Dimension { get; set; }

        /// <summary>
        /// Daata type, Float(default)/INT
        /// </summary>
        [JsonPropertyName("dtype")]
        public Type DataType { get; set; } = typeof(float);

        /// <summary>
        /// Distance measurement
        /// </summary>
        [JsonPropertyName("Metric")]
        public Metric Metric { get; set; } = Metric.Cosine;

        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Fileds, value: Folat/Bool/INT/String
        /// </summary>
        [JsonPropertyName("fields_schema")]
        public Dictionary<string, object> FiledSchema { get; set; }

        /// <summary>
        /// Partition information
        /// </summary>
        [JsonPropertyName("partitions")]
        public Dictionary<string, Status> PartitionStatus { get; set; }
    }
}
