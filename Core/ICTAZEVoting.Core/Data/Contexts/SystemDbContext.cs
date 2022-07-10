using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Interfaces;

using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ICTAZEVoting.Shared.Models;

namespace ICTAZEVoting.Core.Data.Contexts
{
    public class SystemDbContext : AuditableDbContext
    {
        readonly ICurrentUserService currentUserService;
        public SystemDbContext(DbContextOptions<SystemDbContext> options, ICurrentUserService _currentUserService)
            : base(options)
        {
            currentUserService = _currentUserService;

        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>().ToList())
            {
                var userName = await currentUserService.GetUserName();
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.TimeStamp = DateTime.Now;
                        entry.Entity.CreatedBy ??= userName;
                        break;
                    case EntityState.Modified:
                        entry.Property(e => e.CreatedBy).IsModified = false;
                        entry.Property(e => e.TimeStamp).IsModified = false;
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastUpdatedBy??= await currentUserService.GetUserName();
                        break;
                    default:
                        break;
                }


            }
            if (await currentUserService.GetUserId() == Guid.Empty)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(await currentUserService.GetUserId(), cancellationToken);
            }

        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Pre-convention model configuration goes here
            configurationBuilder.Properties<Enum>()
                                .HaveConversion<int>();
            configurationBuilder.Properties<decimal>()
                .HaveColumnType("decimal(18,2)");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Identity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
                entity.HasKey(nameof(UserRole.UserId), nameof(UserRole.RoleId));
            });
            modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });
            modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
                entity.HasKey(nameof(IdentityUserLogin<Guid>.LoginProvider), nameof(IdentityUserLogin<Guid>.ProviderKey));
            });
            modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");
            });
            modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
                entity.HasKey(nameof(IdentityUserToken<Guid>.UserId), nameof(IdentityUserToken<Guid>.LoginProvider), nameof(IdentityUserToken<Guid>.Name));
            });

            #endregion
            #region Domain
            modelBuilder.Entity<Election>(e =>
            {
                e.ToTable("Elections");                
                e.OwnsMany(e => e.Voters, ep =>
                {
                    ep.ToTable("ElectionVoters");
                    ep.Property(p => p.ElectionId);
                    ep.WithOwner(p => p.Election);
                });
            });
            modelBuilder.Entity<SystemAdmin>(e =>
            {
                e.ToTable("SystemAdmins");
                e.OwnsOne(p => p.PersonalDetails, p =>
                {
                    p.ToTable("SystemAdminPersonalDetails");
                    p.Property(p => p.OwnerId);
                });
            });
            modelBuilder.Entity<ElectionPosition>(e=>
            {
                e.ToTable("ElectionPositions");                  
            });
            modelBuilder.Entity<PoliticalParty>(e =>
            {
                e.ToTable("PoliticalParty");
            });
            modelBuilder.Entity<Voter>(e =>
            {
                e.ToTable("Voters");
                e.OwnsOne(v => v.PersonalDetails, p =>
                {
                    p.ToTable("VoterPersonalDetails");
                    p.Property(p=>p.OwnerId);                     
                });

            });
            modelBuilder.Entity<Candidate>(e =>
            {
                e.ToTable("Candidates");
               
                e.OwnsOne(v => v.PersonalDetails, p =>
                {
                    p.ToTable("CandidatePersonalDetails");
                    p.Property(p => p.OwnerId);
                });                

            });
            
            
            modelBuilder.Entity<ElectionType>(e =>
            {
                e.ToTable("ElectionTypes");
            });
            #endregion
        }
    }
}
