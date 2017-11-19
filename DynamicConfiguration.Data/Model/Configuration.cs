using System;
using DynamicConfiguration.Data.Util;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DynamicConfiguration.Data.Model
{
    [CollectionName("configuration")]
    public class Configuration
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        public string Name { get; set; }
        public ConfigType Type { get; set; }
        public String Value { get; set; }
        public bool IsActive { get; set; }
        public string ApplicationName { get; set; }

        public bool CheckType<T>()
        {
            switch (Type)
            {
                case ConfigType.String:
                    return typeof(T) == typeof(string);
                case ConfigType.Bool:
                    return typeof(T) == typeof(bool);
                case ConfigType.Double:
                    return typeof(T) == typeof(double);
                case ConfigType.Int:
                    return typeof(T) == typeof(int);
                default:
                    throw new Exception($"Unsupported config type:{Type}");
            }
        }

        public T GetValue<T>()
        {
            switch (Type)
            {
                case ConfigType.String:
                    return (T)Convert.ChangeType(Value, typeof(T));
                case ConfigType.Bool:
                    return (T)Convert.ChangeType(bool.Parse(Value), typeof(T));
                case ConfigType.Double:
                    return (T)Convert.ChangeType(double.Parse(Value), typeof(T));
                case ConfigType.Int:
                    return (T)Convert.ChangeType(int.Parse(Value), typeof(T));
                default:
                    throw new Exception($"Unsupported config type:{Type}");
            }
        }
    }
}