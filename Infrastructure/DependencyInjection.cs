using Application.Interfaces;
using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IGameDataService, GameDataService>();
        services.AddDbContext<TaikoDbContext>(option =>
        {
            var dbName = configuration["DbFileName"];
            if (string.IsNullOrEmpty(dbName))
            {
                dbName = Constants.DefaultDbName;
            }

            var path = Path.Combine(PathHelper.GetRootPath(), dbName);
            option.UseSqlite($"Data Source={path}");
        });
        services.AddScoped<ITaikoDbContext, TaikoDbContext>(provider =>
            provider.GetService<TaikoDbContext>() ?? throw new InvalidOperationException());
        
        return services;
    }
}