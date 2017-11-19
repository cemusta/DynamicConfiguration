using DynamicConfiguration.Data.Model;

namespace DynamicConfiguration.Data.Repository
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public IRepository<Configuration> Repository { get; set; }

        public ConfigurationRepository(IRepository<Configuration> repository)
        {
            Repository = repository;
        }

        public ConfigurationRepository(string connectionString)
        {
            var repository = new Repository<Configuration>(connectionString);
            Repository = repository;
        }
    }
}
