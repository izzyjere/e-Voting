using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Interfaces;

using ICTAZEVoting.Core.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<int>>().ToList())
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
            if (await currentUserService.GetUserId() == 0)
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
            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
                entity.HasKey(nameof(IdentityUserLogin<int>.LoginProvider), nameof(IdentityUserLogin<int>.ProviderKey));
            });
            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");
            });
            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
                entity.HasKey(nameof(IdentityUserToken<int>.UserId), nameof(IdentityUserToken<int>.LoginProvider), nameof(IdentityUserToken<int>.Name));
            });

            #endregion
            #region Domain
            
            #endregion
        }
    }
}
