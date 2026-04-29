using System.Net.Http.Json;
using System.Diagnostics;
using NewsApp.Models;

namespace NewsApp.Services;

public class NewsApiService
{
    private readonly HttpClient _http;

    public NewsApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(List<Article> articles, bool estFallback)> GetArticles(string categorie = "Tout")
    {
        try
        {
            var apiCategory = MapCategorie(categorie);
            var cat = string.IsNullOrWhiteSpace(apiCategory) ? "" : $"&category={apiCategory}";
            var url = $"top-headlines?country=ca{cat}&pageSize=20&apiKey={Constants.NewsApiKey}";

            // On veut voir le code HTTP et le message d'erreur éventuel (401/429/etc.)
            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                Debug.WriteLine($"NewsAPI HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");
                return (ArticleStore.All.ToList(), true);
            }

            var rep = await resp.Content.ReadFromJsonAsync<NewsApiReponse>();
            return (rep?.Articles ?? new(), false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"NewsAPI exception: {ex}");
            return (ArticleStore.All.ToList(), true);
        }
    }

    private static string MapCategorie(string categorie)
    {
        var c = (categorie ?? "").Trim().ToLowerInvariant();
        return c switch
        {
            "" => "",
            "tout" => "",
            "sport" => "sports",
            "sports" => "sports",
            "technologie" => "technology",
            "technology" => "technology",
            "cinéma" => "entertainment",
            "cinema" => "entertainment",
            "entertainment" => "entertainment",
            // NewsAPI ne propose pas "politics" en category; "general" est le plus proche
            "politique" => "general",
            _ => ""
        };
    }
}

