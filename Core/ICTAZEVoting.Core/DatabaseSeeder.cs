using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Shared.Constants;
using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Security;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System.Security.Cryptography;

namespace ICTAZEVoting.Core
{
    public class DatabaseSeeder : ISeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly SystemDbContext _db;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public DatabaseSeeder(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SystemDbContext db,
            ILogger<DatabaseSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;

        }

        public async void Seed()
        {
            AddConstituency();
            AddAdministrator();
            await MakeAllUsersVoters();
            _db.SaveChanges();
        }
        async Task MakeAllUsersVoters()
        {

            var users = await _userManager.Users.ToListAsync();
            foreach (var u in users)
            {
                if (!await _userManager.IsInRoleAsync(u, RoleConstants.BasicRole))
                {
                    await _userManager.AddToRoleAsync(u, RoleConstants.BasicRole);
                }
            }

        }
        private async void AddConstituency()
        {
            var exists = await _db.Set<Constituency>().AnyAsync();
            if (exists)
            {
                //do nothing
                return;
            }
            else
            {
                var constituency = new Constituency() { Name = "Kitwe Central" };
                constituency.PolingStations.Add(new PollingStation { Name = "Riverside" });
                constituency.PolingStations.Add(new PollingStation { Name = "Chimwemwe" });
                constituency.PolingStations.Add(new PollingStation { Name = "Nkana West" });
                _db.Add(constituency);
                await _db.SaveChangesAsync();
                _logger.LogInformation("Seeded default constituency");
            }
        }
        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new Role
                {
                    Name = RoleConstants.AdministratorRole,
                    Description = "Administrator role with full permissions"

                };
                var voterr = new Role
                {
                    Name = RoleConstants.BasicRole,
                    Description = "Voter role"

                };
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                var adminRoleInDb2 = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }
                if (adminRoleInDb2 == null)
                {
                    await _roleManager.CreateAsync(voterr);                    
                    _logger.LogInformation("Seeded Basic Role.");
                }
                //Check if User Exists
                var superUser = new User
                {

                    Email = "admin@example.com",
                    UserName = "testAdmin",
                    FirstName = "Wisdom",
                    LastName = "Jere",
                    NRC = "150279/34/1",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    PictureUrl = "images/wisdom.png"
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, "test1234");
                    var userGuid = await _userManager.GetUserIdAsync(superUser);
                    var syadmin = new SystemAdmin
                    {
                        PersonalDetails = new()
                        {
                            FirstName = "Wisdom",
                            LastName = "Jere",
                            NRC = "150279/34/1",
                            Email = superUser.Email,
                            PhoneNumber = "+260974855669",
                            Gender = Shared.Enums.Gender.Male,
                            PictureUrl = "images/wisdom.png",
                            UserId = Guid.Parse(userGuid),
                            DateOfBirth = DateTime.Now,
                            Address = "Riverside, Kitwe"
                        },
                        ConstituencyId = await _db.Set<Constituency>().Select(c => c.Id).FirstOrDefaultAsync(),
                        CreatedBy = "SYSTEM",
                        TimeStamp = DateTime.Now,
                        RemoteIp = "127.0.0.1"
                    };
                    var voter = new Voter
                    {
                        PersonalDetails = new()
                        {
                            FirstName = "Wisdom",
                            LastName = "Jere",
                            NRC = "150279/34/1",
                            Email = superUser.Email,
                            PhoneNumber = "+260974855669",
                            Gender = Shared.Enums.Gender.Male,
                            PictureUrl = "images/wisdom.png",
                            UserId = Guid.Parse(userGuid),
                            DateOfBirth = DateTime.Now,
                            Address = "Riverside, Kitwe"
                        },
                        PolingStationId = await _db.Set<PollingStation>().Select(c => c.Id).FirstOrDefaultAsync(),
                        CreatedBy = "SYSTEM",
                        TimeStamp = DateTime.Now,
                        RemoteIp = "127.0.0.1"
                    };
                    //Generate Key
                    var aes = Aes.Create();
                    var Secrete = Guid.NewGuid().ToString();
                    var key = aes.Key;
                    var IV = aes.IV;
                    var encrypted = EncryptionService.EncryptStringToBytes_Aes(Secrete, key, IV);
                    voter.SecreteKey = new Shared.Models.SecreteKey { EncryptedKey = Convert.ToBase64String(encrypted), IV = Convert.ToBase64String(IV) };
                    Console.WriteLine("Key: " + Convert.ToBase64String(key));
                    _db.Set<Voter>().Add(voter);
                    _db.Set<SystemAdmin>().Add(syadmin);
                    await _db.SaveChangesAsync();
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Seeded Default SuperAdmin User.");

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError(message: error.Description);
                        }
                    }
                }

            }).GetAwaiter().GetResult();
        }
    }
}
