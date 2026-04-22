using NewsApp.Models;
using NewsApp.Pages;
using NewsApp.Services;

namespace NewsApp;

public partial class MainPage : ContentPage
{
    private readonly List<Article> allArticles;

    public MainPage()
    {
        InitializeComponent();

        allArticles = ArticleStore.All.ToList();
        ArticleCollection.ItemsSource = allArticles;
    }

    private void OnSearch(object? sender, EventArgs e)
    {
        var query = SearchBar.Text?.ToLower() ?? "";
        ArticleCollection.ItemsSource = allArticles
            .Where(a => a.Titre.ToLower().Contains(query) ||
                        a.Description.ToLower().Contains(query))
            .ToList();
    }

    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        OnSearch(sender, EventArgs.Empty);
    }

    private void OnCategoryClicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            var category = button.Text?.ToLower() ?? "";
            ArticleCollection.ItemsSource = allArticles
                .Where(a => a.Titre.ToLower().Contains(category))
                .ToList();
        }
    }

    private async void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Article selected)
        {
            ArticleCollection.SelectedItem = null;
            await Shell.Current.GoToAsync($"{nameof(ArticleDetailPage)}?id={Uri.EscapeDataString(selected.Id)}");
        }
    }

    private async void OnHomeClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnFavoritesClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FavoritesPage));
    }

    private async void OnProfileClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ProfilePage));
    }
}
