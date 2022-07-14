using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Core.Data.Repositories;
using ICTAZEVoting.Core.Services.Identity;
using ICTAZEVoting.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Reflection;
using SkiaSharp;

namespace ICTAZEVoting.Core.Extensions
{
    public static class HelperExtensions
    {
        public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
            => Guid.Parse(claimsPrincipal.FindFirstValue("UserId"));
        public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue("FullName");
        public static string GetRole(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Role);
       
    }
    
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services
                    .AddTransient<IUnitOfWork<Guid>, UnitOfWork<Guid>>()
                    .AddTransient<ISeeder, DatabaseSeeder>();

            return services;
        }
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
           return services.AddAutoMapper(Assembly.GetExecutingAssembly());
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
