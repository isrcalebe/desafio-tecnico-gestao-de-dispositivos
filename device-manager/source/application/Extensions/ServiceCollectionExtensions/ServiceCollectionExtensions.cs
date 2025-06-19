using Microsoft.Extensions.DependencyInjection;

namespace DeviceManager.Application.Extensions.ServiceCollectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection self)
    {
        self.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        return self;
    }
}
