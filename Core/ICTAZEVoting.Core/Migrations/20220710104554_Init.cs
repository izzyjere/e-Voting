using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTAZEVoting.Core.Migrations;

public partial class Init : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "Identity");

        migrationBuilder.CreateTable(
            name: "AuditTrails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AuditTrails", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ElectionTypes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ElectionTypes", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "PoliticalParty",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Slogan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Manifesto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PoliticalParty", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RoleClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RoleClaims", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SystemAdmins",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                RemoteIp = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemAdmins", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserClaims",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserClaims", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserLogins",
            schema: "Identity",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "Identity",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PictureUrl = table.Column<string>(type: "text", nullable: true),
                IsActive = table.Column<bool>(type: "bit", nullable: false),
                RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "UserTokens",
            schema: "Identity",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            });

        migrationBuilder.CreateTable(
            name: "Voters",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                VoterNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                VerificationCipher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                RemoteIp = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Voters", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Elections",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ElectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ElectionTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Duration = table.Column<double>(type: "float", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Elections", x => x.Id);
                table.ForeignKey(
                    name: "FK_Elections_ElectionTypes_ElectionTypeId",
                    column: x => x.ElectionTypeId,
                    principalTable: "ElectionTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "SystemAdminPersonalDetails",
            columns: table => new
            {
                SystemAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NRC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Gender = table.Column<int>(type: "int", nullable: false),
                PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_SystemAdminPersonalDetails", x => x.SystemAdminId);
                table.ForeignKey(
                    name: "FK_SystemAdminPersonalDetails_SystemAdmins_SystemAdminId",
                    column: x => x.SystemAdminId,
                    principalTable: "SystemAdmins",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            schema: "Identity",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_UserRoles_Roles_RoleId",
                    column: x => x.RoleId,
                    principalSchema: "Identity",
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_UserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalSchema: "Identity",
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "VoterPersonalDetails",
            columns: table => new
            {
                VoterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NRC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Gender = table.Column<int>(type: "int", nullable: false),
                PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_VoterPersonalDetails", x => x.VoterId);
                table.ForeignKey(
                    name: "FK_VoterPersonalDetails_Voters_VoterId",
                    column: x => x.VoterId,
                    principalTable: "Voters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ElectionPositions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ElectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ElectionPositions", x => x.Id);
                table.ForeignKey(
                    name: "FK_ElectionPositions_Elections_ElectionId",
                    column: x => x.ElectionId,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "ElectionVoters",
            columns: table => new
            {
                ElectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                VoterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HasVoted = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ElectionVoters", x => new { x.ElectionId, x.Id });
                table.ForeignKey(
                    name: "FK_ElectionVoters_Elections_ElectionId",
                    column: x => x.ElectionId,
                    principalTable: "Elections",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ElectionVoters_Voters_VoterId",
                    column: x => x.VoterId,
                    principalTable: "Voters",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Candidates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CandidateNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PoliticalPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ElectionPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastUpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                RemoteIp = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Candidates", x => x.Id);
                table.ForeignKey(
                    name: "FK_Candidates_ElectionPositions_ElectionPositionId",
                    column: x => x.ElectionPositionId,
                    principalTable: "ElectionPositions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Candidates_PoliticalParty_PoliticalPartyId",
                    column: x => x.PoliticalPartyId,
                    principalTable: "PoliticalParty",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "CandidatePersonalDetails",
            columns: table => new
            {
                CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                NRC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Gender = table.Column<int>(type: "int", nullable: false),
                PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CandidatePersonalDetails", x => x.CandidateId);
                table.ForeignKey(
                    name: "FK_CandidatePersonalDetails_Candidates_CandidateId",
                    column: x => x.CandidateId,
                    principalTable: "Candidates",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Candidates_ElectionPositionId",
            table: "Candidates",
            column: "ElectionPositionId");

        migrationBuilder.CreateIndex(
            name: "IX_Candidates_PoliticalPartyId",
            table: "Candidates",
            column: "PoliticalPartyId");

        migrationBuilder.CreateIndex(
            name: "IX_ElectionPositions_ElectionId",
            table: "ElectionPositions",
            column: "ElectionId");

        migrationBuilder.CreateIndex(
            name: "IX_Elections_ElectionTypeId",
            table: "Elections",
            column: "ElectionTypeId");

        migrationBuilder.CreateIndex(
            name: "IX_ElectionVoters_VoterId",
            table: "ElectionVoters",
            column: "VoterId");

        migrationBuilder.CreateIndex(
            name: "IX_UserRoles_RoleId",
            schema: "Identity",
            table: "UserRoles",
            column: "RoleId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AuditTrails");

        migrationBuilder.DropTable(
            name: "CandidatePersonalDetails");

        migrationBuilder.DropTable(
            name: "ElectionVoters");

        migrationBuilder.DropTable(
            name: "RoleClaims",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "SystemAdminPersonalDetails");

        migrationBuilder.DropTable(
            name: "UserClaims",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "UserLogins",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "UserRoles",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "UserTokens",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "VoterPersonalDetails");

        migrationBuilder.DropTable(
            name: "Candidates");

        migrationBuilder.DropTable(
            name: "SystemAdmins");

        migrationBuilder.DropTable(
            name: "Roles",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "Identity");

        migrationBuilder.DropTable(
            name: "Voters");

        migrationBuilder.DropTable(
            name: "ElectionPositions");

        migrationBuilder.DropTable(
            name: "PoliticalParty");

        migrationBuilder.DropTable(
            name: "Elections");

        migrationBuilder.DropTable(
            name: "ElectionTypes");
    }
}
