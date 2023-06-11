using UrlShortener.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace UrlShortener.Services;

public class UrlService
{
    private readonly IMongoCollection<Url> _urlCollection;

    public UrlService(
        IOptions<UrlShortenerDatabaseSettings> urlShortenerDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            urlShortenerDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            urlShortenerDatabaseSettings.Value.DatabaseName);

        _urlCollection = mongoDatabase.GetCollection<Url>(
            urlShortenerDatabaseSettings.Value.UrlCollectionName);
    }

    public async Task<List<Url>> GetAsync() =>
        await _urlCollection.Find(_ => true).ToListAsync();

    public async Task<Url?> GetByShortUrlAsync(string shortUrl) =>
        await _urlCollection.Find(x => x.ShortUrl == shortUrl).FirstOrDefaultAsync();
    public async Task<Url?> GetByRealUrlAsync(string realUrl) =>
       await _urlCollection.Find(x => x.RealUrl == realUrl).FirstOrDefaultAsync();
    public async Task<bool> CheckUrlAsync(string url) =>
      await _urlCollection.Find(x => x.RealUrl == url).AnyAsync();
    public async Task CreateAsync(Url url) =>
        await _urlCollection.InsertOneAsync(url);

    public async Task UpdateByRealUrlAsync(string hash, Url url) =>
        await _urlCollection.ReplaceOneAsync(x => x.Hash == hash, url);

}