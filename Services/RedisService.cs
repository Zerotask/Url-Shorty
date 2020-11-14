using System;
using StackExchange.Redis;

namespace UrlShorty.Services
{
    public class RedisService : IRedisService
    {
        private IDatabase _db;

        public RedisService()
        {
            var options = new ConfigurationOptions
            {
                EndPoints =
                {
                    {
                        Environment.GetEnvironmentVariable("REDIS_HOST"),
                        Convert.ToInt32(Environment.GetEnvironmentVariable("REDIS_PORT"))
                    }
                },
                User = Environment.GetEnvironmentVariable("REDIS_USER"),
                Password = Environment.GetEnvironmentVariable("REDIS_PASSWORD")
            };

            var connection = ConnectionMultiplexer.Connect(options);
            _db = connection.GetDatabase();
        }

        public bool Set(string key, string value)
        {
            return _db.StringSet(key, value);
        }

        public string Get(string key)
        {
            return _db.StringGet(key);
        }
    }
}