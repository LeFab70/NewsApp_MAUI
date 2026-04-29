using System.Collections.ObjectModel;
using System.ComponentModel;
using NewsApp.Models;
using NewsApp.Services;

namespace NewsApp.ViewModels;

public class FavoritesViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    private void Notify(string p) => PropertyChanged?.Invoke(this, new(p));

    private readonly FavoritesRepository _repo;

    public FavoritesViewModel(FavoritesRepository repo)
    {
        _repo = repo;
    }

    private ObservableCollection<FavoriteArticle> _favoris = new();
    public ObservableCollection<FavoriteArticle> Favoris
    {
        get => _favoris;
        private set
        {
            _favoris = value;
            Notify(nameof(Favoris));
            Notify(nameof(CountText));
            Notify(nameof(HasItems));
        }
    }

    public string CountText => $"{Favoris.Count} favori(s)";
    public bool HasItems => Favoris.Count > 0;

    public async Task ChargerAsync()
    {
        var list = await _repo.GetAllAsync();
        Favoris = new ObservableCollection<FavoriteArticle>(list);
    }

    public async Task SupprimerAsync(FavoriteArticle fav)
    {
        await _repo.RemoveAsync(fav.Url);
        await ChargerAsync();
    }
}

