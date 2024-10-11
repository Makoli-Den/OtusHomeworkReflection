using System.Diagnostics;

namespace ReflexionSerialization
{
    internal interface ICSVSerializator
    {
        bool NeedExecTimeCheck { get; set; }
        bool NeedFileSave { get; set; }
        string Serialize<T>(T serializationObject);
        string SerializeIEnumerable<T>(IEnumerable<T> collection);
        T Deserialize<T>(string deserializationString) where T : new();
        IEnumerable<T> DeserializeIEnumerable<T>(string deserializationString) where T : new();

        CheckCSVSerializatorResult CheckSerializationTime<T>(T serializationObject)
        {
            if (serializationObject == null)
            {
                return new CheckCSVSerializatorResult();
            }

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

        CheckCSVSerializatorResult CheckIEnumerableSerializationTime<T>(IEnumerable<T> serializationObject)
        {
            if (serializationObject == null)
            {
                return new CheckCSVSerializatorResult();
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var serializedString = SerializeIEnumerable(serializationObject);

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
            if (deserializationString == default)
            {
                return new CheckCSVSerializatorResult();
            }

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

        CheckCSVSerializatorResult CheckIEnumerableDeserializationTime<T>(string deserializationString) where T : new()
        {
            if (deserializationString == default)
            {
                return new CheckCSVSerializatorResult();
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var deserializedObject = DeserializeIEnumerable<T>(deserializationString);

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
