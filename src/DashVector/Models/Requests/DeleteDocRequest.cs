using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Requests
{
    public class DeleteDocRequest
    {
        [JsonPropertyName("ids")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string>? Ids { get; set; }

        [JsonPropertyName("partition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Partition { get; set; }

        [JsonPropertyName("delete_all")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DeleteAll { get; set; } = false;
    }
}
