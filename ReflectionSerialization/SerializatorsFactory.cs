using System.Reflection;

namespace ReflectionSerialization.Serializators
{
    internal class SerializatorsFactory
    {
        public static List<ISerializator> GetAllSerializators()
        {
            var serializatorTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ISerializator).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .ToList();

            var serializators = new List<ISerializator>();
            foreach (var type in serializatorTypes)
            {
                var instance = Activator.CreateInstance(type) as ISerializator;
                if (instance != null)
                {
                    serializators.Add(instance);
                }
            }

            return serializators;
        }

        public static List<ISerializator> GetSerializatorsByType(string typeName)
        {
            var serializators = GetAllSerializators();

            var selectedSerializators = serializators.Where(s => s.SerializatorType == typeName);

            if (selectedSerializators == null || !selectedSerializators.Any())
            {
                throw new Exception($"Сериализаторы с типом '{typeName}' не найден.");
            }

            return selectedSerializators.ToList();
        }
    }
}
