using LoadExpressApi.Application.Common.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace LoadExpressApi.Persistence.Common
{
    public class NewtonSoftService : ISerializerService
    {
        public T Deserialize<T>(string text) => JsonConvert.DeserializeObject<T>(text);

        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter() { CamelCaseText = true }
            }
        });

        public string Serialize<T>(T obj, Type type) => JsonConvert.SerializeObject(obj, type, new());
    }
}
