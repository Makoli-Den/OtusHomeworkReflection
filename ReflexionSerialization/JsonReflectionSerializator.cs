using System.Reflection;
using System.Text;

namespace ReflectionSerialization.Serializators
{
    public class JsonReflectionSerializator : ISerializator
    {
        public string SerializatorType => "Json";

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
            var json = new StringBuilder("[");

            foreach (var item in collection)
            {
                json.Append(Serialize(item));
                json.Append(",");
            }

            if (json.Length > 1)
                json.Length--;

            json.Append("]");

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

            var objects = deserializationString.Trim('[', ']').Split(new[] { "},{" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var objString in objects)
            {
                var obj = Deserialize<T>("{" + objString.Trim('}', '{') + "}");
                list.Add(obj);
            }

            return list;
        }
    }
}
