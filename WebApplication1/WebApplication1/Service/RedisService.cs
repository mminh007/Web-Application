using StackExchange.Redis;


namespace WebApplication1.Service
{
 
    public class RedisService
    {
        private readonly IDatabase _db;
        public RedisService(IConfiguration config)
        {
            var redis = ConnectionMultiplexer.Connect($"{config["Redis:Host"]}:{config["Redis:Port"]}");
            _db = redis.GetDatabase();
        }

        public async Task<string?> GetAsync(string key) => await _db.StringGetAsync(key);

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null) =>
            await _db.StringSetAsync(key, value, expiry ?? TimeSpan.FromMinutes(30));
    }

}
