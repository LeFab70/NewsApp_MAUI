using NewsApp.Models;
using NewsApp.Services;
using NewsApp.ViewModels;

namespace NewsApp.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesViewModel _vm;
    private bool _suppressNextOpenDetails;

    // Shell peut nécessiter un constructeur sans paramètre
    public FavoritesPage()
    {
        InitializeComponent();

        FavoritesViewModel vm;
        try
        {
            vm = AppServices.GetRequired<FavoritesViewModel>();
        }
        catch
        {
            // Fallback ultra-robuste: pas de DI -> instanciation manuelle
            vm = new FavoritesViewModel(new FavoritesRepository());
        }

        _vm = vm;
        BindingContext = _vm;
    }

    public FavoritesPage(FavoritesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = _vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetActiveTab("favorites");
        _ = _vm.ChargerAsync();
    }

    private async void OnDeleteInvoked(object? sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.CommandParameter is FavoriteArticle fav)
            await _vm.SupprimerAsync(fav);
    }

    private async void OnFavoriteTapped(object? sender, TappedEventArgs e)
    {
        if (_suppressNextOpenDetails)
        {
            _suppressNextOpenDetails = false;
            return;
        }

        if (e.Parameter is not FavoriteArticle fav)
            return;

        var article = new Article
        {
            Title = fav.Title,
            Description = fav.Description,
            Url = fav.Url,
            UrlToImage = fav.UrlToImage,
            PublishedAt = fav.PublishedAt,
            Source = new ArticleSource { Name = fav.SourceName }
        };

        await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
        {
            { "Article", article }
        });
    }

    private async void OnDetailsClicked(object? sender, EventArgs e)
    {
        if (sender is not BindableObject bo || bo.BindingContext is not FavoriteArticle fav)
            return;

        _suppressNextOpenDetails = true;

        var article = new Article
        {
            Title = fav.Title,
            Description = fav.Description,
            Url = fav.Url,
            UrlToImage = fav.UrlToImage,
            PublishedAt = fav.PublishedAt,
            Source = new ArticleSource { Name = fav.SourceName }
        };

        await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
        {
            { "Article", article }
        });
    }

    private async void OnHeartTapped(object? sender, TappedEventArgs e)
    {
        _suppressNextOpenDetails = true;

        if (e.Parameter is FavoriteArticle fav)
            await _vm.SupprimerAsync(fav);
    }

    private async void OnHomeClicked(object? sender, EventArgs e)
    {
        SetActiveTab("home");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnFavoritesClicked(object? sender, EventArgs e)
    {
        SetActiveTab("favorites");
        await Task.CompletedTask;
    }

    private async void OnProfileClicked(object? sender, EventArgs e)
    {
        SetActiveTab("profile");
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private void SetActiveTab(string tab)
    {
        var active = Color.FromArgb("#E5E7EB");
        var inactive = Colors.Transparent;

        HomeTab.BackgroundColor = tab == "home" ? active : inactive;
        FavoritesTab.BackgroundColor = tab == "favorites" ? active : inactive;
        ProfileTab.BackgroundColor = tab == "profile" ? active : inactive;
    }
}

