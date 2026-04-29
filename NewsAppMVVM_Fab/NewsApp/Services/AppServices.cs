using Microsoft.Extensions.DependencyInjection;

namespace NewsApp.Services;

public static class AppServices
{
    public static IServiceProvider GetProvider()
    {
        // Après démarrage, MauiContext est disponible
        var provider = App.Current?.Handler?.MauiContext?.Services;
        if (provider != null)
            return provider;

        throw new InvalidOperationException("ServiceProvider indisponible (MauiContext null).");
    }

    public static T GetRequired<T>() where T : notnull =>
        GetProvider().GetRequiredService<T>();
}

