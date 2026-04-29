namespace NewsApp;

using NewsApp.Views;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(FavoritesPage), typeof(FavoritesPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute("detail", typeof(DetailPage));

        UpdateTitleFromPreferences();
    }

    public static void RefreshTitle()
    {
        if (Shell.Current is not AppShell shell)
            return;

        MainThread.BeginInvokeOnMainThread(shell.UpdateTitleFromPreferences);
    }

    private void UpdateTitleFromPreferences()
    {
        var name = Preferences.Get("display_name", string.Empty).Trim();
        ShellTitleLabel.Text = string.IsNullOrWhiteSpace(name) ? "NewsApp" : $"NewsApp • {name}";
    }
}
