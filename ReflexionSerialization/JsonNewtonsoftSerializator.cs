using Newtonsoft.Json;

namespace ReflectionSerialization.Serializators
{
    public class JsonNewtonsoftSerializator : ISerializator
    {
        public string SerializatorType => "Json";

        public string Serialize<T>(T serializationObject)
        {
            return JsonConvert.SerializeObject(serializationObject, Formatting.Indented);
        }

        public string SerializeIEnumerable<T>(IEnumerable<T> collection)
        {
            return JsonConvert.SerializeObject(collection, Formatting.Indented);
        }

        public T Deserialize<T>(string deserializationString) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(deserializationString);
        }

        public IEnumerable<T> DeserializeIEnumerable<T>(string deserializationString) where T : new()
        {
            return JsonConvert.DeserializeObject<IEnumerable<T>>(deserializationString);
        }
    }
}
