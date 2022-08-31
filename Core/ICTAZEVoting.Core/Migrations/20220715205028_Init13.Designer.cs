﻿// <auto-generated />
using System;
using ICTAZEVoting.Core.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ICTAZEVoting.Core.Migrations;

[DbContext(typeof(SystemDbContext))]
[Migration("20220715205028_Init13")]
partial class Init13
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "6.0.6")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

        modelBuilder.Entity("ICTAZEVoting.Core.Models.Audit", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("AffectedColumns")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("DateTime")
                    .HasColumnType("datetime2");

                b.Property<string>("NewValues")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("OldValues")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PrimaryKey")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("TableName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Type")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.ToTable("AuditTrails");
            });

        modelBuilder.Entity("ICTAZEVoting.Core.Models.Role", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("ConcurrencyStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Description")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("NormalizedName")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Roles", "Identity");
            });

        modelBuilder.Entity("ICTAZEVoting.Core.Models.User", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<string>("ConcurrencyStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Email")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("IsActive")
                    .HasColumnType("bit");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("bit");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("MiddleName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("NRC")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("NormalizedEmail")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("NormalizedUserName")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("PasswordHash")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("PersonId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("PhoneNumber")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("PictureUrl")
                    .HasColumnType("text");

                b.Property<string>("RefreshToken")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("RefreshTokenExpiryTime")
                    .HasColumnType("datetime2");

                b.Property<string>("SecurityStamp")
                    .HasColumnType("nvarchar(max)");

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("bit");

                b.Property<string>("UserName")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Users", "Identity");
            });

        modelBuilder.Entity("ICTAZEVoting.Core.Models.UserRole", b =>
            {
                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("UserRoles", "Identity");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Candidate", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("ElectionPositionId")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("LastModifiedOn")
                    .HasColumnType("datetime2");

                b.Property<string>("LastUpdatedBy")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("PoliticalPartyId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("RemoteIp")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("ElectionPositionId");

                b.HasIndex("PoliticalPartyId");

                b.ToTable("Candidates", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Constituency", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("Constituencies", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Election", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<double>("Duration")
                    .HasColumnType("float");

                b.Property<DateTime?>("ElectionDate")
                    .IsRequired()
                    .HasColumnType("datetime2");

                b.Property<Guid>("ElectionTypeId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("ElectionTypeId");

                b.ToTable("Elections", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.ElectionPosition", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("ElectionId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Position")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("ElectionId");

                b.ToTable("ElectionPositions", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.ElectionType", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("ElectionTypes", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.PoliticalParty", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("LogoUrl")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Manifesto")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Slogan")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.ToTable("PoliticalParty", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.PollingStation", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("ConstituencyId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("Id");

                b.HasIndex("ConstituencyId");

                b.ToTable("PolingStations", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.SystemAdmin", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("ConstituencyId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime?>("LastModifiedOn")
                    .HasColumnType("datetime2");

                b.Property<string>("LastUpdatedBy")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("RemoteIp")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("ConstituencyId");

                b.ToTable("SystemAdmins", (string)null);
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Voter", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("CreatedBy")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime?>("LastModifiedOn")
                    .HasColumnType("datetime2");

                b.Property<string>("LastUpdatedBy")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("PolingStationId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("RemoteIp")
                    .HasColumnType("nvarchar(max)");

                b.Property<DateTime>("TimeStamp")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.HasIndex("PolingStationId");

                b.ToTable("Voters", (string)null);
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("RoleId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.ToTable("RoleClaims", "Identity");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                b.Property<string>("ClaimType")
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("ClaimValue")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");

                b.ToTable("UserClaims", "Identity");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ProviderDisplayName")
                    .HasColumnType("nvarchar(max)");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("LoginProvider", "ProviderKey");

                b.ToTable("UserLogins", "Identity");
            });

        modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
            {
                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("Value")
                    .HasColumnType("nvarchar(max)");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("UserTokens", "Identity");
            });

        modelBuilder.Entity("ICTAZEVoting.Core.Models.UserRole", b =>
            {
                b.HasOne("ICTAZEVoting.Core.Models.Role", "Role")
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ICTAZEVoting.Core.Models.User", "User")
                    .WithMany("Roles")
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Role");

                b.Navigation("User");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Candidate", b =>
            {
                b.HasOne("ICTAZEVoting.Shared.Models.ElectionPosition", "Position")
                    .WithMany("Candidates")
                    .HasForeignKey("ElectionPositionId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("ICTAZEVoting.Shared.Models.PoliticalParty", "PoliticalParty")
                    .WithMany("Candidates")
                    .HasForeignKey("PoliticalPartyId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.OwnsOne("ICTAZEVoting.Shared.Models.PersonalDetails", "PersonalDetails", b1 =>
                    {
                        b1.Property<Guid>("CandidateId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("Address")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<DateTime?>("DateOfBirth")
                            .IsRequired()
                            .HasColumnType("datetime2");

                        b1.Property<string>("Email")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("FirstName")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<int>("Gender")
                            .HasColumnType("int");

                        b1.Property<string>("LastName")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("MiddleName")
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("NRC")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<Guid>("OwnerId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("PhoneNumber")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("PictureUrl")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<Guid>("UserId")
                            .HasColumnType("uniqueidentifier");

                        b1.HasKey("CandidateId");

                        b1.ToTable("CandidatePersonalDetails", (string)null);

                        b1.WithOwner()
                            .HasForeignKey("CandidateId");
                    });

                b.Navigation("PersonalDetails")
                    .IsRequired();

                b.Navigation("PoliticalParty");

                b.Navigation("Position");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Election", b =>
            {
                b.HasOne("ICTAZEVoting.Shared.Models.ElectionType", "Type")
                    .WithMany()
                    .HasForeignKey("ElectionTypeId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.OwnsMany("ICTAZEVoting.Shared.Models.ElectionVoter", "Voters", b1 =>
                    {
                        b1.Property<Guid>("ElectionId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<int>("Id")
                            .ValueGeneratedOnAdd()
                            .HasColumnType("int");

                        SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                        b1.Property<bool>("HasVoted")
                            .HasColumnType("bit");

                        b1.Property<Guid>("VoterId")
                            .HasColumnType("uniqueidentifier");

                        b1.HasKey("ElectionId", "Id");

                        b1.HasIndex("VoterId");

                        b1.ToTable("ElectionVoters", (string)null);

                        b1.WithOwner("Election")
                            .HasForeignKey("ElectionId");

                        b1.HasOne("ICTAZEVoting.Shared.Models.Voter", "Voter")
                            .WithMany()
                            .HasForeignKey("VoterId")
                            .OnDelete(DeleteBehavior.Cascade)
                            .IsRequired();

                        b1.Navigation("Election");

                        b1.Navigation("Voter");
                    });

                b.Navigation("Type");

                b.Navigation("Voters");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.ElectionPosition", b =>
            {
                b.HasOne("ICTAZEVoting.Shared.Models.Election", "Election")
                    .WithMany("Positions")
                    .HasForeignKey("ElectionId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Election");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.PollingStation", b =>
            {
                b.HasOne("ICTAZEVoting.Shared.Models.Constituency", "Constituency")
                    .WithMany("PolingStations")
                    .HasForeignKey("ConstituencyId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Constituency");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.SystemAdmin", b =>
            {
                b.HasOne("ICTAZEVoting.Shared.Models.Constituency", "Constituency")
                    .WithMany()
                    .HasForeignKey("ConstituencyId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.OwnsOne("ICTAZEVoting.Shared.Models.PersonalDetails", "PersonalDetails", b1 =>
                    {
                        b1.Property<Guid>("SystemAdminId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("Address")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<DateTime?>("DateOfBirth")
                            .IsRequired()
                            .HasColumnType("datetime2");

                        b1.Property<string>("Email")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("FirstName")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<int>("Gender")
                            .HasColumnType("int");

                        b1.Property<string>("LastName")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("MiddleName")
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("NRC")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<Guid>("OwnerId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("PhoneNumber")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("PictureUrl")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<Guid>("UserId")
                            .HasColumnType("uniqueidentifier");

                        b1.HasKey("SystemAdminId");

                        b1.ToTable("SystemAdminPersonalDetails", (string)null);

                        b1.WithOwner()
                            .HasForeignKey("SystemAdminId");
                    });

                b.Navigation("Constituency");

                b.Navigation("PersonalDetails")
                    .IsRequired();
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Voter", b =>
            {
                b.HasOne("ICTAZEVoting.Shared.Models.PollingStation", "PolingStation")
                    .WithMany()
                    .HasForeignKey("PolingStationId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.OwnsOne("ICTAZEVoting.Shared.Models.SecreteKey", "SecreteKey", b1 =>
                    {
                        b1.Property<Guid>("VoterId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("EncryptedKey")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("IV")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.HasKey("VoterId");

                        b1.ToTable("SecreteKeys", (string)null);

                        b1.WithOwner()
                            .HasForeignKey("VoterId");
                    });

                b.OwnsOne("ICTAZEVoting.Shared.Models.PersonalDetails", "PersonalDetails", b1 =>
                    {
                        b1.Property<Guid>("VoterId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("Address")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<DateTime?>("DateOfBirth")
                            .IsRequired()
                            .HasColumnType("datetime2");

                        b1.Property<string>("Email")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("FirstName")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<int>("Gender")
                            .HasColumnType("int");

                        b1.Property<string>("LastName")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("MiddleName")
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("NRC")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<Guid>("OwnerId")
                            .HasColumnType("uniqueidentifier");

                        b1.Property<string>("PhoneNumber")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<string>("PictureUrl")
                            .IsRequired()
                            .HasColumnType("nvarchar(max)");

                        b1.Property<Guid>("UserId")
                            .HasColumnType("uniqueidentifier");

                        b1.HasKey("VoterId");

                        b1.ToTable("VoterPersonalDetails", (string)null);

                        b1.WithOwner()
                            .HasForeignKey("VoterId");
                    });

                b.Navigation("PersonalDetails")
                    .IsRequired();

                b.Navigation("PolingStation");

                b.Navigation("SecreteKey")
                    .IsRequired();
            });

        modelBuilder.Entity("ICTAZEVoting.Core.Models.User", b =>
            {
                b.Navigation("Roles");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Constituency", b =>
            {
                b.Navigation("PolingStations");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.Election", b =>
            {
                b.Navigation("Positions");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.ElectionPosition", b =>
            {
                b.Navigation("Candidates");
            });

        modelBuilder.Entity("ICTAZEVoting.Shared.Models.PoliticalParty", b =>
            {
                b.Navigation("Candidates");
            });
#pragma warning restore 612, 618
    }
}
