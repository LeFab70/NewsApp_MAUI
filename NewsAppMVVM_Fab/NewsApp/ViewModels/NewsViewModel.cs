using System.Collections.ObjectModel;
using System.ComponentModel;
using NewsApp.Models;
using NewsApp.Services;

namespace NewsApp.ViewModels;

public class NewsViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string p) => PropertyChanged?.Invoke(this, new(p));

    private readonly NewsApiService _service;
    private readonly FavoritesRepository _favorites;

    public NewsViewModel(NewsApiService service, FavoritesRepository favorites)
    {
        _service = service;
        _favorites = favorites;
        _favorites.FavoriteChanged += OnFavoriteChanged;
    }

    private List<Article> _tousLesArticles = new();

    private ObservableCollection<Article> _articles = new();
    public ObservableCollection<Article> Articles
    {
        get => _articles;
        private set
        {
            _articles = value;
            Notify(nameof(Articles));
        }
    }

    private bool _estEnChargement;
    public bool EstEnChargement
    {
        get => _estEnChargement;
        set
        {
            _estEnChargement = value;
            Notify(nameof(EstEnChargement));
        }
    }

    private bool _estFallback;
    public bool EstFallback
    {
        get => _estFallback;
        set
        {
            _estFallback = value;
            Notify(nameof(EstFallback));
        }
    }

    private string _resultatsTexte = "";
    public string ResultatsTexte
    {
        get => _resultatsTexte;
        private set
        {
            _resultatsTexte = value;
            Notify(nameof(ResultatsTexte));
            Notify(nameof(AfficherResultats));
        }
    }

    public bool AfficherResultats => !string.IsNullOrWhiteSpace(ResultatsTexte);

    public async Task ChargerArticles(string categorie = "Tout")
    {
        EstEnChargement = true;

        var (articles, fallback) = await _service.GetArticles(categorie);
        _tousLesArticles = articles ?? new();

        // Si fallback, on filtre localement par "rubrique" (pas d'appel API possible)
        if (fallback && !string.IsNullOrWhiteSpace(categorie) && !categorie.Equals("Tout", StringComparison.OrdinalIgnoreCase))
        {
            var key = categorie.Trim();
            _tousLesArticles = _tousLesArticles
                .Where(a => a.Titre?.Contains(key, StringComparison.OrdinalIgnoreCase) ?? false)
                .ToList();
        }

        foreach (var a in _tousLesArticles)
        {
            a.IsFavorite = !string.IsNullOrWhiteSpace(a.Url) && await _favorites.ExistsAsync(a.Url);
        }

        Articles = new ObservableCollection<Article>(_tousLesArticles);
        EstFallback = fallback;

        ResultatsTexte = "";
        EstEnChargement = false;
    }

    public async Task ToggleFavori(Article article)
    {
        if (string.IsNullOrWhiteSpace(article.Url))
            return;

        var exists = await _favorites.ExistsAsync(article.Url);
        if (exists)
        {
            await _favorites.RemoveAsync(article.Url);
            article.IsFavorite = false;
        }
        else
        {
            await _favorites.AddOrUpdateAsync(article);
            article.IsFavorite = true;
        }

        Articles = new ObservableCollection<Article>(_tousLesArticles);
    }

    private void OnFavoriteChanged(string url, bool isFavorite)
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        var changed = false;

        foreach (var a in _tousLesArticles)
        {
            if (!string.Equals(a.Url, url, StringComparison.OrdinalIgnoreCase))
                continue;

            a.IsFavorite = isFavorite;
            changed = true;
        }

        // refresh UI (Article n'implémente pas INotifyPropertyChanged)
        if (changed)
        {
            Articles = new ObservableCollection<Article>(Articles);
        }
    }

    public void FiltrerLocalement(string recherche)
    {
        if (string.IsNullOrWhiteSpace(recherche))
        {
            Articles = new ObservableCollection<Article>(_tousLesArticles);
            ResultatsTexte = "";
            return;
        }

        var filtrés = _tousLesArticles.Where(a =>
            (a.Titre?.Contains(recherche, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (a.Description?.Contains(recherche, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

        Articles = new ObservableCollection<Article>(filtrés);
        ResultatsTexte = $"{filtrés.Count} résultat(s)";
    }
}

