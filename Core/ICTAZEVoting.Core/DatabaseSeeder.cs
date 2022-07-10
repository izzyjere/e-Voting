﻿using ICTAZEVoting.Core.Data.Contexts;
using ICTAZEVoting.Core.Mappings;
using ICTAZEVoting.Core.Models;
using ICTAZEVoting.Shared.Constants;
using ICTAZEVoting.Shared.Interfaces;
using ICTAZEVoting.Shared.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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

        public void Seed()
        {
            AddAdministrator();
            _db.SaveChanges();
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
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                    _logger.LogInformation("Seeded Administrator Role.");
                }
                //Check if User Exists
                var superUser = new User                 {
                   
                    Email = "admin@example.com",
                    UserName = "testAdmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,                    
                    IsActive = true,
                    PictureUrl = ""                   
                };                
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, "test1234");
                    var userGuid = await _userManager.GetUserIdAsync(superUser);
                    var syadmin = new SystemAdmin { 
                      PersonalDetails = new()
                      {
                          FirstName = "Wisdom",
                          LastName = "Jere",
                          NRC = "150279/34/1" ,
                          Email = superUser.Email,
                          PhoneNumber = "+260974855669",
                          Gender = Shared.Enums.Gender.Male,
                          PictureUrl = "",
                          UserId = Guid.Parse(userGuid),
                          DateOfBirth = DateTime.Now,
                          Address = "Riverside, Kitwe"
                      },CreatedBy = "SYSTEM",TimeStamp = DateTime.Now,RemoteIp="127.0.0.1"
                    };

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
