using DashVector.Enums;
using System;
using System.Collections.Generic;
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
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Collection Dimension(1,20000]
        /// </summary>
        [JsonPropertyName("dimension")]
        public int Dimension { get; set; }

        /// <summary>
        /// Data type, Float(default)/INT
        /// </summary>
        [JsonPropertyName("dtype")]
        public DataType DataType { get; set; } = DataType.FLOAT;

        /// <summary>
        /// Distance measurement
        /// </summary>
        [JsonPropertyName("metric")]
        public string Metric { get; set; } = CollectionInfo.Metric.Cosine;

        /// <summary>
        /// Status
        /// </summary>
        [JsonPropertyName("status")]
        public CollectionStatus Status { get; set; }

        /// <summary>
        /// Fields, value: Float/Bool/INT/String
        /// </summary>
        [JsonPropertyName("fields_schema")]
        public Dictionary<string, FieldType> FiledSchema { get; set; } = [];

        /// <summary>
        /// PartitionName information
        /// </summary>
        [JsonPropertyName("partitions")]
        public Dictionary<string, string> PartitionStatus { get; set; }
    }
}
