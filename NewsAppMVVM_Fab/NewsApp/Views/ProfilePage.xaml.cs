namespace NewsApp.Views;

public partial class ProfilePage : ContentPage
{
    private static class PrefKeys
    {
        public const string DisplayName = "display_name";
        public const string DefaultCategory = "default_category";
        public const string DarkTheme = "dark_theme";
        public const string Notifications = "notifications_enabled";
        public const string NewsCountry = "news_country";
    }

    public ProfilePage()
    {
        InitializeComponent();

        DefaultCategoryPicker.ItemsSource = new List<string>
        {
            "Tout", "Politique", "Sport", "Cinéma", "Technologie"
        };

        CountryPicker.ItemsSource = new List<string>
        {
            "USA (us)",
            "Canada (ca)",
            "France (fr)"
        };
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetActiveTab("profile");

        DisplayNameEntry.Text = Preferences.Get(PrefKeys.DisplayName, "Fabrice");
        var defaultCat = Preferences.Get(PrefKeys.DefaultCategory, "Tout");
        DefaultCategoryPicker.SelectedItem = defaultCat;

        var country = Preferences.Get(PrefKeys.NewsCountry, "us").Trim().ToLowerInvariant();
        CountryPicker.SelectedItem = country switch
        {
            "ca" => "Canada (ca)",
            "fr" => "France (fr)",
            _ => "USA (us)"
        };

        var dark = Preferences.Get(PrefKeys.DarkTheme, false);
        DarkThemeSwitch.IsToggled = dark;
        ApplyTheme(dark);

        var notif = Preferences.Get(PrefKeys.Notifications, true);
        NotificationsSwitch.IsToggled = notif;
    }

    private void OnSaveClicked(object? sender, EventArgs e)
    {
        Preferences.Set(PrefKeys.DisplayName, DisplayNameEntry.Text ?? "");
        Preferences.Set(PrefKeys.DefaultCategory, DefaultCategoryPicker.SelectedItem?.ToString() ?? "Tout");
        Preferences.Set(PrefKeys.DarkTheme, DarkThemeSwitch.IsToggled);
        Preferences.Set(PrefKeys.Notifications, NotificationsSwitch.IsToggled);
        var selectedCountry = CountryPicker.SelectedItem?.ToString() ?? "USA (us)";
        var code = selectedCountry.Contains("(ca)") ? "ca" : selectedCountry.Contains("(fr)") ? "fr" : "us";
        Preferences.Set(PrefKeys.NewsCountry, code);

        ApplyTheme(DarkThemeSwitch.IsToggled);
        AppShell.RefreshTitle();
        DisplayAlertAsync("OK", "Paramètres enregistrés.", "Fermer");
    }

    private static void ApplyTheme(bool dark)
    {
        if (App.Current == null) return;
        App.Current.UserAppTheme = dark ? AppTheme.Dark : AppTheme.Light;
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

