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
using ICTAZEVoting.Shared.Responses;
using System.Collections.Generic;
using System.Diagnostics;

namespace ICTAZEVoting.Api
{
    class ToVerify
    {
        public string FileName1 { get; set; }
        public string FileName2 { get; set; }
    }
    class VerificationResult
    {
        public bool Match { get; set; }
    }
    public static class Extensions
    {
        public static IEndpointRouteBuilder MapEndpointRoutes(this IEndpointRouteBuilder app)
        {
            #region Identity             
            app.MapPost("/token", async (ITokenService tokenService, [FromBody] TokenRequest request) =>
            {
                var res = await tokenService.LoginAsync(request);
                if (res.Succeeded)
                {
                    return new Result<TokenResponse>() { Succeeded = true, Messages = res.Messages, Data = res.Data };
                }
                else
                {
                    return new Result<TokenResponse>() { Succeeded = false, Messages = new List<string> { "Incorect credentials" }, Data = new TokenResponse() };
                }
            });
            app.MapGet("/roles", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IRoleService roleService) =>
            {
                return await roleService.GetAllAsync();
            });
            app.MapPost("/roles/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IRoleService roleService, [FromBody] RoleRequest request) =>
            {
                return await roleService.AddEditAsync(request);
            });
            app.MapGet("/users", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUserService userService) =>
            {

                return await userService.GetAllAsync();
            });
            app.MapGet("/users/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUserService userService, [FromRoute] string id) =>
            {

                return await userService.GetByIdAsync(Guid.Parse(id));
            });
            app.MapGet("/users/roles/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUserService userService, [FromRoute] string id) =>
            {

                return await userService.GetRolesAsync(Guid.Parse(id));
            });
            #endregion
            #region Domain
            app.MapGet("/system-admins", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork) =>
            {
                var result = await unitOfWork.Repository<SystemAdmin>().Entities().Include(c => c.Constituency).ToListAsync();
                return Result<IEnumerable<SystemAdmin>>.Success(result);
            });
            app.MapGet("/system-admins/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var systemAdmin = await unitOfWork.Repository<SystemAdmin>().Entities().Include(c => c.Constituency).FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (systemAdmin == null)
                    {
                        return Result<SystemAdmin>.Fail("Not found.");
                    }
                    return Result<SystemAdmin>.Success(systemAdmin);
                }
                return Result<SystemAdmin>.Fail("Not found.");
            });
            app.MapGet("/system-admins/getbyuserid/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var systemAdmin = await unitOfWork.Repository<SystemAdmin>().Entities().Include(c => c.Constituency).FirstOrDefaultAsync(v => v.PersonalDetails.UserId == myGuid);
                    if (systemAdmin == null)
                    {
                        return Result<SystemAdmin>.Fail("Not found.");
                    }
                    return Result<SystemAdmin>.Success(systemAdmin);
                }
                return Result<SystemAdmin>.Fail("Not found.");
            });
            app.MapPost("/system-admins/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] SystemAdmin entity) =>
            {
                var result = await unitOfWork.Repository<SystemAdmin>().Add(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("SystemAdmin  was created.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapPost("/system-admins/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] SystemAdmin entity) =>
            {
                var result = await unitOfWork.Repository<SystemAdmin>().Update(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("SystemAdmin was updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapDelete("/system-admins/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<SystemAdmin>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return await Result.FailAsync("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<SystemAdmin>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result ? Result.Success($"{entity.PersonalDetails.FullName} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });


            app.MapGet("/voters", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var voters = await unitOfWork.Repository<Voter>().Entities(false).Include(v => v.PolingStation).ToListAsync();
                var result = mapper.Map<List<VoterResponse>>(voters);
                return Result<IEnumerable<VoterResponse>>.Success(result);
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

            app.MapPost("/voters/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IUserService userService, [FromBody] Voter entity) =>
            {
                var userRegister = new RegisterRequest
                {
                    FirstName = entity.PersonalDetails.FirstName,
                    LastName = entity.PersonalDetails.LastName,
                    AutoConfirmEmail = true,
                    ActivateUser = true,
                    Password = Path.GetRandomFileName(),
                    PictureUrl = entity.PersonalDetails.PictureUrl,
                    UserName = entity.PersonalDetails.NRC.Replace("/", String.Empty),
                    Email = entity.PersonalDetails.Email,
                    Role = RoleConstants.BasicRole,
                    PhoneNumber = entity.PersonalDetails.PhoneNumber,
                    NRC = entity.PersonalDetails.NRC
                };
                var register = await userService.RegisterAsync(userRegister);
                if (register.Succeeded)
                {
                    entity.PersonalDetails.UserId = register.Data;
                    //Generate Key
                    var aes = Aes.Create();
                    var Secrete = Guid.NewGuid().ToString();
                    var key = aes.Key;
                    var IV = aes.IV;
                    var encrypted = EncryptionService.EncryptStringToBytes_Aes(Secrete, key, IV);
                    entity.SecreteKey = new Shared.Models.SecreteKey { EncryptedKey = Convert.ToBase64String(encrypted), IV = Convert.ToBase64String(IV) };
                    await unitOfWork.Repository<Voter>().Add(entity);
                    List<string> res = new() { Convert.ToBase64String(key), userRegister.Password };
                    var result = await unitOfWork.Repository<Voter>().Add(entity);
                    result = await unitOfWork.Commit(new CancellationToken()) != 0;
                    //returns the random generated pass and secrete key.
                    //in the future this key has to be stored in a card.
                    return result ? Result<List<string>>.Success(data: res, "Voter registered successifully.") : Result<List<string>>.Fail("An error has occured. Try again.");

                }
                else
                {
                    return Result<List<string>>.Fail($"An error has occured message:{register.Messages.First()}. Try again.");
                }


            });
            app.MapPost("/voters/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Voter entity, IWebHostEnvironment env) =>
            {
                await unitOfWork.Repository<Voter>().Update(entity);
                var result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Voter details were updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapGet("/candidates", [Authorize(Roles = $"{RoleConstants.AdministratorRole},{RoleConstants.BasicRole}")] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var candidates = await unitOfWork.Repository<Candidate>().Entities().Include(c => c.Position).ThenInclude(p => p.Election).Include(c => c.PoliticalParty).ToListAsync();
                var result = mapper.Map<List<CandidateResponse>>(candidates);
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
            app.MapPost("/candidates/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IUserService userService, [FromBody] Candidate entity) =>
            {
                var userRegister = new RegisterRequest
                {
                    FirstName = entity.PersonalDetails.FirstName,
                    LastName = entity.PersonalDetails.LastName,
                    AutoConfirmEmail = true,
                    ActivateUser = true,
                    Password = Path.GetRandomFileName(), //should be changed to a random password generator.
                    PictureUrl = entity.PersonalDetails.PictureUrl,
                    UserName = entity.PersonalDetails.NRC.Replace("/", String.Empty),
                    Email = entity.PersonalDetails.Email,
                    Role = RoleConstants.BasicRole,
                    PhoneNumber = entity.PersonalDetails.PhoneNumber,
                    NRC = entity.PersonalDetails.NRC
                };

                app.MapDelete("/voters/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
                {
                    var myGuid = Guid.Empty;
                    if (Guid.TryParse(id, out myGuid))
                    {
                        var entity = await unitOfWork.Repository<Voter>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                        if (entity == null)
                        {
                            return await Result.FailAsync("Not found.");
                        }
                        else
                        {
                            var result = await unitOfWork.Repository<Voter>().Delete(entity);
                            result = await unitOfWork.Commit(new CancellationToken()) != 0;
                            return result ? Result.Success($"{entity.PersonalDetails.FullName} was deleted.") : Result.Fail("An error occured, try again.");
                        }

                    }
                    return Result.Fail("Not found.");

                });

                var register = await userService.RegisterAsync(userRegister);
                if (register.Succeeded)
                {
                    entity.PersonalDetails.UserId = register.Data;
                    var voter = new Voter
                    {
                        PolingStationId = entity.PollingStationId,
                        PersonalDetails = new PersonalDetails
                        {
                            FirstName = entity.PersonalDetails.FirstName,
                            MiddleName = entity.PersonalDetails.MiddleName,
                            LastName = entity.PersonalDetails.LastName,
                            Gender = entity.PersonalDetails.Gender,
                            DateOfBirth = entity.PersonalDetails.DateOfBirth,
                            Address = entity.PersonalDetails.Address,
                            NRC = entity.PersonalDetails.NRC,
                            PhoneNumber = entity.PersonalDetails.PhoneNumber,
                            Email = entity.PersonalDetails.Email,
                            PictureUrl = entity.PersonalDetails.PictureUrl,
                            UserId = register.Data
                        }

                    };
                    //Generate Key
                    var aes = Aes.Create();
                    var Secrete = Guid.NewGuid().ToString();
                    var key = aes.Key;
                    var IV = aes.IV;
                    var encrypted = EncryptionService.EncryptStringToBytes_Aes(Secrete, key, IV);
                    voter.SecreteKey = new Shared.Models.SecreteKey { EncryptedKey = Convert.ToBase64String(encrypted), IV = Convert.ToBase64String(IV) };
                    await unitOfWork.Repository<Voter>().Add(voter);
                    string[] res = new string[2] { Convert.ToBase64String(key), userRegister.Password };
                    await unitOfWork.Repository<Candidate>().Add(entity);
                    var result = await unitOfWork.Commit(new CancellationToken()) != 0;
                    return result ? Result<string[]>.Success(data: res, "Candidate was registered.") : Result<string[]>.Fail("An error has occured. Try again.");
                }
                else
                {
                    return Result<string[]>.Fail($"An error has occured message:{register.Messages.First()}. Try again.");
                }

            });
            app.MapPut("/candidates/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Candidate entity) =>
            {
                var result = await unitOfWork.Repository<Candidate>().Update(entity);
                return result ? Result.Success("Candidate details were updated.") : Result.Fail("An error has occured. Try again.");

            });
            app.MapGet("/elections/candidates/count/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var result = await unitOfWork.Repository<Candidate>().Entities().Include(e => e.Position).Where(e => e.Position.ElectionId.ToString() == id).CountAsync();
                return Result<int>.Success(result);

            });
            //Election
            app.MapDelete("/elections/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<Election>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return await Result.FailAsync("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<Election>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result ? Result.Success($"{entity.Name} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });
            app.MapGet("/elections", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
             {
                 var elections = await unitOfWork.Repository<Election>().Entities().Include(e => e.Voters).Include(e => e.Positions).ToListAsync();
                 var result = mapper.Map<IEnumerable<ElectionResponse>>(elections);
                 return Result<IEnumerable<ElectionResponse>>.Success(result);
             });
            app.MapGet("/elections/current", [Authorize] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var election = await unitOfWork.Repository<Election>().Entities().Include(e => e.Positions).ThenInclude(p=>p.Candidates).ThenInclude(c=>c.PoliticalParty).FirstOrDefaultAsync(e=>e.IsCurrent);
                var result = mapper.Map<ElectionResponse>(election);
                return Result<ElectionResponse>.Success(result);
            });
            app.MapGet("/elections/pending", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var elections = await unitOfWork.Repository<Election>().Entities().Where(e => e.Status == Shared.Enums.ElectionStatus.Pending).Include(e => e.Voters).Include(e => e.Positions).ToListAsync();
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
                        return result ? Result.Success($"{entity.Name} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });
            app.MapGet("/elections/parties", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var data = await unitOfWork.Repository<PoliticalParty>().Entities().ToListAsync();
                var result = mapper.Map<List<PoliticalPartyResponse>>(data);
                return Result<IEnumerable<PoliticalPartyResponse>>.Success(result);
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
                if (entity == null)
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
            app.MapDelete("candidates/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<Candidate>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return Result.Fail("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<Candidate>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result ? Result.Success($"{entity.PersonalDetails.FullName} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });
            app.MapPost("/userprofile", [Authorize] async (SystemDbContext db, UserManager<User> userManager, [FromBody] UserProfileRequest request) =>
             {
                 var myGuid = Guid.Empty;
                 var id = request.Id;
                 var user = await userManager.FindByIdAsync(id);
                 if (user == null)
                 {
                     return Result<UserProfileResponse>.Fail("Not found.");
                 }
                 else
                 {
                     var profile = new PersonalDetails();

                     profile = (await db.Set<Voter>().FirstOrDefaultAsync(s => s.PersonalDetails.UserId == Guid.Parse(id))).PersonalDetails;
                     if (profile != null)
                     {
                         return Result<UserProfileResponse>.Success(new UserProfileResponse { FullName = profile.FullName, ProfilePicture = profile.PictureUrl });
                     }
                     else
                     {
                         return Result<UserProfileResponse>.Fail("Not found.");
                     }

                 }
             });
            app.MapPost("/verify-face", [Authorize] async ([FromBody] VerifyRequest request, IUploadService uploadService, IWebHostEnvironment env) =>
             {
                 var upload = await uploadService.UploadFileAsync(new UploadRequest { Type = Shared.Enums.UploadType.Temporary, Data = request.Data, FileName = "image.png" });
                 if (!upload.Succeeded)
                 {
                     return Result.Fail("Verification failed. Try again");
                 }
                 var toVer = new ToVerify { FileName1 = request.FileName, FileName2 = upload.Data.Path };
                 HttpClient httpClient = new();
                 httpClient.BaseAddress = new Uri("http://127.0.0.1:5000");
                 var req = await httpClient.PostAsJsonAsync("verify", toVer);
                 var match = false;
                 if (req.IsSuccessStatusCode)
                 {
                     var re = await req.Content.ReadAsStringAsync();
                     var ress = JsonConvert.DeserializeObject<VerificationResult>(re);
                     match = ress.Match;

                 }

                 var path = Path.Combine(env.ContentRootPath, "Files", upload.Data.Path);
                 await Task.Run(() =>
                 {
                     if (File.Exists(path))
                     {
                         File.Delete(path);
                     }
                 });
                 return match ? Result.Success("Face verified.") : Result.Fail("Verification failed");

             });
            app.MapGet("/constituencies", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork) =>
            {
                var result = await unitOfWork.Repository<Constituency>().Entities(false).ToListAsync();
                return Result<IEnumerable<Constituency>>.Success(result);
            });
            app.MapGet("/constituencies/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var constituency = await unitOfWork.Repository<Constituency>().Entities().Include(c => c.PolingStations).FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (constituency == null)
                    {
                        return Result<Constituency>.Fail("Not found.");
                    }
                    return Result<Constituency>.Success(constituency);
                }
                return Result<Constituency>.Fail("Not found.");
            });
            app.MapPost("/constituencies/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Constituency entity) =>
            {
                var result = await unitOfWork.Repository<Constituency>().Add(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Constituencytype was created.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapPost("/constituencies/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] Constituency entity) =>
            {
                var result = await unitOfWork.Repository<Constituency>().Update(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("Constituencywas updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapDelete("/constituencies/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<Constituency>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return Result.Fail("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<Constituency>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result ? Result.Success($"{entity.Name} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });
            app.MapGet("/constituencies/polling-stations", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, IMapper mapper) =>
            {
                var data = await unitOfWork.Repository<PollingStation>().Entities().Include(c => c.Constituency).ToListAsync();
                var res = mapper.Map<List<PollingStationResponse>>(data);
                return Result<List<PollingStationResponse>>.Success(res);
            });
            app.MapGet("/constituencies/polling-station/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var pollingStation = await unitOfWork.Repository<PollingStation>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (pollingStation == null)
                    {
                        return Result<PollingStation>.Fail("Not found.");
                    }
                    return Result<PollingStation>.Success(pollingStation);
                }
                return Result<PollingStation>.Fail("Not found.");
            });
            app.MapPost("/constituencies/polling-station/add", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] PollingStation entity) =>
            {
                var result = await unitOfWork.Repository<PollingStation>().Add(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("PollingStation  was created.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapPost("/constituencies/polling-station/update", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromBody] PollingStation entity) =>
            {
                var result = await unitOfWork.Repository<PollingStation>().Update(entity);
                result = await unitOfWork.Commit(new CancellationToken()) != 0;
                return result ? Result.Success("PollingStation was updated.") : Result.Fail("An error has occured. Try again.");
            });
            app.MapGet("/polling-stations/getbyuserid/{id}", [Authorize(Roles = RoleConstants.AdministratorRole)] async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {

                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var systemAdmin = await unitOfWork.Repository<SystemAdmin>().Entities().Include(c => c.Constituency).ThenInclude(c => c.PolingStations).FirstOrDefaultAsync(v => v.PersonalDetails.UserId == myGuid);
                    if (systemAdmin == null)
                    {
                        return Result<List<PollingStation>>.Fail("Not found.");
                    }
                    var lis = new List<PollingStation>();
                    foreach (var item in systemAdmin.Constituency.PolingStations)
                    {
                        lis.Add(new PollingStation { Id = item.Id, ConstituencyId = item.ConstituencyId, Name = item.Name });
                    }
                    return Result<List<PollingStation>>.Success(lis);
                }
                return Result<List<PollingStation>>.Fail("Not found.");

            });
            app.MapDelete("constituencies/polling-station/delete/{id}", async (IUnitOfWork<Guid> unitOfWork, [FromRoute] string id) =>
            {
                var myGuid = Guid.Empty;
                if (Guid.TryParse(id, out myGuid))
                {
                    var entity = await unitOfWork.Repository<PollingStation>().Entities().FirstOrDefaultAsync(v => v.Id == myGuid);
                    if (entity == null)
                    {
                        return Result.Fail("Not found.");
                    }
                    else
                    {
                        var result = await unitOfWork.Repository<PollingStation>().Delete(entity);
                        result = await unitOfWork.Commit(new CancellationToken()) != 0;
                        return result ? Result.Success($"{entity.Name} was deleted.") : Result.Fail("An error occured, try again.");
                    }

                }
                return Result.Fail("Not found.");

            });
            #endregion
            #region Uploads
            app.MapPost("/biometrics", (IWebHostEnvironment env, [FromBody] UploadRequest request) =>
            {
                var fileName = request.Data.ToImageFile(Path.Combine(env.ContentRootPath, "biometrics"));
                return Result.Success(fileName);
            });
            app.MapPost("/upload", [Authorize] async (IUploadService service, [FromBody] UploadRequest request, HttpContext context) =>
            {
                var result = await service.UploadFileAsync(request);
                if (result.Succeeded)
                {
                    return Result<UploadResponse>.Success(new UploadResponse { Path = result.Data.Path });
                }
                else
                {
                    return Result<UploadResponse>.Fail("An error occured");
                }

            });
            #endregion
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
        internal static async void InitializeFaceApi(this IApplicationBuilder app, IHostEnvironment env)
        {
            var script = Path.Combine(env.ContentRootPath, "scripts", "app.py");
            char[] splitter = { '\r' };
            using Process cmd = new();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.Arguments = $"cd {Path.Combine(env.ContentRootPath, "scripts")} && set FLASK_APP=app.py && set FLASK_ENV=development && flask run";
            cmd.Start();
            Console.WriteLine("Face api started.");

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
                    .AddTransient<IUploadService, UploadService>();
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

