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
using ICTAZEVoting.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using ICTAZEVoting.Shared.Models;
using Microsoft.EntityFrameworkCore;
using ICTAZEVoting.Core.Data.Repositories;
using System.Security.Cryptography;
using SkiaSharp;
using ICTAZEVoting.Shared.Security;
using AutoMapper;
using ICTAZEVoting.Shared.Responses.Domain;
using ICTAZEVoting.Api.Utility;
using System.Net.Http.Headers;

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
            app.MapGet("/roles",[Authorize(Roles =RoleConstants.AdministratorRole)] async (IRoleService roleService) => {
                return await roleService.GetAllAsync();
            });
            app.MapPost("/roles/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IRoleService roleService,[FromBody] RoleRequest request) =>
            {
                return await roleService.AddEditAsync(request);
            });
            app.MapGet("/users", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUserService userService) => { 
            
                 return await userService.GetAllAsync();
            });
            app.MapGet("/users/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUserService userService, [FromRoute]string id) => {

                return await userService.GetByIdAsync(Guid.Parse(id));
            });
            app.MapGet("/users/roles/{id}",[Authorize(Roles = RoleConstants.AdministratorRole)] async (IUserService userService, [FromRoute]string id) => {
                
                return await userService.GetRolesAsync(Guid.Parse(id));
            });
            #endregion
            #region Domain
            app.MapGet("/voters", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork) =>
            {
                var result = await unitOfWork.Repository<Voter>().Entities().ToListAsync();
                return Result<IEnumerable<Voter>>.Success(result);
            });
            app.MapGet("/voters/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var voter = await unitOfWork.Repository<Voter>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (voter == null)
                    {
                        return Result<Voter>.Fail("Not found.");
                    }
                    return Result<Voter>.Success(voter);
                }
                return Result<Voter>.Fail("Not found.");

            });
            app.MapPost("/voters/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Voter entity) =>
            {
                //Generate Key
                var Secrete = Guid.NewGuid().ToString();
                var keyGuid = Guid.NewGuid().ToString();
                var IV = Guid.NewGuid().ToString();
                var encrypted = EncryptionService.EncryptStringToBytes_Aes(Secrete, Encoding.ASCII.GetBytes(keyGuid), Encoding.ASCII.GetBytes(IV));
                entity.SecreteKey = new Shared.Models.SecreteKey { EncryptedKey = Convert.ToBase64String(encrypted), IV = Convert.ToBase64String(Encoding.ASCII.GetBytes(IV)) };
                var result = await unitOfWork.Repository<Voter>().Add(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result<string>.Success(keyGuid,"Voter was registered.") : Result<string>.Fail("An error has occured. Try again.");
            });
            app.MapPut("/voters/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Voter entity) =>
            {
                var result = await unitOfWork.Repository<Voter>().Update(entity);
                return result ? Result.Success("Voter details were updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapGet("/candidates", [Authorize(Roles = $"{RoleConstants.AdministratorRole},{RoleConstants.BasicRole}")] async (IUnitOfWork<Guid> unitOfWork,IMapper mapper) =>
            {
                var candidates = await unitOfWork.Repository<Candidate>().Entities().Include(c => c.Position).ThenInclude(p => p.Election).Include(c => c.PoliticalParty).ToListAsync();
                var result =  mapper.Map<List<CandidateResponse>>(candidates);
                return Result<IEnumerable<CandidateResponse>>.Success(result);
            });
            app.MapGet("/candidates/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var candidate = await unitOfWork.Repository<Candidate>().Entities().Include(c => c.Position).ThenInclude(p => p.Election).Include(c => c.PoliticalParty).FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (candidate == null)
                    {
                        return Result<Candidate>.Fail("Not found.");
                    }
                    return Result<Candidate>.Success(candidate);
                }
                return Result<Candidate>.Fail("Not found.");

            });
            app.MapPost("/candidates/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Candidate entity) =>
            {
                var result = await unitOfWork.Repository<Candidate>().Add(entity);
                return result ? Result.Success("Candidate was registered.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapPut("/candidates/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Candidate entity) =>
            {
                var result = await unitOfWork.Repository<Candidate>().Update(entity);
                return result ? Result.Success("Candidate details were updated.") : Result.Fail("An error has occured. Try again.");

            });
            //Election
            app.MapGet("/elections",[Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var elections = await unitOfWork.Repository<Election>().Entities().Include(e => e.Voters).Include(e => e.Positions).ToListAsync();
                var result = mapper.Map<IEnumerable<ElectionResponse>>(elections);
                return Result<IEnumerable<ElectionResponse>>.Success(result);
            });
            app.MapGet("/elections/pending", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var elections = await unitOfWork.Repository<Election>().Entities().Where(e=>e.Status==Shared.Enums.ElectionStatus.Pending).Include(e => e.Voters).Include(e => e.Positions).ToListAsync();
                var result = mapper.Map<IEnumerable<ElectionResponse>>(elections);
                return Result<IEnumerable<ElectionResponse>>.Success(result);
            });
            app.MapGet("/elections/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var election = await unitOfWork.Repository<Election>().Entities().Include(e => e.Voters).Include(e => e.Positions).ThenInclude(p => p.Candidates).ThenInclude(c => c.PoliticalParty).FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (election == null)
                    {
                        return Result<Election>.Fail("Not found.");
                    }
                    return Result<Election>.Success(election);
                }
                return Result<Election>.Fail("Not found.");
            });
            app.MapPost("/elections/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Election entity) =>
             {
                 var result = await unitOfWork.Repository<Election>().Add(entity);
                 result = await unitOfWork.Commit(new CancellationToken()) != 0;
                 return result ? Result.Success("Election was created.") : Result.Fail("An error has occured. Try again.");
             });
            app.MapPost("/elections/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Election entity) =>
             {
                 var result = await unitOfWork.Repository<Election>().Update(entity);
                 result = await unitOfWork.Commit(new CancellationToken()) != 0;
                 return result ? Result.Success("Election was updated.") : Result.Fail("An error has occured. Try again.");
             });
            app.MapGet("/elections/types", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork) =>
            {
                var result = await unitOfWork.Repository<ElectionType>().Entities().ToListAsync();
                return Result<IEnumerable<ElectionType>>.Success(result);
            });
            app.MapGet("/elections/types/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var electionType = await unitOfWork.Repository<ElectionType>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (electionType == null)
                    {
                        return Result<ElectionType>.Fail("Not found.");
                    }
                    return Result<ElectionType>.Success(electionType);
                }
                return Result<ElectionType>.Fail("Not found.");
            });
            app.MapPost("/elections/types/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] string entity) =>
            {
                var result = await unitOfWork.Repository<ElectionType>().Add(new ElectionType { Name = entity });
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Election type was created.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapPost("/elections/types/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] ElectionType entity) =>
            {
                var result = await unitOfWork.Repository<ElectionType>().Update(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Election was updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapDelete("/elections/types/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<ElectionType>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return Result.Fail("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<ElectionType>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result? Result.Success($"{entity.Name} was deleted."): Result.Fail("An error occured, try again.");
                    }
                   
                }
                return Result.Fail("Not found.");

            });
            app.MapGet("/elections/parties", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork) =>
            {
                var result = await unitOfWork.Repository<PoliticalParty>().Entities().Include(p=>p.Candidates).ThenInclude(c=>c.Position).ToListAsync();
                return Result<IEnumerable<PoliticalParty>>.Success(result);
            });
            app.MapGet("/elections/parties/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var PoliticalParty = await unitOfWork.Repository<PoliticalParty>().Entities().Include(p => p.Candidates).ThenInclude(c => c.Position).FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (PoliticalParty == null)
                    {
                        return Result<PoliticalParty>.Fail("Not found.");
                    }
                    return Result<PoliticalParty>.Success(PoliticalParty);
                }
                return Result<PoliticalParty>.Fail("Not found.");
            });
            app.MapPost("/elections/parties/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] PoliticalParty entity) =>
            {
                var result = await unitOfWork.Repository<PoliticalParty>().Add(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Political Party was created.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapPost("/elections/parties/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] PoliticalPartyUpdateRequest request) =>
            {
                var entity = await unitOfWork.Repository<PoliticalParty>().Get(request.Id);
                if(entity==null)
                {
                    return Result.Fail("Not found.");
                }
                entity.Name = request.Name;
                entity.Slogan = request.Slogan;
                entity.Manifesto = request.Manifesto;
                entity.LogoUrl = request.LogoUrl;
                var result = await unitOfWork.Repository<PoliticalParty>().Update(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Political Party was updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapDelete("/elections/parties/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<PoliticalParty>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return Result.Fail("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<PoliticalParty>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result ? Result.Success($"{entity.Name} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });
            app.MapPost("/userprofile", [Authorize] async (SystemDbContext db, UserManager<User> userManager, [FromBody]UserProfileRequest request) =>
             {
                 var myGuid = Guid.Empty;
                 var id  = request.Id;
                 var user = await userManager.FindByIdAsync(id);
                 if (user == null)
                 {
                     return Result<UserProfileResponse>.Fail("Not found.");
                 }
                 else
                 {
                     var profile = new PersonalDetails();
                     profile = (await db.Set<SystemAdmin>().FirstOrDefaultAsync(s => s.PersonalDetails.UserId == Guid.Parse(id))).PersonalDetails;
                     if(profile != null)
                     {
                         return Result<UserProfileResponse>.Success(new UserProfileResponse { FullName=profile.FullName, ProfilePicture=profile.PictureUrl });
                     }
                     else
                     {
                         profile = profile = (await db.Set<Voter>().FirstOrDefaultAsync(s => s.PersonalDetails.UserId == Guid.Parse(id))).PersonalDetails; ;
                         if (profile != null)
                         {
                             return Result<UserProfileResponse>.Success(new UserProfileResponse { FullName = profile.FullName, ProfilePicture = profile.PictureUrl });
                         }
                         else
                         {
                             profile = (await db.Set<Candidate>().FirstOrDefaultAsync(s => s.PersonalDetails.UserId == Guid.Parse(id))).PersonalDetails;
                             if (profile != null)
                             {
                                 return Result<UserProfileResponse>.Success(new UserProfileResponse { FullName = profile.FullName, ProfilePicture = profile.PictureUrl });
                             }
                             else
                             {
                                 return Result<UserProfileResponse>.Fail("Not found.");
                             }
                         }
                     }

                 }
                
             });
            app.MapPost("/verify-image", async ([FromBody] VerifyRequest request) =>
            {
                HttpClient httpClient = new();
                httpClient.BaseAddress = new Uri("http:127.0.0.1:5000");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var req = await httpClient.PostAsJsonAsync("verify", request);
                var match = JsonConvert.DeserializeObject<bool>(await req.Content.ReadAsStringAsync());
                return match?Result.Success("Face verified."):Result.Fail("Verification failed");

            });
            app.MapPost("/biometrics", (IWebHostEnvironment env, [FromBody]UploadRequest request) =>
            {
                var fileName = request.Data.ToImageFile(Path.Combine(env.ContentRootPath,"biometrics"));
                return Result.Success(fileName);
            });
            #endregion
            app.MapPost("/upload", [Authorize]async (IUploadService service,[FromBody]UploadRequest request, HttpContext context) =>
            {  
                var res= await service.UploadFileAsync(request);
                if(res.Succeeded)
                {
                    return res;
                }
                else
                {
                    return Result.Fail("An error occured");
                }

            });
            return app;
        }

        internal static string ToImageFile(this byte[] bytes, string path)
        {
            var image = SKImage.FromEncodedData(bytes);
            var filename = Path.GetRandomFileName().Replace(".", "_") + ".jpeg";

            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            {
                // save the data to a stream
                using (var stream = File.OpenWrite(Path.Combine(path, filename)))
                {
                    data.SaveTo(stream);
                }
            }
            return filename;
        }
        internal static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var seeder = scope.ServiceProvider.GetService<ISeeder>();
            if (seeder == null)
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
                    .AddDomainServices()
                    .AddTransient<IUploadService,UploadService>();
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

