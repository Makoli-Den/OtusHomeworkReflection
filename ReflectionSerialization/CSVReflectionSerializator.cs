using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ReflectionSerialization.Serializators
{
    internal class CSVReflectionSerializator : ISerializator
    {
        public string AverageSerializationTime { get; set; }
        public string AverageDeserializationTime { get; set; }
        public string SerializatorType => "csv";

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
            List<double> average = new List<double>();

            if (collection != null && collection.Any())
            {
                sb.AppendLine(CreateCsvHeaders<T>());

                foreach (var item in collection)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();

                    sb.AppendLine(CreateCsvContent(item));

                    stopwatch.Stop();
                    var serializationTime = stopwatch.Elapsed.TotalMilliseconds;
                    average.Add(serializationTime);
                }
            }

            AverageSerializationTime = (average.Count > 0 ? average.Sum() / average.Count : 0).ToString();

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

            List<double> average = new List<double>();

            for (int i = 1; i < lines.Length; i++)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

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

                stopwatch.Stop();
                var serializationTime = stopwatch.Elapsed.TotalMilliseconds;
                average.Add(serializationTime);
            }

            AverageDeserializationTime = (average.Count > 0 ? average.Sum() / average.Count : 0).ToString();

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
