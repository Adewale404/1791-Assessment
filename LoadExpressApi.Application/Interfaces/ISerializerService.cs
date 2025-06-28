namespace LoadExpressApi.Application.Interfaces;

public interface ISerializerService : IScopedService
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}