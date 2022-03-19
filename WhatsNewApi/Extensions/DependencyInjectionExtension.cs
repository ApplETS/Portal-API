using WhatsNewApi.Services;
using WhatsNewApi.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthentificationService, AuthentificationService>();
            return services;
        }
    }
}