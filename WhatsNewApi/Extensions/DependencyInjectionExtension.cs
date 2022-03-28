using WhatsNewApi.Models.FirestoreModels;
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
        services.AddSingleton<IWhatsNewService, WhatsNewService>();

        // Repository
        services.AddSingleton<IFirestoreRepository<Project>, FirestoreRepository<Project>>();
        services.AddSingleton<IFirestoreRepository<WhatsNew>, FirestoreRepository<WhatsNew>>();
        return services;
    }
}