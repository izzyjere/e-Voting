using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Wrapper;
using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Core.Factories;
using ICTAZEVoting.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using ICTAZEVoting.Core.Services.Identity;
using ICTAZEVoting.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Core.Extensions;
using Microsoft.OpenApi.Models;
using ICTAZEVoting.Shared.Interfaces;

namespace ICTAZEVoting.Api
{
    public static class Extensions
    {
        public static IEndpointRouteBuilder MapEndpointRoutes(this IEndpointRouteBuilder app)
        {
            #region Identity             
            app.MapPost("/token", async (ITokenService tokenService, [FromBody] TokenRequest request) =>
            {
                var res = await tokenService.LoginAsync(request);
                if (res.Succeeded)
                    return new Result<TokenResponse>() { Succeeded = true, Messages = res.Messages, Data = res.Data };
                else
                    return new Result<TokenResponse>() { Succeeded = false, Messages = new List<string> { "Incorect credentials" }, Data = new TokenResponse() };
            });
            #endregion
            return app;
        }
        internal static IApplicationBuilder Initialize (this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var seeder = scope.ServiceProvider.GetService<ISeeder>();
            if(seeder == null)
            {
                throw new Exception("No Seeder was registered");
            }
            seeder.Seed();
            return app;
        }
        internal static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(async c =>
            {
                //TODO - Lowercase Swagger Documents
                //c.DocumentFilter<LowercaseDocumentFilter>();
                //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f

                // include all project's xml comments
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (!assembly.IsDynamic)
                    {
                        var xmlFile = $"{assembly.GetName().Name}.xml";
                        var xmlPath = Path.Combine(baseDirectory, xmlFile);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    }
                }

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ICTAZ VOTING SYSTEM"                    
                });
        

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
            return services;
        }
        internal static AppConfiguration GetApplicationSettings(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return applicationSettingsConfiguration.Get<AppConfiguration>();
        }

        public static IServiceCollection ConfigureAppSettings(
          this IServiceCollection services,
          IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection(nameof(AppConfiguration));
            services.Configure<AppConfiguration>(applicationSettingsConfiguration);
            return services;
        }
        public static IServiceCollection AddServerIdentity(this IServiceCollection services)
        {
            services
                  .AddIdentity<User, Role>(options =>
                  {
                      options.Password.RequiredLength = 6;
                      options.Password.RequireDigit = false;
                      options.Password.RequireLowercase = false;
                      options.Password.RequireNonAlphanumeric = false;
                      options.Password.RequireUppercase = false;
                      options.User.RequireUniqueEmail = true;
                  })
                  .AddEntityFrameworkStores<SystemDbContext>()
                  .AddDefaultTokenProviders();
            services.AddScoped<IUserClaimsPrincipalFactory<User>, SystemUserClaimsPrincipalFactory>();
            return services;
        }
        public static IServiceCollection AddServerServices(this IServiceCollection services)
        {
            services.AddServerIdentity()
                    .AddMappings()
                    .AddIdentityServices()
                    .AddDomainServices();
            return services;
        }
        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AppConfiguration appConfig)
        {
            var key = Encoding.ASCII.GetBytes(appConfig.Secret);
            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        RoleClaimType = ClaimTypes.Role,
                        ClockSkew = TimeSpan.Zero
                    };


                    bearer.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = c =>
                        {
                            if (c.Exception is SecurityTokenExpiredException)
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                c.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result.Fail("Your Token Has Expired"));
                                return c.Response.WriteAsync(result);
                            }
                            else
                            {
                                c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                c.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result.Fail("An unhandled error has occurred."));
                                return c.Response.WriteAsync(result);
                            }
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result.Fail("You are not Authorized."));
                                return context.Response.WriteAsync(result);
                            }

                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(Result.Fail("You are not authorized to access this resource."));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
            services.AddAuthorization();
            return services;
        }
    }
}

