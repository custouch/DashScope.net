using System.Text.Json.Serialization;
using System.Text.Json;

namespace DashVector
{
    public class LongToStringConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt64(out long value))
            {
                return value;
            }
            if (reader.TokenType == JsonTokenType.String && long.TryParse(reader.GetString(), out value))
            {
                return value;
            }
            throw new JsonException("Expected long or string");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}
