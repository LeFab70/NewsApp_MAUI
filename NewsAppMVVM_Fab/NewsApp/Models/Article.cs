namespace NewsApp.Models;

public class Article
{
    // Champs NewsAPI
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string UrlToImage { get; set; } = string.Empty;
    public string PublishedAt { get; set; } = string.Empty;
    public ArticleSource Source { get; set; } = new();

    // Compatibilité avec les bindings du Devoir 03
    public string Titre => Title;
    public string Image => UrlToImage;
    public string SourceName => Source?.Name ?? "Inconnu";
    public string Date => DateTime.TryParse(PublishedAt, out var d) ? d.ToString("d MMM") : string.Empty;
    public string SourceEtDate => $"{SourceName} / {Date}";
}

public class ArticleSource
{
    public string Name { get; set; } = string.Empty;
}
