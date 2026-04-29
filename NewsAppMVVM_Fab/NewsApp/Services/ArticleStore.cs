using NewsApp.Models;

namespace NewsApp.Services;

public static class ArticleStore
{
    private static readonly List<Article> _all = new()
    {
        new Article
        {
            Title = "Politique : Actualité à la chambre des communes",
            Description = "Le Parlement adopte une nouvelle loi. (fallback)",
            Url = "local://politique-1",
            UrlToImage = "news1.png",
            Content = "Texte local (fallback) — sans connexion Internet.",
            Source = new() { Name = "Radio Canada" },
            PublishedAt = DateTime.UtcNow.AddHours(-2).ToString("O")
        },
        new Article
        {
            Title = "Sport : Victoire spectaculaire en prolongation",
            Description = "Un match intense se conclut sur un but décisif. (fallback)",
            Url = "local://sport-1",
            UrlToImage = "news2.png",
            Content = "Texte local (fallback) — sans connexion Internet.",
            Source = new() { Name = "RDS" },
            PublishedAt = DateTime.UtcNow.AddHours(-4).ToString("O")
        },
        new Article
        {
            Title = "Cinéma : Un film québécois primé à l’international",
            Description = "La production locale reçoit un prix prestigieux. (fallback)",
            Url = "local://cinema-1",
            UrlToImage = "news3.png",
            Content = "Texte local (fallback) — sans connexion Internet.",
            Source = new() { Name = "Le Devoir" },
            PublishedAt = DateTime.UtcNow.AddHours(-6).ToString("O")
        },
        new Article
        {
            Title = "Technologie : Une nouvelle IA révolutionne la recherche",
            Description = "Un modèle améliore la pertinence des résultats. (fallback)",
            Url = "local://tech-1",
            UrlToImage = "news4.png",
            Content = "Texte local (fallback) — sans connexion Internet.",
            Source = new() { Name = "The Verge" },
            PublishedAt = DateTime.UtcNow.AddHours(-8).ToString("O")
        }
    };

    public static IReadOnlyList<Article> All => _all;
}

