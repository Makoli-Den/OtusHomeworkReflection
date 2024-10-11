using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ReflectionSerialization.Serializators
{
    internal class CSVNewtonsoftSerializator : ISerializator
    {
        public string SerializatorType => "CSV";

        public string Serialize<T>(T serializationObject)
        {
            var jsonArray = new JArray();
            jsonArray.Add(JObject.FromObject(serializationObject));
            return ConvertJsonToCsv(jsonArray);
        }

        public string SerializeIEnumerable<T>(IEnumerable<T> collection)
        {
            var jsonArray = JArray.FromObject(collection);
            return ConvertJsonToCsv(jsonArray);
        }

        public T Deserialize<T>(string deserializationString) where T : new()
        {
            var json = ConvertCsvToJson(deserializationString);

            var jsonArray = JArray.Parse(json);

            if (jsonArray.Count > 0)
            {
                return jsonArray[0].ToObject<T>();
            }

            return new T();
        }

        public IEnumerable<T> DeserializeIEnumerable<T>(string deserializationString) where T : new()
        {
            var json = ConvertCsvToJson(deserializationString);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        private string ConvertJsonToCsv(JArray jsonArray)
        {
            if (jsonArray == null || jsonArray.Count == 0)
                return string.Empty;

            var headers = string.Join(",", jsonArray[0].Children<JProperty>().Select(p => p.Name));
            var rows = jsonArray.Select(item => string.Join(",", item.Children<JProperty>().Select(prop => prop.Value?.ToString() ?? string.Empty)));

            return $"{headers}\n{string.Join("\n", rows)}";
        }

        private string ConvertCsvToJson(string csv)
        {
            var lines = csv.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
                return "[]";

            var headers = lines[0].Split(',');
            var jsonList = new List<Dictionary<string, object>>();

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var dict = new Dictionary<string, object>();

                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    var header = headers[j].Trim();
                    var value = values[j].Trim();
                    dict[header] = value;
                }

                jsonList.Add(dict);
            }

            return JsonConvert.SerializeObject(jsonList);
        }
    }
}
