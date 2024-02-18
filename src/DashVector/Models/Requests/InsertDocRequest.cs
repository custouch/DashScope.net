using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Models.Requests
{
    public class InsertDocRequest
    {
        [JsonPropertyName("docs")]
        public List<Doc> Docs { get; set; }

        [JsonPropertyName("partition")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Partition { get; set; }
    }
}
