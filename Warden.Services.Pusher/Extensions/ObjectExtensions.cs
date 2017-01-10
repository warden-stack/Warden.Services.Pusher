using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Warden.Services.Pusher.Extensions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd H:mm:ss",
            Formatting = Formatting.Indented,
            DefaultValueHandling = DefaultValueHandling.Populate,
            NullValueHandling = NullValueHandling.Include,
            Error = (serializer, error) => { error.ErrorContext.Handled = true; },
            Converters = new List<JsonConverter>
            {
                new Newtonsoft.Json.Converters.StringEnumConverter
                {
                    AllowIntegerValues = true,
                    CamelCaseText = true
                }
            }
        };

        //TODO: Remove this when the custom JSON serializer works properly with SignalR.
        public static object ToCamelCase(this object value)
            => JsonConvert.DeserializeObject(value.ToJson());

        public static string ToJson(this object value)
            => JsonConvert.SerializeObject(value, JsonSerializerSettings);
    }
}