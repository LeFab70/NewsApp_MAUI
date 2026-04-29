namespace NewsApp;

public static class Constants
{
    public const string BaseUrl = "https://newsapi.org/v2/";

    // IMPORTANT: la clé ne doit pas être commitée. Mets-la dans Constants.Local.cs
    // (fichier ignoré par git) qui expose une constante NewsApiKey.
    public static string NewsApiKey => ConstantsLocal.NewsApiKey;
}

