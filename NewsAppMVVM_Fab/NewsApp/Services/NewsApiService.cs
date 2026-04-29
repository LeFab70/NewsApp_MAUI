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
        try
        {
            _http.DefaultRequestHeaders.Remove("X-Api-Key");
            _http.DefaultRequestHeaders.TryAddWithoutValidation("X-Api-Key", Constants.NewsApiKey);
            _http.DefaultRequestHeaders.Remove("Accept");
            _http.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            _http.DefaultRequestHeaders.UserAgent.ParseAdd("NewsAppMVVM/1.0");
        }
        catch
        {
            // ignore: if headers cannot be set, request will fallback & show error
        }
    }

    public async Task<(List<Article> articles, bool estFallback, string? error)> GetArticles(string categorie = "Tout")
    {
        try
        {
            var country = Preferences.Get("news_country", "us").Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(country))
                country = "us";

            var apiCategory = MapCategorie(categorie);
            var cat = string.IsNullOrWhiteSpace(apiCategory) ? "" : $"&category={apiCategory}";
            // Auth en double (header + query) pour fiabiliser sur Android
            var url =
                $"top-headlines?country={Uri.EscapeDataString(country)}{cat}&pageSize=20&apiKey={Uri.EscapeDataString(Constants.NewsApiKey)}";

            var baseText = (_http.BaseAddress?.ToString() ?? Constants.BaseUrl).Trim();
            if (!baseText.EndsWith("/"))
                baseText += "/";
            var requestUri = new Uri(new Uri(baseText), url);
            Debug.WriteLine($"NewsAPI URL: {requestUri}");

            // On veut voir le code HTTP et le message d'erreur éventuel (401/429/etc.)
            var resp = await _http.GetAsync(requestUri);
            if (!resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadAsStringAsync();
                var details = ExtractNewsApiErrorDetails(body);
                var ct = resp.Content.Headers.ContentType?.ToString() ?? "unknown";
                var len = resp.Content.Headers.ContentLength?.ToString() ?? "unknown";
                var err = $"NewsAPI HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}{details}\n{requestUri}\nct={ct} len={len}";
                Debug.WriteLine($"{err}\n{body}");
                return (ArticleStore.All.ToList(), true, err);
            }

            var rep = await resp.Content.ReadFromJsonAsync<NewsApiReponse>();
            return (rep?.Articles ?? new(), false, null);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"NewsAPI exception: {ex}");
            return (ArticleStore.All.ToList(), true, ex.Message);
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

    private static string ExtractNewsApiErrorDetails(string body)
    {
        try
        {
            var err = System.Text.Json.JsonSerializer.Deserialize<NewsApiError>(body);
            if (err == null)
                return TruncateBody(body);

            var code = string.IsNullOrWhiteSpace(err.Code) ? "" : $" ({err.Code})";
            var msg = string.IsNullOrWhiteSpace(err.Message) ? "" : $" — {err.Message}";
            var details = $"{code}{msg}";
            return string.IsNullOrWhiteSpace(details) ? TruncateBody(body) : details;
        }
        catch
        {
            return TruncateBody(body);
        }
    }

    private static string TruncateBody(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return "";

        var t = body.Replace("\r", " ").Replace("\n", " ").Trim();
        if (t.Length > 140)
            t = t[..140] + "...";

        return $" — {t}";
    }

    private sealed class NewsApiError
    {
        public string Status { get; set; } = "";
        public string Code { get; set; } = "";
        public string Message { get; set; } = "";
    }
}

