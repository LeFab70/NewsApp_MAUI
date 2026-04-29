namespace NewsApp.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetActiveTab("profile");
    }

    private async void OnHomeClicked(object? sender, EventArgs e)
    {
        SetActiveTab("home");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnFavoritesClicked(object? sender, EventArgs e)
    {
        SetActiveTab("favorites");
        await Shell.Current.GoToAsync(nameof(FavoritesPage));
    }

    private async void OnProfileClicked(object? sender, EventArgs e)
    {
        SetActiveTab("profile");
        await Task.CompletedTask;
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

