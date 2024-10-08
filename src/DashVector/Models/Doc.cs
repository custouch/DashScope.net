using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
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
        public float[] Vector { get; set; } = [];

        /// <summary>
        /// sparse vector data
        /// </summary>
        [JsonPropertyName("sparse_vector")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public SortedDictionary<int, float>? SparseVector { get; set; }

        /// <summary>
        /// document custom fields
        /// </summary>
        [JsonPropertyName("fields")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, FieldValue> Fields { get; set; } = [];

        [JsonPropertyName("score")]
        public float Score { get; set; }

        public void AddField(string key, string value)
        {
            Fields ??= [];
            Fields.Add(key, new FieldValue(value));
        }

        public void AddField(string key, int value)
        {
            Fields ??= [];
            Fields.Add(key, new FieldValue(value));
        }

        public void AddField(string key, float value)
        {
            Fields ??= [];
            Fields.Add(key, new FieldValue(value));
        }

        public void AddField(string key, bool value)
        {
            Fields ??= [];
            Fields.Add(key, new FieldValue(value));
        }
    }

    [JsonConverter(typeof(FieldValueTypeConverter))]
    public class FieldValue
    {
        private object value;

        public FieldValue(string value) => this.value = value;
        public FieldValue(int value) => this.value = value;
        public FieldValue(float value) => this.value = value;
        public FieldValue(bool value) => this.value = value;

        public FieldValue(object value)
        {
            if (value is not string or int or float or bool)
            {
                throw new InvalidOperationException($"Cannot stored value type {value.GetType().FullName}");
            }
            else
            {
                this.value = value;
            }
        }

        public bool TryGetValue<T>(out T result)
        {
            if (value is T)
            {
                result = (T)value;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        public T GetValue<T>()
        {
            if (value is T)
            {
                return (T)value;
            }
            throw new InvalidOperationException($"Cannot convert stored value to {typeof(T).Name}");
        }

        public object RawValue => value;

        public static implicit operator FieldValue(string value) => new FieldValue(value);
        public static implicit operator FieldValue(int value) => new FieldValue(value);
        public static implicit operator FieldValue(float value) => new FieldValue(value);
        public static implicit operator FieldValue(bool value) => new FieldValue(value);

        public static implicit operator string(FieldValue value) => value.GetValue<string>();
        public static implicit operator int(FieldValue value) => value.GetValue<int>();
        public static implicit operator float(FieldValue value) => value.GetValue<float>();
        public static implicit operator bool(FieldValue value) => value.GetValue<bool>();
    }

    public class FieldValueTypeConverter : JsonConverter<FieldValue>
    {
        public override FieldValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return new FieldValue(reader.GetString());
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int intValue))
                {
                    return new FieldValue(intValue);
                }
                else if (reader.TryGetSingle(out float floatValue))
                {
                    return new FieldValue(floatValue);
                }
            }
            else if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return new FieldValue(reader.GetBoolean());
            }
            throw new InvalidOperationException("Cannot convert stored value to FieldValue");
        }

        public override void Write(Utf8JsonWriter writer, FieldValue value, JsonSerializerOptions options)
        {
            if (value.TryGetValue(out string stringValue))
            {
                writer.WriteStringValue(stringValue);
            }
            else if (value.TryGetValue(out int intValue))
            {
                writer.WriteNumberValue(intValue);
            }
            else if (value.TryGetValue(out float floatValue))
            {
                writer.WriteNumberValue(floatValue);
            }
            else if (value.TryGetValue(out bool boolValue))
            {
                writer.WriteBooleanValue(boolValue);
            }
            else
            {
                throw new InvalidOperationException("Cannot convert stored value to FieldValue");
            }
        }
    }
}
