namespace ReflexionSerialization
{
    internal class CheckCSVSerializatorResult
    {
        private string _serializationTime;
        private string _deserializationTime;
        private string _serializedString;
        private object _deserializedObject;
        private Type _serializatorType;
        
        public string SerializationTime { get { return _serializationTime; } }

        public string DeserializationTime { get { return _deserializationTime; } }
        public string SerializedString {  get { return _serializedString; } }
        public object DeserializedObject {  get { return _deserializedObject; } }
        public Type SerializatorType {  get { return _serializatorType; } }

        // Конструктор для результатов сериализации
        public CheckCSVSerializatorResult(string serializedString, string serializationTime, Type serializatorType)
        {
            _serializedString = serializedString;
            _serializationTime = serializationTime;
            _serializatorType = serializatorType;
        }

        // Конструктор для результатов десериализации
        public CheckCSVSerializatorResult(object deserializedObject, string deserializationTime, Type serializatorType)
        {
            _deserializedObject = deserializedObject;
            _deserializationTime = deserializationTime;
            _serializatorType = serializatorType;
        }

        public CheckCSVSerializatorResult()
        {
        }
    }
}
