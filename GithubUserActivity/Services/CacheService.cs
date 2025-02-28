using GithubUserActivity.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;

namespace GithubUserActivity.Services;

public interface ICacheService
{
    Task<List<GithubEvent>> GetCachedOrFetchEvents(string username);
    Task<GithubUser> GetCachedOrFetchUserInfo(string username);
}

public class CacheService : ICacheService
{
    private readonly MemoryCache _memoryCache;
    private readonly HttpClient _client;
    public CacheService()
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.UserAgent.ParseAdd("github-activity-cli");

        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public async Task<List<GithubEvent>> GetCachedOrFetchEvents(string username)
    {
        if (_memoryCache.TryGetValue(username, out List<GithubEvent> cachedEvents))
        {
            return cachedEvents;
        }

        var events = await _client.GetFromJsonAsync<List<GithubEvent>>($"https://api.github.com/users/{username}/events");

        if (events.Count > 0)
        {
            _memoryCache.Set(username, events, TimeSpan.FromMinutes(10));
        }

        return events;
    }

    public async Task<GithubUser> GetCachedOrFetchUserInfo(string username)
    {
        return await _client.GetFromJsonAsync<GithubUser>($"https://api.github.com/users/{username}"); ;
    }
}