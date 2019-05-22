using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Policy_Validator.Models;

namespace Policy_Validator.Controllers
{
    public class StrictStringConverter : JsonConverter
{
    readonly JsonSerializer defaultSerializer = new JsonSerializer();

    public override bool CanConvert(Type objectType) 
    {
        return objectType.IsStringType();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        switch (reader.TokenType)
        {
            case JsonToken.String:
            case JsonToken.Null:
                return defaultSerializer.Deserialize(reader, objectType);
            default:
                throw new JsonSerializationException();
        }
    }

    public override bool CanWrite {get{return false;}}

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

    public static class JsonExtensions
    {
        public static bool IsStringType(this Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(string))
                return true;
            return false;
        }
    }
}