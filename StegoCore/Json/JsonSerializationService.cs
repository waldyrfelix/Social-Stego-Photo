using Newtonsoft.Json;

namespace StegoCore.Json
{
    public class JsonSerializationService : ISerializationService
    {
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
