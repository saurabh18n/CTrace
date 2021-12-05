﻿// <auto-generated />
using System;
using CTrace.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CTrace.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211204181514_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CTrace.Models.User", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("user_id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("user_failcount")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("user_failcount")
                        .HasColumnType("tinyint")
                        .HasDefaultValueSql("0");

                    b.Property<string>("user_fname")
                        .IsRequired()
                        .HasColumnName("user_fname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("user_isadmin")
                        .IsRequired()
                        .HasColumnName("user_isadmin")
                        .HasColumnType("bit");

                    b.Property<DateTime>("user_lastlogin")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("user_lastlogin")
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("user_lname")
                        .HasColumnName("user_lname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("user_name")
                        .IsRequired()
                        .HasColumnName("user_mobile")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("user_pass")
                        .IsRequired()
                        .HasColumnName("user_pass")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("user_salt")
                        .IsRequired()
                        .HasColumnName("user_salt")
                        .HasColumnType("nvarchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("user_id");

                    b.HasIndex("user_name")
                        .IsUnique();

                    b.ToTable("tab_users");

                    b.HasData(
                        new
                        {
                            user_id = 1,
                            user_failcount = (byte)0,
                            user_fname = "Admin",
                            user_isadmin = true,
                            user_lastlogin = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            user_name = "admin",
                            user_pass = "cKqBaajdQlg992yozgv8HIQajXgaC8g9kgQlsAP4VkQ=",
                            user_salt = "+WWMvZ2V1eeXkVnhJ4naHg=="
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
