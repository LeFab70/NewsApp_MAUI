using NewsApp.Models;
using SQLite;

namespace NewsApp.Services;

public class FavoritesRepository
{
    public event Action<string, bool>? FavoriteChanged;

    private SQLiteAsyncConnection? _db;

    private async Task<SQLiteAsyncConnection> GetDb()
    {
        if (_db != null) return _db;

        var path = Path.Combine(FileSystem.AppDataDirectory, "newsapp_favorites.db3");
        _db = new SQLiteAsyncConnection(path);
        await _db.CreateTableAsync<FavoriteArticle>();
        return _db;
    }

    public async Task<List<FavoriteArticle>> GetAllAsync()
    {
        var db = await GetDb();
        return await db.Table<FavoriteArticle>().OrderByDescending(x => x.PublishedAt).ToListAsync();
    }

    public async Task<bool> ExistsAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;
        var db = await GetDb();
        var item = await db.FindAsync<FavoriteArticle>(url);
        return item != null;
    }

    public async Task AddOrUpdateAsync(Article article)
    {
        if (string.IsNullOrWhiteSpace(article.Url)) return;
        var db = await GetDb();

        var fav = new FavoriteArticle
        {
            Url = article.Url,
            Title = article.Title,
            Description = article.Description,
            UrlToImage = article.UrlToImage,
            SourceName = article.SourceName,
            PublishedAt = article.PublishedAt
        };

        await db.InsertOrReplaceAsync(fav);
        FavoriteChanged?.Invoke(article.Url, true);
    }

    public async Task RemoveAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;
        var db = await GetDb();
        await db.DeleteAsync<FavoriteArticle>(url);
        FavoriteChanged?.Invoke(url, false);
    }
}

