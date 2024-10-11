namespace ReflectionSerialization
{
    internal class CheckCSVSerializatorResult
    {
        private string _serializationTime;
        private string _deserializationTime;
        private string _averageSerializationTime;
        private string _averageDeserializationTime;
        private string _serializedString;
        private object _deserializedObject;
        private Type _serializatorType;
        
        public string SerializationTime { get { return _serializationTime; } }
        public string DeserializationTime { get { return _deserializationTime; } }
        public string AverageSerializationTime { get { return _averageSerializationTime; } }

        public string AverageDeserializationTime { get { return _averageDeserializationTime; } }
        public string SerializedString {  get { return _serializedString; } }
        public object DeserializedObject {  get { return _deserializedObject; } }
        public Type SerializatorType {  get { return _serializatorType; } }

        public CheckCSVSerializatorResult(string serializedString, string serializationTime, Type serializatorType)
        {
            _serializedString = serializedString;
            _serializationTime = serializationTime;
            _serializatorType = serializatorType;
        }

        public CheckCSVSerializatorResult(object deserializedObject, string deserializationTime, Type serializatorType)
        {
            _deserializedObject = deserializedObject;
            _deserializationTime = deserializationTime;
            _serializatorType = serializatorType;
        }

        public CheckCSVSerializatorResult(string serializedString, string serializationTime, string averageSerializationTime, Type serializatorType)
        {
            _serializedString = serializedString;
            _serializationTime = serializationTime;
            _averageSerializationTime = averageSerializationTime;
            _serializatorType = serializatorType;
        }

        public CheckCSVSerializatorResult(object deserializedObject, string deserializationTime, string averageDeserializationTime, Type serializatorType)
        {
            _deserializedObject = deserializedObject;
            _deserializationTime = deserializationTime;
            _averageDeserializationTime = averageDeserializationTime;
            _serializatorType = serializatorType;
        }

        public CheckCSVSerializatorResult()
        {
        }
    }
}
