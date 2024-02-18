using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models
{
    public class Doc
    {
        /// <summary>
        /// primary key
        /// </summary>
        [JsonPropertyName("id")]

        public string Id { get; set; }

        /// <summary>
        /// vector data
        /// </summary>
        [JsonPropertyName("vector")]
        public Vector<float> Vector { get; set; }

        /// <summary>
        /// sparse vector data
        /// </summary>
        [JsonPropertyName("sparse_vector")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<int, float> SparseVector { get; set; }

        /// <summary>
        /// document custom fields
        /// </summary>
        [JsonPropertyName("fields")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object> Fields { get; set; }

        [JsonPropertyName("score")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float Score { get; set; }

        public void addFields(string key, string value)
        {
            this.Fields.Add(key, value);
        }

        public void addFields(string key, int value)
        {
            this.Fields.Add(key, value);
        }

        public void addFields(string key, float value)
        {
            this.Fields.Add(key, value);
        }

        public void addFields(string key, bool value)
        {
            this.Fields.Add(key, value);
        }
    }
}
