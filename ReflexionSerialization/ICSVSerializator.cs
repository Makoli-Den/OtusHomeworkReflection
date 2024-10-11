using System.Diagnostics;

namespace ReflexionSerialization
{
    internal interface ICSVSerializator
    {
        string Serialize<T>(T serializationObject);
        T Deserialize<T>(string deserializationString) where T : new();

        string CreateCsvHeaders<T>();
        string CreateCsvContent<T>(T serializationObject);

        CheckCSVSerializatorResult CheckSerializationTime<T>(T serializationObject)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var serializedString = Serialize(serializationObject);

            stopwatch.Stop();
            var serializationTime = stopwatch.Elapsed.TotalMilliseconds;

            return new CheckCSVSerializatorResult(
                serializedString,
                serializationTime.ToString(),
                this.GetType()
            );
        }

        CheckCSVSerializatorResult CheckDeserializationTime<T>(string deserializationString) where T : new()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var deserializedObject = Deserialize<T>(deserializationString);

            stopwatch.Stop();
            var deserializationTime = stopwatch.Elapsed.TotalMilliseconds;

            return new CheckCSVSerializatorResult(
                deserializedObject,
                deserializationTime.ToString(),
                this.GetType()
            );
        }
    }
}
