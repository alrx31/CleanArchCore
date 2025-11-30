using Microsoft.Extensions.DependencyInjection;

namespace Core.DI;

public static class ServiceLocator
{
    private static IServiceProvider? _serviceProvider;

    public static IServiceProvider ServiceProvider
    {
        get => _serviceProvider ?? throw new InvalidOperationException("ServiceLocator not initialized. Call Initialize() first.");
        set => _serviceProvider = value;
    }

    public static T GetService<T>() where T : notnull
        => ServiceProvider.GetRequiredService<T>();
}
