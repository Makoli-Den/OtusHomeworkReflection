namespace ReflexionSerialization
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            var obj = F.Get();

            // Сериализация и десериализация с помощью рефлексии
            ICSVSerializator reflectionSerializator = new CSVReflexionSerializator();
            var reflectionResult = reflectionSerializator.CheckSerializationTime(obj);
            Console.WriteLine($"Reflection serialization time: {reflectionResult.SerializationTime} ms");
            Console.WriteLine($"Reflection serialized string: {reflectionResult.SerializedString}");
            var reflectionDeserializationResult = reflectionSerializator.CheckSerializationTime(reflectionResult.SerializedString);
            Console.WriteLine($"Reflection deserialization time: {reflectionDeserializationResult.DeserializationTime} ms");

            // Сериализация и десериализация с помощью Newtonsoft.Json
            ICSVSerializator jsonSerializator = new CSVNewtonsoftSerializator();
            var jsonResult = jsonSerializator.CheckSerializationTime(obj);
            Console.WriteLine($"JSON serialization time: {jsonResult.SerializationTime} ms");
            Console.WriteLine($"JSON serialized string: {jsonResult.SerializedString}");
            var jsonDeserializationResult = jsonSerializator.CheckSerializationTime(jsonResult.SerializedString);
            Console.WriteLine($"JSON deserialization time: {jsonDeserializationResult.DeserializationTime} ms");

            Console.ReadKey();
        }
    }
}
