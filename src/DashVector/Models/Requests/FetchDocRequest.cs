using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Requests
{
    public class FetchDocRequest
    {
        [JsonPropertyName("Ids")]
        public List<string> Ids { get; set; }

        [JsonPropertyName("PartitionName")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PartitionName { get; set; }
    }
}
