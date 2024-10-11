using System.Reflection;
using System.Text;

namespace ReflexionSerialization
{
    internal class CSVReflexionSerializator : ICSVSerializator
    {
        public string Serialize(object serializationObject)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var properties = serializationObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                // Проверяем, имеет ли свойство параметры
                if (prop.GetIndexParameters().Length == 0) // Избегаем индексаторов
                {
                    var value = prop.GetValue(serializationObject);
                    stringBuilder.Append(value?.ToString() ?? string.Empty).Append(",");
                }
            }

            return stringBuilder.ToString().TrimEnd(',');
        }

        public object Deserialize(string deserializationString)
        {
            // Логика для десериализации
            throw new NotImplementedException();
        }

        public CheckCSVSerializatorResult CheckSerializationTime(object serializationObject)
        {
            var startTime = DateTime.Now;
            var serializedString = Serialize(serializationObject);
            var endTime = DateTime.Now;

            return new CheckCSVSerializatorResult(
                serializedString,
                (endTime - startTime).TotalMilliseconds.ToString(),
                this.GetType()
            );
        }

        public CheckCSVSerializatorResult CheckDeserializationTime(string deserializationString)
        {
            var startTime = DateTime.Now;
            var deserializedObject = Deserialize(deserializationString);
            var endTime = DateTime.Now;

            return new CheckCSVSerializatorResult(
                deserializedObject,
                (endTime - startTime).TotalMilliseconds.ToString(),
                this.GetType()
            );
        }
    }
}
