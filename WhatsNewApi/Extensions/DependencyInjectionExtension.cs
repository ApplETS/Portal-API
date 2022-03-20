using WhatsNewApi.Services;
using WhatsNewApi.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IAuthentificationService, AuthentificationService>();
            services.AddSingleton<IFirebaseService, FirebaseService>();
            return services;
        }
    }
}