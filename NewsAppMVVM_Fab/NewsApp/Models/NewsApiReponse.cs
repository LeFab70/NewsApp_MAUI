namespace NewsApp.Models;

public class NewsApiReponse
{
    public string Status { get; set; } = string.Empty;
    public List<Article> Articles { get; set; } = new();
}

