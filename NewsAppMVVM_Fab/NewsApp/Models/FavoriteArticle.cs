using SQLite;

namespace NewsApp.Models;

public class FavoriteArticle
{
    [PrimaryKey]
    public string Url { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UrlToImage { get; set; } = string.Empty;
    public string SourceName { get; set; } = string.Empty;
    public string PublishedAt { get; set; } = string.Empty;
}

