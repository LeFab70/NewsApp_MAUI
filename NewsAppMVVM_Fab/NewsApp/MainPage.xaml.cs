using NewsApp.Models;
using NewsApp.ViewModels;
using NewsApp.Views;

namespace NewsApp;

public partial class MainPage : ContentPage
{
    private readonly NewsViewModel _vm;
    private bool _suppressNextSelection;

    public MainPage(NewsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        SetActiveTab("home");

        if (_vm.Articles.Count == 0)
        {
            var defaultCat = Preferences.Get("default_category", "Tout");
            await _vm.ChargerArticles(defaultCat);
        }
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        _vm.FiltrerLocalement(e.NewTextValue);
    }

    private async void OnCategoryClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await _vm.ChargerArticles(button.Text ?? "Tout");
        }
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_suppressNextSelection)
        {
            _suppressNextSelection = false;
            ArticleCollection.SelectedItem = null;
            return;
        }

        if (e.CurrentSelection.FirstOrDefault() is Article selected)
        {
            ArticleCollection.SelectedItem = null;
            await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
            {
                { "Article", selected }
            });
        }
    }

    private async void OnToggleFavoriteTapped(object? sender, TappedEventArgs e)
    {
        if (e.Parameter is Article article)
        {
            _suppressNextSelection = true;
            ArticleCollection.SelectedItem = null;
            await _vm.ToggleFavori(article);
        }
    }

    private async void OnDetailsClicked(object? sender, EventArgs e)
    {
        if (sender is not BindableObject bo || bo.BindingContext is not Article article)
            return;

        _suppressNextSelection = true;
        ArticleCollection.SelectedItem = null;

        await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
        {
            { "Article", article }
        });
    }

    private async void OnHomeClicked(object? sender, EventArgs e)
    {
        SetActiveTab("home");
        SearchBar.Text = string.Empty;
        await _vm.ChargerArticles();
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnFavoritesClicked(object? sender, EventArgs e)
    {
        SetActiveTab("favorites");
        try
        {
            await Shell.Current.GoToAsync(nameof(FavoritesPage));
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erreur Favoris", ex.Message, "OK");
        }
    }

    private async void OnProfileClicked(object? sender, EventArgs e)
    {
        SetActiveTab("profile");
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }

    private async void OnRefresh(object? sender, EventArgs e)
    {
        await _vm.ChargerArticles();
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        await _vm.ChargerArticles();
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
