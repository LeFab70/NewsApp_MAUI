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
        if (BindingContext is Article a && !string.IsNullOrWhiteSpace(a.Url))
            await Launcher.OpenAsync(a.Url);
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

