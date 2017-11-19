namespace DynamicConfiguration
{
    public interface IConfigurationReader
    {
        T GetValue<T>(string key);
    }
}