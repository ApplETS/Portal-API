using Microsoft.Extensions.Options;
using WhatsNewApi.Models.Options;

namespace Microsoft.Extensions.DependencyInjection;
public static class MyConfigServiceCollectionExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<FirebaseSettings>(config.GetSection(nameof(FirebaseSettings)));
        services.AddSingleton<IFirebaseSettings>(sp => sp.GetRequiredService<IOptions<FirebaseSettings>>().Value);
        return services;
    }
}
