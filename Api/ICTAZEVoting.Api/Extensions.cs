using ICTAZEvoting.Shared.Contracts;
using ICTAZEvoting.Shared.Wrapper;

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

namespace ICTAZEVoting.Api
{
   
        public static class Extensions
        {

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

