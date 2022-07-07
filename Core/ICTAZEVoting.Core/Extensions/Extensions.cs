using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Core.Data.Repositories;
using ICTAZEVoting.Core.Services.Identity;
using ICTAZEvoting.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ICTAZEVoting.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services
                    .AddTransient<IUnitOfWork<int>, UnitOfWork<int>>()
                    .AddTransient<ISeeder, DatabaseSeeder>();

            return services;
        }
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<IUserAccountService, UserAccountService>()
                    .AddScoped<IRoleService, RoleService>()
                    .AddScoped<IUserService, UserService>()
                    .AddScoped<ITokenService, TokenService>();

            return services;
        }
        public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
        {
            services
                .AddHttpContextAccessor()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddDbContext<SystemDbContext>(optionsBuilder =>
                {
                    optionsBuilder.UseSqlServer(connectionString, options =>
                    {
                        options.MigrationsAssembly(typeof(SystemDbContext).Assembly.FullName);
                        options.EnableRetryOnFailure();
                        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                }, ServiceLifetime.Transient);

            return services;
        }
    }
}
