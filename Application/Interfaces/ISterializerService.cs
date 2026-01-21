namespace FinanceHelper.Application.Interfaces
{
    public interface ISterializerService
    {
        T DeserializeObject<T>(string obj);
        string SerializeObject(object obj);
    }
}