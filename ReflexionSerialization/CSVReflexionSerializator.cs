using System.Reflection;
using System.Text;

namespace ReflexionSerialization
{
    internal class CSVReflexionSerializator : ICSVSerializator
    {
        public string Serialize<T>(T serializationObject)
        {
            var sb = new StringBuilder();

            sb.AppendLine(CreateCsvHeaders<T>());

            sb.AppendLine(CreateCsvContent(serializationObject));

            return sb.ToString();
        }

        public string SerializeIEnumerable<T>(IEnumerable<T> collection)
        {
            var sb = new StringBuilder();

            if (collection != null && collection.Any())
            {
                sb.AppendLine(CreateCsvHeaders<T>());

                foreach (var item in collection)
                {
                    sb.AppendLine(CreateCsvContent(item));
                }
            }

            return sb.ToString();
        }

        public T Deserialize<T>(string deserializationString) where T : new()
        {
            var result = new T();
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var lines = deserializationString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0) return result;

            var headers = lines[0].Split(',');
            var values = lines[1].Split(',');

            for (int i = 0; i < headers.Length && i < values.Length; i++)
            {
                var header = headers[i].Trim();
                var value = values[i].Trim();

                var field = fields.FirstOrDefault(f => f.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
                if (field != null && !string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(value, field.FieldType);
                        field.SetValue(result, convertedValue);
                    }
                    catch (Exception)
                    {

                    }
                }

                var property = properties.FirstOrDefault(p => p.Name.Equals(header, StringComparison.OrdinalIgnoreCase));
                if (property != null && property.CanWrite && !string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(result, convertedValue);
                    }
                    catch (Exception)
                    {

                    }
                }
            }

            return result;
        }


        public IEnumerable<T> DeserializeIEnumerable<T>(string deserializationString) where T : new()
        {
            var resultList = new List<T>();
            var lines = deserializationString.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length == 0) return resultList;

            var headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var result = new T();

                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    var header = headers[j].Trim();
                    var value = values[j].Trim();

                    var field = typeof(T).GetField(header, BindingFlags.Public | BindingFlags.Instance);
                    if (field != null && !string.IsNullOrWhiteSpace(value))
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(value, field.FieldType);
                            field.SetValue(result, convertedValue);
                        }
                        catch (Exception)
                        {

                        }
                    }

                    var property = typeof(T).GetProperty(header, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null && property.CanWrite && !string.IsNullOrWhiteSpace(value))
                    {
                        try
                        {
                            var convertedValue = Convert.ChangeType(value, property.PropertyType);
                            property.SetValue(result, convertedValue);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }

                resultList.Add(result);
            }

            return resultList;
        }


        private string CreateCsvHeaders<T>()
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var fieldNames = fields.Select(f => f.Name);
            var propertyNames = properties.Select(p => p.Name);

            return string.Join(",", fieldNames.Concat(propertyNames));
        }

        private string CreateCsvContent<T>(T serializationObject)
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var fieldValues = fields.Select(f => f.GetValue(serializationObject)?.ToString() ?? string.Empty);
            var propertyValues = properties.Select(p => p.GetValue(serializationObject)?.ToString() ?? string.Empty);

            return string.Join(",", fieldValues.Concat(propertyValues));
        }
    }
}
