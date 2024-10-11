namespace ReflexionSerialization
{
    internal interface ICSVSerializator
    {
        string Serialize(object serializationObject);
        object Deserialize(string deserializationString);
        CheckCSVSerializatorResult CheckSerializationTime(object serializationObject);
        CheckCSVSerializatorResult CheckDeserializationTime(string deserializationString);
    }
}
