﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Repository;

namespace DynamicConfiguration
{
    public class ConfigurationReader : IConfigurationReader
    {
        private readonly string _appName;

        private readonly IConfigurationRepository _repo;
        private readonly CancellationTokenSource _tokenSource;

        private List<Configuration> _configList = new List<Configuration>();

        public ConfigurationReader(string applicationName, string connectionString, int refreshTimerIntervalInMs)
        {
            _tokenSource = new CancellationTokenSource();
            CancellationToken ct = _tokenSource.Token;

            _appName = applicationName;

            _repo = new ConfigurationRepository(connectionString);

            //connect to db get the initial configs.
            GetLatestConfigForApplication();

            //thread açacak. refreshTimerIntervalInMs sanyiede bir git tekrar al.
            Task.Run(() => RefreshConfig(refreshTimerIntervalInMs), ct);
        }

        ~ConfigurationReader()
        {
            _tokenSource.Cancel();
        }
        
        private Task RefreshConfig(int interval)
        {
            if (interval < 5000)
                interval = 5000; //min 5 saniye interval

            do
            {
                Thread.Sleep(interval);
                GetLatestConfigForApplication();

            } while (!_tokenSource.IsCancellationRequested);

            return null;
        }


        private void GetLatestConfigForApplication()
        {
            var result = _repo.Repository.All().Where(x => x.ApplicationName == _appName && x.IsActive);

            _configList = result.ToList();
        }

        public T GetValue<T>(string key)
        {
            var config = _configList.Find(x => x.Name == key);
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