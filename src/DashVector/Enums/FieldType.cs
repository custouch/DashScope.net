using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DashVector.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FieldType
    {
        BOOL = 0,

        STRING = 1,

        INT = 2,

        FLOAT = 3,

        UNRECOGNIZED = -1
    }
}
