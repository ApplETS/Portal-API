using WhatsNewApi.Repos;
using WhatsNewApi.Repos.Abstractions;
using WhatsNewApi.Services;
using WhatsNewApi.Services.Abstractions;

namespace Microsoft.Extensions.DependencyInjection;
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Services
            services.AddSingleton<IAuthentificationService, AuthentificationService>();
            services.AddSingleton<IFirebaseService, FirebaseService>();
            services.AddSingleton<IProjectService, ProjectService>();

            // Repository
            services.AddSingleton<IProjectRepository, ProjectRepository>();
            return services;
        }
    }
