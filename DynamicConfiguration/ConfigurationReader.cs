using System;
using System.Collections.Generic;
using System.Linq;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Repository;

namespace DynamicConfiguration
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly string _appName;
        private int _refreshIntv;

        private IConfigurationRepository repo;

        private List<Configuration> configList = new List<Configuration>();

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _appName = applicationName;
            _refreshIntv = refreshTimerIntervalInMs;

            repo = new ConfigurationRepository(connectionString);

            //connect to db get the initial configs.
            GetLatestConfigForApplication();

            //thread açacak. refreshTimerIntervalInMs sanyiede bir git tekrar al.
        }

        private void GetLatestConfigForApplication()
        {
            var result = repo.Repository.All().Where(x => x.ApplicationName == _appName && x.IsActive);

            configList = result.ToList();
        }

        public T GetValue<T>(string key)
        {
            var config = configList.Find(x => x.Name == key);
            if (config == null)
                throw new Exception($"Configuration for key:{key}/app:{_appName} doesn't exist");

            if (!config.CheckType<T>())
            {
                throw new Exception($"Config type mismatch requested type:{typeof(T)}, config type:{config.Type}");
            }

            return config.GetValue<T>();
        }
    }
}