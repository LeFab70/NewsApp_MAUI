using Microsoft.Extensions.Logging;
using NewsApp.Services;
using NewsApp.ViewModels;
using NewsApp.Views;

namespace NewsApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddHttpClient<NewsApiService>(c =>
            c.BaseAddress = new Uri(Constants.BaseUrl));
        builder.Services.AddSingleton<FavoritesRepository>();
        builder.Services.AddTransient<NewsViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<DetailPage>();

		return builder.Build();
	}
}
