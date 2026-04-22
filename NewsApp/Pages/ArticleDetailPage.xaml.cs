using NewsApp.Models;
using NewsApp.Services;

namespace NewsApp.Pages;

[QueryProperty(nameof(ArticleId), "id")]
public partial class ArticleDetailPage : ContentPage
{
    private string? _articleId;

    public ArticleDetailPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    public Article? Article { get; private set; }

    public string? ArticleId
    {
        get => _articleId;
        set
        {
            _articleId = value;
            Article = string.IsNullOrWhiteSpace(_articleId) ? null : ArticleStore.GetById(_articleId);
            OnPropertyChanged(nameof(Article));
        }
    }
}

