﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FallVerseBotV2.Migrations
{
    [DbContext(typeof(BotDbContext))]
    partial class BotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CurrencyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CurrencyType");
                });

            modelBuilder.Entity("ServerSettings", b =>
                {
                    b.Property<decimal>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<int?>("CasinoCurrencyId")
                        .HasColumnType("integer");

                    b.Property<int>("DailyCurrencyId")
                        .HasColumnType("integer");

                    b.HasKey("GuildId");

                    b.HasIndex("CasinoCurrencyId");

                    b.HasIndex("DailyCurrencyId");

                    b.ToTable("ServerSettings");
                });

            modelBuilder.Entity("UserCurrencyBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Amount")
                        .HasColumnType("integer");

                    b.Property<int>("CurrencyTypeId")
                        .HasColumnType("integer");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyTypeId");

                    b.HasIndex("UserId", "CurrencyTypeId")
                        .IsUnique();

                    b.ToTable("UserCurrencyBalance");
                });

            modelBuilder.Entity("UserEconomy", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("CurrencyAmount")
                        .HasColumnType("integer");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime?>("LastClaimed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StreakCount")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.ToTable("UserEconomy");
                });

            modelBuilder.Entity("UserGameStats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("GameKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<Dictionary<string, object>>("LastGameData")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("jsonb")
                        .HasDefaultValueSql("'{}'::jsonb");

                    b.Property<DateTime>("LastPlayed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Losses")
                        .HasColumnType("integer");

                    b.Property<int>("NetGain")
                        .HasColumnType("integer");

                    b.Property<int>("Ties")
                        .HasColumnType("integer");

                    b.Property<int>("TotalWagered")
                        .HasColumnType("integer");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Wins")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserGameStats");
                });

            modelBuilder.Entity("UserRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("JoinedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserRecord");
                });

            modelBuilder.Entity("ServerSettings", b =>
                {
                    b.HasOne("CurrencyType", "CasinoCurrency")
                        .WithMany()
                        .HasForeignKey("CasinoCurrencyId");

                    b.HasOne("CurrencyType", "DailyCurrency")
                        .WithMany()
                        .HasForeignKey("DailyCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CasinoCurrency");

                    b.Navigation("DailyCurrency");
                });

            modelBuilder.Entity("UserCurrencyBalance", b =>
                {
                    b.HasOne("CurrencyType", "CurrencyType")
                        .WithMany()
                        .HasForeignKey("CurrencyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UserRecord", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrencyType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserEconomy", b =>
                {
                    b.HasOne("UserRecord", "User")
                        .WithOne()
                        .HasForeignKey("UserEconomy", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("UserGameStats", b =>
                {
                    b.HasOne("UserRecord", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
