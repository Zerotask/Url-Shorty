namespace UrlShorty.Services
{
    public interface IRedisService
    {
        public bool Set(string key, string value);

        public string Get(string key);
    }
}