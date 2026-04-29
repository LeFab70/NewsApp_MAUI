using System.Net.Http.Json;
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
            var cat = categorie == "Tout" ? "" : $"&category={categorie.ToLower()}";
            var rep = await _http.GetFromJsonAsync<NewsApiReponse>(
                $"top-headlines?country=ca{cat}&pageSize=20&apiKey={Constants.NewsApiKey}");

            return (rep?.Articles ?? new(), false);
        }
        catch
        {
            return (ArticleStore.All.ToList(), true);
        }
    }
}

