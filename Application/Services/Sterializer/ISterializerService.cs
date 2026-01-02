namespace Application.Services.Sterializer
{
    public interface ISterializerService
    {
        T DeserializeObject<T>(string obj);
        string SerializeObject(object obj);
    }
}