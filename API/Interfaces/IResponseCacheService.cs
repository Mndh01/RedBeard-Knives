using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
        Task<bool> DeleteCachedResponseAsync();
        string GenerateCacheKeyFromRequest(HttpRequest request);


    }
}