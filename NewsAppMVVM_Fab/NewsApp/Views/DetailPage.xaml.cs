using NewsApp.Models;

namespace NewsApp.Views;

[QueryProperty(nameof(Article), "Article")]
public partial class DetailPage : ContentPage
{
    public DetailPage()
    {
        InitializeComponent();
    }

    public Article? Article
    {
        set => BindingContext = value;
    }

    private async void OnLire(object? sender, EventArgs e)
    {
        if (BindingContext is not Article a)
            return;

        var url = a.Url?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(url))
        {
            await DisplayAlertAsync("Lien manquant", "Cet article n'a pas de lien à ouvrir.", "OK");
            return;
        }

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            await DisplayAlertAsync("Lien invalide", "Impossible d'ouvrir ce lien.", "OK");
            return;
        }

        try
        {
            await Launcher.OpenAsync(uri);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erreur", ex.Message, "OK");
        }
    }

    private async void OnPartager(object? sender, EventArgs e)
    {
        if (BindingContext is Article a)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Title = a.Title,
                Uri = a.Url
            });
        }
    }
}

