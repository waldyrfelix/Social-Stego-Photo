namespace StegoCore
{
    public interface ISerializationService
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string jsonString);
    }
}