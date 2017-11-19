using System;
using DynamicConfiguration.Data.Model;
using DynamicConfiguration.Data.Util;
using MongoDB.Driver;

namespace DynamicConfiguration.Data.Helper
{
    public static class DatabaseHelper
    {
        public static IMongoDatabase GetDatabaseFromConnectionString(string connectionstring)
        {
            var client = new MongoClient(connectionstring);
            var cnn = new MongoUrl(connectionstring);
            var database = client.GetDatabase(cnn.DatabaseName);

            return database;
        }

        public static IMongoCollection<T> GetCollectionFromConnectionString<T>(string connectionstring) where T : class
        {
            var collectionName = typeof(T).BaseType == typeof(object) ? GetCollectioNameFromInterface<T>() : GetCollectionNameFromType(typeof(T));

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }

            return GetDatabaseFromConnectionString(connectionstring).GetCollection<T>(collectionName);
        }

        private static string GetCollectioNameFromInterface<T>()
        {
            string collectionname;

            var att = Attribute.GetCustomAttribute(typeof(T), typeof(CollectionName));

            if (att != null)
            {
                collectionname = ((CollectionName)att).Name;
            }
            else
            {
                collectionname = typeof(T).Name;
            }

            return collectionname;
        }

        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            var att = Attribute.GetCustomAttribute(entitytype, typeof(CollectionName));

            if (att != null)
            {
                collectionname = ((CollectionName)att).Name;
            }
            else
            {

                entitytype = entitytype.BaseType;

                return entitytype?.Name;
            }

            return collectionname;
        }

        public static void CreateDatabase(string connectionStr)
        {
            IMongoDatabase db = GetDatabaseFromConnectionString(connectionStr);

            IMongoCollection<Configuration> configs = db.GetCollection<Configuration>("configuration");

            if (configs.Count(x => true) != 0) //Eğer collection boş ise db'yi boş sayıp indexleri yaratıyoruz.
                return;
            
            configs.Indexes.CreateOneAsync(Builders<Configuration>.IndexKeys.Ascending(nameof(Configuration.Name)).Ascending(nameof(Configuration.ApplicationName)));
            configs.Indexes.CreateOneAsync(Builders<Configuration>.IndexKeys.Ascending(nameof(Configuration.ApplicationName)));
            
            var setting1 = new Configuration
            {
                ApplicationName = "LiveTest",
                Name = "SupportedOs",
                Type = ConfigType.String,
                Value = "Windows | Linux",
                IsActive = true
            };
            var setting2 = new Configuration
            {
                ApplicationName = "Live",
                Name = "SupportedOs",
                Type = ConfigType.String,
                Value = "Windows | Linux | Mac",
                IsActive = true
            };
            var setting3 = new Configuration
            {
                ApplicationName = "LiveTest",
                Name = "ApiVersion",
                Type = ConfigType.Double,
                Value = "1.44",
                IsActive = true
            };
            var setting4 = new Configuration
            {
                ApplicationName = "LiveTest",
                Name = "IsDebug",
                Type = ConfigType.Bool,
                Value = "True",
                IsActive = true
            };
            var setting5 = new Configuration
            {
                ApplicationName = "LiveTest",
                Name = "Miliseconds",
                Type = ConfigType.Int,
                Value = "12345",
                IsActive = true
            };

            configs.InsertOne(setting1);
            configs.InsertOne(setting2);
            configs.InsertOne(setting3);
            configs.InsertOne(setting4);
            configs.InsertOne(setting5);

        }
    }
}
