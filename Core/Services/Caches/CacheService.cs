using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Caches
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(ICacheService redis) {
            _database=redis.GetDatabase;
        }
        public async Task<string> GetCacheKeyAsync(string Key){
        var cacheResoinse =await _database.StringGetAsync(Key);
            if (cacheResoinse.IsNullOrEmpty)return null ;
            return cacheResoinse.ToString() ;
        }
        public async Task SetCacheKeyAsync(string Key, object response, TimeSpan expireTime) 
        {
            if (response is null) return;

            var option =new JsonSerializerOption() {PropertyNamePolicy=JsonNamingPolicy.CamelCase};
            await _database.StringSetAsync(Key, JsonSerializer.Serialize(response,option), expireTime);
        }
    }
}
