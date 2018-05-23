﻿// <auto-generated />
using AccountManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AccountManager.Data.Migrations
{
    [DbContext(typeof(AccountManagerContext))]
    partial class AccountManagerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AccountManager.Data.Models.Account", b =>
                {
                    b.Property<long>("AccountId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(50);

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("AccountManager.Data.Models.AccountRole", b =>
                {
                    b.Property<long>("AccountRoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AccountId");

                    b.Property<long?>("RoleId");

                    b.HasKey("AccountRoleId");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRoles");
                });

            modelBuilder.Entity("AccountManager.Data.Models.Role", b =>
                {
                    b.Property<long>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AccountManager.Data.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("Age");

                    b.Property<DateTime?>("Bithrday");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .HasMaxLength(100);

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AccountManager.Data.Models.Account", b =>
                {
                    b.HasOne("AccountManager.Data.Models.User", "User")
                        .WithOne("Account")
                        .HasForeignKey("AccountManager.Data.Models.Account", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AccountManager.Data.Models.AccountRole", b =>
                {
                    b.HasOne("AccountManager.Data.Models.Account", "Account")
                        .WithMany("Roles")
                        .HasForeignKey("AccountId");

                    b.HasOne("AccountManager.Data.Models.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId");
                });
#pragma warning restore 612, 618
        }
    }
}
