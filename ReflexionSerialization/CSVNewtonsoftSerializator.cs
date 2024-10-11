using Newtonsoft.Json;

namespace ReflexionSerialization
{
    internal class CSVNewtonsoftSerializator : ICSVSerializator
    {
        public string Serialize(object serializationObject)
        {
            return JsonConvert.SerializeObject(serializationObject);
        }

        public object Deserialize(string deserializationString)
        {
            return JsonConvert.DeserializeObject<F>(deserializationString);
        }

        public CheckCSVSerializatorResult CheckSerializationTime(object serializationObject)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            string serialized = Serialize(serializationObject);
            stopwatch.Stop();
            string serializationTime = stopwatch.ElapsedMilliseconds.ToString();

            return new CheckCSVSerializatorResult(serialized, serializationTime, serializationObject.GetType());
        }

        public CheckCSVSerializatorResult CheckDeserializationTime(string deserializationString)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            object deserialized = Deserialize(deserializationString);
            stopwatch.Stop();
            string deserializationTime = stopwatch.ElapsedMilliseconds.ToString();

            return new CheckCSVSerializatorResult(deserialized, deserializationTime, typeof(F));
        }
    }
}
