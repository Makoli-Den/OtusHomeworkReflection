using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReflexionSerialization
{
    internal class CSVNewtonsoftSerializator : ICSVSerializator
    {
        public string Serialize<T>(T serializationObject)
        {
            if (serializationObject is IEnumerable enumerable)
            {
                Type itemType = typeof(T).GetGenericArguments()[0];
                var headersMethod = typeof(CSVNewtonsoftSerializator).GetMethod("CreateCsvHeaders");
                var headers = headersMethod.MakeGenericMethod(itemType).Invoke(this, new object[] { });
                var csvContent = new List<string>();
                foreach (var item in enumerable)
                {
                    csvContent.Add(CreateCsvContent(item));
                }
                return $"{headers}{Environment.NewLine}{string.Join(Environment.NewLine, csvContent)}";
            }

            return $"{CreateCsvHeaders<T>()}{Environment.NewLine}{CreateCsvContent(serializationObject)}";
        }

        public T Deserialize<T>(string deserializationString) where T : new()
        {
            var lines = deserializationString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length < 2) return new T();

            var headers = lines[0].Split(',');
            var numberOfHeaders = headers.Length;

            if (typeof(IEnumerable).IsAssignableFrom(typeof(T)))
            {
                Type elementType = typeof(T).GetGenericArguments()[0];
                var results = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                for (int i = 1; i < lines.Length; i++)
                {
                    var obj = Activator.CreateInstance(elementType);
                    var values = lines[i].Split(',');

                    for (int j = 0; j < numberOfHeaders; j++)
                    {
                        if (j < values.Length)
                        {
                            var prop = elementType.GetProperty(headers[j].Trim());
                            if (prop != null)
                            {
                                var value = Convert.ChangeType(values[j].Trim(), prop.PropertyType);
                                prop.SetValue(obj, value);
                            }

                            var field = elementType.GetField(headers[j].Trim());
                            if (field != null)
                            {
                                var value = Convert.ChangeType(values[j].Trim(), field.FieldType);
                                field.SetValue(obj, value);
                            }
                        }
                    }

                    results.Add(obj);
                }

                return (T)results;
            }
            else 
            {
                var obj = new T();
                var values = lines[1].Split(',');

                for (int i = 0; i < numberOfHeaders; i++)
                {
                    var prop = typeof(T).GetProperty(headers[i].Trim());
                    if (prop != null && i < values.Length)
                    {
                        var value = Convert.ChangeType(values[i].Trim(), prop.PropertyType);
                        prop.SetValue(obj, value);
                    }

                    var field = typeof(T).GetField(headers[i].Trim());
                    if (field != null && i < values.Length)
                    {
                        var value = Convert.ChangeType(values[i].Trim(), field.FieldType);
                        field.SetValue(obj, value);
                    }
                }

                return obj;
            }
        }

        public string CreateCsvHeaders<T>()
        {
            var json = JsonConvert.SerializeObject(Activator.CreateInstance<T>());
            var jObject = JObject.Parse(json);
            var headers = string.Join(",", jObject.Properties().Select(p => p.Name));
            return headers;
        }

        public string CreateCsvContent<T>(T serializationObject)
        {
            var json = JsonConvert.SerializeObject(serializationObject);
            var jObject = JObject.Parse(json);
            var values = string.Join(",", jObject.Properties().Select(p => p.Value.ToString()));
            return values;
        }
    }
}
