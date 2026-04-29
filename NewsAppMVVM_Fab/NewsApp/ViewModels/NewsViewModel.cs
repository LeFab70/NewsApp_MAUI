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

    public NewsViewModel(NewsApiService service)
    {
        _service = service;
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
        Articles = new ObservableCollection<Article>(_tousLesArticles);
        EstFallback = fallback;

        ResultatsTexte = "";
        EstEnChargement = false;
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

