namespace NewsApp.Pages;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
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
        // Déjà sur la page Profil: aucune action
        await Task.CompletedTask;
    }
}

