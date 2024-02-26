using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CollectionStatus
    {
        INITIALIZED,
        SERVING,
        DROPPING,
        ERROR
    }
}
