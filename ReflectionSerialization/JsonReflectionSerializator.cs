using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ReflectionSerialization.Serializators
{
    public class JsonReflectionSerializator : ISerializator
    {
        public string AverageSerializationTime { get; set; }
        public string AverageDeserializationTime { get; set; }
        public string SerializatorType => "json";

        public string Serialize<T>(T serializationObject)
        {
            var json = new StringBuilder("{");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = property.GetValue(serializationObject);
                json.AppendFormat("\"{0}\": \"{1}\",", property.Name, value);
            }

            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var value = field.GetValue(serializationObject);
                json.AppendFormat("\"{0}\": \"{1}\",", field.Name, value);
            }

            if (json.Length > 1)
                json.Length--;

            json.Append("}");

            return json.ToString();
        }

        public string SerializeIEnumerable<T>(IEnumerable<T> collection)
        {
            List<double> average = new List<double>();
            var json = new StringBuilder("[");

            foreach (var item in collection)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                json.Append(Serialize(item));
                json.Append(",");

                stopwatch.Stop();
                var serializationTime = stopwatch.Elapsed.TotalMilliseconds;
                average.Add(serializationTime);
            }

            if (json.Length > 1)
                json.Length--;

            json.Append("]");

            AverageSerializationTime = (average.Count > 0 ? average.Sum() / average.Count : 0).ToString();

            return json.ToString();
        }

        public T Deserialize<T>(string deserializationString) where T : new()
        {
            T obj = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

            var keyValuePairs = deserializationString.Trim('{', '}').Split(',');

            foreach (var pair in keyValuePairs)
            {
                var keyValue = pair.Split(':');
                var key = keyValue[0].Trim(' ', '\"');
                var value = keyValue[1].Trim(' ', '\"');

                foreach (var property in properties)
                {
                    if (property.Name == key)
                    {
                        property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
                    }
                }

                foreach (var field in fields)
                {
                    if (field.Name == key)
                    {
                        field.SetValue(obj, Convert.ChangeType(value, field.FieldType));
                    }
                }
            }

            return obj;
        }

        public IEnumerable<T> DeserializeIEnumerable<T>(string deserializationString) where T : new()
        {
            var list = new List<T>();
            List<double> average = new List<double>();

            var objects = deserializationString.Trim('[', ']').Split(new[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var objString in objects)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var obj = Deserialize<T>("{" + objString.Trim('}', '{') + "}");
                list.Add(obj);

                stopwatch.Stop();
                var serializationTime = stopwatch.Elapsed.TotalMilliseconds;
                average.Add(serializationTime);
            }

            AverageDeserializationTime = (average.Count > 0 ? average.Sum() / average.Count : 0).ToString();

            return list;
        }
    }
}
