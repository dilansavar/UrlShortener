namespace UrlShortener.Models;

public class UrlShortenerDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string UrlCollectionName { get; set; } = null!;
}