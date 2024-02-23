using System.Text.Json.Serialization;

namespace DashVector.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DataType
    {
        FLOAT,
        INT
    }
}
