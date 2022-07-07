﻿using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Core.Data.Repositories;
using ICTAZEVoting.Core.Services.Identity;
using ICTAZEvoting.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ICTAZEVoting.Core.Extensions
{
    public static class HelperExtensions
    {
        public static int GetId(this ClaimsPrincipal claimsPrincipal)
             => Convert.ToInt32(claimsPrincipal.FindFirstValue("UserId"));
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