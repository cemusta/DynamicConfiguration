using DynamicConfiguration.Data.Model;

namespace DynamicConfiguration.Data.Repository
{
    public interface IConfigurationRepository
    {
        IRepository<Configuration> Repository { get; }
    }
}
