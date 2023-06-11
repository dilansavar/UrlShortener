using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UrlShortener.Models;

public class Url
{
    [BsonId]
    public string? Hash { get; set; }

    [BsonElement("Url")]
    public string RealUrl { get; set; } = null!;
    public string ShortUrl { get; set; } = null!;
}