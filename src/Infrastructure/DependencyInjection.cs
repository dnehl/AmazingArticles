using AmazingArticles.Application.Common.Interfaces;
using AmazingArticles.Domain.Entities;
using AmazingArticles.Infrastructure.Persistence;
using AmazingArticles.Infrastructure.Persistence.Configurations;
using AmazingArticles.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AmazingArticles.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseSettings>(configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(x =>
                x.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<MongoDatabaseCRUD>();
            services.AddSingleton<IApplicationRepository<Article>, ArticlesRepository>();
            services.AddTransient<IDateTime, DateTimeService>();
            return services;
        }
    }
}